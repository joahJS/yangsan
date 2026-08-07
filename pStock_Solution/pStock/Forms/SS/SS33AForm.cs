using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS33A.pas / SS33A.dfm (TfrmSS33A) 이식 — 계산서 자동발행.
/// 기간/거래처코드 범위를 지정하면 해당 기간의 출고(SALE_M/D)+보관료(IPCHF)+입고(IPGOF)
/// 합계를 거래처별로 집계해 계산서(TAXF, TNO1='A' 자동생성분)를 일괄 생성한다.
/// </summary>
public class SS33AForm : Form
{
    private readonly DateTimePicker dtpDate1 = new();  // 집계 시작일
    private readonly DateTimePicker dtpDate2 = new();  // 집계 종료일
    private readonly DateTimePicker dtpDate = new();   // 발행일자
    private readonly TextBox edtCd1 = new();
    private readonly TextBox edtCd2 = new();
    private readonly RadioButton rdo1 = new() { Text = "영수" };
    private readonly RadioButton rdo2 = new() { Text = "청구", Checked = true };

    private readonly Button btnYes = new() { Text = "실행(F2)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public bool Executed { get; private set; }

    public SS33AForm()
    {
        Text = "계산서 자동발행";
        Width = 480;
        Height = 320;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();
        Load += (_, _) => ClearForm();
        KeyDown += SS33AForm_KeyDown;
    }

    private void BuildLayout()
    {
        int y = 20, col1 = 20;
        AddRow("집계기간", dtpDate1, col1, ref y, 130);
        var lblT = new Label { Text = "~", Left = col1 + 300, Top = y - 26 + 3, AutoSize = true };
        dtpDate2.Left = col1 + 320; dtpDate2.Top = y - 26; dtpDate2.Width = 130; dtpDate2.Format = DateTimePickerFormat.Short;
        Controls.AddRange(new Control[] { lblT, dtpDate2 });

        AddRow("발행일자", dtpDate, col1, ref y, 130);

        var lblCd = new Label { Text = "거래처코드 범위", Left = col1, Top = y + 3, AutoSize = true };
        edtCd1.Left = col1 + 150; edtCd1.Top = y; edtCd1.Width = 80;
        var lblCdT = new Label { Text = "~", Left = col1 + 235, Top = y + 3, AutoSize = true };
        edtCd2.Left = col1 + 255; edtCd2.Top = y; edtCd2.Width = 80;
        var btnLookup = new Button { Text = "검색", Left = col1 + 340, Top = y - 2, Width = 60 };
        btnLookup.Click += (_, _) => LookupCvcod();
        Controls.AddRange(new Control[] { lblCd, edtCd1, edtCd2, lblCdT, btnLookup });
        y += 30;

        rdo1.Left = col1 + 150; rdo1.Top = y; rdo1.AutoSize = true;
        rdo2.Left = col1 + 230; rdo2.Top = y; rdo2.AutoSize = true;
        var lblGu = new Label { Text = "발행구분", Left = col1, Top = y + 3, AutoSize = true };
        Controls.AddRange(new Control[] { lblGu, rdo1, rdo2 });
        y += 40;

        var lblHint = new Label
        {
            Text = "선택한 발행일자에 이미 자동생성된(TNO1='A') 계산서가 있으면\r\n" +
                   "해당 거래처코드 범위 내에서 삭제 후 다시 생성합니다.",
            Left = col1, Top = y, AutoSize = true
        };
        y += 50;

        btnYes.Left = 130; btnYes.Top = y; btnYes.Width = 100;
        btnClose.Left = 250; btnClose.Top = y; btnClose.Width = 100;
        Controls.AddRange(new Control[] { lblHint, btnYes, btnClose });

        btnYes.Click += (_, _) => RunBatch();
        btnClose.Click += (_, _) => Close();
    }

    private void AddRow(string caption, Control edit, int left, ref int y, int width)
    {
        var lbl = new Label { Text = caption, Left = left, Top = y + 3, AutoSize = true };
        edit.Left = left + 150;
        edit.Top = y;
        edit.Width = width;
        if (edit is DateTimePicker dtp) dtp.Format = DateTimePickerFormat.Short;
        Controls.Add(lbl);
        Controls.Add(edit);
        y += 30;
    }

    private void SS33AForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2) btnYes.PerformClick();
        else if (e.KeyCode == Keys.Escape) Close();
    }

    /// <summary>원본 prcClear.</summary>
    private void ClearForm()
    {
        var now = DateTime.Now;
        dtpDate1.Value = new DateTime(now.Year, now.Month, 1);
        dtpDate2.Value = now;
        dtpDate.Value = now;
        edtCd1.Clear();
        edtCd2.Text = "9999";
        rdo2.Checked = true;
    }

    /// <summary>원본 Label11Click.</summary>
    private void LookupCvcod()
    {
        using var dlg = new CvcodLookupForm(cvguFilter: "2");
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            edtCd1.Text = dlg.SelectedCode;
            edtCd2.Text = dlg.SelectedCode;
        }
        else
        {
            edtCd1.Clear();
            edtCd2.Text = "9999";
        }
    }

    /// <summary>원본 btnYesClick.</summary>
    private void RunBatch()
    {
        if (MessageBox.Show("계산서를 자동발행하시겠습니까?\r\n(같은 조건으로 이미 만든 자동계산서는 다시 만들어집니다)",
                "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        var tdate = dtpDate.Value.ToString("yyyy-MM-dd");
        var date1 = dtpDate1.Value.ToString("yyyy-MM-dd");
        var date2 = dtpDate2.Value.ToString("yyyy-MM-dd");
        var cd1 = edtCd1.Text.Trim();
        var cd2 = edtCd2.Text.Trim();
        int count = 0;

        try
        {
            AppDb.BeginTransaction();

            using (var delQ = new DbQuery())
            {
                delQ.Add("DELETE FROM TAXF WHERE TDATE=@TDATE AND TNO1='A' AND CVCOD BETWEEN @CD1 AND @CD2");
                delQ.ParamByName("TDATE").AsString = tdate;
                delQ.ParamByName("CD1").AsString = cd1;
                delQ.ParamByName("CD2").AsString = cd2;
                delQ.ExecSQL();
            }

            using var loopQ = new DbQuery();
            loopQ.Add("SELECT CVCOD, SUM(SAMT) SAMT, SUM(JAMT) JAMT FROM");
            loopQ.Add(" (SELECT CVCOD, SUM(TRAMT) SAMT, SUM(JAMT1+JAMT2+JAMT3) JAMT");
            loopQ.Add("    FROM SALE_M A1");
            loopQ.Add("    LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
            loopQ.Add("   WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            loopQ.Add("   GROUP BY CVCOD");
            loopQ.Add("  UNION ALL");
            loopQ.Add("  SELECT CVCOD, SUM(OAMT), SUM(JAMT1+JAMT2+JAMT3)");
            loopQ.Add("    FROM IPCHF");
            loopQ.Add("   WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND GUBN1='1'");
            loopQ.Add("   GROUP BY CVCOD");
            loopQ.Add("  UNION ALL");
            loopQ.Add("  SELECT CVCOD, SUM(IAMT), SUM(JAMT1+JAMT2+JAMT3)");
            loopQ.Add("    FROM IPGOF");
            loopQ.Add("   WHERE IDATE BETWEEN @DATE1 AND @DATE2");
            loopQ.Add("   GROUP BY CVCOD) AA");
            loopQ.Add(" WHERE CVCOD BETWEEN @CD1 AND @CD2");
            loopQ.Add(" GROUP BY CVCOD");
            loopQ.ParamByName("DATE1").AsString = date1;
            loopQ.ParamByName("DATE2").AsString = date2;
            loopQ.ParamByName("CD1").AsString = cd1;
            loopQ.ParamByName("CD2").AsString = cd2;
            loopQ.Open();

            if (!loopQ.IsEmpty)
            {
                loopQ.First();
                while (!loopQ.Eof)
                {
                    var cvcod = loopQ.FieldByName("CVCOD").AsString;
                    var samt = (int)loopQ.FieldByName("SAMT").AsFloat;
                    var jamt = (int)loopQ.FieldByName("JAMT").AsFloat;
                    var tamt = samt + jamt;

                    using var maxQ = new DbQuery();
                    maxQ.Add("SELECT MAX(SEQNO) AS MAXSEQ FROM TAXF WHERE TDATE=@TDATE");
                    maxQ.ParamByName("TDATE").AsString = tdate;
                    maxQ.Open();
                    int seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 1 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                    using var ins = new DbQuery();
                    ins.Add("INSERT INTO TAXF(TDATE,SEQNO,CVCOD,TAMT,TVAT,TGUBN,MDATE,TNO1,");
                    ins.Add("       MMDD1,ITNBR1,ITDSC1,TQTY1,TDANGA1,TAMT1,TVAT1,");
                    ins.Add("       MMDD2,ITNBR2,ITDSC2,TQTY2,TDANGA2,TAMT2,TVAT2,");
                    ins.Add("       MMDD3,ITNBR3,ITDSC3,TQTY3,TDANGA3,TAMT3,TVAT3,");
                    ins.Add("       MMDD4,ITNBR4,ITDSC4,TQTY4,TDANGA4,TAMT4,TVAT4)");
                    ins.Add("    VALUES(@TDATE,@SEQNO,@CVCOD,@TAMT,@TVAT,@TGUBN,@MDATE,'A',");
                    ins.Add("       @MMDD1,@ITNBR1,@ITDSC1,0,0,@AMT1,@VAT1,");
                    ins.Add("       '','',@ITDSC2,0,0,@AMT2,@VAT2,");
                    ins.Add("       '','','',0,0,0,0,");
                    ins.Add("       '','','',0,0,0,0)");
                    ins.ParamByName("TDATE").AsString = tdate;
                    ins.ParamByName("SEQNO").AsInteger = seqNo;
                    ins.ParamByName("CVCOD").AsString = cvcod;
                    ins.ParamByName("TAMT").AsCurrency = tamt;
                    ins.ParamByName("TVAT").AsCurrency = (decimal)Math.Truncate(tamt / 10.0);
                    ins.ParamByName("TGUBN").AsString = rdo1.Checked ? "영수" : "청구";
                    ins.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                    ins.ParamByName("MMDD1").AsString = tdate.Length >= 10 ? tdate.Substring(5, 5) : "";
                    ins.ParamByName("ITNBR1").AsString = "";
                    ins.ParamByName("ITDSC1").AsString = "창고료";
                    ins.ParamByName("AMT1").AsCurrency = samt;
                    ins.ParamByName("VAT1").AsCurrency = (decimal)Math.Truncate(samt / 10.0);
                    if (jamt > 0)
                    {
                        ins.ParamByName("ITDSC2").AsString = "작업료";
                        ins.ParamByName("AMT2").AsCurrency = jamt;
                        ins.ParamByName("VAT2").AsCurrency = (decimal)Math.Truncate(jamt / 10.0);
                    }
                    else
                    {
                        ins.ParamByName("ITDSC2").AsString = "";
                        ins.ParamByName("AMT2").AsCurrency = 0;
                        ins.ParamByName("VAT2").AsCurrency = 0;
                    }
                    ins.ExecSQL();

                    count++;
                    loopQ.Next();
                }
            }

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("계산서 자동입력시 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Executed = true;
        MessageBox.Show($"{count} 건의 계산서가 자동생성되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }
}
