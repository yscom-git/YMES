namespace YMES.FX.MainForm.Base
{
    partial class MsgBox
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
            this.Lbl_Title = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.Btn_Yes = new System.Windows.Forms.Button();
            this.Btn_No = new System.Windows.Forms.Button();
            this.Lbl_Msg = new System.Windows.Forms.Label();
            this.Lbl_Icon = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lbl_Title
            // 
            this.Lbl_Title.BackColor = System.Drawing.Color.Blue;
            this.Lbl_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lbl_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.Lbl_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Title.ForeColor = System.Drawing.Color.White;
            this.Lbl_Title.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Title.Name = "Lbl_Title";
            this.Lbl_Title.Size = new System.Drawing.Size(619, 37);
            this.Lbl_Title.TabIndex = 1;
            this.Lbl_Title.Text = "Title";
            this.Lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Lbl_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Title_MouseDown);
            this.Lbl_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Title_MouseMove);
            this.Lbl_Title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lbl_Title_MouseUp);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.Btn_OK);
            this.flowLayoutPanel1.Controls.Add(this.Btn_Yes);
            this.flowLayoutPanel1.Controls.Add(this.Btn_No);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(218, 119);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(389, 50);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // Btn_OK
            // 
            this.Btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Btn_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_OK.Location = new System.Drawing.Point(3, 3);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(115, 41);
            this.Btn_OK.TabIndex = 3;
            this.Btn_OK.Text = "Confirm";
            this.Btn_OK.UseVisualStyleBackColor = true;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // Btn_Yes
            // 
            this.Btn_Yes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Btn_Yes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Yes.Location = new System.Drawing.Point(124, 3);
            this.Btn_Yes.Name = "Btn_Yes";
            this.Btn_Yes.Size = new System.Drawing.Size(91, 41);
            this.Btn_Yes.TabIndex = 2;
            this.Btn_Yes.Text = "Yes";
            this.Btn_Yes.UseVisualStyleBackColor = true;
            this.Btn_Yes.Click += new System.EventHandler(this.Btn_Yes_Click);
            // 
            // Btn_No
            // 
            this.Btn_No.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Btn_No.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_No.Location = new System.Drawing.Point(221, 3);
            this.Btn_No.Name = "Btn_No";
            this.Btn_No.Size = new System.Drawing.Size(91, 41);
            this.Btn_No.TabIndex = 4;
            this.Btn_No.Text = "No";
            this.Btn_No.UseVisualStyleBackColor = true;
            this.Btn_No.Click += new System.EventHandler(this.Btn_No_Click);
            // 
            // Lbl_Msg
            // 
            this.Lbl_Msg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_Msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Msg.Location = new System.Drawing.Point(168, 47);
            this.Lbl_Msg.Name = "Lbl_Msg";
            this.Lbl_Msg.Size = new System.Drawing.Size(439, 69);
            this.Lbl_Msg.TabIndex = 7;
            this.Lbl_Msg.Text = "Message1\r\nMessage2\r\nMessage3";
            this.Lbl_Msg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_Icon
            // 
            this.Lbl_Icon.BackColor = System.Drawing.Color.Red;
            this.Lbl_Icon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Lbl_Icon.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Icon.ForeColor = System.Drawing.Color.Yellow;
            this.Lbl_Icon.Location = new System.Drawing.Point(6, 45);
            this.Lbl_Icon.Name = "Lbl_Icon";
            this.Lbl_Icon.Size = new System.Drawing.Size(157, 124);
            this.Lbl_Icon.TabIndex = 6;
            this.Lbl_Icon.Text = "ERROR";
            this.Lbl_Icon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Blue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "▲";
            // 
            // MsgBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(619, 174);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.Lbl_Msg);
            this.Controls.Add(this.Lbl_Icon);
            this.Controls.Add(this.Lbl_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MsgBox";
            this.Text = "MsgBox";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Lbl_Title;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button Btn_OK;
        private System.Windows.Forms.Button Btn_Yes;
        private System.Windows.Forms.Button Btn_No;
        private System.Windows.Forms.Label Lbl_Msg;
        private System.Windows.Forms.Label Lbl_Icon;
        private System.Windows.Forms.Label label1;
    }
}