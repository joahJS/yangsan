using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS36.pas / SS36.dfm (TfrmSS36) 이식 — 품목원장 조회(품목 1건의 입고/출고/보관 이력 + 재고 잔량).
/// 품목 검색 팝업(BA00D)은 아직 변환되지 않아 품번 직접입력만 지원한다.
/// </summary>
public class SS36Form : Form
{
    private readonly DateTimePicker dtpDate1 = new();
    private readonly DateTimePicker dtpDate2 = new();
    private readonly TextBox edtCode = new();
    private readonly TextBox dspName = new() { ReadOnly = true };
    private readonly TextBox dspDanwi = new() { ReadOnly = true };
    private readonly Label lblCvnam = new() { AutoSize = true };
    private readonly FastDataGridView grid = new();

    private readonly Button btnNew = new() { Text = "초기화(F1)" };
    private readonly Button btnSearch = new() { Text = "조회(F5)" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public SS36Form()
    {
        Text = "품목원장 조회";
        Width = 1100;
        Height = 650;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) =>
        {
            var now = DateTime.Now;
            dtpDate1.Value = new DateTime(now.Year, now.Month, 1);
            dtpDate2.Value = now;
        };
        KeyDown += SS36Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnSearch, btnExcel, btnPrint, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnSearch, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(grid, "품목원장조회");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(grid, "품목원장조회");

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        dtpDate1.Left = 50; dtpDate1.Top = 8; dtpDate1.Width = 110; dtpDate1.Format = DateTimePickerFormat.Short;
        var lblTilde = new Label { Text = "~", Left = 165, Top = 12, AutoSize = true };
        dtpDate2.Left = 180; dtpDate2.Top = 8; dtpDate2.Width = 110; dtpDate2.Format = DateTimePickerFormat.Short;
        var lblCode = new Label { Text = "품번", Left = 310, Top = 12, AutoSize = true };
        edtCode.Left = 350; edtCode.Top = 8; edtCode.Width = 100;
        edtCode.KeyDown += EdtCode_KeyDown;
        dspName.Left = 460; dspName.Top = 8; dspName.Width = 200;
        dspDanwi.Left = 670; dspDanwi.Top = 8; dspDanwi.Width = 60;
        lblCvnam.Left = 740; lblCvnam.Top = 12;

        editPanel.Controls.AddRange(new Control[]
        {
            lblDate, dtpDate1, lblTilde, dtpDate2, lblCode, edtCode, dspName, dspDanwi, lblCvnam
        });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.Columns.Add("DATE", "일자");
        grid.Columns.Add("GUBN", "구분");
        grid.Columns.Add("IQTY", "입고(수량/금액)");
        grid.Columns.Add("OQTY", "출고(수량/금액)");
        grid.Columns.Add("BALANCE", "재고잔량");
        grid.Columns.Add("BQTY", "보관(수량/금액)");
        grid.Columns.Add("BIGO", "비고");

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => { edtCode.Clear(); dspName.Clear(); dspDanwi.Clear(); lblCvnam.Text = ""; grid.Rows.Clear(); edtCode.Focus(); };
        btnSearch.Click += (_, _) => Search();
        btnClose.Click += (_, _) => Close();
    }

