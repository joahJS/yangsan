using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS34.pas / SS34.dfm (TfrmSS34) 이식 — 미수금 조회(MISUF 월별 누계).
/// 원본 DFM 확인 SQL: SELECT A.*,CVNAM,OWNAM,TELNO FROM MISUF A
///                     LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD WHERE MYEAR=:YEAR
/// </summary>
public partial class SS34Form : Form
{
    private const string BaseSql =
        "SELECT A.*,CVNAM,OWNAM,TELNO FROM MISUF A" +
        "  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD" +
        " WHERE MYEAR=@YEAR";

    public SS34Form()
    {
        InitializeComponent();
    }

    private void SS34Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void BtnExcel_Click(object? sender, EventArgs e) => pStock.Common.ExcelExporter.Export(this.grid, "미수금조회");
    private void BtnPrint_Click(object? sender, EventArgs e) => pStock.Common.GridPrinter.Print(this.grid, "미수금조회");

    private void EdtMonth_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) this.Search();
    }

    private void BtnMonthDown_Click(object? sender, EventArgs e)
    {
        this.ShiftMonth(-1); this.Search();
    }

    private void BtnMonthUp_Click(object? sender, EventArgs e)
    {
        this.ShiftMonth(1); this.Search();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        this.edtCvcod.Clear(); this.dspName.Clear(); this.edtCvcod.Focus();
    }

    private void BtnSearch_Click(object? sender, EventArgs e) => this.Search();
    private void BtnClose_Click(object? sender, EventArgs e) => this.Close();

    private void SS34Form_Load(object? sender, EventArgs e)
    {
        this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); this.Search();
    }

    private void ShiftMonth(int delta)
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var d))
            d = DateTime.Now;
        edtMonth.Text = d.AddMonths(delta).ToString("yyyy-MM");
    }

    /// <summary>원본 edtCvcodKeyDown.</summary>
    private void EdtCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            using var dlg = new Common.CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCvcod.Text = dlg.SelectedCode;
                dspName.Text = dlg.SelectedName;
            }
            else
            {
                dspName.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD AND CVGU IN('2','3')");
        q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtCvcod.Text = q.FieldByName("CVCOD").AsString;
            dspName.Text = q.FieldByName("CVNAM").AsString;
        }
        Search();
    }

    /// <summary>원본 prcListDisplay.</summary>
    private void Search()
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var monthDate))
        {
            MessageBox.Show("조회월 형식이 올바르지 않습니다 (YYYY-MM).", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        int month = monthDate.Month;
        var monthStr = month.ToString("00");

        var terms = new List<string>();
        for (int i = 1; i < month; i++)
        {
            var m = i.ToString("00");
            terms.Add($"+OAMT{m}+OVAT{m}-SAMT{m}");
        }
        var extraWhere = $"AND ((BAMT{string.Concat(terms)})<>0 OR " +
            $"(OAMT{monthStr}<>0 OR OVAT{monthStr}<>0 OR SAMT{monthStr}<>0))";

        using var q = new DbQuery();
        q.Add(BaseSql);
        q.Add(extraWhere);
        q.ParamByName("YEAR").AsString = monthDate.Year.ToString();
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            q.Add("AND CVCOD = @FCVCOD");
            q.ParamByName("FCVCOD").AsString = edtCvcod.Text.Trim();
        }
        q.Add("ORDER BY CVNAM");
        q.Open();

        var table = q.Table ?? new DataTable();
        AddCalculatedColumns(table, month);
        grid.DataSource = table;
        ApplyGridHeaders();

        // 원본: 조회월까지 누계 미수잔액 합계 표시
        var sumTerms = new List<string> { "BAMT" };
        for (int i = 1; i <= month; i++)
        {
            var m = i.ToString("00");
            sumTerms.Add($"+OAMT{m}+OVAT{m}-SAMT{m}");
        }
        using var q2 = new DbQuery();
        q2.Add($"SELECT SUM({string.Concat(sumTerms)}) AS TOTAL FROM MISUF WHERE MYEAR=@YEAR");
        q2.ParamByName("YEAR").AsString = monthDate.Year.ToString();
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            q2.Add("AND CVCOD = @FCVCOD");
            q2.ParamByName("FCVCOD").AsString = edtCvcod.Text.Trim();
        }
        q2.Open();
        lblAmt.Text = q2.IsEmpty || q2.FieldByName("TOTAL").IsNull
            ? "0" : PublicLib.MoneyToStr((long)q2.FieldByName("TOTAL").AsFloat);
    }

    /// <summary>원본 qryListCalcFields.</summary>
    private static void AddCalculatedColumns(DataTable table, int month)
    {
        table.Columns.Add("BBAMT", typeof(double));
        table.Columns.Add("IAMT", typeof(double));
        table.Columns.Add("OAMT_", typeof(double));
        table.Columns.Add("JAMT_", typeof(double));

        foreach (DataRow row in table.Rows)
        {
            double amt = ToDbl(row["BAMT"]);
            for (int i = 1; i < month; i++)
            {
                var m = i.ToString("00");
                amt += ToDbl(row[$"OAMT{m}"]) + ToDbl(row[$"OVAT{m}"]) - ToDbl(row[$"SAMT{m}"]);
            }
            var mm = month.ToString("00");
            double iamt = ToDbl(row[$"OAMT{mm}"]) + ToDbl(row[$"OVAT{mm}"]);
            double oamt = ToDbl(row[$"SAMT{mm}"]);

            row["BBAMT"] = amt;
            row["IAMT"] = iamt;
            row["OAMT_"] = oamt;
            row["JAMT_"] = amt + iamt - oamt;
        }
    }

    private static double ToDbl(object v) => v == null || v == DBNull.Value ? 0 : Convert.ToDouble(v);

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명", ["OWNAM"] = "대표자", ["TELNO"] = "전화번호",
            ["BBAMT"] = "이월잔액", ["IAMT"] = "당월발생", ["OAMT_"] = "당월수금", ["JAMT_"] = "미수잔액",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
        {
            if (col.Name.StartsWith("OAMT0") || col.Name.StartsWith("OAMT1") ||
                col.Name.StartsWith("OVAT") || col.Name.StartsWith("SAMT") ||
                col.Name is "BAMT" or "MYEAR" or "MDATE")
                col.Visible = false;
        }
    }
}
