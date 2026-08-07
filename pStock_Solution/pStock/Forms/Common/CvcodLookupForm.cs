using System.Data;
using pStock.Data;
using pStock.Common;

namespace pStock.Forms.Common;

/// <summary>
/// 원본 BA00C.pas / BA00C.dfm (TfrmBA00C) 이식 — 거래처 검색 팝업(CVMAST).
/// 화면 곳곳의 edtCvcodKeyDown(Enter, 코드칸이 빈 상태)에서 이 팝업을 띄운다.
/// 사용법: using var dlg = new CvcodLookupForm(cvguFilter: "2"); // "2"=매출처, "1"=매입처, null=전체
///        if (dlg.ShowDialog() == DialogResult.OK) { var code = dlg.SelectedCode; var name = dlg.SelectedName; }
/// </summary>
public class CvcodLookupForm : Form
{
    private readonly string? _cvguFilter;
    private readonly ComboBox cboSort = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox edtSearch = new();
    private readonly FastDataGridView grid = new();
    private readonly Button btnConfirm = new() { Text = "확인" };
    private readonly Button btnClose = new() { Text = "닫기" };

    public string SelectedCode { get; private set; } = string.Empty;
    public string SelectedName { get; private set; } = string.Empty;

    public CvcodLookupForm(string? cvguFilter = null)
    {
        _cvguFilter = cvguFilter;
        Text = "거래처 검색";
        Width = 500;
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
        cboSort.Items.AddRange(new object[] { "거래처코드", "거래처명" });

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
        btnConfirm.Left = 300; btnConfirm.Top = 6; btnConfirm.Width = 80;
        btnClose.Left = 390; btnClose.Top = 6; btnClose.Width = 80;
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
        q.Add("SELECT * FROM CVMAST");
        q.Add(cboSort.SelectedIndex == 0 ? "WHERE CVCOD LIKE @WORD" : "WHERE CVNAM LIKE @WORD");
        q.ParamByName("WORD").AsString = edtSearch.Text.Trim() + "%";
        if (!string.IsNullOrEmpty(_cvguFilter))
        {
            q.Add("AND CVGU IN (@CVGU,'3')");
            q.ParamByName("CVGU").AsString = _cvguFilter;
        }
        q.Add(cboSort.SelectedIndex == 0 ? "ORDER BY CVCOD" : "ORDER BY CVNAM");
        q.Open();

        grid.DataSource = q.Table;
        if (grid.Columns["CVCOD"] != null) grid.Columns["CVCOD"]!.HeaderText = "거래처코드";
        if (grid.Columns["CVNAM"] != null) grid.Columns["CVNAM"]!.HeaderText = "거래처명";
        if (grid.Columns["OWNAM"] != null) grid.Columns["OWNAM"]!.HeaderText = "대표자";
        if (grid.Columns["TELNO"] != null) grid.Columns["TELNO"]!.HeaderText = "전화번호";
    }

    private void LocateInGrid()
    {
        var field = cboSort.SelectedIndex == 0 ? "CVCOD" : "CVNAM";
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
            SelectedCode = row["CVCOD"].ToString() ?? string.Empty;
            SelectedName = row["CVNAM"].ToString() ?? string.Empty;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
