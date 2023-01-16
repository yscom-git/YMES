namespace YMES.FX.MainForm
{
    partial class BaseMainForm
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
            this.components = new System.ComponentModel.Container();
            YMES.FX.MainForm.Base.MsgType msgType1 = new YMES.FX.MainForm.Base.MsgType();
            this.pn_Msg = new System.Windows.Forms.Panel();
            this.StatusProgress = new System.Windows.Forms.ProgressBar();
            this.Btn_Config = new System.Windows.Forms.Button();
            this.lbl_WorkStatus = new System.Windows.Forms.Label();
            this.lbl_Msg = new System.Windows.Forms.Label();
            this.lbl_MsgTitle = new System.Windows.Forms.Label();
            this.pn_Main = new System.Windows.Forms.Panel();
            this.Pan_Title = new System.Windows.Forms.Panel();
            this.lbl_MainTitle = new System.Windows.Forms.Label();
            this.lbl_Time = new System.Windows.Forms.Label();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.Btn_ProcResult = new System.Windows.Forms.Button();
            this.lbl_SubTitle = new System.Windows.Forms.Label();
            this.lbl_WorkShift = new System.Windows.Forms.Label();
            this.Pan_Logo = new System.Windows.Forms.Panel();
            this.pb_Logo = new System.Windows.Forms.PictureBox();
            this.btn_WinMax = new System.Windows.Forms.Button();
            this.btn_WinMin = new System.Windows.Forms.Button();
            this.btn_WorkStandard = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.lbl_Bizcd = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Pan_Body = new System.Windows.Forms.Panel();
            this.TmrTimeBase = new System.Windows.Forms.Timer(this.components);
            this.MainFrmDesign = new YMES.FX.MainForm.Base.MainFormDesign();
            this.pn_Msg.SuspendLayout();
            this.pn_Main.SuspendLayout();
            this.Pan_Title.SuspendLayout();
            this.Pan_Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Logo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pn_Msg
            // 
            this.pn_Msg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_Msg.Controls.Add(this.StatusProgress);
            this.pn_Msg.Controls.Add(this.Btn_Config);
            this.pn_Msg.Controls.Add(this.lbl_WorkStatus);
            this.pn_Msg.Controls.Add(this.lbl_Msg);
            this.pn_Msg.Controls.Add(this.lbl_MsgTitle);
            this.pn_Msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn_Msg.Location = new System.Drawing.Point(3, 702);
            this.pn_Msg.Name = "pn_Msg";
            this.pn_Msg.Size = new System.Drawing.Size(1018, 33);
            this.pn_Msg.TabIndex = 13;
            // 
            // StatusProgress
            // 
            this.StatusProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusProgress.Location = new System.Drawing.Point(865, 3);
            this.StatusProgress.MarqueeAnimationSpeed = 10;
            this.StatusProgress.Name = "StatusProgress";
            this.StatusProgress.Size = new System.Drawing.Size(50, 24);
            this.StatusProgress.Step = 20;
            this.StatusProgress.TabIndex = 21;
            this.StatusProgress.Value = 100;
            // 
            // Btn_Config
            // 
            this.Btn_Config.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Config.BackColor = System.Drawing.Color.SkyBlue;
            this.Btn_Config.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_Config.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_Config.Location = new System.Drawing.Point(752, 3);
            this.Btn_Config.Name = "Btn_Config";
            this.Btn_Config.Size = new System.Drawing.Size(107, 25);
            this.Btn_Config.TabIndex = 20;
            this.Btn_Config.Text = "Config";
            this.Btn_Config.UseVisualStyleBackColor = false;
            this.Btn_Config.Visible = false;
            this.Btn_Config.Click += new System.EventHandler(this.OnConfig);
            // 
            // lbl_WorkStatus
            // 
            this.lbl_WorkStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_WorkStatus.BackColor = System.Drawing.Color.Silver;
            this.lbl_WorkStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_WorkStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_WorkStatus.ForeColor = System.Drawing.Color.Black;
            this.lbl_WorkStatus.Location = new System.Drawing.Point(920, 2);
            this.lbl_WorkStatus.Name = "lbl_WorkStatus";
            this.lbl_WorkStatus.Size = new System.Drawing.Size(91, 25);
            this.lbl_WorkStatus.TabIndex = 3;
            this.lbl_WorkStatus.Text = "Wait";
            this.lbl_WorkStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_WorkStatus.DoubleClick += new System.EventHandler(this.lbl_WorkStatus_DoubleClick);
            // 
            // lbl_Msg
            // 
            this.lbl_Msg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Msg.BackColor = System.Drawing.Color.AntiqueWhite;
            this.lbl_Msg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Msg.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Msg.Location = new System.Drawing.Point(93, 2);
            this.lbl_Msg.Name = "lbl_Msg";
            this.lbl_Msg.Size = new System.Drawing.Size(768, 27);
            this.lbl_Msg.TabIndex = 2;
            this.lbl_Msg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_MsgTitle
            // 
            this.lbl_MsgTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_MsgTitle.BackColor = System.Drawing.Color.Blue;
            this.lbl_MsgTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_MsgTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_MsgTitle.ForeColor = System.Drawing.Color.White;
            this.lbl_MsgTitle.Location = new System.Drawing.Point(3, 2);
            this.lbl_MsgTitle.Name = "lbl_MsgTitle";
            this.lbl_MsgTitle.Size = new System.Drawing.Size(87, 26);
            this.lbl_MsgTitle.TabIndex = 2;
            this.lbl_MsgTitle.Text = "NOTICE";
            this.lbl_MsgTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pn_Main
            // 
            this.pn_Main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_Main.Controls.Add(this.Pan_Title);
            this.pn_Main.Controls.Add(this.Pan_Logo);
            this.pn_Main.Controls.Add(this.btn_WinMax);
            this.pn_Main.Controls.Add(this.btn_WinMin);
            this.pn_Main.Controls.Add(this.btn_WorkStandard);
            this.pn_Main.Controls.Add(this.btn_Exit);
            this.pn_Main.Controls.Add(this.lbl_Bizcd);
            this.pn_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn_Main.Location = new System.Drawing.Point(0, 0);
            this.pn_Main.Margin = new System.Windows.Forms.Padding(0);
            this.pn_Main.Name = "pn_Main";
            this.pn_Main.Size = new System.Drawing.Size(1024, 68);
            this.pn_Main.TabIndex = 14;
            // 
            // Pan_Title
            // 
            this.Pan_Title.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pan_Title.Controls.Add(this.lbl_MainTitle);
            this.Pan_Title.Controls.Add(this.lbl_Time);
            this.Pan_Title.Controls.Add(this.lbl_Date);
            this.Pan_Title.Controls.Add(this.Btn_ProcResult);
            this.Pan_Title.Controls.Add(this.lbl_SubTitle);
            this.Pan_Title.Controls.Add(this.lbl_WorkShift);
            this.Pan_Title.Location = new System.Drawing.Point(160, 0);
            this.Pan_Title.Name = "Pan_Title";
            this.Pan_Title.Size = new System.Drawing.Size(629, 64);
            this.Pan_Title.TabIndex = 13;
            // 
            // lbl_MainTitle
            // 
            this.lbl_MainTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_MainTitle.BackColor = System.Drawing.Color.DarkBlue;
            this.lbl_MainTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_MainTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_MainTitle.ForeColor = System.Drawing.Color.White;
            this.lbl_MainTitle.Location = new System.Drawing.Point(1, 1);
            this.lbl_MainTitle.Name = "lbl_MainTitle";
            this.lbl_MainTitle.Size = new System.Drawing.Size(388, 37);
            this.lbl_MainTitle.TabIndex = 10;
            this.lbl_MainTitle.Text = "LineName";
            this.lbl_MainTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_MainTitle.DoubleClick += new System.EventHandler(this.lbl_MainTitle_DoubleClick);
            this.lbl_MainTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_MainTitle_MouseDown);
            this.lbl_MainTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_MainTitle_MouseMove);
            this.lbl_MainTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_MainTitle_MouseUp);
            // 
            // lbl_Time
            // 
            this.lbl_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Time.BackColor = System.Drawing.Color.Black;
            this.lbl_Time.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Time.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Time.ForeColor = System.Drawing.Color.White;
            this.lbl_Time.Location = new System.Drawing.Point(390, 38);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(115, 26);
            this.lbl_Time.TabIndex = 5;
            this.lbl_Time.Text = "17:10:45";
            this.lbl_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Date
            // 
            this.lbl_Date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Date.BackColor = System.Drawing.Color.Black;
            this.lbl_Date.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Date.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Date.ForeColor = System.Drawing.Color.White;
            this.lbl_Date.Location = new System.Drawing.Point(390, 1);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(115, 37);
            this.lbl_Date.TabIndex = 6;
            this.lbl_Date.Text = "2013-06-17";
            this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_ProcResult
            // 
            this.Btn_ProcResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_ProcResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_ProcResult.Location = new System.Drawing.Point(507, 30);
            this.Btn_ProcResult.Name = "Btn_ProcResult";
            this.Btn_ProcResult.Size = new System.Drawing.Size(119, 33);
            this.Btn_ProcResult.TabIndex = 14;
            this.Btn_ProcResult.Text = "Result";
            this.Btn_ProcResult.UseVisualStyleBackColor = true;
            this.Btn_ProcResult.Click += new System.EventHandler(this.OnResult);
            // 
            // lbl_SubTitle
            // 
            this.lbl_SubTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_SubTitle.BackColor = System.Drawing.Color.DarkBlue;
            this.lbl_SubTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_SubTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SubTitle.ForeColor = System.Drawing.Color.White;
            this.lbl_SubTitle.Location = new System.Drawing.Point(1, 38);
            this.lbl_SubTitle.Name = "lbl_SubTitle";
            this.lbl_SubTitle.Size = new System.Drawing.Size(388, 26);
            this.lbl_SubTitle.TabIndex = 11;
            this.lbl_SubTitle.Text = "Process Name";
            this.lbl_SubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WorkShift
            // 
            this.lbl_WorkShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_WorkShift.BackColor = System.Drawing.SystemColors.Control;
            this.lbl_WorkShift.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_WorkShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_WorkShift.ForeColor = System.Drawing.Color.Black;
            this.lbl_WorkShift.Location = new System.Drawing.Point(508, 2);
            this.lbl_WorkShift.Name = "lbl_WorkShift";
            this.lbl_WorkShift.Size = new System.Drawing.Size(117, 28);
            this.lbl_WorkShift.TabIndex = 13;
            this.lbl_WorkShift.Text = "DAY";
            this.lbl_WorkShift.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pan_Logo
            // 
            this.Pan_Logo.BackColor = System.Drawing.Color.White;
            this.Pan_Logo.Controls.Add(this.pb_Logo);
            this.Pan_Logo.Location = new System.Drawing.Point(2, 2);
            this.Pan_Logo.Margin = new System.Windows.Forms.Padding(0);
            this.Pan_Logo.Name = "Pan_Logo";
            this.Pan_Logo.Size = new System.Drawing.Size(84, 61);
            this.Pan_Logo.TabIndex = 17;
            // 
            // pb_Logo
            // 
            this.pb_Logo.BackColor = System.Drawing.Color.White;
            this.pb_Logo.Location = new System.Drawing.Point(3, 3);
            this.pb_Logo.Margin = new System.Windows.Forms.Padding(0);
            this.pb_Logo.Name = "pb_Logo";
            this.pb_Logo.Size = new System.Drawing.Size(79, 55);
            this.pb_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Logo.TabIndex = 0;
            this.pb_Logo.TabStop = false;
            // 
            // btn_WinMax
            // 
            this.btn_WinMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_WinMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_WinMax.Location = new System.Drawing.Point(896, 1);
            this.btn_WinMax.Name = "btn_WinMax";
            this.btn_WinMax.Size = new System.Drawing.Size(30, 31);
            this.btn_WinMax.TabIndex = 16;
            this.btn_WinMax.Text = "□";
            this.btn_WinMax.UseVisualStyleBackColor = true;
            this.btn_WinMax.Click += new System.EventHandler(this.OnMaxWindow);
            // 
            // btn_WinMin
            // 
            this.btn_WinMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_WinMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_WinMin.Location = new System.Drawing.Point(896, 32);
            this.btn_WinMin.Name = "btn_WinMin";
            this.btn_WinMin.Size = new System.Drawing.Size(31, 31);
            this.btn_WinMin.TabIndex = 15;
            this.btn_WinMin.Text = "_";
            this.btn_WinMin.UseVisualStyleBackColor = true;
            this.btn_WinMin.Click += new System.EventHandler(this.OnMinWindow);
            // 
            // btn_WorkStandard
            // 
            this.btn_WorkStandard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_WorkStandard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_WorkStandard.Location = new System.Drawing.Point(790, 0);
            this.btn_WorkStandard.Name = "btn_WorkStandard";
            this.btn_WorkStandard.Size = new System.Drawing.Size(105, 65);
            this.btn_WorkStandard.TabIndex = 12;
            this.btn_WorkStandard.Text = "WORK\r\nSTANDARD";
            this.btn_WorkStandard.UseVisualStyleBackColor = true;
            this.btn_WorkStandard.Click += new System.EventHandler(this.OnWorkStandard);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Exit.BackColor = System.Drawing.Color.Brown;
            this.btn_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Exit.ForeColor = System.Drawing.Color.White;
            this.btn_Exit.Location = new System.Drawing.Point(927, 0);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(90, 65);
            this.btn_Exit.TabIndex = 4;
            this.btn_Exit.Text = "EXIT";
            this.btn_Exit.UseVisualStyleBackColor = false;
            this.btn_Exit.Click += new System.EventHandler(this.OnCloseBtn);
            // 
            // lbl_Bizcd
            // 
            this.lbl_Bizcd.BackColor = System.Drawing.Color.Brown;
            this.lbl_Bizcd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Bizcd.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Bizcd.ForeColor = System.Drawing.Color.White;
            this.lbl_Bizcd.Location = new System.Drawing.Point(88, 1);
            this.lbl_Bizcd.Name = "lbl_Bizcd";
            this.lbl_Bizcd.Size = new System.Drawing.Size(70, 63);
            this.lbl_Bizcd.TabIndex = 1;
            this.lbl_Bizcd.Text = "PLANT";
            this.lbl_Bizcd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pn_Msg, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pn_Main, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Pan_Body, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 738);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // Pan_Body
            // 
            this.Pan_Body.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Pan_Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pan_Body.Location = new System.Drawing.Point(0, 68);
            this.Pan_Body.Margin = new System.Windows.Forms.Padding(0);
            this.Pan_Body.Name = "Pan_Body";
            this.Pan_Body.Size = new System.Drawing.Size(1024, 631);
            this.Pan_Body.TabIndex = 15;
            // 
            // TmrTimeBase
            // 
            this.TmrTimeBase.Interval = 500;
            this.TmrTimeBase.Tick += new System.EventHandler(this.TmrTimeBase_Tick);
            // 
            // MainFrmDesign
            // 
            this.MainFrmDesign.ContainerControl = this;
            this.MainFrmDesign.Exit_Dlg_Contents = "Do you want to exit program?";
            this.MainFrmDesign.Exit_Dlg_Title = "Exit of Program";
            this.MainFrmDesign.LogoImg = global::YMES.FX.MainForm.Properties.Resources.PB_LOGO;
            msgType1.MSG_ALARM = "NOTICE";
            msgType1.MSG_ERROR = "ERROR";
            msgType1.MSG_TRACE = "TRACE";
            msgType1.MSG_WARN = "WARNING";
            this.MainFrmDesign.MsgTypeText = msgType1;
            this.MainFrmDesign.Parent = null;
            this.MainFrmDesign.TIT_Config = "Config";
            this.MainFrmDesign.TIT_Config_FONT = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.MainFrmDesign.TIT_DateFormat = YMES.FX.MainForm.Base.Common.DateFormatEnum.YYYYMMDD;
            this.MainFrmDesign.TIT_Exit = "EXIT";
            this.MainFrmDesign.TIT_Exit_FONT = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.MainFrmDesign.TIT_Line = "This is Production Line";
            this.MainFrmDesign.TIT_Line_FONT = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.MainFrmDesign.TIT_Plant = "PLANT";
            this.MainFrmDesign.TIT_Plant_FONT = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.MainFrmDesign.TIT_Result = "Result";
            this.MainFrmDesign.TIT_Result_FONT = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.MainFrmDesign.TIT_Station = "This is WorkStation";
            this.MainFrmDesign.TIT_Station_FONT = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.MainFrmDesign.TIT_WorkStandard = "WORK\r\nSTANDARD";
            this.MainFrmDesign.TIT_WorkStandard_FONT = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // BaseMainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 738);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BaseMainForm";
            this.Text = "BaseForm";
            this.pn_Msg.ResumeLayout(false);
            this.pn_Main.ResumeLayout(false);
            this.Pan_Title.ResumeLayout(false);
            this.Pan_Logo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Logo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pn_Msg;
        private System.Windows.Forms.ProgressBar StatusProgress;
        private System.Windows.Forms.Button Btn_Config;
        private System.Windows.Forms.Label lbl_WorkStatus;
        private System.Windows.Forms.Label lbl_Msg;
        private System.Windows.Forms.Label lbl_MsgTitle;
        private System.Windows.Forms.Panel pn_Main;
        private System.Windows.Forms.Button btn_WinMax;
        private System.Windows.Forms.Button btn_WinMin;
        private System.Windows.Forms.Button Btn_ProcResult;
        private System.Windows.Forms.Label lbl_WorkShift;
        private System.Windows.Forms.Button btn_WorkStandard;
        private System.Windows.Forms.Label lbl_SubTitle;
        private System.Windows.Forms.Label lbl_MainTitle;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Label lbl_Time;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label lbl_Bizcd;
        private System.Windows.Forms.PictureBox pb_Logo;
        private System.Windows.Forms.Panel Pan_Logo;
        private System.Windows.Forms.Panel Pan_Title;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Pan_Body;
        private System.Windows.Forms.Timer TmrTimeBase;
        public Base.MainFormDesign MainFrmDesign;
    }
}