using PacketStudio.Controls;
using PacketStudio.Controls.PacketsDef;

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
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbstatusPanel = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbHexViewPanel = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.Office2016ColorTable office2016ColorTable1 = new Syncfusion.Windows.Forms.Tools.Office2016ColorTable();
            this.dockingManager1 = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
            this.livePreviewPanel = new System.Windows.Forms.Panel();
            this.packetTreeView = new PacketStudio.Controls.TreeViewWithArrows();
            this.packetTabsPanel = new System.Windows.Forms.Panel();
            this.packetTabsList = new System.Windows.Forms.ListBox();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.livePrevStatusPanel = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.livePreviewTextBox = new System.Windows.Forms.Label();
            this.HexViewPanel = new System.Windows.Forms.Panel();
            this.hexViewBox = new Be.Windows.Forms.HexBox();
            this.dockingClientPanel1 = new Syncfusion.Windows.Forms.Tools.DockingClientPanel();
            this.tabControl = new PacketStudio.Controls.AdvancedTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.packetDefineControl1 = new PacketStudio.Controls.PacketsDef.PacketDefineControl();
            this.plusTab = new System.Windows.Forms.TabPage();
            this.ribbonControlAdv1 = new Syncfusion.Windows.Forms.Tools.RibbonControlAdv();
            this.homeToolStripTabItem = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.newOpenSaveToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.wiresharkToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.pcapToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.previewToolStripTabItem = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.livePreviewToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.previewtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewContextToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.livePrevToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.exitOfficeButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.toolStripTabItem2 = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager1)).BeginInit();
            this.livePreviewPanel.SuspendLayout();
            this.packetTabsPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.livePrevStatusPanel)).BeginInit();
            this.livePrevStatusPanel.SuspendLayout();
            this.HexViewPanel.SuspendLayout();
            this.dockingClientPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlAdv1)).BeginInit();
            this.ribbonControlAdv1.SuspendLayout();
            this.homeToolStripTabItem.Panel.SuspendLayout();
            this.newOpenSaveToolStrip.SuspendLayout();
            this.copyToolStrip.SuspendLayout();
            this.wiresharkToolStrip.SuspendLayout();
            this.previewToolStripTabItem.Panel.SuspendLayout();
            this.livePreviewToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockingManager1
            // 
            this.dockingManager1.ActiveCaptionFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager1.AnimateAutoHiddenWindow = true;
            this.dockingManager1.AutoHideSelectionStyle = Syncfusion.Windows.Forms.Tools.AutoHideSelectionStyle.Click;
            this.dockingManager1.AutoHideTabForeColor = System.Drawing.Color.Empty;
            this.dockingManager1.DockBehavior = Syncfusion.Windows.Forms.Tools.DockBehavior.VS2010;
            this.dockingManager1.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager1.DockLayoutStream")));
            this.dockingManager1.DockTabForeColor = System.Drawing.Color.Empty;
            this.dockingManager1.DockTabPadX = 0F;
            this.dockingManager1.DragProviderStyle = Syncfusion.Windows.Forms.Tools.DragProviderStyle.VS2012;
            this.dockingManager1.HostControl = this;
            this.dockingManager1.InActiveCaptionBackground = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(211)))), ((int)(((byte)(212))))));
            this.dockingManager1.InActiveCaptionFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager1.MetroButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dockingManager1.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(158)))), ((int)(((byte)(218)))));
            this.dockingManager1.MetroSplitterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(159)))), ((int)(((byte)(183)))));
            this.dockingManager1.ReduceFlickeringInRtl = false;
            this.dockingManager1.ThemesEnabled = true;
            this.dockingManager1.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Metro;
            this.dockingManager1.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Pin, "PinButton"));
            this.dockingManager1.SetDockLabel(this.livePreviewPanel, "Live Preview");
            this.dockingManager1.SetEnableDocking(this.livePreviewPanel, true);
            ccblivePreviewPanel.MergeWith(this.dockingManager1.CaptionButtons, false);
            this.dockingManager1.SetCustomCaptionButtons(this.livePreviewPanel, ccblivePreviewPanel);
            this.dockingManager1.SetDockLabel(this.packetTabsPanel, "Packets List");
            this.dockingManager1.SetEnableDocking(this.packetTabsPanel, true);
            ccbpacketTabsPanel.MergeWith(this.dockingManager1.CaptionButtons, false);
            this.dockingManager1.SetCustomCaptionButtons(this.packetTabsPanel, ccbpacketTabsPanel);
            this.dockingManager1.SetDockLabel(this.statusPanel, "statusPanel");
            this.dockingManager1.SetEnableDocking(this.statusPanel, true);
            ccbstatusPanel.MergeWith(this.dockingManager1.CaptionButtons, false);
            this.dockingManager1.SetCustomCaptionButtons(this.statusPanel, ccbstatusPanel);
            this.dockingManager1.SetDockLabel(this.HexViewPanel, "HexViewPanel");
            this.dockingManager1.SetEnableDocking(this.HexViewPanel, true);
            ccbHexViewPanel.MergeWith(this.dockingManager1.CaptionButtons, false);
            this.dockingManager1.SetCustomCaptionButtons(this.HexViewPanel, ccbHexViewPanel);
            // 
            // livePreviewPanel
            // 
            this.livePreviewPanel.BackColor = System.Drawing.SystemColors.Control;
            this.livePreviewPanel.Controls.Add(this.packetTreeView);
            this.livePreviewPanel.Location = new System.Drawing.Point(1, 24);
            this.livePreviewPanel.Name = "livePreviewPanel";
            this.livePreviewPanel.Size = new System.Drawing.Size(455, 672);
            this.livePreviewPanel.TabIndex = 10;
            // 
            // packetTreeView
            // 
            this.packetTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packetTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.packetTreeView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packetTreeView.HideSelection = false;
            this.packetTreeView.Location = new System.Drawing.Point(10, 9);
            this.packetTreeView.Margin = new System.Windows.Forms.Padding(10);
            this.packetTreeView.Name = "packetTreeView";
            this.packetTreeView.ShowLines = false;
            this.packetTreeView.Size = new System.Drawing.Size(434, 654);
            this.packetTreeView.TabIndex = 8;
            this.packetTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.packetTreeView_AfterSelect);
            this.packetTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.packetTreeView_KeyDown);
            this.packetTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.packetTreeView_MouseClick);
            this.packetTreeView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.packetTreeView_PreviewKeyDown);
            // 
            // packetTabsPanel
            // 
            this.packetTabsPanel.Controls.Add(this.packetTabsList);
            this.packetTabsPanel.Location = new System.Drawing.Point(1, 24);
            this.packetTabsPanel.Name = "packetTabsPanel";
            this.packetTabsPanel.Size = new System.Drawing.Size(204, 855);
            this.packetTabsPanel.TabIndex = 26;
            // 
            // packetTabsList
            // 
            this.packetTabsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packetTabsList.FormattingEnabled = true;
            this.packetTabsList.Location = new System.Drawing.Point(3, 5);
            this.packetTabsList.Name = "packetTabsList";
            this.packetTabsList.Size = new System.Drawing.Size(198, 836);
            this.packetTabsList.Sorted = true;
            this.packetTabsList.TabIndex = 0;
            this.packetTabsList.SelectedIndexChanged += new System.EventHandler(this.packetTabsList_SelectedIndexChanged);
            this.packetTabsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.packetTabsList_MouseDown);
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.SystemColors.Control;
            this.statusPanel.Controls.Add(this.livePrevStatusPanel);
            this.statusPanel.Location = new System.Drawing.Point(1, 24);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(455, 154);
            this.statusPanel.TabIndex = 15;
            // 
            // livePrevStatusPanel
            // 
            this.livePrevStatusPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.livePrevStatusPanel.BackColor = System.Drawing.Color.Transparent;
            this.livePrevStatusPanel.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.None, System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(251)))), ((int)(((byte)(184))))), System.Drawing.Color.Gray);
            this.livePrevStatusPanel.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.livePrevStatusPanel.BorderColor = System.Drawing.Color.DimGray;
            this.livePrevStatusPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.livePrevStatusPanel.Controls.Add(this.livePreviewTextBox);
            this.livePrevStatusPanel.Location = new System.Drawing.Point(9, 10);
            this.livePrevStatusPanel.Name = "livePrevStatusPanel";
            this.livePrevStatusPanel.Size = new System.Drawing.Size(435, 133);
            this.livePrevStatusPanel.TabIndex = 9;
            // 
            // livePreviewTextBox
            // 
            this.livePreviewTextBox.AutoSize = true;
            this.livePreviewTextBox.Location = new System.Drawing.Point(3, 3);
            this.livePreviewTextBox.MaximumSize = new System.Drawing.Size(430, 0);
            this.livePreviewTextBox.Name = "livePreviewTextBox";
            this.livePreviewTextBox.Size = new System.Drawing.Size(24, 13);
            this.livePreviewTextBox.TabIndex = 0;
            this.livePreviewTextBox.Text = "Idle";
            // 
            // HexViewPanel
            // 
            this.HexViewPanel.BackColor = System.Drawing.SystemColors.Control;
            this.HexViewPanel.Controls.Add(this.hexViewBox);
            this.HexViewPanel.Location = new System.Drawing.Point(1, 24);
            this.HexViewPanel.Name = "HexViewPanel";
            this.HexViewPanel.Size = new System.Drawing.Size(723, 374);
            this.HexViewPanel.TabIndex = 12;
            // 
            // hexViewBox
            // 
            this.hexViewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hexViewBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.hexViewBox.HexCasing = Be.Windows.Forms.HexCasing.Lower;
            this.hexViewBox.LineInfoVisible = true;
            this.hexViewBox.Location = new System.Drawing.Point(7, 7);
            this.hexViewBox.Margin = new System.Windows.Forms.Padding(10);
            this.hexViewBox.Name = "hexViewBox";
            this.hexViewBox.Padding = new System.Windows.Forms.Padding(5);
            this.hexViewBox.ReadOnly = true;
            this.hexViewBox.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.hexViewBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexViewBox.Size = new System.Drawing.Size(710, 363);
            this.hexViewBox.TabIndex = 0;
            this.hexViewBox.UseFixedBytesPerLine = true;
            this.hexViewBox.VScrollBarVisible = true;
            this.hexViewBox.Copied += new System.EventHandler(this.hexViewBox_Copied);
            // 
            // dockingClientPanel1
            // 
            this.dockingClientPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.dockingClientPanel1.Controls.Add(this.tabControl);
            this.dockingClientPanel1.Location = new System.Drawing.Point(212, 119);
            this.dockingClientPanel1.Name = "dockingClientPanel1";
            this.dockingClientPanel1.Size = new System.Drawing.Size(725, 477);
            this.dockingClientPanel1.SizeToFit = true;
            this.dockingClientPanel1.TabIndex = 9;
            // 
            // tabControl
            // 
            this.tabControl.AllowClose = false;
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.plusTab);
            this.tabControl.Location = new System.Drawing.Point(11, 9);
            this.tabControl.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(709, 463);
            this.tabControl.TabIndex = 4;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            this.tabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl_DragEnter);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.packetDefineControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(701, 437);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Packet 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // packetDefineControl1
            // 
            this.packetDefineControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packetDefineControl1.Location = new System.Drawing.Point(3, 3);
            this.packetDefineControl1.Margin = new System.Windows.Forms.Padding(5);
            this.packetDefineControl1.Name = "packetDefineControl1";
            this.packetDefineControl1.Size = new System.Drawing.Size(695, 431);
            this.packetDefineControl1.TabIndex = 0;
            this.packetDefineControl1.Load += new System.EventHandler(this.packetDefineControl1_Load);
            // 
            // plusTab
            // 
            this.plusTab.Location = new System.Drawing.Point(4, 22);
            this.plusTab.Name = "plusTab";
            this.plusTab.Padding = new System.Windows.Forms.Padding(3);
            this.plusTab.Size = new System.Drawing.Size(701, 437);
            this.plusTab.TabIndex = 1;
            this.plusTab.Text = "+";
            this.plusTab.UseVisualStyleBackColor = true;
            // 
            // ribbonControlAdv1
            // 
            this.ribbonControlAdv1.CaptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonControlAdv1.Dock = Syncfusion.Windows.Forms.Tools.DockStyleEx.Top;
            this.ribbonControlAdv1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ribbonControlAdv1.Header.AddMainItem(homeToolStripTabItem);
            this.ribbonControlAdv1.Header.AddMainItem(previewToolStripTabItem);
            this.ribbonControlAdv1.LauncherStyle = Syncfusion.Windows.Forms.Tools.LauncherStyle.Metro;
            this.ribbonControlAdv1.Location = new System.Drawing.Point(2, 0);
            this.ribbonControlAdv1.MenuButtonFont = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ribbonControlAdv1.MenuButtonImage = global::PacketStudio.Properties.Resources.emblem_system;
            this.ribbonControlAdv1.MenuButtonText = "";
            this.ribbonControlAdv1.MenuButtonVisible = false;
            this.ribbonControlAdv1.MenuButtonWidth = 56;
            this.ribbonControlAdv1.MenuColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.ribbonControlAdv1.Name = "ribbonControlAdv1";
            this.ribbonControlAdv1.Office2016ColorTable.Add(office2016ColorTable1);
            this.ribbonControlAdv1.OfficeColorScheme = Syncfusion.Windows.Forms.Tools.ToolStripEx.ColorScheme.Black;
            // 
            // ribbonControlAdv1.OfficeMenu
            // 
            this.ribbonControlAdv1.OfficeMenu.MainPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitOfficeButton});
            this.ribbonControlAdv1.OfficeMenu.Name = "OfficeMenu";
            this.ribbonControlAdv1.OfficeMenu.ShowItemToolTips = true;
            this.ribbonControlAdv1.OfficeMenu.Size = new System.Drawing.Size(114, 71);
            this.ribbonControlAdv1.QuickPanelImageLayout = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ribbonControlAdv1.RibbonHeaderImage = Syncfusion.Windows.Forms.Tools.RibbonHeaderImage.None;
            this.ribbonControlAdv1.RibbonStyle = Syncfusion.Windows.Forms.Tools.RibbonStyle.TouchStyle;
            this.ribbonControlAdv1.SelectedTab = this.previewToolStripTabItem;
            this.ribbonControlAdv1.ShowQuickItemsDropDownButton = false;
            this.ribbonControlAdv1.ShowRibbonDisplayOptionButton = false;
            this.ribbonControlAdv1.Size = new System.Drawing.Size(1396, 119);
            this.ribbonControlAdv1.SystemText.QuickAccessDialogDropDownName = "Start menu";
            this.ribbonControlAdv1.SystemText.RenameDisplayLabelText = "&Display Name:";
            this.ribbonControlAdv1.TabIndex = 5;
            this.ribbonControlAdv1.Text = "ribbonControlAdv1";
            this.ribbonControlAdv1.TitleColor = System.Drawing.Color.White;
            // 
            // homeToolStripTabItem
            // 
            this.homeToolStripTabItem.Name = "homeToolStripTabItem";
            // 
            // ribbonControlAdv1.ribbonPanel1
            // 
            this.homeToolStripTabItem.Panel.Controls.Add(this.newOpenSaveToolStrip);
            this.homeToolStripTabItem.Panel.Controls.Add(this.copyToolStrip);
            this.homeToolStripTabItem.Panel.Controls.Add(this.wiresharkToolStrip);
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
            // newOpenSaveToolStrip
            // 
            this.newOpenSaveToolStrip.AutoSize = false;
            this.newOpenSaveToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.newOpenSaveToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.newOpenSaveToolStrip.ForeColor = System.Drawing.Color.Black;
            this.newOpenSaveToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.newOpenSaveToolStrip.Image = null;
            this.newOpenSaveToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.newOpenSaveToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton});
            this.newOpenSaveToolStrip.Location = new System.Drawing.Point(0, 1);
            this.newOpenSaveToolStrip.Name = "newOpenSaveToolStrip";
            this.newOpenSaveToolStrip.Office12Mode = false;
            this.newOpenSaveToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.newOpenSaveToolStrip.ShowCaption = false;
            this.newOpenSaveToolStrip.Size = new System.Drawing.Size(147, 64);
            this.newOpenSaveToolStrip.TabIndex = 0;
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.Image = global::PacketStudio.Properties.Resources.na_new;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.newToolStripButton.Name = "newToolStripButton";
            this.SetShortcut(this.newToolStripButton, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N))));
            this.newToolStripButton.Size = new System.Drawing.Size(36, 55);
            this.newToolStripButton.Text = "New";
            this.newToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.Image = global::PacketStudio.Properties.Resources.na_open;
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.openToolStripButton.Name = "openToolStripButton";
            this.SetShortcut(this.openToolStripButton, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O))));
            this.openToolStripButton.Size = new System.Drawing.Size(40, 55);
            this.openToolStripButton.Text = "Open";
            this.openToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.openToolStripButton.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.Image = global::PacketStudio.Properties.Resources.na_save;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.SetShortcut(this.saveToolStripButton, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S))));
            this.saveToolStripButton.Size = new System.Drawing.Size(36, 55);
            this.saveToolStripButton.Text = "Save";
            this.saveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.saveToolStripButton.Click += new System.EventHandler(this.saveFileToolStripMenuItem_Click);
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
            this.toolStripButton4});
            this.copyToolStrip.Location = new System.Drawing.Point(149, 1);
            this.copyToolStrip.Name = "copyToolStrip";
            this.copyToolStrip.Office12Mode = false;
            this.copyToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.copyToolStrip.ShowCaption = false;
            this.copyToolStrip.ShowLauncher = false;
            this.copyToolStrip.Size = new System.Drawing.Size(91, 64);
            this.copyToolStrip.TabIndex = 1;
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = global::PacketStudio.Properties.Resources.csharp;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(74, 55);
            this.toolStripButton4.Text = "Copy For C#";
            this.toolStripButton4.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.toolStripButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton4.Click += new System.EventHandler(this.copyForCToolStripMenuItem_Click);
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
            this.toolStripButton5});
            this.wiresharkToolStrip.Location = new System.Drawing.Point(242, 1);
            this.wiresharkToolStrip.Name = "wiresharkToolStrip";
            this.wiresharkToolStrip.Office12Mode = false;
            this.wiresharkToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.wiresharkToolStrip.ShowCaption = false;
            this.wiresharkToolStrip.ShowLauncher = false;
            this.wiresharkToolStrip.Size = new System.Drawing.Size(177, 64);
            this.wiresharkToolStrip.TabIndex = 2;
            // 
            // pcapToolStripButton
            // 
            this.pcapToolStripButton.Image = global::PacketStudio.Properties.Resources.wireshark;
            this.pcapToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pcapToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.pcapToolStripButton.Name = "pcapToolStripButton";
            this.pcapToolStripButton.Padding = new System.Windows.Forms.Padding(11, 0, 11, 0);
            this.pcapToolStripButton.Size = new System.Drawing.Size(60, 55);
            this.pcapToolStripButton.Text = "Pcap!";
            this.pcapToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pcapToolStripButton.Click += new System.EventHandler(this.GeneratePcapButton_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = global::PacketStudio.Properties.Resources.ws_dir;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(99, 56);
            this.toolStripButton5.Text = "Locate Wireshark";
            this.toolStripButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton5.Click += new System.EventHandler(this.locateWireshark_Click);
            // 
            // previewToolStripTabItem
            // 
            this.previewToolStripTabItem.Name = "previewToolStripTabItem";
            // 
            // ribbonControlAdv1.ribbonPanel2
            // 
            this.previewToolStripTabItem.Panel.Controls.Add(this.livePreviewToolStrip);
            this.previewToolStripTabItem.Panel.Name = "ribbonPanel2";
            this.previewToolStripTabItem.Panel.ScrollPosition = 0;
            this.previewToolStripTabItem.Panel.TabIndex = 4;
            this.previewToolStripTabItem.Panel.Text = "Live Preview";
            this.previewToolStripTabItem.Position = 1;
            this.previewToolStripTabItem.Size = new System.Drawing.Size(89, 29);
            this.previewToolStripTabItem.Text = "Live Preview";
            // 
            // livePreviewToolStrip
            // 
            this.livePreviewToolStrip.AutoSize = false;
            this.livePreviewToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.livePreviewToolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.livePreviewToolStrip.ForeColor = System.Drawing.Color.Black;
            this.livePreviewToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.livePreviewToolStrip.Image = null;
            this.livePreviewToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.livePreviewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewtoolStripButton,
            this.previewContextToolStripButton,
            this.toolStripLabel2,
            this.livePrevToolStripTextBox});
            this.livePreviewToolStrip.Location = new System.Drawing.Point(0, 1);
            this.livePreviewToolStrip.Name = "livePreviewToolStrip";
            this.livePreviewToolStrip.Office12Mode = false;
            this.livePreviewToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.livePreviewToolStrip.ShowCaption = false;
            this.livePreviewToolStrip.ShowLauncher = false;
            this.livePreviewToolStrip.Size = new System.Drawing.Size(404, 64);
            this.livePreviewToolStrip.TabIndex = 3;
            // 
            // previewtoolStripButton
            // 
            this.previewtoolStripButton.Checked = true;
            this.previewtoolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewtoolStripButton.Image = global::PacketStudio.Properties.Resources.preview;
            this.previewtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewtoolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.previewtoolStripButton.Name = "previewtoolStripButton";
            this.previewtoolStripButton.Size = new System.Drawing.Size(88, 55);
            this.previewtoolStripButton.Text = "Enable Preview";
            this.previewtoolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.previewtoolStripButton.Click += new System.EventHandler(this.livePreviewToolStripMenuItem_Click);
            // 
            // previewContextToolStripButton
            // 
            this.previewContextToolStripButton.Checked = true;
            this.previewContextToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewContextToolStripButton.Image = global::PacketStudio.Properties.Resources.preview_cntx;
            this.previewContextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewContextToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.previewContextToolStripButton.Name = "previewContextToolStripButton";
            this.previewContextToolStripButton.Size = new System.Drawing.Size(106, 55);
            this.previewContextToolStripButton.Text = "Preview In Context";
            this.previewContextToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.previewContextToolStripButton.Click += new System.EventHandler(this.previewInBatPContextToolStripMenuItem_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(80, 55);
            this.toolStripLabel2.Text = "Preview Delay:";
            // 
            // livePrevToolStripTextBox
            // 
            this.livePrevToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.livePrevToolStripTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.livePrevToolStripTextBox.Name = "livePrevToolStripTextBox";
            this.livePrevToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.livePrevToolStripTextBox.TextChanged += new System.EventHandler(this.livePreviewDelayBox_TextChanged);
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
            this.exitOfficeButton.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 1000);
            this.ColorScheme = Syncfusion.Windows.Forms.Tools.RibbonForm.ColorSchemeType.Black;
            this.Controls.Add(this.ribbonControlAdv1);
            this.Controls.Add(this.dockingClientPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager1)).EndInit();
            this.livePreviewPanel.ResumeLayout(false);
            this.packetTabsPanel.ResumeLayout(false);
            this.statusPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.livePrevStatusPanel)).EndInit();
            this.livePrevStatusPanel.ResumeLayout(false);
            this.livePrevStatusPanel.PerformLayout();
            this.HexViewPanel.ResumeLayout(false);
            this.dockingClientPanel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlAdv1)).EndInit();
            this.ribbonControlAdv1.ResumeLayout(false);
            this.ribbonControlAdv1.PerformLayout();
            this.homeToolStripTabItem.Panel.ResumeLayout(false);
            this.newOpenSaveToolStrip.ResumeLayout(false);
            this.newOpenSaveToolStrip.PerformLayout();
            this.copyToolStrip.ResumeLayout(false);
            this.copyToolStrip.PerformLayout();
            this.wiresharkToolStrip.ResumeLayout(false);
            this.wiresharkToolStrip.PerformLayout();
            this.previewToolStripTabItem.Panel.ResumeLayout(false);
            this.livePreviewToolStrip.ResumeLayout(false);
            this.livePreviewToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

	    #endregion
		private Syncfusion.Windows.Forms.Tools.DockingClientPanel dockingClientPanel1;
		private Syncfusion.Windows.Forms.Tools.DockingManager dockingManager1;
		private System.Windows.Forms.Panel livePreviewPanel;
		private Syncfusion.Windows.Forms.Tools.GradientPanel livePrevStatusPanel;
		private System.Windows.Forms.Label livePreviewTextBox;
		private Syncfusion.Windows.Forms.Tools.RibbonControlAdv ribbonControlAdv1;
		private Syncfusion.Windows.Forms.Tools.ToolStripTabItem homeToolStripTabItem;
		private TreeViewWithArrows packetTreeView;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx newOpenSaveToolStrip;
		private System.Windows.Forms.ToolStripButton newToolStripButton;
		private System.Windows.Forms.ToolStripButton openToolStripButton;
		private System.Windows.Forms.ToolStripButton saveToolStripButton;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx copyToolStrip;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolStripTabItem2;
		private Syncfusion.Windows.Forms.Tools.ToolStripTabItem previewToolStripTabItem;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx livePreviewToolStrip;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripTextBox livePrevToolStripTextBox;
		private Syncfusion.Windows.Forms.Tools.OfficeButton exitOfficeButton;
		private System.Windows.Forms.ToolStripButton previewtoolStripButton;
		private System.Windows.Forms.ToolStripButton previewContextToolStripButton;
		private AdvancedTabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private PacketDefineControl packetDefineControl1;
		private System.Windows.Forms.TabPage plusTab;
		private System.Windows.Forms.Panel HexViewPanel;
		private Be.Windows.Forms.HexBox hexViewBox;
		private Syncfusion.Windows.Forms.Tools.ToolStripEx wiresharkToolStrip;
		private System.Windows.Forms.ToolStripButton pcapToolStripButton;
		private System.Windows.Forms.ToolStripButton toolStripButton5;
		private System.Windows.Forms.Panel statusPanel;
		private System.Windows.Forms.Panel packetTabsPanel;
		private System.Windows.Forms.ListBox packetTabsList;
	}
}

