using PacketStudio.Controls;
using PacketStudio.Controls.PacketsDef;
using Syncfusion.Windows.Forms.Tools;

namespace PacketStudio
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccblivePreviewPanel = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbpacketTabsPanel = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbHexViewPanel = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Syncfusion.Windows.Forms.Tools.Office2016ColorTable office2016ColorTable1 = new Syncfusion.Windows.Forms.Tools.Office2016ColorTable();
            Syncfusion.Windows.Forms.Tools.TouchStyleColorTable touchStyleColorTable1 = new Syncfusion.Windows.Forms.Tools.TouchStyleColorTable();
            this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
            this.livePreviewPanel = new System.Windows.Forms.Panel();
            this.packetTreeView = new PacketStudio.Controls.TreeViewWithArrows();
            this.packetTabsPanel = new System.Windows.Forms.Panel();
            this.greyBorderPanel = new System.Windows.Forms.Panel();
            this.whiteBackgroundPanel = new System.Windows.Forms.Panel();
            this._packetsListDataGrid = new System.Windows.Forms.DataGridView();
            this.HexViewPanel = new System.Windows.Forms.Panel();
            this.hexViewBox = new Be.Windows.Forms.HexBox();
            this.mainPanel = new Syncfusion.Windows.Forms.Tools.DockingClientPanel();
            this.tabControl = new PacketStudio.Controls.AdvancedTabControl();
            this.tabPage1 = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.packetDefineControl1 = new PacketStudio.Controls.PacketsDef.PacketDefineControl();
            this.plusTab = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ribbonControl = new Syncfusion.Windows.Forms.Tools.RibbonControlAdv();
            this.homeToolStripTabItem = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.fileToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.csharpCopyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refactorDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.normalizeHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flattenProtocolStackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertAsciiToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.wiresharkToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.pcapToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.locateWsDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.locateWiresharkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewToolStripTabItem = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.livePreviewBasicToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.previewtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.prevDelayToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.livePrevToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.livePreviewOptionsToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.previewContextToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.packetListPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.heurDissectorsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exitOfficeButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.toolStripTabItem2 = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.statusBar = new Syncfusion.Windows.Forms.Tools.StatusBarAdv();
            this.statusTextPanel = new Syncfusion.Windows.Forms.Tools.StatusBarAdvPanel();
            this.wsVerPanel = new Syncfusion.Windows.Forms.Tools.StatusBarAdvPanel();
            this.netStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.sendToComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.sendToLabel = new System.Windows.Forms.ToolStripLabel();
            this.sendToStripButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
            this.livePreviewPanel.SuspendLayout();
            this.packetTabsPanel.SuspendLayout();
            this.greyBorderPanel.SuspendLayout();
            this.whiteBackgroundPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._packetsListDataGrid)).BeginInit();
            this.HexViewPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.ribbonControl.SuspendLayout();
            this.homeToolStripTabItem.Panel.SuspendLayout();
            this.fileToolStrip.SuspendLayout();
            this.copyToolStrip.SuspendLayout();
            this.wiresharkToolStrip.SuspendLayout();
            this.previewToolStripTabItem.Panel.SuspendLayout();
            this.livePreviewBasicToolStrip.SuspendLayout();
            this.livePreviewOptionsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBar)).BeginInit();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusTextPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsVerPanel)).BeginInit();
            this.netStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockingManager
            // 
            this.dockingManager.AnimateAutoHiddenWindow = true;
            this.dockingManager.AutoHideSelectionStyle = Syncfusion.Windows.Forms.Tools.AutoHideSelectionStyle.Click;
            this.dockingManager.AutoHideTabForeColor = System.Drawing.Color.Empty;
            this.dockingManager.DockBehavior = Syncfusion.Windows.Forms.Tools.DockBehavior.VS2010;
            this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
            this.dockingManager.DockTabPadX = 0F;
            this.dockingManager.DragProviderStyle = Syncfusion.Windows.Forms.Tools.DragProviderStyle.VS2012;
            this.dockingManager.HostControl = this;
            this.dockingManager.InActiveCaptionBackground = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(211)))), ((int)(((byte)(212))))));
            this.dockingManager.MetroButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dockingManager.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(158)))), ((int)(((byte)(218)))));
            this.dockingManager.MetroSplitterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(159)))), ((int)(((byte)(183)))));
            this.dockingManager.ReduceFlickeringInRtl = false;
            this.dockingManager.ThemesEnabled = true;
            this.dockingManager.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Metro;
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Pin, "PinButton"));
            this.dockingManager.SetDockLabel(this.livePreviewPanel, "Live Preview");
            this.dockingManager.SetEnableDocking(this.livePreviewPanel, true);
            ccblivePreviewPanel.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.livePreviewPanel, ccblivePreviewPanel);
            this.dockingManager.SetDockLabel(this.packetTabsPanel, "Packets List");
            this.dockingManager.SetEnableDocking(this.packetTabsPanel, true);
            ccbpacketTabsPanel.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.packetTabsPanel, ccbpacketTabsPanel);
            this.dockingManager.SetDockLabel(this.HexViewPanel, "Hex View");
            this.dockingManager.SetEnableDocking(this.HexViewPanel, true);
            ccbHexViewPanel.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.HexViewPanel, ccbHexViewPanel);
            // 
            // livePreviewPanel
            // 
            this.livePreviewPanel.BackColor = System.Drawing.SystemColors.Control;
            this.livePreviewPanel.Controls.Add(this.packetTreeView);
            this.livePreviewPanel.Location = new System.Drawing.Point(1, 24);
            this.livePreviewPanel.Name = "livePreviewPanel";
            this.livePreviewPanel.Padding = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.livePreviewPanel.Size = new System.Drawing.Size(455, 555);
            this.livePreviewPanel.TabIndex = 10;
            // 
            // packetTreeView
            // 
            this.packetTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packetTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.packetTreeView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packetTreeView.HideSelection = false;
            this.packetTreeView.Location = new System.Drawing.Point(0, 0);
            this.packetTreeView.Margin = new System.Windows.Forms.Padding(22);
            this.packetTreeView.Name = "packetTreeView";
            this.packetTreeView.ShowLines = false;
            this.packetTreeView.Size = new System.Drawing.Size(450, 550);
            this.packetTreeView.TabIndex = 8;
            this.packetTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PacketTreeView_AfterSelect);
            this.packetTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PacketTreeView_KeyDown);
            this.packetTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PacketTreeView_MouseClick);
            this.packetTreeView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PacketTreeView_PreviewKeyDown);
            // 
            // packetTabsPanel
            // 
            this.packetTabsPanel.Controls.Add(this.greyBorderPanel);
            this.packetTabsPanel.Location = new System.Drawing.Point(1, 24);
            this.packetTabsPanel.Name = "packetTabsPanel";
            this.packetTabsPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.packetTabsPanel.Size = new System.Drawing.Size(1394, 248);
            this.packetTabsPanel.TabIndex = 26;
            // 
            // greyBorderPanel
            // 
            this.greyBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.greyBorderPanel.Controls.Add(this.whiteBackgroundPanel);
            this.greyBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greyBorderPanel.Location = new System.Drawing.Point(5, 0);
            this.greyBorderPanel.Name = "greyBorderPanel";
            this.greyBorderPanel.Padding = new System.Windows.Forms.Padding(1);
            this.greyBorderPanel.Size = new System.Drawing.Size(1389, 243);
            this.greyBorderPanel.TabIndex = 1;
            // 
            // whiteBackgroundPanel
            // 
            this.whiteBackgroundPanel.BackColor = System.Drawing.Color.White;
            this.whiteBackgroundPanel.Controls.Add(this._packetsListDataGrid);
            this.whiteBackgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whiteBackgroundPanel.Location = new System.Drawing.Point(1, 1);
            this.whiteBackgroundPanel.Margin = new System.Windows.Forms.Padding(1);
            this.whiteBackgroundPanel.Name = "whiteBackgroundPanel";
            this.whiteBackgroundPanel.Size = new System.Drawing.Size(1387, 241);
            this.whiteBackgroundPanel.TabIndex = 1;
            // 
            // _packetsListDataGrid
            // 
            this._packetsListDataGrid.AllowUserToAddRows = false;
            this._packetsListDataGrid.AllowUserToDeleteRows = false;
            this._packetsListDataGrid.AllowUserToOrderColumns = true;
            this._packetsListDataGrid.AllowUserToResizeRows = false;
            this._packetsListDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this._packetsListDataGrid.BackgroundColor = System.Drawing.Color.White;
            this._packetsListDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._packetsListDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this._packetsListDataGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._packetsListDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._packetsListDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._packetsListDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this._packetsListDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._packetsListDataGrid.GridColor = System.Drawing.Color.White;
            this._packetsListDataGrid.Location = new System.Drawing.Point(0, 0);
            this._packetsListDataGrid.MultiSelect = false;
            this._packetsListDataGrid.Name = "_packetsListDataGrid";
            this._packetsListDataGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._packetsListDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this._packetsListDataGrid.RowHeadersVisible = false;
            this._packetsListDataGrid.RowHeadersWidth = 21;
            this._packetsListDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._packetsListDataGrid.RowTemplate.Height = 17;
            this._packetsListDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._packetsListDataGrid.ShowCellToolTips = false;
            this._packetsListDataGrid.Size = new System.Drawing.Size(1387, 241);
            this._packetsListDataGrid.TabIndex = 2;
            this._packetsListDataGrid.SelectionChanged += new System.EventHandler(this._packetsListDataGrid_SelectionChanged);
            // 
            // HexViewPanel
            // 
            this.HexViewPanel.BackColor = System.Drawing.SystemColors.Control;
            this.HexViewPanel.Controls.Add(this.hexViewBox);
            this.HexViewPanel.Location = new System.Drawing.Point(1, 24);
            this.HexViewPanel.Name = "HexViewPanel";
            this.HexViewPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.HexViewPanel.Size = new System.Drawing.Size(933, 225);
            this.HexViewPanel.TabIndex = 12;
            // 
            // hexViewBox
            // 
            this.hexViewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexViewBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.hexViewBox.HexCasing = Be.Windows.Forms.HexCasing.Lower;
            this.hexViewBox.LineInfoVisible = true;
            this.hexViewBox.Location = new System.Drawing.Point(0, 0);
            this.hexViewBox.Margin = new System.Windows.Forms.Padding(10);
            this.hexViewBox.Name = "hexViewBox";
            this.hexViewBox.Padding = new System.Windows.Forms.Padding(5);
            this.hexViewBox.ReadOnly = true;
            this.hexViewBox.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.hexViewBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexViewBox.Size = new System.Drawing.Size(933, 220);
            this.hexViewBox.TabIndex = 0;
            this.hexViewBox.UseFixedBytesPerLine = true;
            this.hexViewBox.VScrollBarVisible = true;
            this.hexViewBox.Copied += new System.EventHandler(this.HexViewBox_Copied);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainPanel.Controls.Add(this.tabControl);
            this.mainPanel.Location = new System.Drawing.Point(2, 397);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(3);
            this.mainPanel.Size = new System.Drawing.Size(935, 326);
            this.mainPanel.SizeToFit = true;
            this.mainPanel.TabIndex = 9;
            // 
            // tabControl
            // 
            this.tabControl.ActiveTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tabControl.ActiveTabForeColor = System.Drawing.Color.Empty;
            this.tabControl.AllowClose = false;
            this.tabControl.BeforeTouchSize = new System.Drawing.Size(929, 320);
            this.tabControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabControl.BorderWidth = 1;
            this.tabControl.CloseButtonForeColor = System.Drawing.Color.Empty;
            this.tabControl.CloseButtonHoverForeColor = System.Drawing.Color.Empty;
            this.tabControl.CloseButtonPressedForeColor = System.Drawing.Color.Empty;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.plusTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.FixedSingleBorderColor = System.Drawing.SystemColors.WindowFrame;
            this.tabControl.FocusOnTabClick = false;
            this.tabControl.InActiveTabForeColor = System.Drawing.Color.Empty;
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SeparatorColor = System.Drawing.SystemColors.ControlDark;
            this.tabControl.ShowSeparator = false;
            this.tabControl.Size = new System.Drawing.Size(929, 320);
            this.tabControl.TabIndex = 4;
            this.tabControl.TabPanelBackColor = System.Drawing.SystemColors.ControlLight;
            this.tabControl.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererDockingWhidbey);
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabControl_DragDrop);
            this.tabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabControl_DragEnter);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabControl_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.packetDefineControl1);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage1.Image = null;
            this.tabPage1.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPage1.Location = new System.Drawing.Point(1, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.ShowCloseButton = true;
            this.tabPage1.Size = new System.Drawing.Size(927, 293);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Packet 1";
            this.tabPage1.ThemesEnabled = false;
            // 
            // packetDefineControl1
            // 
            this.packetDefineControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packetDefineControl1.Location = new System.Drawing.Point(3, 3);
            this.packetDefineControl1.Margin = new System.Windows.Forms.Padding(5);
            this.packetDefineControl1.Name = "packetDefineControl1";
            this.packetDefineControl1.Size = new System.Drawing.Size(921, 287);
            this.packetDefineControl1.TabIndex = 0;
            // 
            // plusTab
            // 
            this.plusTab.BackColor = System.Drawing.SystemColors.Control;
            this.plusTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.plusTab.Image = null;
            this.plusTab.ImageSize = new System.Drawing.Size(16, 16);
            this.plusTab.Location = new System.Drawing.Point(1, 26);
            this.plusTab.Name = "plusTab";
            this.plusTab.Padding = new System.Windows.Forms.Padding(3);
            this.plusTab.ShowCloseButton = true;
            this.plusTab.Size = new System.Drawing.Size(927, 293);
            this.plusTab.TabIndex = 1;
            this.plusTab.Text = "+";
            this.plusTab.ThemesEnabled = false;
            // 
            // ribbonControl
            // 
            this.ribbonControl.CaptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonControl.Dock = Syncfusion.Windows.Forms.Tools.DockStyleEx.Top;
            this.ribbonControl.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ribbonControl.Header.AddMainItem(homeToolStripTabItem);
            this.ribbonControl.Header.AddMainItem(previewToolStripTabItem);
            this.ribbonControl.LauncherStyle = Syncfusion.Windows.Forms.Tools.LauncherStyle.Metro;
            this.ribbonControl.Location = new System.Drawing.Point(2, 0);
            this.ribbonControl.MenuButtonFont = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ribbonControl.MenuButtonText = "";
            this.ribbonControl.MenuButtonVisible = false;
            this.ribbonControl.MenuButtonWidth = 56;
            this.ribbonControl.MenuColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Office2016ColorTable.Add(office2016ColorTable1);
            this.ribbonControl.OfficeColorScheme = Syncfusion.Windows.Forms.Tools.ToolStripEx.ColorScheme.Black;
            // 
            // ribbonControl.OfficeMenu
            // 
            this.ribbonControl.OfficeMenu.MainPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitOfficeButton});
            this.ribbonControl.OfficeMenu.Name = "OfficeMenu";
            this.ribbonControl.OfficeMenu.ShowItemToolTips = true;
            this.ribbonControl.OfficeMenu.Size = new System.Drawing.Size(114, 71);
            this.ribbonControl.QuickPanelImageLayout = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ribbonControl.RibbonHeaderImage = Syncfusion.Windows.Forms.Tools.RibbonHeaderImage.None;
            this.ribbonControl.RibbonStyle = Syncfusion.Windows.Forms.Tools.RibbonStyle.TouchStyle;
            touchStyleColorTable1.ActiveToolStripTabItemBackColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123)))));
            touchStyleColorTable1.BackStageButtonForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201)))));
            touchStyleColorTable1.BackStageCaptionColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageCloseButtonBackground = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            touchStyleColorTable1.BackStageCloseButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            touchStyleColorTable1.BackStageCloseButtonHoverForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageCloseButtonPressedForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageMaximizeButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            touchStyleColorTable1.BackStageMaximizeButtonHoverForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageMaximizeButtonPressedForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageMinimizeButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            touchStyleColorTable1.BackStageMinimizeButtonHoverForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageMinimizeButtonPressedForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageNavigationButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123)))));
            touchStyleColorTable1.BackStageNavigationButtonForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageRestoreButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            touchStyleColorTable1.BackStageRestoreButtonHoverForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageRestoreButtonPressedForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageSysytemButtonBackground = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.BackStageTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201)))));
            touchStyleColorTable1.BackStageTabForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.BackStageTabHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201)))));
            touchStyleColorTable1.BottomToolStripBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ButtonCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            touchStyleColorTable1.ButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            touchStyleColorTable1.ButtonPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            touchStyleColorTable1.CheckBoxForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            touchStyleColorTable1.CheckedToolstripTabItemForeColor = System.Drawing.Color.Black;
            touchStyleColorTable1.CloseButtonBackground = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            touchStyleColorTable1.CloseButtonForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.CloseButtonHoverForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.CloseButtonPressed = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            touchStyleColorTable1.CloseButtonPressedForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.DropDownBodyColor = System.Drawing.Color.White;
            touchStyleColorTable1.DropDownMenuItemBackground = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201)))));
            touchStyleColorTable1.DropDownSelectedTextForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.DropDownTextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.DropDownTitleBackground = System.Drawing.Color.White;
            touchStyleColorTable1.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.HoverTabBackColor = System.Drawing.Color.White;
            touchStyleColorTable1.HoverTabForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ImageMargin = System.Drawing.Color.White;
            touchStyleColorTable1.InActiveToolStripTabItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.MaximizeButtonForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.MaximizeButtonHoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.MaximizeButtonPressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.MenuButtonArrowColor = System.Drawing.Color.White;
            touchStyleColorTable1.MenuButtonHoverArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201)))));
            touchStyleColorTable1.MinimizeButtonForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.MinimizeButtonHoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.MinimizeButtonPressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.OverFlowArrowColor = System.Drawing.Color.White;
            touchStyleColorTable1.QATButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            touchStyleColorTable1.QATDownArrowColor = System.Drawing.Color.White;
            touchStyleColorTable1.RestoreButtonForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.RestoreButtonHoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.RestoreButtonPressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.RibbonPanelBackColor = System.Drawing.Color.White;
            touchStyleColorTable1.SplitButtonPressed = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.SplitButtonSelected = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201)))));
            touchStyleColorTable1.SystemButtonBackground = System.Drawing.Color.White;
            touchStyleColorTable1.SystemButtonPressed = System.Drawing.Color.White;
            touchStyleColorTable1.TabGroupColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(203)))), ((int)(((byte)(29)))));
            touchStyleColorTable1.TabScrollArrowColor = System.Drawing.Color.White;
            touchStyleColorTable1.TitleColor = System.Drawing.Color.White;
            touchStyleColorTable1.ToolstripActiveTabItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ToolStripArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ToolStripBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ToolstripButtonPressedBorder = System.Drawing.Color.Black;
            touchStyleColorTable1.ToolStripDropDownBackColor = System.Drawing.Color.White;
            touchStyleColorTable1.ToolStripDropDownButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            touchStyleColorTable1.ToolStripDropDownButtonSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            touchStyleColorTable1.ToolstripSelectedTabItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ToolStripSpliterColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ToolstripTabItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            touchStyleColorTable1.ToolstripTabItemCheckedGradientBegin = System.Drawing.Color.Empty;
            touchStyleColorTable1.ToolstripTabItemForeColor = System.Drawing.Color.White;
            touchStyleColorTable1.ToolstripTabItemSelectedGradientBegin = System.Drawing.Color.Empty;
            this.ribbonControl.RibbonTouchStyleColorTable.Add(touchStyleColorTable1);
            this.ribbonControl.SelectedTab = this.homeToolStripTabItem;
            this.ribbonControl.ShowQuickItemsDropDownButton = false;
            this.ribbonControl.ShowRibbonDisplayOptionButton = false;
            this.ribbonControl.Size = new System.Drawing.Size(1396, 120);
            this.ribbonControl.SystemText.QuickAccessDialogDropDownName = "Start menu";
            this.ribbonControl.SystemText.RenameDisplayLabelText = "&Display Name:";
            this.ribbonControl.TabIndex = 5;
            this.ribbonControl.Text = "ribbonControlAdv1";
            this.ribbonControl.TitleColor = System.Drawing.Color.White;
            // 
            // homeToolStripTabItem
            // 
            this.homeToolStripTabItem.Name = "homeToolStripTabItem";
            // 
            // ribbonControl.ribbonPanel1
            // 
            this.homeToolStripTabItem.Panel.Controls.Add(this.fileToolStrip);
            this.homeToolStripTabItem.Panel.Controls.Add(this.copyToolStrip);
            this.homeToolStripTabItem.Panel.Controls.Add(this.wiresharkToolStrip);
            this.homeToolStripTabItem.Panel.Controls.Add(this.netStrip);
            this.homeToolStripTabItem.Panel.Name = "ribbonPanel1";
            this.homeToolStripTabItem.Panel.ScrollPosition = 0;
            this.homeToolStripTabItem.Panel.ShowCaption = false;
            this.homeToolStripTabItem.Panel.ShowLauncher = false;
            this.homeToolStripTabItem.Panel.TabIndex = 2;
            this.homeToolStripTabItem.Panel.Text = "Home";
            this.homeToolStripTabItem.Position = 0;
            this.homeToolStripTabItem.Size = new System.Drawing.Size(58, 29);
            this.homeToolStripTabItem.Tag = "1";
            this.homeToolStripTabItem.Text = "Home";
            // 
            // fileToolStrip
            // 
            this.fileToolStrip.AutoSize = false;
            this.fileToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.fileToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.fileToolStrip.ForeColor = System.Drawing.Color.Black;
            this.fileToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.fileToolStrip.Image = null;
            this.fileToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.fileToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton});
            this.fileToolStrip.Location = new System.Drawing.Point(0, 1);
            this.fileToolStrip.Name = "fileToolStrip";
            this.fileToolStrip.Office12Mode = false;
            this.fileToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.fileToolStrip.ShowCaption = false;
            this.fileToolStrip.Size = new System.Drawing.Size(147, 65);
            this.fileToolStrip.TabIndex = 0;
            this.fileToolStrip.Text = "File Tool Strip";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.Image = global::PacketStudio.Properties.Resources.na_new;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.newToolStripButton.Name = "newToolStripButton";
            this.SetShortcut(this.newToolStripButton, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N))));
            this.newToolStripButton.Size = new System.Drawing.Size(36, 56);
            this.newToolStripButton.Text = "New";
            this.newToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.newToolStripButton.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.Image = global::PacketStudio.Properties.Resources.na_open;
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.openToolStripButton.Name = "openToolStripButton";
            this.SetShortcut(this.openToolStripButton, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O))));
            this.openToolStripButton.Size = new System.Drawing.Size(40, 56);
            this.openToolStripButton.Text = "Open";
            this.openToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.openToolStripButton.Click += new System.EventHandler(this.LoadFileToolStripMenuItem_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.Image = global::PacketStudio.Properties.Resources.na_save;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.SetShortcut(this.saveToolStripButton, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S))));
            this.saveToolStripButton.Size = new System.Drawing.Size(36, 56);
            this.saveToolStripButton.Text = "Save";
            this.saveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.saveToolStripButton.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // copyToolStrip
            // 
            this.copyToolStrip.AutoSize = false;
            this.copyToolStrip.BorderStyle = Syncfusion.Windows.Forms.Tools.ToolStripBorderStyle.Etched;
            this.copyToolStrip.CaptionStyle = Syncfusion.Windows.Forms.Tools.CaptionStyle.Bottom;
            this.copyToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.copyToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.copyToolStrip.ForeColor = System.Drawing.Color.Black;
            this.copyToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.copyToolStrip.Image = null;
            this.copyToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.copyToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.csharpCopyToolStripButton,
            this.refactorDropDownButton,
            this.insertAsciiToolStripButton});
            this.copyToolStrip.Location = new System.Drawing.Point(149, 1);
            this.copyToolStrip.Name = "copyToolStrip";
            this.copyToolStrip.Office12Mode = false;
            this.copyToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.copyToolStrip.ShowCaption = false;
            this.copyToolStrip.ShowLauncher = false;
            this.copyToolStrip.Size = new System.Drawing.Size(229, 65);
            this.copyToolStrip.TabIndex = 1;
            this.copyToolStrip.Text = "Copy Tool Strip";
            // 
            // csharpCopyToolStripButton
            // 
            this.csharpCopyToolStripButton.Image = global::PacketStudio.Properties.Resources.csharp;
            this.csharpCopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.csharpCopyToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.csharpCopyToolStripButton.Name = "csharpCopyToolStripButton";
            this.csharpCopyToolStripButton.Size = new System.Drawing.Size(74, 56);
            this.csharpCopyToolStripButton.Text = "Copy For C#";
            this.csharpCopyToolStripButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.csharpCopyToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.csharpCopyToolStripButton.Click += new System.EventHandler(this.CopyForCToolStripMenuItem_Click);
            // 
            // refactorDropDownButton
            // 
            this.refactorDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalizeHexToolStripMenuItem,
            this.flattenProtocolStackToolStripMenuItem});
            this.refactorDropDownButton.Image = global::PacketStudio.Properties.Resources.hammer500;
            this.refactorDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refactorDropDownButton.Name = "refactorDropDownButton";
            this.refactorDropDownButton.Size = new System.Drawing.Size(72, 57);
            this.refactorDropDownButton.Text = "Refactor...";
            this.refactorDropDownButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // normalizeHexToolStripMenuItem
            // 
            this.normalizeHexToolStripMenuItem.Name = "normalizeHexToolStripMenuItem";
            this.normalizeHexToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.normalizeHexToolStripMenuItem.Text = "Normalize Hex";
            this.normalizeHexToolStripMenuItem.Click += new System.EventHandler(this.NormalizeHexToolStripMenuItem_Click_1);
            // 
            // flattenProtocolStackToolStripMenuItem
            // 
            this.flattenProtocolStackToolStripMenuItem.Name = "flattenProtocolStackToolStripMenuItem";
            this.flattenProtocolStackToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.flattenProtocolStackToolStripMenuItem.Text = "Flatten Protocol Stack";
            this.flattenProtocolStackToolStripMenuItem.Click += new System.EventHandler(this.FlattenProtocolStackToolStripMenuItem_Click_1);
            // 
            // insertAsciiToolStripButton
            // 
            this.insertAsciiToolStripButton.Image = global::PacketStudio.Properties.Resources.text_box;
            this.insertAsciiToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.insertAsciiToolStripButton.Name = "insertAsciiToolStripButton";
            this.insertAsciiToolStripButton.Size = new System.Drawing.Size(69, 57);
            this.insertAsciiToolStripButton.Text = "Insert ASCII";
            this.insertAsciiToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.insertAsciiToolStripButton.Click += new System.EventHandler(this.insertAsciiToolStripButton_Click);
            // 
            // wiresharkToolStrip
            // 
            this.wiresharkToolStrip.AutoSize = false;
            this.wiresharkToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.wiresharkToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.wiresharkToolStrip.ForeColor = System.Drawing.Color.Black;
            this.wiresharkToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.wiresharkToolStrip.Image = null;
            this.wiresharkToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.wiresharkToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pcapToolStripButton,
            this.locateWsDropDownButton});
            this.wiresharkToolStrip.Location = new System.Drawing.Point(380, 1);
            this.wiresharkToolStrip.Name = "wiresharkToolStrip";
            this.wiresharkToolStrip.Office12Mode = false;
            this.wiresharkToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.wiresharkToolStrip.ShowCaption = false;
            this.wiresharkToolStrip.ShowLauncher = false;
            this.wiresharkToolStrip.Size = new System.Drawing.Size(215, 65);
            this.wiresharkToolStrip.TabIndex = 2;
            this.wiresharkToolStrip.Text = "Wireshark Tool Strip";
            // 
            // pcapToolStripButton
            // 
            this.pcapToolStripButton.Image = global::PacketStudio.Properties.Resources.WS;
            this.pcapToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pcapToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.pcapToolStripButton.Name = "pcapToolStripButton";
            this.pcapToolStripButton.Padding = new System.Windows.Forms.Padding(11, 0, 11, 0);
            this.pcapToolStripButton.Size = new System.Drawing.Size(60, 56);
            this.pcapToolStripButton.Text = "Pcap!";
            this.pcapToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pcapToolStripButton.Click += new System.EventHandler(this.GeneratePcapButton_Click);
            // 
            // locateWsDropDownButton
            // 
            this.locateWsDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.locateWiresharkToolStripMenuItem});
            this.locateWsDropDownButton.Image = global::PacketStudio.Properties.Resources.ws_dir1;
            this.locateWsDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.locateWsDropDownButton.Name = "locateWsDropDownButton";
            this.locateWsDropDownButton.Size = new System.Drawing.Size(108, 57);
            this.locateWsDropDownButton.Text = "Locate Wireshark";
            this.locateWsDropDownButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // locateWiresharkToolStripMenuItem
            // 
            this.locateWiresharkToolStripMenuItem.Name = "locateWiresharkToolStripMenuItem";
            this.locateWiresharkToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.locateWiresharkToolStripMenuItem.Text = "Locate...";
            this.locateWiresharkToolStripMenuItem.Click += new System.EventHandler(this.LocateWireshark_Click);
            // 
            // previewToolStripTabItem
            // 
            this.previewToolStripTabItem.Name = "previewToolStripTabItem";
            // 
            // ribbonControl.ribbonPanel2
            // 
            this.previewToolStripTabItem.Panel.Controls.Add(this.livePreviewBasicToolStrip);
            this.previewToolStripTabItem.Panel.Controls.Add(this.livePreviewOptionsToolStrip);
            this.previewToolStripTabItem.Panel.Name = "ribbonPanel2";
            this.previewToolStripTabItem.Panel.ScrollPosition = 0;
            this.previewToolStripTabItem.Panel.TabIndex = 4;
            this.previewToolStripTabItem.Panel.Text = "Live Preview";
            this.previewToolStripTabItem.Position = 1;
            this.previewToolStripTabItem.Size = new System.Drawing.Size(89, 29);
            this.previewToolStripTabItem.Text = "Live Preview";
            // 
            // livePreviewBasicToolStrip
            // 
            this.livePreviewBasicToolStrip.AutoSize = false;
            this.livePreviewBasicToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.livePreviewBasicToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.livePreviewBasicToolStrip.ForeColor = System.Drawing.Color.Black;
            this.livePreviewBasicToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.livePreviewBasicToolStrip.Image = null;
            this.livePreviewBasicToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.livePreviewBasicToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewtoolStripButton,
            this.prevDelayToolStripLabel,
            this.livePrevToolStripTextBox});
            this.livePreviewBasicToolStrip.Location = new System.Drawing.Point(0, 1);
            this.livePreviewBasicToolStrip.Name = "livePreviewBasicToolStrip";
            this.livePreviewBasicToolStrip.Office12Mode = false;
            this.livePreviewBasicToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.livePreviewBasicToolStrip.ShowCaption = false;
            this.livePreviewBasicToolStrip.ShowLauncher = false;
            this.livePreviewBasicToolStrip.Size = new System.Drawing.Size(294, 65);
            this.livePreviewBasicToolStrip.TabIndex = 3;
            this.livePreviewBasicToolStrip.Text = "Live Preview Tool Strip";
            // 
            // previewtoolStripButton
            // 
            this.previewtoolStripButton.Checked = true;
            this.previewtoolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewtoolStripButton.Image = global::PacketStudio.Properties.Resources.preview;
            this.previewtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewtoolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.previewtoolStripButton.Name = "previewtoolStripButton";
            this.previewtoolStripButton.Size = new System.Drawing.Size(88, 56);
            this.previewtoolStripButton.Text = "Enable Preview";
            this.previewtoolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.previewtoolStripButton.Click += new System.EventHandler(this.LivePreviewToolStripMenuItem_Click);
            // 
            // prevDelayToolStripLabel
            // 
            this.prevDelayToolStripLabel.Margin = new System.Windows.Forms.Padding(2);
            this.prevDelayToolStripLabel.Name = "prevDelayToolStripLabel";
            this.prevDelayToolStripLabel.Size = new System.Drawing.Size(80, 56);
            this.prevDelayToolStripLabel.Text = "Preview Delay:";
            // 
            // livePrevToolStripTextBox
            // 
            this.livePrevToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.livePrevToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.livePrevToolStripTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.livePrevToolStripTextBox.Name = "livePrevToolStripTextBox";
            this.livePrevToolStripTextBox.Size = new System.Drawing.Size(100, 56);
            this.livePrevToolStripTextBox.TextChanged += new System.EventHandler(this.LivePreviewDelayBox_TextChanged);
            // 
            // livePreviewOptionsToolStrip
            // 
            this.livePreviewOptionsToolStrip.AutoSize = false;
            this.livePreviewOptionsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.livePreviewOptionsToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.livePreviewOptionsToolStrip.ForeColor = System.Drawing.Color.Black;
            this.livePreviewOptionsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.livePreviewOptionsToolStrip.Image = null;
            this.livePreviewOptionsToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.livePreviewOptionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewContextToolStripButton,
            this.packetListPreviewToolStripButton,
            this.heurDissectorsToolStripButton});
            this.livePreviewOptionsToolStrip.Location = new System.Drawing.Point(296, 1);
            this.livePreviewOptionsToolStrip.Name = "livePreviewOptionsToolStrip";
            this.livePreviewOptionsToolStrip.Office12Mode = false;
            this.livePreviewOptionsToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.livePreviewOptionsToolStrip.ShowCaption = false;
            this.livePreviewOptionsToolStrip.ShowLauncher = false;
            this.livePreviewOptionsToolStrip.Size = new System.Drawing.Size(406, 65);
            this.livePreviewOptionsToolStrip.TabIndex = 3;
            this.livePreviewOptionsToolStrip.Text = "Live Preview Tool Strip";
            // 
            // previewContextToolStripButton
            // 
            this.previewContextToolStripButton.Checked = true;
            this.previewContextToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewContextToolStripButton.Image = global::PacketStudio.Properties.Resources.preview_cntx;
            this.previewContextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewContextToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.previewContextToolStripButton.Name = "previewContextToolStripButton";
            this.previewContextToolStripButton.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.previewContextToolStripButton.Size = new System.Drawing.Size(111, 56);
            this.previewContextToolStripButton.Text = "Preview In Context";
            this.previewContextToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.previewContextToolStripButton.Click += new System.EventHandler(this.previewInBatPContextToolStripMenuItem_Click);
            // 
            // packetListPreviewToolStripButton
            // 
            this.packetListPreviewToolStripButton.Image = global::PacketStudio.Properties.Resources.bulleted_list;
            this.packetListPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.packetListPreviewToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.packetListPreviewToolStripButton.Name = "packetListPreviewToolStripButton";
            this.packetListPreviewToolStripButton.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.packetListPreviewToolStripButton.Size = new System.Drawing.Size(128, 56);
            this.packetListPreviewToolStripButton.Text = "Get Packet List Details";
            this.packetListPreviewToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.packetListPreviewToolStripButton.ToolTipText = "Experimental!";
            this.packetListPreviewToolStripButton.Click += new System.EventHandler(this.packetListPreviewToolStripButton_Click);
            // 
            // heurDissectorsToolStripButton
            // 
            this.heurDissectorsToolStripButton.Image = global::PacketStudio.Properties.Resources.evidence;
            this.heurDissectorsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.heurDissectorsToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.heurDissectorsToolStripButton.Name = "heurDissectorsToolStripButton";
            this.heurDissectorsToolStripButton.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.heurDissectorsToolStripButton.Size = new System.Drawing.Size(116, 56);
            this.heurDissectorsToolStripButton.Text = "Heuristic Dissectors";
            this.heurDissectorsToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.heurDissectorsToolStripButton.Click += new System.EventHandler(this.heurDissectorsToolStripButton_Click);
            // 
            // exitOfficeButton
            // 
            this.exitOfficeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exitOfficeButton.Image = ((System.Drawing.Image)(resources.GetObject("exitOfficeButton.Image")));
            this.exitOfficeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitOfficeButton.Name = "exitOfficeButton";
            this.exitOfficeButton.Padding = new System.Windows.Forms.Padding(10, 0, 60, 0);
            this.exitOfficeButton.Size = new System.Drawing.Size(102, 23);
            this.exitOfficeButton.Text = "Exit";
            this.exitOfficeButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exitOfficeButton.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolStripTabItem2
            // 
            this.toolStripTabItem2.Name = "toolStripTabItem2";
            // 
            // 
            // 
            this.toolStripTabItem2.Panel.Name = "";
            this.toolStripTabItem2.Panel.ScrollPosition = 0;
            this.toolStripTabItem2.Panel.TabIndex = 3;
            this.toolStripTabItem2.Panel.Text = "toolStripTabItem2";
            this.toolStripTabItem2.Position = -1;
            this.toolStripTabItem2.Size = new System.Drawing.Size(127, 30);
            this.toolStripTabItem2.Text = "toolStripTabItem2";
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.statusBar.BeforeTouchSize = new System.Drawing.Size(1396, 22);
            this.statusBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.statusBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusBar.Controls.Add(this.statusTextPanel);
            this.statusBar.Controls.Add(this.wsVerPanel);
            this.statusBar.CustomLayoutBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.ForeColor = System.Drawing.Color.White;
            this.statusBar.Location = new System.Drawing.Point(2, 977);
            this.statusBar.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.statusBar.Name = "statusBar";
            this.statusBar.Padding = new System.Windows.Forms.Padding(3);
            this.statusBar.Size = new System.Drawing.Size(1396, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.Spacing = new System.Drawing.Size(2, 2);
            this.statusBar.Style = Syncfusion.Windows.Forms.Tools.StatusbarStyle.Metro;
            this.statusBar.TabIndex = 26;
            // 
            // statusTextPanel
            // 
            this.statusTextPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.statusTextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.statusTextPanel.BeforeTouchSize = new System.Drawing.Size(41, 16);
            this.statusTextPanel.Border3DStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.statusTextPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.statusTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusTextPanel.ForeColor = System.Drawing.Color.White;
            this.statusTextPanel.Location = new System.Drawing.Point(0, 2);
            this.statusTextPanel.Margin = new System.Windows.Forms.Padding(0);
            this.statusTextPanel.Name = "statusTextPanel";
            this.statusTextPanel.Size = new System.Drawing.Size(41, 16);
            this.statusTextPanel.SizeToContent = true;
            this.statusTextPanel.TabIndex = 0;
            this.statusTextPanel.Text = "Status";
            this.statusTextPanel.Click += new System.EventHandler(this.statusTextPanel_Click);
            // 
            // wsVerPanel
            // 
            this.wsVerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.wsVerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.wsVerPanel.BeforeTouchSize = new System.Drawing.Size(48, 16);
            this.wsVerPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255)))));
            this.wsVerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wsVerPanel.ForeColor = System.Drawing.Color.White;
            this.wsVerPanel.HAlign = Syncfusion.Windows.Forms.Tools.HorzFlowAlign.Right;
            this.wsVerPanel.Location = new System.Drawing.Point(1339, 2);
            this.wsVerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.wsVerPanel.Name = "wsVerPanel";
            this.wsVerPanel.Size = new System.Drawing.Size(48, 16);
            this.wsVerPanel.SizeToContent = true;
            this.wsVerPanel.TabIndex = 1;
            this.wsVerPanel.Text = "Version";
            // 
            // netStrip
            // 
            this.netStrip.AutoSize = false;
            this.netStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.netStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.netStrip.ForeColor = System.Drawing.Color.MidnightBlue;
            this.netStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.netStrip.Image = null;
            this.netStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToLabel,
            this.sendToComboBox,
            this.sendToStripButton});
            this.netStrip.Location = new System.Drawing.Point(597, 1);
            this.netStrip.Name = "netStrip";
            this.netStrip.Office12Mode = false;
            this.netStrip.Size = new System.Drawing.Size(243, 65);
            this.netStrip.TabIndex = 3;
            // 
            // sendToComboBox
            // 
            this.sendToComboBox.Name = "sendToComboBox";
            this.sendToComboBox.Size = new System.Drawing.Size(121, 65);
            // 
            // sendToLabel
            // 
            this.sendToLabel.Name = "sendToLabel";
            this.sendToLabel.Size = new System.Drawing.Size(50, 62);
            this.sendToLabel.Text = "Send to:";
            // 
            // sendToStripButton
            // 
            this.sendToStripButton.Image = ((System.Drawing.Image)(resources.GetObject("sendToStripButton.Image")));
            this.sendToStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sendToStripButton.Name = "sendToStripButton";
            this.sendToStripButton.Size = new System.Drawing.Size(40, 62);
            this.sendToStripButton.Text = "Send!";
            this.sendToStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.sendToStripButton.Click += new System.EventHandler(this.sendToStripButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 1000);
            this.ColorScheme = Syncfusion.Windows.Forms.Tools.RibbonForm.ColorSchemeType.Black;
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.ribbonControl);
            this.Controls.Add(this.mainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 280);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.ShowApplicationIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainFormLoaded);
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
            this.livePreviewPanel.ResumeLayout(false);
            this.packetTabsPanel.ResumeLayout(false);
            this.greyBorderPanel.ResumeLayout(false);
            this.whiteBackgroundPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._packetsListDataGrid)).EndInit();
            this.HexViewPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.ribbonControl.ResumeLayout(false);
            this.ribbonControl.PerformLayout();
            this.homeToolStripTabItem.Panel.ResumeLayout(false);
            this.fileToolStrip.ResumeLayout(false);
            this.fileToolStrip.PerformLayout();
            this.copyToolStrip.ResumeLayout(false);
            this.copyToolStrip.PerformLayout();
            this.wiresharkToolStrip.ResumeLayout(false);
            this.wiresharkToolStrip.PerformLayout();
            this.previewToolStripTabItem.Panel.ResumeLayout(false);
            this.livePreviewBasicToolStrip.ResumeLayout(false);
            this.livePreviewBasicToolStrip.PerformLayout();
            this.livePreviewOptionsToolStrip.ResumeLayout(false);
            this.livePreviewOptionsToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBar)).EndInit();
            this.statusBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusTextPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wsVerPanel)).EndInit();
            this.netStrip.ResumeLayout(false);
            this.netStrip.PerformLayout();
            this.ResumeLayout(false);

        }

	    #endregion
		private Syncfusion.Windows.Forms.Tools.DockingClientPanel mainPanel;
		private Syncfusion.Windows.Forms.Tools.DockingManager dockingManager;
		private System.Windows.Forms.Panel livePreviewPanel;
		private Syncfusion.Windows.Forms.Tools.RibbonControlAdv ribbonControl;
		private Syncfusion.Windows.Forms.Tools.ToolStripTabItem homeToolStripTabItem;
		private TreeViewWithArrows packetTreeView;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx fileToolStrip;
		private System.Windows.Forms.ToolStripButton newToolStripButton;
		private System.Windows.Forms.ToolStripButton openToolStripButton;
		private System.Windows.Forms.ToolStripButton saveToolStripButton;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx copyToolStrip;
		private System.Windows.Forms.ToolStripButton csharpCopyToolStripButton;
		private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolStripTabItem2;
		private Syncfusion.Windows.Forms.Tools.ToolStripTabItem previewToolStripTabItem;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx livePreviewBasicToolStrip;
		private System.Windows.Forms.ToolStripLabel prevDelayToolStripLabel;
		private System.Windows.Forms.ToolStripTextBox livePrevToolStripTextBox;
		private Syncfusion.Windows.Forms.Tools.OfficeButton exitOfficeButton;
		private System.Windows.Forms.ToolStripButton previewtoolStripButton;
		private System.Windows.Forms.ToolStripButton previewContextToolStripButton;
		private AdvancedTabControl tabControl;
		private TabPageAdv tabPage1;
		private PacketDefineControl packetDefineControl1;
		private TabPageAdv plusTab;
		private System.Windows.Forms.Panel HexViewPanel;
		private Be.Windows.Forms.HexBox hexViewBox;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx wiresharkToolStrip;
		private System.Windows.Forms.ToolStripButton pcapToolStripButton;
		private System.Windows.Forms.Panel packetTabsPanel;
        private System.Windows.Forms.ToolStripDropDownButton refactorDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem normalizeHexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flattenProtocolStackToolStripMenuItem;
        private System.Windows.Forms.Panel greyBorderPanel;
        private System.Windows.Forms.Panel whiteBackgroundPanel;
        private Syncfusion.Windows.Forms.Tools.StatusBarAdv statusBar;
        private Syncfusion.Windows.Forms.Tools.StatusBarAdvPanel statusTextPanel;
        private Syncfusion.Windows.Forms.Tools.StatusBarAdvPanel wsVerPanel;
        private System.Windows.Forms.ToolStripButton insertAsciiToolStripButton;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx livePreviewOptionsToolStrip;
        private System.Windows.Forms.ToolStripButton packetListPreviewToolStripButton;
        private System.Windows.Forms.ToolStripButton heurDissectorsToolStripButton;
        private System.Windows.Forms.DataGridView _packetsListDataGrid;
        private System.Windows.Forms.ToolStripDropDownButton locateWsDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem locateWiresharkToolStripMenuItem;
        private ToolStripEx netStrip;
        private System.Windows.Forms.ToolStripLabel sendToLabel;
        private System.Windows.Forms.ToolStripComboBox sendToComboBox;
        private System.Windows.Forms.ToolStripButton sendToStripButton;
    }
}

