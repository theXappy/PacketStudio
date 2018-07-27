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
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbHexViewPanel = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.Office2016ColorTable office2016ColorTable1 = new Syncfusion.Windows.Forms.Tools.Office2016ColorTable();
            this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
            this.livePreviewPanel = new System.Windows.Forms.Panel();
            this.packetTreeView = new PacketStudio.Controls.TreeViewWithArrows();
            this.packetTabsPanel = new System.Windows.Forms.Panel();
            this.greyBorderPanel = new System.Windows.Forms.Panel();
            this.whiteBackgroundPanel = new System.Windows.Forms.Panel();
            this.packetTabsList = new System.Windows.Forms.ListBox();
            this.HexViewPanel = new System.Windows.Forms.Panel();
            this.hexViewBox = new Be.Windows.Forms.HexBox();
            this.mainPanel = new Syncfusion.Windows.Forms.Tools.DockingClientPanel();
            this.tabControl = new PacketStudio.Controls.AdvancedTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.packetDefineControl1 = new PacketStudio.Controls.PacketsDef.PacketDefineControl();
            this.plusTab = new System.Windows.Forms.TabPage();
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
            this.wiresharkToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.pcapToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.locateWsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewToolStripTabItem = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.livePreviewToolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.previewtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewContextToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.prevDelayToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.livePrevToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.exitOfficeButton = new Syncfusion.Windows.Forms.Tools.OfficeButton();
            this.toolStripTabItem2 = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.statusBar = new Syncfusion.Windows.Forms.Tools.StatusBarAdv();
            this.statusTextPanel = new Syncfusion.Windows.Forms.Tools.StatusBarAdvPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
            this.livePreviewPanel.SuspendLayout();
            this.packetTabsPanel.SuspendLayout();
            this.greyBorderPanel.SuspendLayout();
            this.whiteBackgroundPanel.SuspendLayout();
            this.HexViewPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.ribbonControl.SuspendLayout();
            this.homeToolStripTabItem.Panel.SuspendLayout();
            this.fileToolStrip.SuspendLayout();
            this.copyToolStrip.SuspendLayout();
            this.wiresharkToolStrip.SuspendLayout();
            this.previewToolStripTabItem.Panel.SuspendLayout();
            this.livePreviewToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBar)).BeginInit();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusTextPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // dockingManager
            // 
            this.dockingManager.ActiveCaptionFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.AnimateAutoHiddenWindow = true;
            this.dockingManager.AutoHideSelectionStyle = Syncfusion.Windows.Forms.Tools.AutoHideSelectionStyle.Click;
            this.dockingManager.AutoHideTabForeColor = System.Drawing.Color.Empty;
            this.dockingManager.DockBehavior = Syncfusion.Windows.Forms.Tools.DockBehavior.VS2010;
            this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
            this.dockingManager.DockTabPadX = 0F;
            this.dockingManager.DragProviderStyle = Syncfusion.Windows.Forms.Tools.DragProviderStyle.VS2012;
            this.dockingManager.HostControl = this;
            this.dockingManager.InActiveCaptionBackground = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(211)))), ((int)(((byte)(212))))));
            this.dockingManager.InActiveCaptionFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
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
            this.livePreviewPanel.Size = new System.Drawing.Size(460, 832);
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
            this.packetTreeView.Size = new System.Drawing.Size(460, 832);
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
            this.packetTabsPanel.Size = new System.Drawing.Size(210, 832);
            this.packetTabsPanel.TabIndex = 26;
            // 
            // greyBorderPanel
            // 
            this.greyBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.greyBorderPanel.Controls.Add(this.whiteBackgroundPanel);
            this.greyBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greyBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.greyBorderPanel.Name = "greyBorderPanel";
            this.greyBorderPanel.Padding = new System.Windows.Forms.Padding(1);
            this.greyBorderPanel.Size = new System.Drawing.Size(210, 832);
            this.greyBorderPanel.TabIndex = 1;
            // 
            // whiteBackgroundPanel
            // 
            this.whiteBackgroundPanel.BackColor = System.Drawing.Color.White;
            this.whiteBackgroundPanel.Controls.Add(this.packetTabsList);
            this.whiteBackgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whiteBackgroundPanel.Location = new System.Drawing.Point(1, 1);
            this.whiteBackgroundPanel.Margin = new System.Windows.Forms.Padding(1);
            this.whiteBackgroundPanel.Name = "whiteBackgroundPanel";
            this.whiteBackgroundPanel.Size = new System.Drawing.Size(208, 830);
            this.whiteBackgroundPanel.TabIndex = 1;
            // 
            // packetTabsList
            // 
            this.packetTabsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.packetTabsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packetTabsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.packetTabsList.FormattingEnabled = true;
            this.packetTabsList.ItemHeight = 19;
            this.packetTabsList.Location = new System.Drawing.Point(0, 0);
            this.packetTabsList.Name = "packetTabsList";
            this.packetTabsList.Size = new System.Drawing.Size(208, 830);
            this.packetTabsList.Sorted = true;
            this.packetTabsList.TabIndex = 1;
            this.packetTabsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.PacketTabsList_DrawItem);
            this.packetTabsList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.PacketTabsList_MeasureItem);
            this.packetTabsList.SelectedIndexChanged += new System.EventHandler(this.PacketTabsList_SelectedIndexChanged);
            this.packetTabsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PacketTabsList_MouseDown);
            // 
            // HexViewPanel
            // 
            this.HexViewPanel.BackColor = System.Drawing.SystemColors.Control;
            this.HexViewPanel.Controls.Add(this.hexViewBox);
            this.HexViewPanel.Location = new System.Drawing.Point(1, 24);
            this.HexViewPanel.Name = "HexViewPanel";
            this.HexViewPanel.Size = new System.Drawing.Size(712, 423);
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
            this.hexViewBox.Size = new System.Drawing.Size(712, 423);
            this.hexViewBox.TabIndex = 0;
            this.hexViewBox.UseFixedBytesPerLine = true;
            this.hexViewBox.VScrollBarVisible = true;
            this.hexViewBox.Copied += new System.EventHandler(this.HexViewBox_Copied);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainPanel.Controls.Add(this.tabControl);
            this.mainPanel.Location = new System.Drawing.Point(218, 120);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(714, 405);
            this.mainPanel.SizeToFit = true;
            this.mainPanel.TabIndex = 9;
            // 
            // tabControl
            // 
            this.tabControl.AllowClose = false;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.plusTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(714, 405);
            this.tabControl.TabIndex = 4;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabControl_DragDrop);
            this.tabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabControl_DragEnter);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabControl_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.packetDefineControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(706, 379);
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
            this.packetDefineControl1.Size = new System.Drawing.Size(700, 373);
            this.packetDefineControl1.TabIndex = 0;
            // 
            // plusTab
            // 
            this.plusTab.Location = new System.Drawing.Point(4, 22);
            this.plusTab.Name = "plusTab";
            this.plusTab.Padding = new System.Windows.Forms.Padding(3);
            this.plusTab.Size = new System.Drawing.Size(706, 379);
            this.plusTab.TabIndex = 1;
            this.plusTab.Text = "+";
            this.plusTab.UseVisualStyleBackColor = true;
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
            this.refactorDropDownButton});
            this.copyToolStrip.Location = new System.Drawing.Point(149, 1);
            this.copyToolStrip.Name = "copyToolStrip";
            this.copyToolStrip.Office12Mode = false;
            this.copyToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.copyToolStrip.ShowCaption = false;
            this.copyToolStrip.ShowLauncher = false;
            this.copyToolStrip.Size = new System.Drawing.Size(155, 65);
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
            this.locateWsToolStripButton});
            this.wiresharkToolStrip.Location = new System.Drawing.Point(306, 1);
            this.wiresharkToolStrip.Name = "wiresharkToolStrip";
            this.wiresharkToolStrip.Office12Mode = false;
            this.wiresharkToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.wiresharkToolStrip.ShowCaption = false;
            this.wiresharkToolStrip.ShowLauncher = false;
            this.wiresharkToolStrip.Size = new System.Drawing.Size(177, 65);
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
            // locateWsToolStripButton
            // 
            this.locateWsToolStripButton.Image = global::PacketStudio.Properties.Resources.ws_dir1;
            this.locateWsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.locateWsToolStripButton.Name = "locateWsToolStripButton";
            this.locateWsToolStripButton.Size = new System.Drawing.Size(99, 57);
            this.locateWsToolStripButton.Text = "Locate Wireshark";
            this.locateWsToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.locateWsToolStripButton.Click += new System.EventHandler(this.LocateWireshark_Click);
            // 
            // previewToolStripTabItem
            // 
            this.previewToolStripTabItem.Name = "previewToolStripTabItem";
            // 
            // ribbonControl.ribbonPanel2
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
            this.prevDelayToolStripLabel,
            this.livePrevToolStripTextBox});
            this.livePreviewToolStrip.Location = new System.Drawing.Point(0, 1);
            this.livePreviewToolStrip.Name = "livePreviewToolStrip";
            this.livePreviewToolStrip.Office12Mode = false;
            this.livePreviewToolStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.livePreviewToolStrip.ShowCaption = false;
            this.livePreviewToolStrip.ShowLauncher = false;
            this.livePreviewToolStrip.Size = new System.Drawing.Size(404, 64);
            this.livePreviewToolStrip.TabIndex = 3;
            this.livePreviewToolStrip.Text = "Live Preview Tool Strip";
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
            this.previewtoolStripButton.Click += new System.EventHandler(this.LivePreviewToolStripMenuItem_Click);
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
            // prevDelayToolStripLabel
            // 
            this.prevDelayToolStripLabel.Margin = new System.Windows.Forms.Padding(2);
            this.prevDelayToolStripLabel.Name = "prevDelayToolStripLabel";
            this.prevDelayToolStripLabel.Size = new System.Drawing.Size(80, 55);
            this.prevDelayToolStripLabel.Text = "Preview Delay:";
            // 
            // livePrevToolStripTextBox
            // 
            this.livePrevToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.livePrevToolStripTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.livePrevToolStripTextBox.Name = "livePrevToolStripTextBox";
            this.livePrevToolStripTextBox.Size = new System.Drawing.Size(100, 55);
            this.livePrevToolStripTextBox.TextChanged += new System.EventHandler(this.LivePreviewDelayBox_TextChanged);
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
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123)))));
            this.statusBar.BeforeTouchSize = new System.Drawing.Size(1396, 22);
            this.statusBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123)))));
            this.statusBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusBar.Controls.Add(this.statusTextPanel);
            this.statusBar.CustomLayoutBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.ForeColor = System.Drawing.Color.White;
            this.statusBar.Location = new System.Drawing.Point(2, 977);
            this.statusBar.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123)))));
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
            this.statusTextPanel.BackColor = System.Drawing.Color.Transparent;
            this.statusTextPanel.BeforeTouchSize = new System.Drawing.Size(1344, 16);
            this.statusTextPanel.Border3DStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.statusTextPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123)))));
            this.statusTextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusTextPanel.ForeColor = System.Drawing.Color.White;
            this.statusTextPanel.Location = new System.Drawing.Point(0, 2);
            this.statusTextPanel.Margin = new System.Windows.Forms.Padding(0);
            this.statusTextPanel.Name = "statusTextPanel";
            this.statusTextPanel.Size = new System.Drawing.Size(1344, 16);
            this.statusTextPanel.TabIndex = 0;
            this.statusTextPanel.Text = null;
            this.statusTextPanel.Click += new System.EventHandler(this.statusTextPanel_Click);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
            this.livePreviewPanel.ResumeLayout(false);
            this.packetTabsPanel.ResumeLayout(false);
            this.greyBorderPanel.ResumeLayout(false);
            this.whiteBackgroundPanel.ResumeLayout(false);
            this.HexViewPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
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
            this.livePreviewToolStrip.ResumeLayout(false);
            this.livePreviewToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBar)).EndInit();
            this.statusBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusTextPanel)).EndInit();
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
		private Syncfusion.Windows.Forms.Tools.ToolStripEx livePreviewToolStrip;
		private System.Windows.Forms.ToolStripLabel prevDelayToolStripLabel;
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
		private System.Windows.Forms.ToolStripButton locateWsToolStripButton;
		private System.Windows.Forms.Panel packetTabsPanel;
        private System.Windows.Forms.ToolStripDropDownButton refactorDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem normalizeHexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flattenProtocolStackToolStripMenuItem;
        private System.Windows.Forms.Panel greyBorderPanel;
        private System.Windows.Forms.Panel whiteBackgroundPanel;
        private System.Windows.Forms.ListBox packetTabsList;
        private Syncfusion.Windows.Forms.Tools.StatusBarAdv statusBar;
        private Syncfusion.Windows.Forms.Tools.StatusBarAdvPanel statusTextPanel;
    }
}

