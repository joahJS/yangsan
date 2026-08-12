using pStock.Data;
using pStock.Common;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA05.pas / JA05.dfm (TfrmJA05) 이식 — 운송현황(SALE_M 기준 배송처/금액 조회).
/// </summary>
public partial class JA05Form : Form
{
    private static readonly string[] SearchFields = { "A.CVCOD", "CVNAM" };

    public JA05Form()
    {
        InitializeComponent();
    }

    private void JA05Form_KeyDown(object? sender, KeyEventArgs e)
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
        var where = $" AND {SearchFields[cSrcd.SelectedIndex]} LIKE '%{eSrwd.Text.Trim()}%'";

        using var q = new DbQuery();
        q.Add("SELECT SALNO, TDATE, DATENAME(dw,tdate) AS TMONTH1, CVNAM, ISNULL(LNNAM,'') LNNAM");
        q.Add("  ,ADDR1, SSAMT FROM SALE_M A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add("  LEFT OUTER JOIN REACH C ON A.LNCOD=C.LNCOD");
        q.Add("  WHERE SUBSTRING(TDATE,1,7) LIKE @TDATE" + where);
        if (chkOnlyNonZero.Checked) q.Add("  AND SSAMT <> 0");
        q.ParamByName("TDATE").AsString = edtMonth.Text.Trim() + "%";
        q.Open();

        grid.Rows.Clear();
        double sum = 0;
        if (!q.IsEmpty)
        {
            q.First();
            while (!q.Eof)
            {
                var amt = q.FieldByName("SSAMT").AsFloat;
                sum += amt;
                grid.Rows.Add(
                    q.FieldByName("SALNO").AsString,
                    q.FieldByName("TDATE").AsString,
                    q.FieldByName("CVNAM").AsString,
                    q.FieldByName("LNNAM").AsString,
                    q.FieldByName("ADDR1").AsString,
                    amt.ToString("#,0"));
                q.Next();
            }
        }
        lblSum.Text = "합계금액: " + sum.ToString("#,0") + " 원";
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
        this.cSrcd.SelectedIndex = 1; this.eSrwd.Clear(); this.grid.Rows.Clear(); this.lblSum.Text = string.Empty; this.eSrwd.Focus();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        ReloadList();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void JA05Form_Load(object? sender, EventArgs e)
    {
        this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); this.cSrcd.SelectedIndex = 1;
    }
}
