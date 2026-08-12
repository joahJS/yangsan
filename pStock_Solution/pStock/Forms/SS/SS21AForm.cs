using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS21A.pas / SS21A.dfm (TfrmSS21A) 이식 — 입고 등록/수정(IPGOF).
/// 저장 시 재고파일(ITEMBL), 미수파일(MISUF) 갱신 + 보관전표(IPCHF) 자동생성까지 원본 그대로 수행한다.
/// </summary>
public partial class SS21AForm : Form
{
    /// <summary>원본 vJob: 'I'=신규, 'U'=수정.</summary>
    public string Job { get; set; } = "I";
    /// <summary>원본 vHouse: 수정 시 원래 저장위치(창고 변경 제한 검사용).</summary>
    public string OriginalHouse { get; set; } = string.Empty;
    /// <summary>원본 vUpdateQty: 수정 시 기존 입고수량(재고 원복용).</summary>
    public int OriginalQty { get; set; }
    /// <summary>원본 vUpdateAmt: 수정 시 기존 입고금액 합계(미수 원복용).</summary>
    public int OriginalAmt { get; set; }

    /// <summary>저장 성공 시 true.</summary>
    public bool Saved { get; private set; }

    public SS21AForm()
    {
        InitializeComponent();
    }

    private void SS21AForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: ClearForm(); edtItnbr.Focus(); break;
            case Keys.F2: if (Job == "I") btnAdd.PerformClick(); break;
            case Keys.F3: btnOne.PerformClick(); break;
            case Keys.Escape: Close(); break;
        }
    }

    private void ResetHouseList()
    {
        cboHouse.Items.Clear();
        cboHouse.Items.Add(" ");
        foreach (var (refno, _) in ReffpfCache.Get("CG")) cboHouse.Items.Add(refno);
        cboHouse.SelectedIndex = 0;
    }

    /// <summary>원본 prcClear.</summary>
    public void ClearForm()
    {
        dspCvcod.Clear(); dspCvnam.Clear();
        edtNo.Clear(); chkCancel.Checked = false;
        edtItnbr.Clear(); dspItdsc.Clear(); dspDanwi.Clear();
        edtIqty.Value = 0; edtIcost.Value = 0; edtIamt.Value = 0;
        dspBcost.Value = 0; dspOcost.Value = 0;
        edtJamt1.Value = 0; edtJamt2.Value = 0; edtJamt3.Value = 0;
        if (cboHouse.Items.Count > 0) cboHouse.SelectedIndex = 0;
        mskYdate.Clear(); edtBigo.Clear();
        OriginalQty = 0; OriginalAmt = 0; dspOqty.Text = "0";
        Job = "I";
        pnlJob.Text = "입고등록중";
    }

    /// <summary>거래처코드 칸에서 Enter 입력시 거래처 검색 팝업을 띄운다(품번 입력시 자동으로도 채워지지만,
    /// 거래처를 먼저 선택하고 싶은 경우를 위한 수동 선택 경로).</summary>
    private void DspCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        using var dlg = new CvcodLookupForm(cvguFilter: "1");
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            dspCvcod.Text = dlg.SelectedCode;
            dspCvnam.Text = dlg.SelectedName;
        }
    }

    /// <summary>원본 edtItnbrKeyDown.</summary>
    private void EdtItnbr_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtItnbr.Text))
        {
            using var dlg = new ItemLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtItnbr.Text = dlg.SelectedItnbr;
                dspItdsc.Text = $"{dlg.SelectedItdsc} {dlg.SelectedIspec}".Trim();
                dspDanwi.Text = dlg.SelectedDanwi;
                edtIcost.Value = dlg.SelectedIcost;
                dspBcost.Value = dlg.SelectedBcost;
                dspOcost.Value = dlg.SelectedOcost;

                LoadCvcodFromItem(edtItnbr.Text);
            }
            else
            {
                dspItdsc.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT A.*,B.* FROM ITEMAS A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON SUBSTRING(A.ITNBR,1,4)=B.CVCOD");
        q.Add(" WHERE ITNBR=@ITNBR");
        q.ParamByName("ITNBR").AsString = edtItnbr.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            dspItdsc.Text = $"{q.FieldByName("ITDSC").AsString} {q.FieldByName("ISPEC").AsString}".Trim();
            dspDanwi.Text = q.FieldByName("DANWI").AsString;
            dspCvcod.Text = q.FieldByName("CVCOD").AsString;
            dspCvnam.Text = q.FieldByName("CVNAM").AsString;
            edtIcost.Value = (decimal)q.FieldByName("ICOST").AsFloat;
            dspBcost.Value = (decimal)q.FieldByName("BCOST").AsFloat;
            dspOcost.Value = (decimal)q.FieldByName("OCOST").AsFloat;
        }
    }

    private void LoadCvcodFromItem(string itnbr)
    {
        var cv = itnbr.Length >= 4 ? itnbr[..4] : itnbr;
        using var q = new DbQuery();
        q.Add($"SELECT * FROM CVMAST WHERE CVCOD='{cv}'");
        q.Open();
        if (!q.IsEmpty)
        {
            dspCvcod.Text = q.FieldByName("CVCOD").AsString;
            dspCvnam.Text = q.FieldByName("CVNAM").AsString;
        }
    }

    /// <summary>원본 edtIqty1Exit.</summary>
    private void RecalcAmt()
    {
        edtIamt.Value = edtIqty.Value * edtIcost.Value;
    }

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtItnbr.Text))
        {
            MessageBox.Show("품목코드는 필수항목입니다.\r\n반드시 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtItnbr.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(cboHouse.Text))
        {
            MessageBox.Show("저장위치를 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboHouse.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(dspCvcod.Text))
        {
            MessageBox.Show("거래처코드는 필수항목입니다.\r\n반드시 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (edtIqty.Value == 0)
        {
            MessageBox.Show("입고수량을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtIqty.Focus();
            return false;
        }
        if (PublicLib.StrToMoney(dspOqty.Text) > 0 && OriginalHouse != cboHouse.Text)
        {
            MessageBox.Show("출고중인 것은 저장위치를 수정할 수 없습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    /// <summary>원본 fncAddJob.</summary>
    private bool SaveEntry()
    {
        if (!ErrCheck()) return false;

        var dateStr = dtpDate.Value.ToString("yyyy-MM-dd");
        var year = dtpDate.Value.Year.ToString();
        var month = dtpDate.Value.Month.ToString("00");

        int seqNo;
        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            if (Job == "I")
            {
                using var maxQ = new DbQuery();
                maxQ.Add("SELECT MAX(ISEQ) AS MAXSEQ FROM IPGOF WHERE IDATE=@IDATE");
                maxQ.ParamByName("IDATE").AsString = dateStr;
                maxQ.Open();
                seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 1 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                q.Add("INSERT INTO IPGOF (IDATE,ISEQ,IGUBN,CVCOD,ITNBR,DANWI,IQTY,");
                q.Add("       OQTY,IDAN,IAMT,JAMT1,JAMT2,JAMT3,TPRO,HOUSE,YDATE,");
                q.Add("       IBIGO,MDATE)");
                q.Add("  VALUES(@IDATE,@ISEQ,@IGUBN,@CVCOD,@ITNBR,@DANWI,@IQTY,0,");
                q.Add("       @IDAN,@IAMT,@JAMT1,@JAMT2,@JAMT3,@TPRO,@HOUSE,");
                q.Add("       @YDATE,@IBIGO,@MDATE)");
            }
            else
            {
                seqNo = PublicLib.StrToMoney(edtNo.Text);
                q.Add("UPDATE IPGOF SET IGUBN=@IGUBN,CVCOD=@CVCOD,ITNBR=@ITNBR,");
                q.Add("    DANWI=@DANWI,IQTY=@IQTY,IDAN=@IDAN,");
                q.Add("    IAMT=@IAMT,JAMT1=@JAMT1,JAMT2=@JAMT2,JAMT3=@JAMT3,");
                q.Add("    TPRO=@TPRO,HOUSE=@HOUSE,YDATE=@YDATE,IBIGO=@IBIGO,");
                q.Add("    MDATE=@MDATE");
                q.Add(" WHERE IDATE=@IDATE AND ISEQ=@ISEQ");
            }

            q.ParamByName("IDATE").AsString = dateStr;
            q.ParamByName("ISEQ").AsInteger = seqNo;
            q.ParamByName("IGUBN").AsString = chkCancel.Checked ? "3" : "1";
            q.ParamByName("CVCOD").AsString = dspCvcod.Text.Trim();
            q.ParamByName("ITNBR").AsString = edtItnbr.Text.Trim();
            q.ParamByName("DANWI").AsString = dspDanwi.Text.Trim();
            q.ParamByName("IQTY").AsInteger = (int)edtIqty.Value;
            q.ParamByName("IDAN").AsInteger = (int)edtIcost.Value;
            q.ParamByName("IAMT").AsInteger = (int)edtIamt.Value;
            q.ParamByName("JAMT1").AsInteger = (int)edtJamt1.Value;
            q.ParamByName("JAMT2").AsInteger = (int)edtJamt2.Value;
            q.ParamByName("JAMT3").AsInteger = (int)edtJamt3.Value;
            q.ParamByName("TPRO").AsInteger = 0;
            q.ParamByName("HOUSE").AsString = cboHouse.Text.Trim();
            q.ParamByName("YDATE").AsString = mskYdate.Text.Trim();
            q.ParamByName("IBIGO").AsString = edtBigo.Text.Trim();
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
            q.ExecSQL();

            if (Job != "I")
            {
                if (!LedgerUpdates.ItemblUpdate(year, month, edtItnbr.Text.Trim(), OriginalHouse, -OriginalQty, 0, 0))
                {
                    AppDb.Rollback();
                    MessageBox.Show("재고파일에 쓰는중 에러발생(-)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (!LedgerUpdates.ItemblUpdate(year, month, edtItnbr.Text.Trim(), cboHouse.Text.Trim(), (int)edtIqty.Value, 0, 0))
            {
                AppDb.Rollback();
                MessageBox.Show("재고파일에 쓰는중 에러발생(+)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var cvPrefix = edtItnbr.Text.Length >= 4 ? edtItnbr.Text[..4] : edtItnbr.Text;
            var totalAmt = (int)edtIamt.Value + (int)edtJamt1.Value + (int)edtJamt2.Value + (int)edtJamt3.Value;
            if (!LedgerUpdates.MisuUpdate(year, month, cvPrefix, 1, totalAmt - OriginalAmt, 0, 0))
            {
                AppDb.Rollback();
                MessageBox.Show("미수파일 쓰는중 에러발생(+)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var keyNo = dateStr + seqNo.ToString("000");
            var bAmt = (int)(edtIqty.Value * dspBcost.Value);
            if (!LedgerUpdates.SaveJob(Job, dateStr, dspCvcod.Text.Trim(), edtItnbr.Text.Trim(), dspDanwi.Text.Trim(),
                    cboHouse.Text.Trim(), "자동전표", keyNo, (int)edtIqty.Value, (int)dspBcost.Value, bAmt, 0, 0, 0, 0))
            {
                AppDb.Rollback();
                MessageBox.Show("입고에 의한 보관전표 자동생성시 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var originalBAmt = (int)(OriginalQty * (double)dspBcost.Value);
            if (!LedgerUpdates.MisuUpdate(year, month, cvPrefix, 1, bAmt - originalBAmt, 0, 0))
            {
                AppDb.Rollback();
                MessageBox.Show("미수파일 쓰는중 에러발생(보관내역)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("입고전표 보수시 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>원본 SS21.DBGrid1DblClick: 목록에서 선택한 행 값을 편집 폼에 채운다.</summary>
    public void LoadForEdit(System.Data.DataRowView row)
    {
        dtpDate.Value = Convert.ToDateTime(row["IDATE"]);
        edtNo.Text = row["ISEQ"].ToString();
        dspCvcod.Text = row["CVCOD"].ToString();
        dspCvnam.Text = row.Row.Table.Columns.Contains("CVNAM") ? row["CVNAM"].ToString() : "";
        chkCancel.Checked = row["IGUBN"].ToString() == "3";
        edtItnbr.Text = row["ITNBR"].ToString();
        dspItdsc.Text = row.Row.Table.Columns.Contains("ITDSC") ? row["ITDSC"].ToString() : "";
        dspDanwi.Text = row["DANWI"].ToString();
        edtIqty.Value = Convert.ToDecimal(row["IQTY"]);
        edtIcost.Value = Convert.ToDecimal(row["IDAN"]);
        edtIamt.Value = Convert.ToDecimal(row["IAMT"]);
        dspBcost.Value = row.Row.Table.Columns.Contains("BCOST") ? Convert.ToDecimal(row["BCOST"]) : 0;
        dspOcost.Value = row.Row.Table.Columns.Contains("OCOST") ? Convert.ToDecimal(row["OCOST"]) : 0;
        edtJamt1.Value = row.Row.Table.Columns.Contains("JAMT1") ? Convert.ToDecimal(row["JAMT1"]) : 0;
        edtJamt2.Value = row.Row.Table.Columns.Contains("JAMT2") ? Convert.ToDecimal(row["JAMT2"]) : 0;
        edtJamt3.Value = row.Row.Table.Columns.Contains("JAMT3") ? Convert.ToDecimal(row["JAMT3"]) : 0;
        if (cboHouse.Items.Count == 0) ResetHouseList();
        var houseIdx = cboHouse.Items.IndexOf((row["HOUSE"].ToString() ?? "").Trim());
        cboHouse.SelectedIndex = cboHouse.Items.Count > 0 ? Math.Max(houseIdx, 0) : -1;
        mskYdate.Text = row["YDATE"].ToString();
        edtBigo.Text = row["IBIGO"].ToString();
        dspOqty.Text = row.Row.Table.Columns.Contains("OQTY") ? PublicLib.MoneyToStr(Convert.ToInt64(row["OQTY"])) : "0";

        pnlJob.Text = "입고수정중";
        dtpDate.Enabled = false;
        edtItnbr.Enabled = false;
        btnAdd.Enabled = false;
    }
}
