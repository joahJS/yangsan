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
public partial class VersionUploadForm : Form
{
    private readonly List<(string fileName, long fileBytes, byte[] fileNo)> _pendingFiles = new();

    public bool Saved { get; private set; }

    public VersionUploadForm()
    {
        InitializeComponent();
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
