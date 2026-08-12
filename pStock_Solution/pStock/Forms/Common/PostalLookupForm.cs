using System.Data;
using pStock.Data;
using pStock.Common;

namespace pStock.Forms.Common;

/// <summary>
/// 원본 PublicLib.gp_LoadFormPOSTF / cm001U05.pas(Tcm001F05) 이식 — 우편번호 검색 팝업(POSTF).
/// 사용법: using var dlg = new PostalLookupForm(initialWord: edtAddr.Text);
///        if (dlg.ShowDialog() == DialogResult.OK) { edtPost.Text = dlg.SelectedZip; edtAddr.Text = dlg.SelectedAddr; }
/// </summary>
public partial class PostalLookupForm : Form
{
    public string SelectedDDD { get; private set; } = string.Empty;   // 지역번호
    public string SelectedZip { get; private set; } = string.Empty;   // 우편번호
    public string SelectedAddr { get; private set; } = string.Empty;  // 주소(기본주소+동/도로명)

    public PostalLookupForm(string initialWord = "")
    {
        InitializeComponent();
        edtSearch.Text = initialWord;
    }

    /// <summary>원본 up_Srch / gp_CallNamePOSTF의 검색 SQL.</summary>
    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT A1.*, (CASE WHEN DNG1<>'' THEN (DNG1+' '+DNG2) ELSE DNG2 END) DONG");
        q.Add("  FROM POSTF A1");
        q.Add(" WHERE (ZCD+DNG1+DNG2+DORO) LIKE @WORD");
        q.Add(" ORDER BY DORO");
        q.ParamByName("WORD").AsString = "%" + edtSearch.Text.Trim() + "%";
        q.Open();

        grid.DataSource = q.Table;
        var map = new Dictionary<string, string>
        {
            ["ZCD"] = "우편번호", ["DDD"] = "지역번호", ["ADDR1"] = "기본주소",
            ["DORO"] = "도로명", ["DONG"] = "동",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;
    }

    private void Confirm()
    {
        if (grid.CurrentRow?.DataBoundItem is DataRowView row)
        {
            SelectedDDD = row["DDD"].ToString() ?? string.Empty;
            SelectedZip = row["ZCD"].ToString() ?? string.Empty;
            var doro = row.Row.Table.Columns.Contains("DORO") ? row["DORO"].ToString() : "";
            SelectedAddr = $"{row["ADDR1"]} {doro}".Trim();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
