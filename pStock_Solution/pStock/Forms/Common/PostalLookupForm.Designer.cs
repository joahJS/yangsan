using pStock.Common;

namespace pStock.Forms.Common;

partial class PostalLookupForm
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

    private TextBox edtSearch;
    private FastDataGridView grid;
    private Button btnConfirm;
    private Button btnClose;
    private Panel panelTop;
    private Label lblSearch;
    private Button btnSearch;
    private Panel panelBottom;

    private void InitializeComponent()
    {
        this.edtSearch = new TextBox();
        this.grid = new FastDataGridView();
        this.btnConfirm = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.lblSearch = new Label();
        this.btnSearch = new Button();
        this.panelBottom = new Panel();
        this.panelTop.SuspendLayout();
        this.panelBottom.SuspendLayout();
        this.SuspendLayout();
        //
        // lblSearch
        //
        this.lblSearch.Text = "검색어(동/도로명/우편번호)";
        this.lblSearch.Left = 10;
        this.lblSearch.Top = 12;
        this.lblSearch.AutoSize = true;
        //
        // edtSearch
        //
        this.edtSearch.Left = 190;
        this.edtSearch.Top = 8;
        this.edtSearch.Width = 250;
        //
        // btnSearch
        //
        this.btnSearch.Text = "검색(F5)";
        this.btnSearch.Left = 450;
        this.btnSearch.Top = 6;
        this.btnSearch.Width = 80;
        //
        // panelTop (원본 BuildLayout()의 top 패널)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.lblSearch, this.edtSearch, this.btnSearch });
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
        this.btnConfirm.Left = 500;
        this.btnConfirm.Top = 6;
        this.btnConfirm.Width = 80;
        this.btnClose.Text = "닫기";
        this.btnClose.Left = 590;
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
        this.edtSearch.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) Search(); };
        this.btnSearch.Click += (_, _) => Search();
        this.grid.CellDoubleClick += (_, _) => Confirm();
        this.btnConfirm.Click += (_, _) => Confirm();
        this.btnClose.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        //
        // PostalLookupForm
        //
        this.Text = "우편번호 검색";
        this.Width = 700;
        this.Height = 500;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelBottom);
        this.Controls.Add(this.panelTop);
        this.Load += (_, _) => Search();
        this.KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };
        this.panelTop.ResumeLayout(false);
        this.panelTop.PerformLayout();
        this.panelBottom.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
