using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA04.pas / JA04.dfm (TfrmJA04) 이식 — 재고관리-품목(월별 입출고 집계, 상/하순 구분).
/// 원본 prcDBopen의 UNION ALL SQL(IPGOF 입고 + SALE_M/D 출고 + IPCHF 보관 + ITEMBL 이월)을 그대로 이식.
/// </summary>
public class JA04Form : Form
{
    private static readonly string[] SearchFields = { "A1.ITNBR", "ITDSC", "SUBSTRING(A1.ITNBR,1,4)", "CVNAM" };

    private readonly FastDataGridView grid = new();
    private readonly TextBox edtMonth = new();
    private readonly ComboBox cSrcd = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox eSrwd = new();

    private readonly Button btnNew = new() { Text = "초기화(F1)" };
    private readonly Button btnSearch = new() { Text = "조회(F5)" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };
    private readonly Button btnMonthDown = new() { Text = "◀" };
    private readonly Button btnMonthUp = new() { Text = "▶" };

    public JA04Form()
    {
        Text = "재고관리-품목";
        Width = 1200;
        Height = 650;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) =>
        {
            edtMonth.Text = DateTime.Now.ToString("yyyy-MM");
            cSrcd.SelectedIndex = 1;
        };
        KeyDown += JA04Form_KeyDown;
    }

    private void BuildLayout()
    {
        cSrcd.Items.AddRange(new object[] { "품번", "품명", "거래처코드", "거래처명" });

        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnSearch, btnExcel, btnPrint, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnSearch, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(grid, "재고관리-품목");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(grid, "재고관리-품목");

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblMonth = new Label { Text = "조회월(YYYY-MM)", Left = 10, Top = 12, AutoSize = true };
        edtMonth.Left = 140; edtMonth.Top = 8; edtMonth.Width = 80;
        edtMonth.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) ReloadList(); };
        btnMonthDown.Left = 225; btnMonthDown.Top = 8; btnMonthDown.Width = 30;
        btnMonthUp.Left = 258; btnMonthUp.Top = 8; btnMonthUp.Width = 30;
        btnMonthDown.Click += (_, _) => { ShiftMonth(-1); ReloadList(); };
        btnMonthUp.Click += (_, _) => { ShiftMonth(1); ReloadList(); };

        var lblSearch = new Label { Text = "검색조건", Left = 320, Top = 12, AutoSize = true };
        cSrcd.Left = 390; cSrcd.Top = 8; cSrcd.Width = 100;
        eSrwd.Left = 500; eSrwd.Top = 8; eSrwd.Width = 200;

        editPanel.Controls.AddRange(new Control[]
        {
            lblMonth, edtMonth, btnMonthDown, btnMonthUp, lblSearch, cSrcd, eSrwd
        });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.Columns.Add("ITDSC", "품명");
        grid.Columns.Add("ITNBR", "품번");
        grid.Columns.Add("BQTY", "기초");
        grid.Columns.Add("IQT1", "입고(상순)");
        grid.Columns.Add("IQT2", "입고(하순)");
        grid.Columns.Add("OQT1", "출고(상순)");
        grid.Columns.Add("OQT2", "출고(하순)");
        grid.Columns.Add("JQTY1", "재고(상순)");
        grid.Columns.Add("JQTY2", "재고(하순)");
        grid.Columns.Add("IAMT", "입고금액");
        grid.Columns.Add("OAMT", "보관금액");
        grid.Columns.Add("JAM1", "출고금액1");
        grid.Columns.Add("JAM2", "출고금액2");
        grid.Columns.Add("TAMT", "합계금액");

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => { cSrcd.SelectedIndex = 1; eSrwd.Clear(); grid.Rows.Clear(); eSrwd.Focus(); };
        btnSearch.Click += (_, _) => ReloadList();
        btnClose.Click += (_, _) => Close();
    }

    private void JA04Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void ShiftMonth(int delta)
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var d))
            d = DateTime.Now;
        edtMonth.Text = d.AddMonths(delta).ToString("yyyy-MM");
    }

    /// <summary>원본 prcDBopen.</summary>
    private void ReloadList()
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var monthDate))
        {
            MessageBox.Show("조회월 형식이 올바르지 않습니다 (YYYY-MM).", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int mm = monthDate.Month;
        var carryTerms = new List<string> { "BBALQ" };
        for (int i = 1; i < mm; i++)
        {
            var m = i.ToString("00");
            carryTerms.Add($"+I1QT{m}-O1QT{m}-IOQT{m}");
        }
        var carryExpr = string.Concat(carryTerms);

        var where = string.IsNullOrWhiteSpace(eSrwd.Text)
            ? string.Empty
            : $" WHERE {SearchFields[cSrcd.SelectedIndex]} LIKE '%{eSrwd.Text.Trim()}%'";

        using var q = new DbQuery();
        q.Add("SELECT A1.ITNBR, ITDSC, ISPEC, DANWI, SUM(BQTY) BQTY,");
        q.Add("       SUM(IQTY1) IQT1, SUM(IQTY2) IQT2,");
        q.Add("       SUM(OQTY1) OQT1, SUM(OQTY2) OQT2,");
        q.Add("       SUM(IAMT) IAMT, SUM(OAMT) OAMT,");
        q.Add("       SUM(JAMT1) JAM1, SUM(JAMT2) JAM2");
        q.Add("  FROM");
        q.Add(" (SELECT ITNBR, 0 BQTY,");
        q.Add("         SUM(CASE WHEN SUBSTRING(IDATE,9,2) BETWEEN '01' AND '15' THEN IQTY ELSE 0 END) IQTY1,");
        q.Add("         SUM(CASE WHEN SUBSTRING(IDATE,9,2) BETWEEN '16' AND '31' THEN IQTY ELSE 0 END) IQTY2,");
        q.Add("         SUM(IAMT+JAMT1+JAMT2+JAMT3) IAMT,");
        q.Add("         0 OQTY1, 0 OQTY2, 0 OAMT, 0 JAMT1, 0 JAMT2");
        q.Add("    FROM IPGOF");
        q.Add("   WHERE IDATE LIKE @YYMM");
        q.Add("   GROUP BY ITNBR");
        q.Add("  UNION ALL");
        q.Add("  SELECT ITCOD, 0, 0,0,0,");
        q.Add("         SUM(CASE WHEN SUBSTRING(TDATE,9,2) BETWEEN '01' AND '15' THEN TRQTY ELSE 0 END) OQTY1,");
        q.Add("         SUM(CASE WHEN SUBSTRING(TDATE,9,2) BETWEEN '16' AND '31' THEN TRQTY ELSE 0 END) OQTY2,");
        q.Add("         SUM(TRAMT+JAMT1+JAMT2+JAMT3) OAMT,0,0");
        q.Add("    FROM SALE_M A1");
        q.Add("    LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add("   WHERE TDATE LIKE @YYMM");
        q.Add("   GROUP BY ITCOD");
        q.Add("  UNION ALL");
        q.Add("  SELECT ITNBR, 0, 0,0,SUM(CASE WHEN (TPRO='0') THEN OAMT+JAMT1+JAMT2+JAMT3 ELSE 0 END),");
        q.Add("         0,0,0,");
        q.Add("         SUM(CASE WHEN SUBSTRING(TDATE,9,2) BETWEEN '01' AND '15' THEN OAMT ELSE 0 END),");
        q.Add("         SUM(CASE WHEN SUBSTRING(TDATE,9,2) BETWEEN '16' AND '31' THEN OAMT ELSE 0 END)");
        q.Add("    FROM IPCHF");
        q.Add("   WHERE TDATE LIKE @YYMM");
        q.Add("     AND GUBN1='1'");
        q.Add("   GROUP BY ITNBR");
        q.Add("  UNION ALL");
        q.Add($"  SELECT ITNBR, SUM({carryExpr}) BQTY, 0,0,0, 0,0,0, 0,0");
        q.Add("    FROM ITEMBL");
        q.Add("   WHERE IYEAR = SUBSTRING(@YYMM, 1, 4)");
        q.Add("   GROUP BY ITNBR) A1");
        q.Add("  LEFT OUTER JOIN ITEMAS B1 ON A1.ITNBR=B1.ITNBR");
        q.Add("  LEFT OUTER JOIN CVMAST B2 ON SUBSTRING(A1.ITNBR,1,4)=B2.CVCOD" + where);
        q.Add(" GROUP BY A1.ITNBR, ITDSC, ISPEC, DANWI");
        q.Add(" ORDER BY ITDSC");
        q.ParamByName("YYMM").AsString = edtMonth.Text.Trim() + "%";
        q.Open();

        grid.Rows.Clear();
        if (q.IsEmpty) return;

        q.First();
        while (!q.Eof)
        {
            double bqty = q.FieldByName("BQTY").AsFloat;
            double iqt1 = q.FieldByName("IQT1").AsFloat;
            double iqt2 = q.FieldByName("IQT2").AsFloat;
            double oqt1 = q.FieldByName("OQT1").AsFloat;
            double oqt2 = q.FieldByName("OQT2").AsFloat;
            double iamt = q.FieldByName("IAMT").AsFloat;
            double oamt = q.FieldByName("OAMT").AsFloat;
            double jam1 = q.FieldByName("JAM1").AsFloat;
            double jam2 = q.FieldByName("JAM2").AsFloat;

            if (bqty + iqt1 + iqt2 + oqt1 + oqt2 != 0)
            {
                double jqty1 = bqty + iqt1 - oqt1;
                double jqty2 = jqty1 + iqt2 - oqt2;

                grid.Rows.Add(
                    q.FieldByName("ITDSC").AsString,
                    q.FieldByName("ITNBR").AsString,
                    PublicLib.MoneyToStr((long)bqty),
                    PublicLib.MoneyToStr((long)iqt1),
                    PublicLib.MoneyToStr((long)iqt2),
                    PublicLib.MoneyToStr((long)oqt1),
                    PublicLib.MoneyToStr((long)oqt2),
                    PublicLib.MoneyToStr((long)jqty1),
                    PublicLib.MoneyToStr((long)jqty2),
                    PublicLib.MoneyToStr((long)iamt),
                    PublicLib.MoneyToStr((long)oamt),
                    PublicLib.MoneyToStr((long)jam1),
                    PublicLib.MoneyToStr((long)jam2),
                    PublicLib.MoneyToStr((long)(iamt + oamt + jam1 + jam2)));
            }
            q.Next();
        }
    }
}
