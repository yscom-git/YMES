namespace YMES.Logics.MES
{
    partial class TagPrint
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.yDataGridView1 = new YMES.FX.Controls.YDataGridView();
            this.yBitLabel1 = new YMES.FX.Controls.YBitLabel();
            ((System.ComponentModel.ISupportInitialize)(this.yDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // yDataGridView1
            // 
            this.yDataGridView1.AllowUserToAddRows = false;
            this.yDataGridView1.AllowUserToResizeColumns = false;
            this.yDataGridView1.AllowUserToResizeRows = false;
            this.yDataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.yDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.yDataGridView1.ColumnHeadersHeight = 30;
            this.yDataGridView1.FixedSort = true;
            this.yDataGridView1.GridMode = YMES.FX.Controls.YDataGridView.GridModeEnum.QueryNomal;
            this.yDataGridView1.HeaderAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.yDataGridView1.Key = "";
            this.yDataGridView1.Location = new System.Drawing.Point(18, 15);
            this.yDataGridView1.MultiSelect = false;
            this.yDataGridView1.Name = "yDataGridView1";
            this.yDataGridView1.ReadOnly = true;
            this.yDataGridView1.RowHeadersVisible = false;
            this.yDataGridView1.RowTemplate.Height = 40;
            this.yDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.yDataGridView1.Size = new System.Drawing.Size(240, 522);
            this.yDataGridView1.TabIndex = 0;
            // 
            // yBitLabel1
            // 
            this.yBitLabel1.BackColor = System.Drawing.Color.Black;
            this.yBitLabel1.ForeColor = System.Drawing.Color.White;
            this.yBitLabel1.Key = "";
            this.yBitLabel1.Location = new System.Drawing.Point(279, 15);
            this.yBitLabel1.Name = "yBitLabel1";
            this.yBitLabel1.OffBGColor = System.Drawing.Color.Black;
            this.yBitLabel1.OffForeColor = System.Drawing.Color.White;
            this.yBitLabel1.OnBGColor = System.Drawing.Color.Lime;
            this.yBitLabel1.OnForeColor = System.Drawing.Color.Black;
            this.yBitLabel1.Size = new System.Drawing.Size(228, 66);
            this.yBitLabel1.TabIndex = 1;
            this.yBitLabel1.Text = "yBitLabel1";
            this.yBitLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TagPrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.yBitLabel1);
            this.Controls.Add(this.yDataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "TagPrint";
            ((System.ComponentModel.ISupportInitialize)(this.yDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FX.Controls.YDataGridView yDataGridView1;
        private FX.Controls.YBitLabel yBitLabel1;
    }
}
