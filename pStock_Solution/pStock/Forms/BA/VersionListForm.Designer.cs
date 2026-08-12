using pStock.Common;

namespace pStock.Forms.BA;

partial class VersionListForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private FastDataGridView grid;
    private DateTimePicker dtpFrom;
    private DateTimePicker dtpTo;

    private Button btnAdd;
    private Button btnSearch;
    private Button btnClose;
    private Panel panelTop;
    private Panel editPanel;
    private Label lblDate;
    private Label lblTilde;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.dtpFrom = new DateTimePicker();
        this.dtpTo = new DateTimePicker();
        this.btnAdd = new Button();
        this.btnSearch = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.editPanel = new Panel();
        this.lblDate = new Label();
        this.lblTilde = new Label();
        this.SuspendLayout();
        //
        // btnAdd / btnSearch / btnClose
        //
        this.btnAdd.Text = "등록(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnClose.Text = "닫기(Esc)";
        //
        // VersionListForm (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "버전관리";
        this.Width = 900;
        this.Height = 600;
        this.KeyPreview = true;
        //
        // 원본 BuildLayout()
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.Add(this.btnAdd);
        this.panelTop.Controls.Add(this.btnSearch);
        this.panelTop.Controls.Add(this.btnClose);
        this.btnAdd.Left = 5; this.btnAdd.Top = 8; this.btnAdd.Width = 90;
        this.btnSearch.Left = 100; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnClose.Left = 195; this.btnClose.Top = 8; this.btnClose.Width = 90;

        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 40;
        this.lblDate.Text = "기간"; this.lblDate.Left = 5; this.lblDate.Top = 12; this.lblDate.AutoSize = true;
        this.dtpFrom.Left = 50; this.dtpFrom.Top = 8; this.dtpFrom.Width = 110; this.dtpFrom.Format = DateTimePickerFormat.Short;
        this.lblTilde.Text = "~"; this.lblTilde.Left = 165; this.lblTilde.Top = 12; this.lblTilde.AutoSize = true;
        this.dtpTo.Left = 180; this.dtpTo.Top = 8; this.dtpTo.Width = 110; this.dtpTo.Format = DateTimePickerFormat.Short;
        this.editPanel.Controls.AddRange(new Control[] { this.lblDate, this.dtpFrom, this.lblTilde, this.dtpTo });

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellFormatting += new DataGridViewCellFormattingEventHandler(this.Grid_CellFormatting);

        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.panelTop);

        this.btnAdd.Click += (_, _) => OpenUpload();
        this.btnSearch.Click += (_, _) => Search();
        this.btnClose.Click += (_, _) => Close();

        this.Shown += (_, _) =>
        {
            this.dtpFrom.Value = DateTime.Now.AddMonths(-3);
            this.dtpTo.Value = DateTime.Now;
            Search();
        };
        this.KeyDown += new KeyEventHandler(this.VersionListForm_KeyDown);
        this.ResumeLayout(false);
    }
}
