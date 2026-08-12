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
public partial class CvcodLookupForm : Form
{
    private readonly string? _cvguFilter;

    public string SelectedCode { get; private set; } = string.Empty;
    public string SelectedName { get; private set; } = string.Empty;

    public CvcodLookupForm(string? cvguFilter = null)
    {
        _cvguFilter = cvguFilter;
        InitializeComponent();
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
