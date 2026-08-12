using pStock.Common;

namespace pStock.Forms.Common;

partial class CvcodLookupForm
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

    private ComboBox cboSort;
    private TextBox edtSearch;
    private FastDataGridView grid;
    private Button btnConfirm;
    private Button btnClose;
    private Panel panelTop;
    private Label lblSearch;
    private Panel panelBottom;

    private void InitializeComponent()
    {
        this.cboSort = new ComboBox();
        this.edtSearch = new TextBox();
        this.grid = new FastDataGridView();
        this.btnConfirm = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.lblSearch = new Label();
        this.panelBottom = new Panel();
        this.panelTop.SuspendLayout();
        this.panelBottom.SuspendLayout();
        this.SuspendLayout();
        //
        // cboSort
        //
        this.cboSort.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cboSort.Items.AddRange(new object[] { "거래처코드", "거래처명" });
        this.cboSort.Left = 50;
        this.cboSort.Top = 8;
        this.cboSort.Width = 100;
        //
        // lblSearch
        //
        this.lblSearch.Text = "검색";
        this.lblSearch.Left = 10;
        this.lblSearch.Top = 12;
        this.lblSearch.AutoSize = true;
        //
        // edtSearch
        //
        this.edtSearch.Left = 160;
        this.edtSearch.Top = 8;
        this.edtSearch.Width = 200;
        //
        // panelTop (원본 BuildLayout()의 top 패널)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.lblSearch, this.cboSort, this.edtSearch });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        //
        // btnConfirm / btnClose
        //
        this.btnConfirm.Text = "확인";
        this.btnConfirm.Left = 300;
        this.btnConfirm.Top = 6;
        this.btnConfirm.Width = 80;
        this.btnClose.Text = "닫기";
        this.btnClose.Left = 390;
        this.btnClose.Top = 6;
        this.btnClose.Width = 80;
        //
        // panelBottom (원본 BuildLayout()의 bottom 패널)
        //
        this.panelBottom.Dock = DockStyle.Bottom;
        this.panelBottom.Height = 40;
        this.panelBottom.Controls.AddRange(new Control[] { this.btnConfirm, this.btnClose });
        //
        // 이벤트 배선 (원본 BuildLayout()에서 그대로 이동)
        //
        this.cboSort.SelectedIndexChanged += new EventHandler(this.CboSort_SelectedIndexChanged);
        this.edtSearch.KeyDown += new KeyEventHandler(this.EdtSearch_KeyDown);
        this.grid.CellDoubleClick += new DataGridViewCellEventHandler(this.Grid_CellDoubleClick);
        this.btnConfirm.Click += new EventHandler(this.BtnConfirm_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // CvcodLookupForm
        //
        this.Text = "거래처 검색";
        this.Width = 500;
        this.Height = 500;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelBottom);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.CvcodLookupForm_Load);
        this.KeyDown += new KeyEventHandler(this.CvcodLookupForm_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelTop.PerformLayout();
        this.panelBottom.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
