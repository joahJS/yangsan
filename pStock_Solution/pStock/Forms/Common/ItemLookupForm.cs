using System.Data;
using pStock.Data;
using pStock.Common;

namespace pStock.Forms.Common;

/// <summary>
/// 원본 BA00D.pas / BA00D.dfm (TfrmBA00D) 이식 — 품목 검색 팝업(ITEMAS).
/// 사용법: using var dlg = new ItemLookupForm();
///        if (dlg.ShowDialog() == DialogResult.OK) { var no = dlg.SelectedItnbr; ... }
/// </summary>
public partial class ItemLookupForm : Form
{
    public string SelectedItnbr { get; private set; } = string.Empty;
    public string SelectedItdsc { get; private set; } = string.Empty;
    public string SelectedIspec { get; private set; } = string.Empty;
    public string SelectedDanwi { get; private set; } = string.Empty;
    public decimal SelectedIcost { get; private set; }
    public decimal SelectedBcost { get; private set; }
    public decimal SelectedOcost { get; private set; }

    public ItemLookupForm()
    {
        InitializeComponent();
    }

    private void CboSort_SelectedIndexChanged(object? sender, EventArgs e)
    {
        Search();
    }

    private void EdtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) LocateInGrid();
    }

    private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        Confirm();
    }

    private void BtnConfirm_Click(object? sender, EventArgs e)
    {
        Confirm();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void ItemLookupForm_Load(object? sender, EventArgs e)
    {
        cboSort.SelectedIndex = 1;
        Search();
    }

    private void ItemLookupForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Close();
    }

    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT A.*,B.CVNAM FROM ITEMAS A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON SUBSTRING(A.ITNBR,1,4)=B.CVCOD");
        q.Add(cboSort.SelectedIndex == 0 ? "WHERE ITNBR LIKE @WORD" : "WHERE ITDSC LIKE @WORD");
        q.ParamByName("WORD").AsString = edtSearch.Text.Trim() + "%";
        q.Add(cboSort.SelectedIndex == 0 ? "ORDER BY ITNBR" : "ORDER BY ITDSC");
        q.Open();

        grid.DataSource = q.Table;
        var map = new Dictionary<string, string>
        {
            ["ITNBR"] = "품번", ["ITDSC"] = "품명", ["ISPEC"] = "규격", ["DANWI"] = "단위",
            ["ICOST"] = "입고단가", ["BCOST"] = "기준단가", ["OCOST"] = "출고단가", ["CVNAM"] = "거래처명",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;
    }

    private void LocateInGrid()
    {
        var field = cboSort.SelectedIndex == 0 ? "ITNBR" : "ITDSC";
        var word = edtSearch.Text.Trim();
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.Cells[field].Value?.ToString()?.StartsWith(word, StringComparison.OrdinalIgnoreCase) == true)
            {
                grid.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    private void Confirm()
    {
        if (grid.CurrentRow?.DataBoundItem is DataRowView row)
        {
            SelectedItnbr = row["ITNBR"].ToString() ?? string.Empty;
            SelectedItdsc = row["ITDSC"].ToString() ?? string.Empty;
            SelectedIspec = row["ISPEC"].ToString() ?? string.Empty;
            SelectedDanwi = row["DANWI"].ToString() ?? string.Empty;
            SelectedIcost = row["ICOST"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ICOST"]);
            SelectedBcost = row["BCOST"] == DBNull.Value ? 0 : Convert.ToDecimal(row["BCOST"]);
            SelectedOcost = row["OCOST"] == DBNull.Value ? 0 : Convert.ToDecimal(row["OCOST"]);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
