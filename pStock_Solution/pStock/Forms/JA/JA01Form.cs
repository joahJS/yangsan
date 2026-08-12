using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA01.pas / JA01.dfm (TfrmJA01) 이식 — 재고관리(ITEMBL 월별 수불부 조회).
/// ITEMBL 테이블은 품목별로 월별 입고(I1QT01~12)/출고(O1QT01~12)/조정(IOQT01~12) 수량과
/// 기초잔량(BBALQ)을 한 행에 저장하는 구조. 선택한 월 기준으로
/// 기초(BQTY)/입고(IQTY)/출고(OQTY)/조정(XQTY)/재고(JQTY)를 계산해서 보여준다.
///
/// 탭 구성(원본 PageControl1): 1=품목별, 2=거래처별, 3=저장위치별.
/// 원본 4번째 탭(Drum, qryDrum 별도 쿼리)은 DFM에서 SQL을 확인하지 못해 이번 단계에서는
/// 3번째 탭과 동일한 조회로 대체해 두었다 — 원본 SQL을 확인되는 대로 갱신 필요.
/// </summary>
public partial class JA01Form : Form
{
    private static readonly string BaseSql = pStock.Common.SqlFragments.ItemblBaseSql;

    public JA01Form()
    {
        InitializeComponent();
    }

    private void JA01Form_KeyDown(object? sender, KeyEventArgs e)
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
        d = d.AddMonths(delta);
        edtMonth.Text = d.ToString("yyyy-MM");
    }

    /// <summary>원본 prcHouseReset.</summary>
    private void ResetHouseList()
    {
        cboHouse.Items.Clear();
        cboHouse.Items.Add(" ");
        foreach (var (refno, _) in ReffpfCache.Get("CG")) cboHouse.Items.Add(refno);
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

        int month = monthDate.Month;
        string year = monthDate.Year.ToString();

        // 원본: BBALQ + 1~(월-1)월 누계가 0이 아니거나, 해당월 입/출/조정이 0이 아닌 행만 표시
        var monthStr = month.ToString("00");
        var terms = new List<string>();
        for (int i = 1; i < month; i++)
        {
            var m = i.ToString("00");
            terms.Add($"+I1QT{m}-O1QT{m}-IOQT{m}");
        }
        var cumulativeExpr = "BBALQ" + string.Concat(terms);
        var extraWhere = $" AND (({cumulativeExpr}<>0) OR (I1QT{monthStr}<>0 OR O1QT{monthStr}<>0 OR IOQT{monthStr}<>0))";

        using var q = new DbQuery();
        q.Add(BaseSql);
        q.Add(extraWhere);
        q.ParamByName("YEAR").AsString = year;

        if (tabs.SelectedTab == tab1)
        {
            q.Add($"AND ITDSC LIKE '{edtNo.Text.Trim()}%'");
            q.Add("ORDER BY ITDSC,ISPEC,HOUSE");
        }
        else if (tabs.SelectedTab == tab2)
        {
            q.Add($"AND CVNAM LIKE '{edtCvnam.Text.Trim()}%'");
            q.Add("ORDER BY CVNAM,ITDSC,ISPEC,HOUSE");
        }
        else
        {
            q.Add($"AND HOUSE LIKE '{cboHouse.Text.Trim()}%'");
            q.Add("ORDER BY HOUSE,ITDSC,A.ITNBR");
        }

        q.Open();
        var table = q.Table ?? new DataTable();
        AddCalculatedColumns(table, month);
        grid.DataSource = table;
        ApplyGridHeaders();
    }

    /// <summary>원본 qryListCalcFields: 월 기준 기초/입고/출고/조정/재고 계산.</summary>
    private static void AddCalculatedColumns(DataTable table, int month)
    {
        table.Columns.Add("BQTY", typeof(double));
        table.Columns.Add("IQTY", typeof(double));
        table.Columns.Add("OQTY", typeof(double));
        table.Columns.Add("XQTY", typeof(double));
        table.Columns.Add("JQTY", typeof(double));

        foreach (DataRow row in table.Rows)
        {
            double bqty = ToDouble(row["BBALQ"]);
            for (int i = 1; i < month; i++)
            {
                var m = i.ToString("00");
                bqty += ToDouble(row[$"I1QT{m}"]) - ToDouble(row[$"O1QT{m}"]) - ToDouble(row[$"IOQT{m}"]);
            }
            var mm = month.ToString("00");
            double iqty = ToDouble(row[$"I1QT{mm}"]);
            double oqty = ToDouble(row[$"O1QT{mm}"]);
            double xqty = ToDouble(row[$"IOQT{mm}"]);

            row["BQTY"] = bqty;
            row["IQTY"] = iqty;
            row["OQTY"] = oqty;
            row["XQTY"] = xqty;
            row["JQTY"] = bqty + iqty - oqty - xqty;
        }
    }

    private static double ToDouble(object v) => v == null || v == DBNull.Value ? 0 : Convert.ToDouble(v);

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["ITNBR"] = "품번", ["ITDSC"] = "품명", ["ISPEC"] = "규격", ["DANWI"] = "단위",
            ["HOUSE"] = "저장위치", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["ICOST"] = "입고단가", ["BCOST"] = "기준단가", ["OCOST"] = "출고단가",
            ["BQTY"] = "기초", ["IQTY"] = "입고", ["OQTY"] = "출고", ["XQTY"] = "조정", ["JQTY"] = "재고",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        // 원본 화면은 월별 원본 수량 컬럼(I1QT01..)과 내부 집계용 컬럼은 숨기고 계산된 요약만 노출한다.
        foreach (DataGridViewColumn col in grid.Columns)
        {
            if (col.Name.StartsWith("I1QT") || col.Name.StartsWith("O1QT") || col.Name.StartsWith("IOQT") ||
                col.Name is "IYEAR" or "BBALQ" or "CODNAM")
                col.Visible = false;
        }
    }

    private void LocateInGrid(string field, string word)
    {
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.Cells[field]?.Value?.ToString()?.StartsWith(word, StringComparison.OrdinalIgnoreCase) == true)
            {
                grid.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    private void BtnExcel_Click(object? sender, EventArgs e)
    {
        pStock.Common.ExcelExporter.Export(this.grid, "재고관리");
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        pStock.Common.GridPrinter.Print(this.grid, "재고관리");
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

    private void EdtNo_KeyUp(object? sender, KeyEventArgs e)
    {
        LocateInGrid("ITDSC", this.edtNo.Text);
    }

    private void EdtCvnam_KeyUp(object? sender, KeyEventArgs e)
    {
        LocateInGrid("CVNAM", this.edtCvnam.Text);
    }

    private void CboHouse_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LocateInGrid("HOUSE", this.cboHouse.Text);
    }

    private void Tabs_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ReloadList();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        this.edtNo.Clear(); this.edtCvnam.Clear(); this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); this.grid.DataSource = null;
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        ReloadList();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void JA01Form_Load(object? sender, EventArgs e)
    {
        ResetHouseList(); this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); ReloadList();
    }
}
