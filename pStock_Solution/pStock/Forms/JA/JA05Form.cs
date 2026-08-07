using pStock.Data;
using pStock.Common;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA05.pas / JA05.dfm (TfrmJA05) 이식 — 운송현황(SALE_M 기준 배송처/금액 조회).
/// </summary>
public class JA05Form : Form
{
    private static readonly string[] SearchFields = { "A.CVCOD", "CVNAM" };

    private readonly FastDataGridView grid = new();
    private readonly TextBox edtMonth = new();
    private readonly ComboBox cSrcd = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox eSrwd = new();
    private readonly CheckBox chkOnlyNonZero = new() { Text = "금액 0 제외" };

    private readonly Button btnNew = new() { Text = "초기화(F1)" };
    private readonly Button btnSearch = new() { Text = "조회(F5)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };
    private readonly Button btnMonthDown = new() { Text = "◀" };
    private readonly Button btnMonthUp = new() { Text = "▶" };
    private readonly Label lblSum = new() { AutoSize = true };

    public JA05Form()
    {
        Text = "운송현황";
        Width = 1000;
        Height = 600;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); cSrcd.SelectedIndex = 1; };
        KeyDown += JA05Form_KeyDown;
    }

    private void BuildLayout()
    {
        cSrcd.Items.AddRange(new object[] { "거래처코드", "거래처명" });

        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnSearch, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnSearch, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }

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
        eSrwd.Left = 500; eSrwd.Top = 8; eSrwd.Width = 180;
        chkOnlyNonZero.Left = 700; chkOnlyNonZero.Top = 10; chkOnlyNonZero.AutoSize = true;

        editPanel.Controls.AddRange(new Control[]
        {
            lblMonth, edtMonth, btnMonthDown, btnMonthUp, lblSearch, cSrcd, eSrwd, chkOnlyNonZero
        });

        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 30 };
        lblSum.Left = 10; lblSum.Top = 6;
        bottom.Controls.Add(lblSum);

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.Columns.Add("SALNO", "전표번호");
        grid.Columns.Add("TDATE", "일자");
        grid.Columns.Add("CVNAM", "거래처명");
        grid.Columns.Add("LNNAM", "착지처명");
        grid.Columns.Add("ADDR1", "주소");
        grid.Columns.Add("SSAMT", "금액");

        Controls.Add(grid);
        Controls.Add(bottom);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => { cSrcd.SelectedIndex = 1; eSrwd.Clear(); grid.Rows.Clear(); lblSum.Text = string.Empty; eSrwd.Focus(); };
        btnSearch.Click += (_, _) => ReloadList();
        btnClose.Click += (_, _) => Close();
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
}
