namespace pStockUpdate
{
    partial class Version_Check_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Version_Check_Form));
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.Bt_Close = new DevExpress.XtraEditors.SimpleButton();
            this.Bt_Update = new DevExpress.XtraEditors.SimpleButton();
            this.separatorPanel = new System.Windows.Forms.Panel();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.Lb_VerInfo = new DevExpress.XtraEditors.LabelControl();
            this.Lb_VerChkMsg = new DevExpress.XtraEditors.LabelControl();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.bodyPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // bodyPanel
            //
            this.bodyPanel.BackColor = System.Drawing.Color.White;
            this.bodyPanel.Controls.Add(this.infoPanel);
            this.bodyPanel.Controls.Add(this.Lb_VerChkMsg);
            this.bodyPanel.Controls.Add(this.bottomPanel);
            this.bodyPanel.Controls.Add(this.headerPanel);
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyPanel.Location = new System.Drawing.Point(1, 1);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(418, 258);
            this.bodyPanel.TabIndex = 0;
            //
            // bottomPanel
            //
            this.bottomPanel.BackColor = System.Drawing.Color.White;
            this.bottomPanel.Controls.Add(this.Bt_Close);
            this.bottomPanel.Controls.Add(this.Bt_Update);
            this.bottomPanel.Controls.Add(this.separatorPanel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 200);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(418, 58);
            this.bottomPanel.TabIndex = 3;
            //
            // Bt_Close
            //
            this.Bt_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_Close.Appearance.Options.UseFont = true;
            this.Bt_Close.Location = new System.Drawing.Point(332, 14);
            this.Bt_Close.Name = "Bt_Close";
            this.Bt_Close.Size = new System.Drawing.Size(74, 30);
            this.Bt_Close.TabIndex = 4;
            this.Bt_Close.Text = "닫기";
            this.Bt_Close.Click += new System.EventHandler(this.Bt_Close_Click);
            //
            // Bt_Update
            //
            this.Bt_Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_Update.Appearance.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.Bt_Update.Appearance.ForeColor = System.Drawing.Color.White;
            this.Bt_Update.Appearance.Options.UseBackColor = true;
            this.Bt_Update.Appearance.Options.UseFont = true;
            this.Bt_Update.Appearance.Options.UseForeColor = true;
            this.Bt_Update.Location = new System.Drawing.Point(244, 14);
            this.Bt_Update.Name = "Bt_Update";
            this.Bt_Update.Size = new System.Drawing.Size(80, 30);
            this.Bt_Update.TabIndex = 3;
            this.Bt_Update.Text = "업데이트";
            this.Bt_Update.Click += new System.EventHandler(this.Bt_Update_Click);
            //
            // separatorPanel
            //
            this.separatorPanel.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.separatorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.separatorPanel.Location = new System.Drawing.Point(0, 0);
            this.separatorPanel.Name = "separatorPanel";
            this.separatorPanel.Size = new System.Drawing.Size(418, 1);
            this.separatorPanel.TabIndex = 0;
            //
            // infoPanel
            //
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(246, 247, 249);
            this.infoPanel.Controls.Add(this.Lb_VerInfo);
            this.infoPanel.Location = new System.Drawing.Point(24, 132);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(370, 44);
            this.infoPanel.TabIndex = 2;
            //
            // Lb_VerInfo
            //
            this.Lb_VerInfo.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Lb_VerInfo.Appearance.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.Lb_VerInfo.Appearance.Options.UseFont = true;
            this.Lb_VerInfo.Appearance.Options.UseForeColor = true;
            this.Lb_VerInfo.Appearance.Options.UseTextOptions = true;
            this.Lb_VerInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Lb_VerInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Lb_VerInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Lb_VerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lb_VerInfo.Location = new System.Drawing.Point(0, 0);
            this.Lb_VerInfo.Name = "Lb_VerInfo";
            this.Lb_VerInfo.Size = new System.Drawing.Size(370, 44);
            this.Lb_VerInfo.TabIndex = 0;
            this.Lb_VerInfo.Text = "버전정보";
            //
            // Lb_VerChkMsg
            //
            this.Lb_VerChkMsg.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Lb_VerChkMsg.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.Lb_VerChkMsg.Appearance.Options.UseFont = true;
            this.Lb_VerChkMsg.Appearance.Options.UseForeColor = true;
            this.Lb_VerChkMsg.Appearance.Options.UseTextOptions = true;
            this.Lb_VerChkMsg.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Lb_VerChkMsg.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.Lb_VerChkMsg.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Lb_VerChkMsg.Location = new System.Drawing.Point(24, 76);
            this.Lb_VerChkMsg.Name = "Lb_VerChkMsg";
            this.Lb_VerChkMsg.Size = new System.Drawing.Size(370, 46);
            this.Lb_VerChkMsg.TabIndex = 1;
            this.Lb_VerChkMsg.Text = "버전체크 메시지";
            //
            // headerPanel
            //
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(418, 58);
            this.headerPanel.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Appearance.Options.UseForeColor = true;
            this.lblTitle.Location = new System.Drawing.Point(24, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(120, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "업데이트 확인";
            //
            // lblSubtitle
            //
            this.lblSubtitle.Appearance.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(220, 232, 245);
            this.lblSubtitle.Appearance.Options.UseFont = true;
            this.lblSubtitle.Appearance.Options.UseForeColor = true;
            this.lblSubtitle.Location = new System.Drawing.Point(24, 34);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(180, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "최신 버전 여부를 확인합니다";
            //
            // Version_Check_Form
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(218, 218, 218);
            this.ClientSize = new System.Drawing.Size(420, 260);
            this.Controls.Add(this.bodyPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Version_Check_Form";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Version_Check_Form";
            this.Load += new System.EventHandler(this.Version_Check_Form_Load);
            this.bodyPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bodyPanel;
        private System.Windows.Forms.Panel headerPanel;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl lblSubtitle;
        private DevExpress.XtraEditors.LabelControl Lb_VerChkMsg;
        private System.Windows.Forms.Panel infoPanel;
        private DevExpress.XtraEditors.LabelControl Lb_VerInfo;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel separatorPanel;
        private DevExpress.XtraEditors.SimpleButton Bt_Update;
        private DevExpress.XtraEditors.SimpleButton Bt_Close;
    }
}
