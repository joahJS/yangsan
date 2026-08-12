namespace pStock.Forms;

partial class PlaceholderForm
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

    private Label lblMessage;

    private void InitializeComponent()
    {
        this.lblMessage = new Label();
        this.SuspendLayout();
        //
        // lblMessage
        //
        this.lblMessage.Dock = DockStyle.Fill;
        this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
        //
        // PlaceholderForm
        //
        this.Width = 500;
        this.Height = 300;
        this.Controls.Add(this.lblMessage);
        this.ResumeLayout(false);
    }
}
