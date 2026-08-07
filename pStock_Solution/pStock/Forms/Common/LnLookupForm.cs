using System.Data;
using pStock.Data;
using pStock.Common;

namespace pStock.Forms.Common;

/// <summary>
/// 착지처 검색 팝업(REACH 테이블). CvcodLookupForm/ItemLookupForm과 동일한 패턴.
/// 출고등록(SS020F01)의 착지처코드 칸이 빈 상태에서 Enter 시 이 팝업을 띄운다.
/// </summary>
public class LnLookupForm : Form
{
    private readonly ComboBox cboSort = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox edtSearch = new();
    private readonly FastDataGridView grid = new();
    private readonly Button btnConfirm = new() { Text = "확인" };
    private readonly Button btnClose = new() { Text = "닫기" };

    public string SelectedCode { get; private set; } = string.Empty;
    public string SelectedName { get; private set; } = string.Empty;
    public string SelectedAddr { get; private set; } = string.Empty;

    public LnLookupForm()
    {
        Text = "착지처 검색";
        Width = 600;
        Height = 500;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { cboSort.SelectedIndex = 1; Search(); };
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };
    }

    private void BuildLayout()
    {
        cboSort.Items.AddRange(new object[] { "착지처코드", "착지처명" });

        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lbl = new Label { Text = "검색", Left = 10, Top = 12, AutoSize = true };
        cboSort.Left = 50; cboSort.Top = 8; cboSort.Width = 100;
        cboSort.SelectedIndexChanged += (_, _) => Search();
        edtSearch.Left = 160; edtSearch.Top = 8; edtSearch.Width = 200;
        edtSearch.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) LocateInGrid(); };
        top.Controls.AddRange(new Control[] { lbl, cboSort, edtSearch });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.CellDoubleClick += (_, _) => Confirm();

        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 40 };
        btnConfirm.Left = 400; btnConfirm.Top = 6; btnConfirm.Width = 80;
        btnClose.Left = 490; btnClose.Top = 6; btnClose.Width = 80;
        bottom.Controls.AddRange(new Control[] { btnConfirm, btnClose });

        Controls.Add(grid);
        Controls.Add(bottom);
        Controls.Add(top);

        btnConfirm.Click += (_, _) => Confirm();
        btnClose.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
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
