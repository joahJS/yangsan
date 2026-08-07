using System.Data;
using System.IO;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 버전 등록 팝업 — 업로드해주신 SY001F04_POP00(DevExpress)을 pStock 방식으로 변환.
/// 원본 Bt_Save_Click의 INSERT문(테이블/컬럼명 포함)을 그대로 옮겼다:
///   INSERT INTO DBO.zSYS_VERSION (VERSION_ID, FILE_NAME, FILE_NO, FILE_BYTE, UPLOAD_DT, VERSION_RMK)
///   VALUES(...)
/// </summary>
public class VersionUploadForm : Form
{
    private readonly TextBox edtVersionId = new();
    private readonly TextBox edtRmk = new();
    private readonly FastDataGridView grid = new();

    private readonly Button btnUpload = new() { Text = "파일선택" };
    private readonly Button btnSave = new() { Text = "저장(F3)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    private readonly List<(string fileName, long fileBytes, byte[] fileNo)> _pendingFiles = new();

    public bool Saved { get; private set; }

    public VersionUploadForm()
    {
        Text = "버전 등록";
        Width = 620;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();
        KeyDown += VersionUploadForm_KeyDown;

        Shown += (_, _) => edtVersionId.Focus();
    }

    private void BuildLayout()
    {
        var layout = new GridLayout(this, 20, 15, 280, 90, 30, 2);
        layout.Add("VersionID", edtVersionId, editWidth: 150);
        layout.NewRow();
        layout.Add("비고", edtRmk, span: 2, editWidth: 470);
        layout.NewRow();

        var gridButtons = new Panel { Left = 20, Top = layout.Bottom(5), Width = 560, Height = 30 };
        btnUpload.Left = 0; btnUpload.Top = 0; btnUpload.Width = 100;
        gridButtons.Controls.Add(btnUpload);

        var lblHint = new Label
        {
            Text = "※ debug 폴더내의 pStock.zip 파일 업로드",
            ForeColor = Color.Red,
            AutoSize = true,
            Left = btnUpload.Right + 25,
            Top = btnUpload.Top + 4,
        };
        gridButtons.Controls.Add(lblHint);

        grid.Left = 20; grid.Top = gridButtons.Bottom + 5; grid.Width = 560; grid.Height = 220;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.Columns.Add("FILE_NAME", "파일명");
        grid.Columns.Add("FILE_BYTE", "크기");

        var bottom = new Panel { Left = 20, Top = grid.Bottom + 10, Width = 560, Height = 35 };
        btnSave.Left = 340; btnSave.Top = 0; btnSave.Width = 100;
        btnClose.Left = 450; btnClose.Top = 0; btnClose.Width = 100;
        bottom.Controls.AddRange(new Control[] { btnSave, btnClose });

        Controls.AddRange(new Control[] { gridButtons, grid, bottom });
        ClientSize = new Size(600, bottom.Bottom + 15);

        btnUpload.Click += (_, _) => PickFiles();
        btnSave.Click += (_, _) => Save();
        btnClose.Click += (_, _) => Close();
    }

    private void VersionUploadForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F3) btnSave.PerformClick();
        else if (e.KeyCode == Keys.Escape) btnClose.PerformClick();
    }

    /// <summary>원본 Bt_Upload_Click.</summary>
    private void PickFiles()
    {
        // dotnet publish 결과 폴더(exe+dll+deps.json+runtimeconfig.json+참조 라이브러리)를 통째로
        // 압축한 zip 파일을 등록한다. exe 하나만 등록하면 실제 로직이 담긴 dll이 갱신되지 않아
        // "버전 정보는 최신인데 실제 프로그램은 이전 버전"이 되는 문제가 있었기 때문.
        using var dlg = new OpenFileDialog
        {
            InitialDirectory = "c:\\",
            Filter = "Zip Files (.zip)|*.zip|All Files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true,
            Multiselect = true,
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        _pendingFiles.Clear();
        foreach (var path in dlg.FileNames)
        {
            var info = new FileInfo(path);
            var data = File.ReadAllBytes(path);
            _pendingFiles.Add((info.Name, info.Length, data));
        }

        RefreshGrid();

        btnSave.Enabled = true;
        MessageBox.Show("저장을 눌러주세요.");
    }

    private void RefreshGrid()
    {
        grid.Rows.Clear();
        foreach (var f in _pendingFiles) grid.Rows.Add(f.fileName, FormatBytes(f.fileBytes));
    }

    /// <summary>원본 FormatBytes.</summary>
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

    /// <summary>원본 Bt_Save_Click.</summary>
    private void Save()
    {
        var versionId = edtVersionId.Text.Trim();
        if (string.IsNullOrEmpty(versionId))
        {
            MessageBox.Show("VersionID를 입력하세요(예 : 1.1.001)");
            edtVersionId.Focus();
            return;
        }

        try
        {
            AppDb.BeginTransaction();

            foreach (var f in _pendingFiles)
            {
                using var q = new DbQuery();
                q.Add("INSERT INTO DBO.zSYS_VERSION");
                q.Add("           ( VERSION_ID, FILE_NAME, FILE_NO, FILE_BYTE, UPLOAD_DT, VERSION_RMK )");
                q.Add("     VALUES( @VERSION_ID, @FILE_NAME, @FILE_NO, @FILE_BYTE, @UPLOAD_DT, @VERSION_RMK )");
                q.ParamByName("VERSION_ID").AsString = versionId;
                q.ParamByName("FILE_NAME").AsString = f.fileName;
                q.ParamByName("FILE_NO").AsBinary = f.fileNo;
                q.ParamByName("FILE_BYTE").AsInteger = (int)f.fileBytes;
                q.ParamByName("UPLOAD_DT").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                q.ParamByName("VERSION_RMK").AsString = edtRmk.Text.Trim();
                q.ExecSQL();
            }

            AppDb.Commit();
            MessageBox.Show("저장이 완료되었습니다.", "확인");
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show(ex.Message);
            return;
        }

        Saved = true;
        Close();
    }
}
