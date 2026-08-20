using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA04.pas / JA04.dfm (TfrmJA04) 이식 — 재고관리-품목(월별 입출고 집계, 상/하순 구분).
/// 원본 prcDBopen의 UNION ALL SQL(IPGOF 입고 + SALE_M/D 출고 + IPCHF 보관 + ITEMBL 이월)을 그대로 이식.
/// </summary>
public partial class JA04Form : Form
{
    private static readonly string[] SearchFields = { "A1.ITNBR", "ITDSC", "SUBSTRING(A1.ITNBR,1,4)", "CVNAM" };

    public JA04Form()
    {
        InitializeComponent();
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

    private void BtnExcel_Click(object? sender, EventArgs e)
    {
        pStock.Common.ExcelExporter.Export(this.grid, "재고관리-품목");
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        pStock.Common.GridPrinter.Print(this.grid, "재고관리-품목");
    }

    private void EdtMonth_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) ReloadList();
    }

    private void BtnMonthDown_Click(object? sender, EventArgs e)
    {
        ShiftMonth(-1); ReloadList();
    }

    private void BtnMonthUp_Click(object? sender, EventArgs e)
    {
        ShiftMonth(1); ReloadList();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        this.cSrcd.SelectedIndex = 1; this.eSrwd.Clear(); this.grid.Rows.Clear(); this.eSrwd.Focus();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        ReloadList();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void JA04Form_Load(object? sender, EventArgs e)
    {
        this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM");
        this.cSrcd.SelectedIndex = 1;
    }
}
