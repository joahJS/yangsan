using System.Data;
using pStock.Data;
using pStock.Common;

namespace pStock.Forms.Common;

/// <summary>
/// 착지처 검색 팝업(REACH 테이블). CvcodLookupForm/ItemLookupForm과 동일한 패턴.
/// 출고등록(SS020F01)의 착지처코드 칸이 빈 상태에서 Enter 시 이 팝업을 띄운다.
/// </summary>
public partial class LnLookupForm : Form
{
    public string SelectedCode { get; private set; } = string.Empty;
    public string SelectedName { get; private set; } = string.Empty;
    public string SelectedAddr { get; private set; } = string.Empty;

    public LnLookupForm()
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

    private void LnLookupForm_Load(object? sender, EventArgs e)
    {
        cboSort.SelectedIndex = 1;
        Search();
    }

    private void LnLookupForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Close();
    }

    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT *, (ADDR1+' '+ADDR2) ADDR FROM REACH");
        q.Add(cboSort.SelectedIndex == 0 ? "WHERE LNCOD LIKE @WORD" : "WHERE LNNAM LIKE @WORD");
        q.ParamByName("WORD").AsString = edtSearch.Text.Trim() + "%";
        q.Add(cboSort.SelectedIndex == 0 ? "ORDER BY LNCOD" : "ORDER BY LNNAM");
        q.Open();

        grid.DataSource = q.Table;
        var map = new Dictionary<string, string>
        {
            ["LNCOD"] = "착지처코드", ["LNNAM"] = "착지처명", ["ADDR"] = "주소", ["LNTEL"] = "전화번호",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;
    }

    private void LocateInGrid()
    {
        var field = cboSort.SelectedIndex == 0 ? "LNCOD" : "LNNAM";
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
            SelectedCode = row["LNCOD"].ToString() ?? string.Empty;
            SelectedName = row["LNNAM"].ToString() ?? string.Empty;
            SelectedAddr = row["ADDR"].ToString() ?? string.Empty;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