    private void SS36Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 edtCodeKeyDown.</summary>
    private void EdtCode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCode.Text))
        {
            using var dlg = new Common.ItemLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCode.Text = dlg.SelectedItnbr;
                dspName.Text = $"{dlg.SelectedItdsc} {dlg.SelectedIspec}".Trim();
                dspDanwi.Text = dlg.SelectedDanwi;
            }
            else
            {
                dspName.Clear();
                dspDanwi.Clear();
            }
            LoadCvnam();
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM ITEMAS WHERE ITNBR=@ITNBR");
        q.ParamByName("ITNBR").AsString = edtCode.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtCode.Text = q.FieldByName("ITNBR").AsString;
            dspName.Text = $"{q.FieldByName("ITDSC").AsString} {q.FieldByName("ISPEC").AsString}".Trim();
            dspDanwi.Text = q.FieldByName("DANWI").AsString;
        }
        LoadCvnam();
    }

    private void LoadCvnam()
    {
        var cv = edtCode.Text.Length >= 4 ? edtCode.Text[..4] : edtCode.Text;
        using var q = new DbQuery();
        q.Add($"SELECT * FROM CVMAST WHERE CVCOD='{cv}'");
        q.Open();
        lblCvnam.Text = q.IsEmpty ? "" : q.FieldByName("CVNAM").AsString;
    }

    /// <summary>원본 prcPriorAmtGet: 조회 시작월 이전 이월 재고수량.</summary>
    private double GetPriorQty()
    {
        var year = dtpDate1.Value.Year;
        var month = dtpDate1.Value.Month;

        using var q = new DbQuery();
        q.Add("SELECT SUM(I1QT01) I01,SUM(I1QT02) I02,SUM(I1QT03) I03,SUM(I1QT04) I04,");
        q.Add("       SUM(I1QT05) I05,SUM(I1QT06) I06,SUM(I1QT07) I07,SUM(I1QT08) I08,");
        q.Add("       SUM(I1QT09) I09,SUM(I1QT10) I10,SUM(I1QT11) I11,SUM(I1QT12) I12,");
        q.Add("       SUM(O1QT01+IOQT01) O01,SUM(O1QT02+IOQT02) O02,");
        q.Add("       SUM(O1QT03+IOQT03) O03,SUM(O1QT04+IOQT04) O04,");
        q.Add("       SUM(O1QT05+IOQT05) O05,SUM(O1QT06+IOQT06) O06,");
        q.Add("       SUM(O1QT07+IOQT07) O07,SUM(O1QT08+IOQT08) O08,");
        q.Add("       SUM(O1QT09+IOQT09) O09,SUM(O1QT10+IOQT10) O10,");
        q.Add("       SUM(O1QT11+IOQT11) O11,SUM(O1QT12+IOQT12) O12,");
        q.Add("       SUM(BBALQ) BQTY");
        q.Add("  FROM ITEMBL");
        q.Add(" WHERE IYEAR = @YEAR");
        if (!string.IsNullOrWhiteSpace(edtCode.Text)) q.Add("AND ITNBR=@ITNBR");
        q.ParamByName("YEAR").AsString = year.ToString();
        if (!string.IsNullOrWhiteSpace(edtCode.Text)) q.ParamByName("ITNBR").AsString = edtCode.Text.Trim();
        q.Open();

        if (q.IsEmpty) return 0;
        double qty = q.FieldByName("BQTY").AsFloat;
        for (int i = 1; i < month; i++)
        {
            var idx = i.ToString("00");
            qty += q.FieldByName($"I{idx}").AsFloat - q.FieldByName($"O{idx}").AsFloat;
        }

        if (dtpDate1.Value.Day != 1)
        {
            var itnbrFilter = string.IsNullOrWhiteSpace(edtCode.Text) ? "" : $" WHERE ITNBR='{edtCode.Text.Trim()}'";
            using var q2 = new DbQuery();
            q2.Add("SELECT SUM(IOQTY) AS TOTAL FROM (");
            q2.Add("SELECT ITNBR, SUM(IQTY) IOQTY FROM IPGOF");
            q2.Add(" WHERE IDATE BETWEEN @DATE1 AND @DATE2");
            q2.Add(" GROUP BY ITNBR");
            q2.Add(" UNION");
            q2.Add("SELECT ITCOD, SUM(TRQTY)*-1 FROM SALE_M A1");
            q2.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
            q2.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            q2.Add(" GROUP BY ITCOD) AA" + itnbrFilter);
            q2.ParamByName("DATE1").AsString = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
            q2.ParamByName("DATE2").AsString = dtpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd");
            q2.Open();
            qty += q2.IsEmpty || q2.FieldByName("TOTAL").IsNull ? 0 : q2.FieldByName("TOTAL").AsFloat;
        }

        return qty;
    }

    /// <summary>원본 prcItemDisplay.</summary>
    private void Search()
    {
        grid.Rows.Clear();
        double running = GetPriorQty();
        grid.Rows.Add("", "이월", "", "", PublicLib.MoneyToStr((long)running), "", "");

        using var q = new DbQuery();
        q.Add("SELECT AA.*");
        q.Add("  FROM");
        q.Add(" (SELECT IDATE, CVCOD, ITNBR, 'I' GUBN, ISEQ, IQTY, IAMT, IBIGO");
        q.Add("    FROM IPGOF");
        q.Add("   WHERE IDATE BETWEEN @FDATE AND @TDATE");
        q.Add("  UNION ALL");
        q.Add("  SELECT TDATE, CVCOD, ITNBR, '1',SEQNO, IOQTY, OAMT, TBIGO");
        q.Add("    FROM IPCHF");
        q.Add("   WHERE TDATE BETWEEN @FDATE AND @TDATE AND GUBN1='1'");
        q.Add("  UNION ALL");
        q.Add("  SELECT TDATE, CVCOD, ITCOD, '2', SEQNO, TRQTY, TRAMT, DBIGO");
        q.Add("    FROM SALE_M A1");
        q.Add("    LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add("   WHERE TDATE BETWEEN @FDATE AND @TDATE )AA");
        if (!string.IsNullOrWhiteSpace(edtCode.Text)) q.Add(" WHERE ITNBR = @ITNBR");
        q.Add(" ORDER BY IDATE, CVCOD, ITNBR, GUBN");
        q.ParamByName("FDATE").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("TDATE").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        if (!string.IsNullOrWhiteSpace(edtCode.Text)) q.ParamByName("ITNBR").AsString = edtCode.Text.Trim();
        q.Open();

        double iQty = 0, iAmt = 0, oQty = 0, oAmt = 0, bQty = 0, bAmt = 0;
        if (!q.IsEmpty)
        {
            q.First();
            while (!q.Eof)
            {
                var gubn = q.FieldByName("GUBN").AsString;
                var qty = q.FieldByName("IQTY").AsFloat;
                var amt = q.FieldByName("IAMT").AsFloat;
                var date = q.FieldByName("IDATE").AsString;
                var bigo = q.FieldByName("IBIGO").AsString;

                string iCol = "", oCol = "", bCol = "", gubnLabel;
                if (gubn == "I")
                {
                    gubnLabel = "입고"; iCol = $"{PublicLib.MoneyToStr((long)qty)} / {PublicLib.MoneyToStr((long)amt)}";
                    running += qty; iQty += qty; iAmt += amt;
                }
                else if (gubn == "2")
                {
                    gubnLabel = "출고"; oCol = $"{PublicLib.MoneyToStr((long)qty)} / {PublicLib.MoneyToStr((long)amt)}";
                    running -= qty; oQty += qty; oAmt += amt;
                }
                else
                {
                    gubnLabel = "보관"; bCol = $"{PublicLib.MoneyToStr((long)qty)} / {PublicLib.MoneyToStr((long)amt)}";
                    bQty += qty; bAmt += amt;
                }

                grid.Rows.Add(date, gubnLabel, iCol, oCol, PublicLib.MoneyToStr((long)running), bCol, bigo);
                q.Next();
            }
        }

        grid.Rows.Add("", "합  계",
            $"{PublicLib.MoneyToStr((long)iQty)} / {PublicLib.MoneyToStr((long)iAmt)}",
            $"{PublicLib.MoneyToStr((long)oQty)} / {PublicLib.MoneyToStr((long)oAmt)}",
            "", $"{PublicLib.MoneyToStr((long)bQty)} / {PublicLib.MoneyToStr((long)bAmt)}", "");
    }
}
