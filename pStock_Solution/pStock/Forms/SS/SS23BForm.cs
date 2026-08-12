using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS23B.pas / SS23B.dfm (TfrmSS23B) 이식 — 보관료 자동계산(반월 단위 일괄 보관전표 생성).
/// 원본 로직을 그대로 따른다: 선택한 반월(1~15일 또는 16~말일) 구간의 기존 자동 보관전표를
/// 삭제한 뒤, 전 품목의 해당 반월말 재고(JQTY)를 계산해 재고가 있는 품목만 보관전표(IPCHF)를
/// 새로 생성하고 미수파일(MISUF)에 반영한다.
///
/// 참고: 원본 코드에는 "삭제된 전표의 미수 반영분을 되돌리는" 조회가 이미 삭제된 데이터를
/// 대상으로 실행되어 사실상 실행되지 않는(no-op) 코드가 있었다. 여기서는 그 죽은 코드는
/// 제외하고 나머지 로직은 그대로 이식했다.
/// </summary>
public partial class SS23BForm : Form
{
    public bool Executed { get; private set; }

    public SS23BForm()
    {
        InitializeComponent();
    }

    private void DtpDate_ValueChanged(object? sender, EventArgs e)
    {
        UpdateTermLabel();
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        RunBatch();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void SS23BForm_Load(object? sender, EventArgs e)
    {
        var now = DateTime.Now;
        this.dtpDate.Value = now.Day < 16 ? new DateTime(now.Year, now.Month, 1) : new DateTime(now.Year, now.Month, 16);
        UpdateTermLabel();
    }

    private void SS23BForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2) btnOk.PerformClick();
        else if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.Escape) Close();
    }

    private void UpdateTermLabel() => lblTerm.Text = (dtpDate.Value.Day <= 15 ? "1" : "2") + " 기";

    private void RunBatch()
    {
        if (MessageBox.Show("보관료 자동계산을 실행하시겠습니까?\r\n기존 자동전표는 삭제 후 재계산됩니다.",
                "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        btnOk.Enabled = false;
        progress.Visible = true;

        var baseDate = dtpDate.Value;
        bool firstHalf = baseDate.Day < 16;
        var monthPrefix = baseDate.ToString("yyyy-MM");
        var date1 = firstHalf ? $"{monthPrefix}-01" : $"{monthPrefix}-16";
        var date2 = firstHalf ? $"{monthPrefix}-15" : $"{monthPrefix}-31";
        var year = baseDate.Year.ToString();
        var month = baseDate.Month;

        try
        {
            AppDb.BeginTransaction();

            using (var delQ = new DbQuery())
            {
                delQ.Add("DELETE FROM IPCHF WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND GUBN1='1'");
                delQ.ParamByName("DATE1").AsString = date1;
                delQ.ParamByName("DATE2").AsString = date2;
                delQ.ExecSQL();
            }

            using var loopQ = new DbQuery();
            loopQ.Add("SELECT SUM(I1QT01) I01,SUM(I1QT02) I02,SUM(I1QT03) I03,SUM(I1QT04) I04,SUM(I1QT05) I05,SUM(I1QT06) I06,");
            loopQ.Add("       SUM(I1QT07) I07,SUM(I1QT08) I08,SUM(I1QT09) I09,SUM(I1QT10) I10,SUM(I1QT11) I11,SUM(I1QT12) I12,");
            loopQ.Add("       SUM(O1QT01) O01,SUM(O1QT02) O02,SUM(O1QT03) O03,SUM(O1QT04) O04,SUM(O1QT05) O05,SUM(O1QT06) O06,");
            loopQ.Add("       SUM(O1QT07) O07,SUM(O1QT08) O08,SUM(O1QT09) O09,SUM(O1QT10) O10,SUM(O1QT11) O11,SUM(O1QT12) O12,");
            loopQ.Add("       SUM(BBALQ) BBALQ,IYEAR,A.ITNBR,ITDSC,ISPEC,DANWI,BCOST,CVCOD,CVNAM FROM ITEMBL A");
            loopQ.Add("  LEFT OUTER JOIN ITEMAS B ON A.ITNBR=B.ITNBR");
            loopQ.Add("  LEFT OUTER JOIN CVMAST C ON SUBSTRING(A.ITNBR,1,4)=C.CVCOD");
            loopQ.Add(" WHERE IYEAR=@YEAR");
            loopQ.Add(" GROUP BY IYEAR,A.ITNBR,ITDSC,ISPEC,DANWI,BCOST,CVCOD,CVNAM");
            loopQ.ParamByName("YEAR").AsString = year;
            loopQ.Open();

            if (loopQ.IsEmpty)
            {
                AppDb.Rollback();
                MessageBox.Show("산정할 자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            progress.Maximum = loopQ.RecordCount;
            progress.Value = 0;

            loopQ.First();
            int done = 0;
            while (!loopQ.Eof)
            {
                double jQty = loopQ.FieldByName("BBALQ").AsFloat;
                if (month != 1)
                {
                    for (int i = 1; i <= month - 1; i++)
                    {
                        var idx = i.ToString("00");
                        jQty += loopQ.FieldByName($"I{idx}").AsFloat - loopQ.FieldByName($"O{idx}").AsFloat;
                    }
                }

                var itnbr = loopQ.FieldByName("ITNBR").AsString;

                using (var iq = new DbQuery())
                {
                    iq.Add("SELECT SUM(IQTY) AS IQTY FROM IPGOF WHERE IDATE BETWEEN @DATE1 AND @DATE2 AND ITNBR=@ITNBR");
                    iq.ParamByName("DATE1").AsString = date1;
                    iq.ParamByName("DATE2").AsString = date2;
                    iq.ParamByName("ITNBR").AsString = itnbr;
                    iq.Open();
                    double iQty = iq.IsEmpty || iq.FieldByName("IQTY").IsNull ? 0 : iq.FieldByName("IQTY").AsFloat;

                    // 원본 SQL 그대로: 출고 조회 구간이 반월 구분과 무관하게 항상 그 달 1~15일로 고정되어 있음(원본 버그 재현)
                    using var oq = new DbQuery();
                    oq.Add("SELECT SUM(A2.TRQTY) AS OQTY FROM SALE_M A1");
                    oq.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
                    oq.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND ITCOD=@ITNBR");
                    oq.ParamByName("DATE1").AsString = $"{monthPrefix}-01";
                    oq.ParamByName("DATE2").AsString = $"{monthPrefix}-15";
                    oq.ParamByName("ITNBR").AsString = itnbr;
                    oq.Open();
                    double oQty = oq.IsEmpty || oq.FieldByName("OQTY").IsNull ? 0 : oq.FieldByName("OQTY").AsFloat;

                    jQty = firstHalf ? jQty + iQty : jQty + iQty - oQty;
                }

                if (jQty > 0)
                {
                    var bcost = loopQ.FieldByName("BCOST").AsFloat;
                    var cvcod = loopQ.FieldByName("CVCOD").AsString;
                    var danwi = loopQ.FieldByName("DANWI").AsString;
                    var tdate = firstHalf ? $"{monthPrefix}-15" : $"{monthPrefix}-28";

                    using (var maxQ = new DbQuery())
                    {
                        maxQ.Add("SELECT MAX(SEQNO) AS MAXSEQ FROM IPCHF WHERE TDATE=@TDATE AND GUBN1='1' AND SEQNO>5000");
                        maxQ.ParamByName("TDATE").AsString = tdate;
                        maxQ.Open();
                        int seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 5001 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                        var oamt = (int)Math.Truncate(jQty * bcost);

                        using var ins = new DbQuery();
                        ins.Add("INSERT INTO IPCHF (TDATE,SEQNO,GUBN1,CVCOD,ITNBR,DANWI,IOQTY,");
                        ins.Add("       ODAN,OAMT,JAMT1,JAMT2,JAMT3,TPRO,HOUSE,TBIGO,KEYNO,MDATE)");
                        ins.Add("  VALUES(@TDATE,@SEQNO,'1',@CVCOD,@ITNBR,@DANWI,@IOQTY,");
                        ins.Add("       @ODAN,@OAMT,0,0,0,@TPRO,'','','',@MDATE)");
                        ins.ParamByName("TDATE").AsString = tdate;
                        ins.ParamByName("SEQNO").AsInteger = seqNo;
                        ins.ParamByName("CVCOD").AsString = cvcod;
                        ins.ParamByName("ITNBR").AsString = itnbr;
                        ins.ParamByName("DANWI").AsString = danwi;
                        ins.ParamByName("IOQTY").AsInteger = (int)jQty;
                        ins.ParamByName("ODAN").AsInteger = (int)bcost;
                        ins.ParamByName("OAMT").AsInteger = oamt;
                        ins.ParamByName("TPRO").AsInteger = PublicLib.StrToMoney(baseDate.ToString("yyyyMM") + (firstHalf ? "1" : "2"));
                        ins.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                        ins.ExecSQL();

                        if (!LedgerUpdates.MisuUpdate(year, month.ToString("00"), cvcod, 1, oamt, 0, 0))
                        {
                            AppDb.Rollback();
                            MessageBox.Show("미수파일 쓰는중 에러발생(+)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                done++;
                progress.Value = Math.Min(done, progress.Maximum);
                lblStatus.Text = $"{done} / {progress.Maximum} 품목 처리중...";
                Application.DoEvents();

                loopQ.Next();
            }

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("보관료 산정중 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnOk.Enabled = true;
            progress.Visible = false;
            return;
        }

        MessageBox.Show("보관료 산정 전표 생성이 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Executed = true;
        Close();
    }
}
