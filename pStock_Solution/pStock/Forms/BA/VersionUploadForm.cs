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
        BuildDynamicLayout();
    }

    /// <summary>
    /// 원본 BuildLayout() — GridLayout 헬퍼로 좌표를 동적으로 계산하기 때문에(지역변수 사용)
    /// WinForms 디자이너가 InitializeComponent() 안에서는 처리하지 못해 이 메서드로 분리했다.
    /// InitializeComponent() 호출 직후 생성자에서 실행되므로 동작은 이전과 동일하다.
    /// </summary>
    private void BuildDynamicLayout()
    {
        var layout = new GridLayout(this, 20, 15, 280, 90, 30, 2);
        layout.Add("VersionID", this.edtVersionId, editWidth: 150);
        layout.NewRow();
        layout.Add("비고", this.edtRmk, span: 2, editWidth: 470);
        layout.NewRow();

        this.gridButtons.Left = 20; this.gridButtons.Top = layout.Bottom(5); this.gridButtons.Width = 560; this.gridButtons.Height = 30;
        this.btnUpload.Left = 0; this.btnUpload.Top = 0; this.btnUpload.Width = 100;
        this.gridButtons.Controls.Add(this.btnUpload);

        this.lblHint.Text = "※ debug 폴더내의 pStock.zip 파일 업로드";
        this.lblHint.ForeColor = Color.Red;
        this.lblHint.AutoSize = true;
        this.lblHint.Left = this.btnUpload.Right + 25;
        this.lblHint.Top = this.btnUpload.Top + 4;
        this.gridButtons.Controls.Add(this.lblHint);

        this.grid.Left = 20; this.grid.Top = this.gridButtons.Bottom + 5; this.grid.Width = 560; this.grid.Height = 220;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("FILE_NAME", "파일명");
        this.grid.Columns.Add("FILE_BYTE", "크기");

        this.bottom.Left = 20; this.bottom.Top = this.grid.Bottom + 10; this.bottom.Width = 560; this.bottom.Height = 35;
        this.btnSave.Left = 340; this.btnSave.Top = 0; this.btnSave.Width = 100;
        this.btnClose.Left = 450; this.btnClose.Top = 0; this.btnClose.Width = 100;
        this.bottom.Controls.AddRange(new Control[] { this.btnSave, this.btnClose });

        this.Controls.AddRange(new Control[] { this.gridButtons, this.grid, this.bottom });
        this.ClientSize = new Size(600, this.bottom.Bottom + 15);

        this.btnUpload.Click += (_, _) => PickFiles();
        this.btnSave.Click += (_, _) => Save();
        this.btnClose.Click += (_, _) => Close();
    }

    private void VersionUploadForm_Shown(object? sender, EventArgs e)
    {
        this.edtVersionId.Focus();
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
