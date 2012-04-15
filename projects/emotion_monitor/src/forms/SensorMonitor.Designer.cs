namespace emophyz
{
    partial class m_frmEmotionMonitor
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
			SpPerfChart.ChartPen chartPen1 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen2 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen3 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen4 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen5 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen6 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen7 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen8 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen9 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen10 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen11 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen12 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen13 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen14 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen15 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen16 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen17 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen18 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen19 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen20 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen21 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen22 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen23 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen24 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen25 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen26 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen27 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen28 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen29 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen30 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen31 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen32 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen33 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen34 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen35 = new SpPerfChart.ChartPen();
			SpPerfChart.ChartPen chartPen36 = new SpPerfChart.ChartPen();
			this.button_connect = new System.Windows.Forms.Button();
			this.m_plotGSR = new SpPerfChart.PerfChart();
			this.m_backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.m_trcbWorkerWait = new System.Windows.Forms.TrackBar();
			this.m_lblPlotSpeed = new System.Windows.Forms.Label();
			this.m_plotHR = new SpPerfChart.PerfChart();
			this.m_plotValence = new SpPerfChart.PerfChart();
			this.m_plotArousal = new SpPerfChart.PerfChart();
			this.m_plotBoredom = new SpPerfChart.PerfChart();
			this.m_plotFun = new SpPerfChart.PerfChart();
			this.m_plotExcitement = new SpPerfChart.PerfChart();
			this.m_plotEKGFrown = new SpPerfChart.PerfChart();
			this.m_plotEKGSmile = new SpPerfChart.PerfChart();
			this.m_grpbxEmotions = new System.Windows.Forms.GroupBox();
			this.m_grpbxAVSpace = new System.Windows.Forms.GroupBox();
			this.m_grpbxSensors = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.m_trcbWorkerWait)).BeginInit();
			this.m_grpbxEmotions.SuspendLayout();
			this.m_grpbxAVSpace.SuspendLayout();
			this.m_grpbxSensors.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// button_connect
			// 
			this.button_connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_connect.Location = new System.Drawing.Point(929, 588);
			this.button_connect.Name = "button_connect";
			this.button_connect.Size = new System.Drawing.Size(75, 23);
			this.button_connect.TabIndex = 2;
			this.button_connect.Text = "Connect";
			this.button_connect.UseVisualStyleBackColor = true;
			this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
			// 
			// m_plotGSR
			// 
			this.m_plotGSR.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotGSR.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotGSR.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotGSR.Location = new System.Drawing.Point(741, 3);
			this.m_plotGSR.Name = "m_plotGSR";
			this.m_plotGSR.PerfChartStyle.AntiAliasing = true;
			chartPen1.Color = System.Drawing.Color.LightGreen;
			chartPen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen1.Width = 1F;
			this.m_plotGSR.PerfChartStyle.AvgLinePen = chartPen1;
			this.m_plotGSR.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotGSR.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen2.Color = System.Drawing.Color.Gold;
			chartPen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen2.Width = 1F;
			this.m_plotGSR.PerfChartStyle.ChartLinePen = chartPen2;
			chartPen3.Color = System.Drawing.Color.DarkGray;
			chartPen3.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen3.Width = 1F;
			this.m_plotGSR.PerfChartStyle.HorizontalGridPen = chartPen3;
			this.m_plotGSR.PerfChartStyle.ShowAverageLine = true;
			this.m_plotGSR.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotGSR.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen4.Color = System.Drawing.Color.DarkGray;
			chartPen4.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen4.Width = 1F;
			this.m_plotGSR.PerfChartStyle.VerticalGridPen = chartPen4;
			this.m_plotGSR.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotGSR.Size = new System.Drawing.Size(242, 163);
			this.m_plotGSR.TabIndex = 3;
			this.m_plotGSR.TimerInterval = 100;
			this.m_plotGSR.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_backgroundWorker
			// 
			this.m_backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerDoWork);
			this.m_backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerWorkComplete);
			// 
			// m_trcbWorkerWait
			// 
			this.m_trcbWorkerWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_trcbWorkerWait.LargeChange = 10;
			this.m_trcbWorkerWait.Location = new System.Drawing.Point(91, 588);
			this.m_trcbWorkerWait.Maximum = 100;
			this.m_trcbWorkerWait.Minimum = 10;
			this.m_trcbWorkerWait.Name = "m_trcbWorkerWait";
			this.m_trcbWorkerWait.Size = new System.Drawing.Size(155, 42);
			this.m_trcbWorkerWait.TabIndex = 4;
			this.m_trcbWorkerWait.TickStyle = System.Windows.Forms.TickStyle.None;
			this.m_trcbWorkerWait.Value = 17;
			this.m_trcbWorkerWait.ValueChanged += new System.EventHandler(this.m_trcbWorkerWait_ValueChanged);
			// 
			// m_lblPlotSpeed
			// 
			this.m_lblPlotSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_lblPlotSpeed.AutoSize = true;
			this.m_lblPlotSpeed.Location = new System.Drawing.Point(12, 593);
			this.m_lblPlotSpeed.Name = "m_lblPlotSpeed";
			this.m_lblPlotSpeed.Size = new System.Drawing.Size(73, 13);
			this.m_lblPlotSpeed.TabIndex = 5;
			this.m_lblPlotSpeed.Text = "Speed: 17 fps";
			// 
			// m_plotHR
			// 
			this.m_plotHR.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotHR.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotHR.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotHR.Location = new System.Drawing.Point(495, 3);
			this.m_plotHR.Name = "m_plotHR";
			this.m_plotHR.PerfChartStyle.AntiAliasing = true;
			chartPen5.Color = System.Drawing.Color.LightGreen;
			chartPen5.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen5.Width = 1F;
			this.m_plotHR.PerfChartStyle.AvgLinePen = chartPen5;
			this.m_plotHR.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotHR.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen6.Color = System.Drawing.Color.Gold;
			chartPen6.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen6.Width = 1F;
			this.m_plotHR.PerfChartStyle.ChartLinePen = chartPen6;
			chartPen7.Color = System.Drawing.Color.DarkGray;
			chartPen7.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen7.Width = 1F;
			this.m_plotHR.PerfChartStyle.HorizontalGridPen = chartPen7;
			this.m_plotHR.PerfChartStyle.ShowAverageLine = true;
			this.m_plotHR.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotHR.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen8.Color = System.Drawing.Color.DarkGray;
			chartPen8.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen8.Width = 1F;
			this.m_plotHR.PerfChartStyle.VerticalGridPen = chartPen8;
			this.m_plotHR.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotHR.Size = new System.Drawing.Size(240, 163);
			this.m_plotHR.TabIndex = 11;
			this.m_plotHR.TimerInterval = 100;
			this.m_plotHR.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotValence
			// 
			this.m_plotValence.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotValence.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotValence.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotValence.Location = new System.Drawing.Point(496, 3);
			this.m_plotValence.Name = "m_plotValence";
			this.m_plotValence.PerfChartStyle.AntiAliasing = true;
			chartPen9.Color = System.Drawing.Color.LightGreen;
			chartPen9.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen9.Width = 1F;
			this.m_plotValence.PerfChartStyle.AvgLinePen = chartPen9;
			this.m_plotValence.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotValence.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen10.Color = System.Drawing.Color.Gold;
			chartPen10.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen10.Width = 1F;
			this.m_plotValence.PerfChartStyle.ChartLinePen = chartPen10;
			chartPen11.Color = System.Drawing.Color.DarkGray;
			chartPen11.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen11.Width = 1F;
			this.m_plotValence.PerfChartStyle.HorizontalGridPen = chartPen11;
			this.m_plotValence.PerfChartStyle.ShowAverageLine = true;
			this.m_plotValence.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotValence.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen12.Color = System.Drawing.Color.DarkGray;
			chartPen12.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen12.Width = 1F;
			this.m_plotValence.PerfChartStyle.VerticalGridPen = chartPen12;
			this.m_plotValence.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotValence.Size = new System.Drawing.Size(487, 165);
			this.m_plotValence.TabIndex = 15;
			this.m_plotValence.TimerInterval = 100;
			this.m_plotValence.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotArousal
			// 
			this.m_plotArousal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotArousal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotArousal.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotArousal.Location = new System.Drawing.Point(3, 3);
			this.m_plotArousal.Name = "m_plotArousal";
			this.m_plotArousal.PerfChartStyle.AntiAliasing = true;
			chartPen13.Color = System.Drawing.Color.LightGreen;
			chartPen13.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen13.Width = 1F;
			this.m_plotArousal.PerfChartStyle.AvgLinePen = chartPen13;
			this.m_plotArousal.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotArousal.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen14.Color = System.Drawing.Color.Gold;
			chartPen14.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen14.Width = 1F;
			this.m_plotArousal.PerfChartStyle.ChartLinePen = chartPen14;
			chartPen15.Color = System.Drawing.Color.DarkGray;
			chartPen15.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen15.Width = 1F;
			this.m_plotArousal.PerfChartStyle.HorizontalGridPen = chartPen15;
			this.m_plotArousal.PerfChartStyle.ShowAverageLine = true;
			this.m_plotArousal.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotArousal.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen16.Color = System.Drawing.Color.DarkGray;
			chartPen16.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen16.Width = 1F;
			this.m_plotArousal.PerfChartStyle.VerticalGridPen = chartPen16;
			this.m_plotArousal.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotArousal.Size = new System.Drawing.Size(487, 165);
			this.m_plotArousal.TabIndex = 13;
			this.m_plotArousal.TimerInterval = 100;
			this.m_plotArousal.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotBoredom
			// 
			this.m_plotBoredom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotBoredom.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotBoredom.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotBoredom.Location = new System.Drawing.Point(331, 3);
			this.m_plotBoredom.Name = "m_plotBoredom";
			this.m_plotBoredom.PerfChartStyle.AntiAliasing = true;
			chartPen17.Color = System.Drawing.Color.LightGreen;
			chartPen17.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen17.Width = 1F;
			this.m_plotBoredom.PerfChartStyle.AvgLinePen = chartPen17;
			this.m_plotBoredom.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotBoredom.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen18.Color = System.Drawing.Color.Gold;
			chartPen18.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen18.Width = 1F;
			this.m_plotBoredom.PerfChartStyle.ChartLinePen = chartPen18;
			chartPen19.Color = System.Drawing.Color.DarkGray;
			chartPen19.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen19.Width = 1F;
			this.m_plotBoredom.PerfChartStyle.HorizontalGridPen = chartPen19;
			this.m_plotBoredom.PerfChartStyle.ShowAverageLine = true;
			this.m_plotBoredom.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotBoredom.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen20.Color = System.Drawing.Color.DarkGray;
			chartPen20.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen20.Width = 1F;
			this.m_plotBoredom.PerfChartStyle.VerticalGridPen = chartPen20;
			this.m_plotBoredom.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotBoredom.Size = new System.Drawing.Size(322, 163);
			this.m_plotBoredom.TabIndex = 19;
			this.m_plotBoredom.TimerInterval = 100;
			this.m_plotBoredom.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotFun
			// 
			this.m_plotFun.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotFun.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotFun.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotFun.Location = new System.Drawing.Point(3, 3);
			this.m_plotFun.Name = "m_plotFun";
			this.m_plotFun.PerfChartStyle.AntiAliasing = true;
			chartPen21.Color = System.Drawing.Color.LightGreen;
			chartPen21.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen21.Width = 1F;
			this.m_plotFun.PerfChartStyle.AvgLinePen = chartPen21;
			this.m_plotFun.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotFun.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen22.Color = System.Drawing.Color.Gold;
			chartPen22.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen22.Width = 1F;
			this.m_plotFun.PerfChartStyle.ChartLinePen = chartPen22;
			chartPen23.Color = System.Drawing.Color.DarkGray;
			chartPen23.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen23.Width = 1F;
			this.m_plotFun.PerfChartStyle.HorizontalGridPen = chartPen23;
			this.m_plotFun.PerfChartStyle.ShowAverageLine = true;
			this.m_plotFun.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotFun.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen24.Color = System.Drawing.Color.DarkGray;
			chartPen24.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen24.Width = 1F;
			this.m_plotFun.PerfChartStyle.VerticalGridPen = chartPen24;
			this.m_plotFun.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotFun.Size = new System.Drawing.Size(322, 163);
			this.m_plotFun.TabIndex = 17;
			this.m_plotFun.TimerInterval = 100;
			this.m_plotFun.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotExcitement
			// 
			this.m_plotExcitement.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotExcitement.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotExcitement.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotExcitement.Location = new System.Drawing.Point(659, 3);
			this.m_plotExcitement.Name = "m_plotExcitement";
			this.m_plotExcitement.PerfChartStyle.AntiAliasing = true;
			chartPen25.Color = System.Drawing.Color.LightGreen;
			chartPen25.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen25.Width = 1F;
			this.m_plotExcitement.PerfChartStyle.AvgLinePen = chartPen25;
			this.m_plotExcitement.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotExcitement.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen26.Color = System.Drawing.Color.Gold;
			chartPen26.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen26.Width = 1F;
			this.m_plotExcitement.PerfChartStyle.ChartLinePen = chartPen26;
			chartPen27.Color = System.Drawing.Color.DarkGray;
			chartPen27.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen27.Width = 1F;
			this.m_plotExcitement.PerfChartStyle.HorizontalGridPen = chartPen27;
			this.m_plotExcitement.PerfChartStyle.ShowAverageLine = true;
			this.m_plotExcitement.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotExcitement.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen28.Color = System.Drawing.Color.DarkGray;
			chartPen28.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen28.Width = 1F;
			this.m_plotExcitement.PerfChartStyle.VerticalGridPen = chartPen28;
			this.m_plotExcitement.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotExcitement.Size = new System.Drawing.Size(324, 163);
			this.m_plotExcitement.TabIndex = 21;
			this.m_plotExcitement.TimerInterval = 100;
			this.m_plotExcitement.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotEKGFrown
			// 
			this.m_plotEKGFrown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotEKGFrown.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotEKGFrown.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotEKGFrown.Location = new System.Drawing.Point(3, 3);
			this.m_plotEKGFrown.Name = "m_plotEKGFrown";
			this.m_plotEKGFrown.PerfChartStyle.AntiAliasing = true;
			chartPen29.Color = System.Drawing.Color.LightGreen;
			chartPen29.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen29.Width = 1F;
			this.m_plotEKGFrown.PerfChartStyle.AvgLinePen = chartPen29;
			this.m_plotEKGFrown.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotEKGFrown.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen30.Color = System.Drawing.Color.Gold;
			chartPen30.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen30.Width = 1F;
			this.m_plotEKGFrown.PerfChartStyle.ChartLinePen = chartPen30;
			chartPen31.Color = System.Drawing.Color.DarkGray;
			chartPen31.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen31.Width = 1F;
			this.m_plotEKGFrown.PerfChartStyle.HorizontalGridPen = chartPen31;
			this.m_plotEKGFrown.PerfChartStyle.ShowAverageLine = true;
			this.m_plotEKGFrown.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotEKGFrown.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen32.Color = System.Drawing.Color.DarkGray;
			chartPen32.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen32.Width = 1F;
			this.m_plotEKGFrown.PerfChartStyle.VerticalGridPen = chartPen32;
			this.m_plotEKGFrown.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotEKGFrown.Size = new System.Drawing.Size(240, 163);
			this.m_plotEKGFrown.TabIndex = 25;
			this.m_plotEKGFrown.TimerInterval = 100;
			this.m_plotEKGFrown.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_plotEKGSmile
			// 
			this.m_plotEKGSmile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_plotEKGSmile.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_plotEKGSmile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_plotEKGSmile.Location = new System.Drawing.Point(249, 3);
			this.m_plotEKGSmile.Name = "m_plotEKGSmile";
			this.m_plotEKGSmile.PerfChartStyle.AntiAliasing = true;
			chartPen33.Color = System.Drawing.Color.LightGreen;
			chartPen33.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen33.Width = 1F;
			this.m_plotEKGSmile.PerfChartStyle.AvgLinePen = chartPen33;
			this.m_plotEKGSmile.PerfChartStyle.BackgroundColorBottom = System.Drawing.SystemColors.ControlDark;
			this.m_plotEKGSmile.PerfChartStyle.BackgroundColorTop = System.Drawing.SystemColors.ControlDark;
			chartPen34.Color = System.Drawing.Color.Gold;
			chartPen34.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen34.Width = 1F;
			this.m_plotEKGSmile.PerfChartStyle.ChartLinePen = chartPen34;
			chartPen35.Color = System.Drawing.Color.DarkGray;
			chartPen35.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen35.Width = 1F;
			this.m_plotEKGSmile.PerfChartStyle.HorizontalGridPen = chartPen35;
			this.m_plotEKGSmile.PerfChartStyle.ShowAverageLine = true;
			this.m_plotEKGSmile.PerfChartStyle.ShowHorizontalGridLines = true;
			this.m_plotEKGSmile.PerfChartStyle.ShowVerticalGridLines = true;
			chartPen36.Color = System.Drawing.Color.DarkGray;
			chartPen36.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			chartPen36.Width = 1F;
			this.m_plotEKGSmile.PerfChartStyle.VerticalGridPen = chartPen36;
			this.m_plotEKGSmile.ScaleMode = SpPerfChart.ScaleMode.Absolute;
			this.m_plotEKGSmile.Size = new System.Drawing.Size(240, 163);
			this.m_plotEKGSmile.TabIndex = 23;
			this.m_plotEKGSmile.TimerInterval = 100;
			this.m_plotEKGSmile.TimerMode = SpPerfChart.TimerMode.Disabled;
			// 
			// m_grpbxEmotions
			// 
			this.m_grpbxEmotions.Controls.Add(this.tableLayoutPanel2);
			this.m_grpbxEmotions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grpbxEmotions.Location = new System.Drawing.Point(0, 0);
			this.m_grpbxEmotions.Margin = new System.Windows.Forms.Padding(0);
			this.m_grpbxEmotions.Name = "m_grpbxEmotions";
			this.m_grpbxEmotions.Size = new System.Drawing.Size(992, 188);
			this.m_grpbxEmotions.TabIndex = 26;
			this.m_grpbxEmotions.TabStop = false;
			this.m_grpbxEmotions.Text = "Emotions";
			// 
			// m_grpbxAVSpace
			// 
			this.m_grpbxAVSpace.Controls.Add(this.tableLayoutPanel3);
			this.m_grpbxAVSpace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grpbxAVSpace.Location = new System.Drawing.Point(0, 376);
			this.m_grpbxAVSpace.Margin = new System.Windows.Forms.Padding(0);
			this.m_grpbxAVSpace.Name = "m_grpbxAVSpace";
			this.m_grpbxAVSpace.Size = new System.Drawing.Size(992, 190);
			this.m_grpbxAVSpace.TabIndex = 27;
			this.m_grpbxAVSpace.TabStop = false;
			this.m_grpbxAVSpace.Text = "AV Space";
			// 
			// m_grpbxSensors
			// 
			this.m_grpbxSensors.Controls.Add(this.tableLayoutPanel1);
			this.m_grpbxSensors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grpbxSensors.Location = new System.Drawing.Point(0, 188);
			this.m_grpbxSensors.Margin = new System.Windows.Forms.Padding(0);
			this.m_grpbxSensors.Name = "m_grpbxSensors";
			this.m_grpbxSensors.Size = new System.Drawing.Size(992, 188);
			this.m_grpbxSensors.TabIndex = 28;
			this.m_grpbxSensors.TabStop = false;
			this.m_grpbxSensors.Text = "Sensors";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.m_plotEKGFrown, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.m_plotEKGSmile, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.m_plotHR, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.m_plotGSR, 3, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(986, 169);
			this.tableLayoutPanel1.TabIndex = 29;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.Controls.Add(this.m_plotFun, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.m_plotBoredom, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.m_plotExcitement, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(986, 169);
			this.tableLayoutPanel2.TabIndex = 29;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Controls.Add(this.m_plotArousal, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.m_plotValence, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(986, 171);
			this.tableLayoutPanel3.TabIndex = 29;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.m_grpbxAVSpace, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.m_grpbxSensors, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.m_grpbxEmotions, 0, 0);
			this.tableLayoutPanel4.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 3;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(992, 566);
			this.tableLayoutPanel4.TabIndex = 29;
			// 
			// m_frmEmotionMonitor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1016, 623);
			this.Controls.Add(this.tableLayoutPanel4);
			this.Controls.Add(this.m_lblPlotSpeed);
			this.Controls.Add(this.m_trcbWorkerWait);
			this.Controls.Add(this.button_connect);
			this.Name = "m_frmEmotionMonitor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Emotion Monitor";
			((System.ComponentModel.ISupportInitialize)(this.m_trcbWorkerWait)).EndInit();
			this.m_grpbxEmotions.ResumeLayout(false);
			this.m_grpbxAVSpace.ResumeLayout(false);
			this.m_grpbxSensors.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button button_connect;
		private SpPerfChart.PerfChart m_plotGSR;
		private System.ComponentModel.BackgroundWorker m_backgroundWorker;
		private System.Windows.Forms.TrackBar m_trcbWorkerWait;
		private System.Windows.Forms.Label m_lblPlotSpeed;
		private SpPerfChart.PerfChart m_plotHR;
		private SpPerfChart.PerfChart m_plotValence;
		private SpPerfChart.PerfChart m_plotArousal;
		private SpPerfChart.PerfChart m_plotBoredom;
		private SpPerfChart.PerfChart m_plotFun;
		private SpPerfChart.PerfChart m_plotExcitement;
		private SpPerfChart.PerfChart m_plotEKGFrown;
		private SpPerfChart.PerfChart m_plotEKGSmile;
		private System.Windows.Forms.GroupBox m_grpbxEmotions;
		private System.Windows.Forms.GroupBox m_grpbxAVSpace;
		private System.Windows.Forms.GroupBox m_grpbxSensors;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}

