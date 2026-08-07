using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.BA;

/// <summary>
/// 버전관리 화면 — 업로드해주신 SY001F04(DevExpress)를 pStock 방식으로 변환.
/// 원본이 쓰던 테이블/컬럼명(zSYS_VERSION, VERSION_ID, FILE_NAME, FILE_NO, FILE_BYTE,
/// UPLOAD_DT, VERSION_RMK)을 그대로 사용한다(SEQNO는 실제 테이블에 없어서 제외했다).
/// </summary>
public class VersionListForm : Form
{
    private readonly FastDataGridView grid = new();
    private readonly DateTimePicker dtpFrom = new();
    private readonly DateTimePicker dtpTo = new();

    private readonly Button btnAdd = new() { Text = "등록(F1)" };
    private readonly Button btnSearch = new() { Text = "조회(F5)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public VersionListForm()
    {
        Text = "버전관리";
        Width = 900;
        Height = 600;
        KeyPreview = true;

        BuildLayout();

        Shown += (_, _) =>
        {
            dtpFrom.Value = DateTime.Now.AddMonths(-3);
            dtpTo.Value = DateTime.Now;
            Search();
        };
        KeyDown += VersionListForm_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnAdd, btnSearch, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnAdd, btnSearch, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        dtpFrom.Left = 50; dtpFrom.Top = 8; dtpFrom.Width = 110; dtpFrom.Format = DateTimePickerFormat.Short;
        var lblTilde = new Label { Text = "~", Left = 165, Top = 12, AutoSize = true };
        dtpTo.Left = 180; dtpTo.Top = 8; dtpTo.Width = 110; dtpTo.Format = DateTimePickerFormat.Short;
        editPanel.Controls.AddRange(new Control[] { lblDate, dtpFrom, lblTilde, dtpTo });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.CellFormatting += Grid_CellFormatting;

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnAdd.Click += (_, _) => OpenUpload();
        btnSearch.Click += (_, _) => Search();
        btnClose.Click += (_, _) => Close();
    }

    private void VersionListForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnAdd.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 Bt_Retr_Click(CMD=VERSION_LIST, DATE_FROM/DATE_TO).</summary>
    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT VERSION_ID,FILE_NAME,FILE_BYTE,UPLOAD_DT,VERSION_RMK FROM zSYS_VERSION");
        q.Add(" WHERE UPLOAD_DT BETWEEN @DATE1 AND @DATE2");
        q.Add(" ORDER BY UPLOAD_DT DESC, VERSION_ID DESC");
        q.ParamByName("DATE1").AsString = dtpFrom.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpTo.Value.ToString("yyyy-MM-dd");

        try
        {
            q.Open();
        }
        catch (Exception ex)
        {
            MessageBox.Show("조회중 오류가 발생했습니다.\r\n\r\n" + ex.Message, "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        grid.DataSource = q.Table;
        ApplyGridHeaders();
    }

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["VERSION_ID"] = "버전번호", ["FILE_NAME"] = "파일명", ["FILE_BYTE"] = "크기",
            ["UPLOAD_DT"] = "업로드일자", ["VERSION_RMK"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;
    }

    /// <summary>FILE_BYTE(바이트 수, int)를 사람이 읽기 편한 단위로 표시.</summary>
    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (grid.Columns[e.ColumnIndex].Name != "FILE_BYTE" || e.Value == null || e.Value == DBNull.Value) return;
        if (long.TryParse(e.Value.ToString(), out var bytes))
        {
            e.Value = FormatBytes(bytes);
            e.FormattingApplied = true;
        }
    }

    private static string FormatBytes(long bytes)
    {
        const int scale = 1024;
        string[] orders = { "GB", "MB", "KB", "Bytes" };
        long max = (long)Math.Pow(scale, orders.Length - 1);

        foreach (var order in orders)
        {
            if (bytes > max) return string.Format("{0:##.##} {1}", (decimal)bytes / max, order);
            max /= scale;
        }
        return "0 Bytes";
    }

    private void OpenUpload()
    {
        using var dlg = new VersionUploadForm();
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }
}
