namespace YMES.DEPLOY
{
    partial class CustomizedMainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizedMainFrm));
            this.SuspendLayout();
            // 
            // MainFrmDesign
            // 
            this.MainFrmDesign.AppIcon = ((System.Drawing.Icon)(resources.GetObject("MainFrmDesign.AppIcon")));
            this.MainFrmDesign.TIT_DateFormat = YMES.FX.MainForm.Base.Common.DateFormatEnum.DDMMYYYY;
            this.MainFrmDesign.TIT_Line = "D/TRIM";
            this.MainFrmDesign.TIT_Plant = "AP";
            this.MainFrmDesign.TIT_Station = "Station #1";
            this.MainFrmDesign.Xml_DBSvr_NM = "SVR";
            // 
            // CustomizedMainFrm
            // 
            this.AppIcon = ((System.Drawing.Icon)(resources.GetObject("$this.AppIcon")));
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 644);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomizedMainFrm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
    }
}