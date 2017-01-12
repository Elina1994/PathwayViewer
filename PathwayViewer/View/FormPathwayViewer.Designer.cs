namespace PathwayViewer
{
    partial class FormPathwayViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPathwayViewer));
            this.buttonMapAccessionsToLocustag = new System.Windows.Forms.Button();
            this.buttonMapAccessionToTaxIds = new System.Windows.Forms.Button();
            this.buttonMapPathwayData = new System.Windows.Forms.Button();
            this.buttonGenerateSsuLsuFiles = new System.Windows.Forms.Button();
            this.buttonMultipleSequenceAlignment = new System.Windows.Forms.Button();
            this.buttonVisualizeMappedDataToTree = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.splitContainerRoot = new System.Windows.Forms.SplitContainer();
            this.tabControlFunctions = new System.Windows.Forms.TabControl();
            this.tabPageDataCollector = new System.Windows.Forms.TabPage();
            this.splitContainerDataCollector = new System.Windows.Forms.SplitContainer();
            this.groupBoxDataCollectorNcbi = new System.Windows.Forms.GroupBox();
            this.buttonGetGenbankFiles = new System.Windows.Forms.Button();
            this.groupBoxDataCollectorKegg = new System.Windows.Forms.GroupBox();
            this.buttonCollectAllKeggEntryFiles = new System.Windows.Forms.Button();
            this.buttonCollectAllOrganism = new System.Windows.Forms.Button();
            this.tabPageDataMapping = new System.Windows.Forms.TabPage();
            this.tabPageDataVisualisation = new System.Windows.Forms.TabPage();
            this.buttonVisualizeMappedDataToPieChart = new System.Windows.Forms.Button();
            this.buttonReadDataInMemory = new System.Windows.Forms.Button();
            this.textBoxAccessionCutOff = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRoot)).BeginInit();
            this.splitContainerRoot.Panel1.SuspendLayout();
            this.splitContainerRoot.Panel2.SuspendLayout();
            this.splitContainerRoot.SuspendLayout();
            this.tabControlFunctions.SuspendLayout();
            this.tabPageDataCollector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataCollector)).BeginInit();
            this.splitContainerDataCollector.Panel1.SuspendLayout();
            this.splitContainerDataCollector.Panel2.SuspendLayout();
            this.splitContainerDataCollector.SuspendLayout();
            this.groupBoxDataCollectorNcbi.SuspendLayout();
            this.groupBoxDataCollectorKegg.SuspendLayout();
            this.tabPageDataMapping.SuspendLayout();
            this.tabPageDataVisualisation.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMapAccessionsToLocustag
            // 
            this.buttonMapAccessionsToLocustag.Location = new System.Drawing.Point(6, 21);
            this.buttonMapAccessionsToLocustag.Name = "buttonMapAccessionsToLocustag";
            this.buttonMapAccessionsToLocustag.Size = new System.Drawing.Size(70, 45);
            this.buttonMapAccessionsToLocustag.TabIndex = 2;
            this.buttonMapAccessionsToLocustag.TabStop = false;
            this.buttonMapAccessionsToLocustag.UseVisualStyleBackColor = true;
            this.buttonMapAccessionsToLocustag.Click += new System.EventHandler(this.buttonMapAccessionsToLocustag_Click);
            this.buttonMapAccessionsToLocustag.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonMapAccessionToTaxIds
            // 
            this.buttonMapAccessionToTaxIds.Location = new System.Drawing.Point(6, 123);
            this.buttonMapAccessionToTaxIds.Name = "buttonMapAccessionToTaxIds";
            this.buttonMapAccessionToTaxIds.Size = new System.Drawing.Size(70, 45);
            this.buttonMapAccessionToTaxIds.TabIndex = 1;
            this.buttonMapAccessionToTaxIds.UseVisualStyleBackColor = true;
            this.buttonMapAccessionToTaxIds.Click += new System.EventHandler(this.buttonMapAccessionToTaxIds_Click);
            this.buttonMapAccessionToTaxIds.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonMapPathwayData
            // 
            this.buttonMapPathwayData.Location = new System.Drawing.Point(6, 72);
            this.buttonMapPathwayData.Name = "buttonMapPathwayData";
            this.buttonMapPathwayData.Size = new System.Drawing.Size(70, 45);
            this.buttonMapPathwayData.TabIndex = 0;
            this.buttonMapPathwayData.UseVisualStyleBackColor = true;
            this.buttonMapPathwayData.Click += new System.EventHandler(this.buttonMapPathwayData_Click);
            this.buttonMapPathwayData.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonGenerateSsuLsuFiles
            // 
            this.buttonGenerateSsuLsuFiles.Location = new System.Drawing.Point(5, 60);
            this.buttonGenerateSsuLsuFiles.Name = "buttonGenerateSsuLsuFiles";
            this.buttonGenerateSsuLsuFiles.Size = new System.Drawing.Size(70, 45);
            this.buttonGenerateSsuLsuFiles.TabIndex = 1;
            this.buttonGenerateSsuLsuFiles.UseVisualStyleBackColor = true;
            this.buttonGenerateSsuLsuFiles.Click += new System.EventHandler(this.buttonGenerateSsuLsuFiles_Click);
            this.buttonGenerateSsuLsuFiles.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonMultipleSequenceAlignment
            // 
            this.buttonMultipleSequenceAlignment.Location = new System.Drawing.Point(5, 111);
            this.buttonMultipleSequenceAlignment.Name = "buttonMultipleSequenceAlignment";
            this.buttonMultipleSequenceAlignment.Size = new System.Drawing.Size(70, 45);
            this.buttonMultipleSequenceAlignment.TabIndex = 2;
            this.buttonMultipleSequenceAlignment.UseVisualStyleBackColor = true;
            this.buttonMultipleSequenceAlignment.Click += new System.EventHandler(this.buttonMultipleSequenceAlignment_Click);
            this.buttonMultipleSequenceAlignment.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonVisualizeMappedDataToTree
            // 
            this.buttonVisualizeMappedDataToTree.Location = new System.Drawing.Point(5, 162);
            this.buttonVisualizeMappedDataToTree.Name = "buttonVisualizeMappedDataToTree";
            this.buttonVisualizeMappedDataToTree.Size = new System.Drawing.Size(70, 45);
            this.buttonVisualizeMappedDataToTree.TabIndex = 3;
            this.buttonVisualizeMappedDataToTree.UseVisualStyleBackColor = true;
            this.buttonVisualizeMappedDataToTree.Click += new System.EventHandler(this.buttonVisualizeMappedData_Click);
            this.buttonVisualizeMappedDataToTree.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLog.BackColor = System.Drawing.Color.Black;
            this.richTextBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.richTextBoxLog.Location = new System.Drawing.Point(5, 3);
            this.richTextBoxLog.MaxLength = 100000;
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(770, 191);
            this.richTextBoxLog.TabIndex = 4;
            this.richTextBoxLog.Text = "";
            // 
            // splitContainerRoot
            // 
            this.splitContainerRoot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRoot.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRoot.Name = "splitContainerRoot";
            this.splitContainerRoot.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRoot.Panel1
            // 
            this.splitContainerRoot.Panel1.Controls.Add(this.tabControlFunctions);
            // 
            // splitContainerRoot.Panel2
            // 
            this.splitContainerRoot.Panel2.Controls.Add(this.richTextBoxLog);
            this.splitContainerRoot.Size = new System.Drawing.Size(784, 562);
            this.splitContainerRoot.SplitterDistance = 357;
            this.splitContainerRoot.TabIndex = 5;
            // 
            // tabControlFunctions
            // 
            this.tabControlFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlFunctions.Controls.Add(this.tabPageDataCollector);
            this.tabControlFunctions.Controls.Add(this.tabPageDataMapping);
            this.tabControlFunctions.Controls.Add(this.tabPageDataVisualisation);
            this.tabControlFunctions.Location = new System.Drawing.Point(3, 10);
            this.tabControlFunctions.Name = "tabControlFunctions";
            this.tabControlFunctions.SelectedIndex = 0;
            this.tabControlFunctions.Size = new System.Drawing.Size(774, 340);
            this.tabControlFunctions.TabIndex = 4;
            // 
            // tabPageDataCollector
            // 
            this.tabPageDataCollector.Controls.Add(this.splitContainerDataCollector);
            this.tabPageDataCollector.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataCollector.Name = "tabPageDataCollector";
            this.tabPageDataCollector.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataCollector.Size = new System.Drawing.Size(766, 314);
            this.tabPageDataCollector.TabIndex = 0;
            this.tabPageDataCollector.Text = "Data Collector";
            this.tabPageDataCollector.UseVisualStyleBackColor = true;
            // 
            // splitContainerDataCollector
            // 
            this.splitContainerDataCollector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerDataCollector.Location = new System.Drawing.Point(6, 6);
            this.splitContainerDataCollector.Name = "splitContainerDataCollector";
            // 
            // splitContainerDataCollector.Panel1
            // 
            this.splitContainerDataCollector.Panel1.Controls.Add(this.groupBoxDataCollectorNcbi);
            // 
            // splitContainerDataCollector.Panel2
            // 
            this.splitContainerDataCollector.Panel2.Controls.Add(this.groupBoxDataCollectorKegg);
            this.splitContainerDataCollector.Size = new System.Drawing.Size(754, 302);
            this.splitContainerDataCollector.SplitterDistance = 377;
            this.splitContainerDataCollector.TabIndex = 4;
            // 
            // groupBoxDataCollectorNcbi
            // 
            this.groupBoxDataCollectorNcbi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDataCollectorNcbi.Controls.Add(this.buttonGetGenbankFiles);
            this.groupBoxDataCollectorNcbi.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDataCollectorNcbi.Name = "groupBoxDataCollectorNcbi";
            this.groupBoxDataCollectorNcbi.Size = new System.Drawing.Size(371, 296);
            this.groupBoxDataCollectorNcbi.TabIndex = 2;
            this.groupBoxDataCollectorNcbi.TabStop = false;
            this.groupBoxDataCollectorNcbi.Text = "NCBI";
            // 
            // buttonGetGenbankFiles
            // 
            this.buttonGetGenbankFiles.Location = new System.Drawing.Point(6, 19);
            this.buttonGetGenbankFiles.Name = "buttonGetGenbankFiles";
            this.buttonGetGenbankFiles.Size = new System.Drawing.Size(70, 45);
            this.buttonGetGenbankFiles.TabIndex = 1;
            this.buttonGetGenbankFiles.UseVisualStyleBackColor = true;
            this.buttonGetGenbankFiles.Click += new System.EventHandler(this.buttonGetGenbankFiles_Click);
            this.buttonGetGenbankFiles.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // groupBoxDataCollectorKegg
            // 
            this.groupBoxDataCollectorKegg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDataCollectorKegg.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxDataCollectorKegg.Controls.Add(this.buttonCollectAllKeggEntryFiles);
            this.groupBoxDataCollectorKegg.Controls.Add(this.buttonCollectAllOrganism);
            this.groupBoxDataCollectorKegg.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDataCollectorKegg.Name = "groupBoxDataCollectorKegg";
            this.groupBoxDataCollectorKegg.Size = new System.Drawing.Size(367, 296);
            this.groupBoxDataCollectorKegg.TabIndex = 3;
            this.groupBoxDataCollectorKegg.TabStop = false;
            this.groupBoxDataCollectorKegg.Text = "KEGG";
            // 
            // buttonCollectAllKeggEntryFiles
            // 
            this.buttonCollectAllKeggEntryFiles.Location = new System.Drawing.Point(6, 70);
            this.buttonCollectAllKeggEntryFiles.Name = "buttonCollectAllKeggEntryFiles";
            this.buttonCollectAllKeggEntryFiles.Size = new System.Drawing.Size(70, 45);
            this.buttonCollectAllKeggEntryFiles.TabIndex = 1;
            this.buttonCollectAllKeggEntryFiles.UseVisualStyleBackColor = true;
            this.buttonCollectAllKeggEntryFiles.Click += new System.EventHandler(this.buttonCollectAllKeggEntryFiles_Click);
            this.buttonCollectAllKeggEntryFiles.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonCollectAllOrganism
            // 
            this.buttonCollectAllOrganism.Location = new System.Drawing.Point(6, 19);
            this.buttonCollectAllOrganism.Name = "buttonCollectAllOrganism";
            this.buttonCollectAllOrganism.Size = new System.Drawing.Size(70, 45);
            this.buttonCollectAllOrganism.TabIndex = 0;
            this.buttonCollectAllOrganism.UseVisualStyleBackColor = true;
            this.buttonCollectAllOrganism.Click += new System.EventHandler(this.buttonCollectAllOrganism_Click);
            this.buttonCollectAllOrganism.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // tabPageDataMapping
            // 
            this.tabPageDataMapping.Controls.Add(this.buttonMapAccessionToTaxIds);
            this.tabPageDataMapping.Controls.Add(this.buttonMapAccessionsToLocustag);
            this.tabPageDataMapping.Controls.Add(this.buttonMapPathwayData);
            this.tabPageDataMapping.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataMapping.Name = "tabPageDataMapping";
            this.tabPageDataMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataMapping.Size = new System.Drawing.Size(766, 314);
            this.tabPageDataMapping.TabIndex = 1;
            this.tabPageDataMapping.Text = "Data Mapping";
            this.tabPageDataMapping.UseVisualStyleBackColor = true;
            // 
            // tabPageDataVisualisation
            // 
            this.tabPageDataVisualisation.Controls.Add(this.buttonVisualizeMappedDataToPieChart);
            this.tabPageDataVisualisation.Controls.Add(this.buttonReadDataInMemory);
            this.tabPageDataVisualisation.Controls.Add(this.textBoxAccessionCutOff);
            this.tabPageDataVisualisation.Controls.Add(this.buttonVisualizeMappedDataToTree);
            this.tabPageDataVisualisation.Controls.Add(this.buttonMultipleSequenceAlignment);
            this.tabPageDataVisualisation.Controls.Add(this.buttonGenerateSsuLsuFiles);
            this.tabPageDataVisualisation.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataVisualisation.Name = "tabPageDataVisualisation";
            this.tabPageDataVisualisation.Size = new System.Drawing.Size(766, 314);
            this.tabPageDataVisualisation.TabIndex = 2;
            this.tabPageDataVisualisation.Text = "Data Visualisation";
            this.tabPageDataVisualisation.UseVisualStyleBackColor = true;
            // 
            // buttonVisualizeMappedDataToPieChart
            // 
            this.buttonVisualizeMappedDataToPieChart.Location = new System.Drawing.Point(81, 162);
            this.buttonVisualizeMappedDataToPieChart.Name = "buttonVisualizeMappedDataToPieChart";
            this.buttonVisualizeMappedDataToPieChart.Size = new System.Drawing.Size(70, 45);
            this.buttonVisualizeMappedDataToPieChart.TabIndex = 6;
            this.buttonVisualizeMappedDataToPieChart.UseVisualStyleBackColor = true;
            this.buttonVisualizeMappedDataToPieChart.Click += new System.EventHandler(this.buttonVisualizeMappedDataToPieChart_Click);
            this.buttonVisualizeMappedDataToPieChart.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // buttonReadDataInMemory
            // 
            this.buttonReadDataInMemory.Location = new System.Drawing.Point(5, 9);
            this.buttonReadDataInMemory.Name = "buttonReadDataInMemory";
            this.buttonReadDataInMemory.Size = new System.Drawing.Size(70, 45);
            this.buttonReadDataInMemory.TabIndex = 5;
            this.buttonReadDataInMemory.UseVisualStyleBackColor = true;
            this.buttonReadDataInMemory.Click += new System.EventHandler(this.buttonReadDataInMemory_Click);
            this.buttonReadDataInMemory.MouseHover += new System.EventHandler(this.button_MouseHover);
            // 
            // textBoxAccessionCutOff
            // 
            this.textBoxAccessionCutOff.Location = new System.Drawing.Point(81, 73);
            this.textBoxAccessionCutOff.MaxLength = 10;
            this.textBoxAccessionCutOff.Name = "textBoxAccessionCutOff";
            this.textBoxAccessionCutOff.Size = new System.Drawing.Size(138, 20);
            this.textBoxAccessionCutOff.TabIndex = 4;
            this.textBoxAccessionCutOff.Text = "100";
            // 
            // FormPathwayViewer
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.IndianRed;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainerRoot);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPathwayViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.splitContainerRoot.Panel1.ResumeLayout(false);
            this.splitContainerRoot.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRoot)).EndInit();
            this.splitContainerRoot.ResumeLayout(false);
            this.tabControlFunctions.ResumeLayout(false);
            this.tabPageDataCollector.ResumeLayout(false);
            this.splitContainerDataCollector.Panel1.ResumeLayout(false);
            this.splitContainerDataCollector.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataCollector)).EndInit();
            this.splitContainerDataCollector.ResumeLayout(false);
            this.groupBoxDataCollectorNcbi.ResumeLayout(false);
            this.groupBoxDataCollectorKegg.ResumeLayout(false);
            this.tabPageDataMapping.ResumeLayout(false);
            this.tabPageDataVisualisation.ResumeLayout(false);
            this.tabPageDataVisualisation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonMapPathwayData;
        private System.Windows.Forms.Button buttonVisualizeMappedDataToTree;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.SplitContainer splitContainerRoot;
        private System.Windows.Forms.Button buttonMapAccessionsToLocustag;
        private System.Windows.Forms.Button buttonMapAccessionToTaxIds;
        private System.Windows.Forms.Button buttonMultipleSequenceAlignment;
        private System.Windows.Forms.Button buttonGenerateSsuLsuFiles;
        private System.Windows.Forms.TabControl tabControlFunctions;
        private System.Windows.Forms.TabPage tabPageDataCollector;
        private System.Windows.Forms.TabPage tabPageDataMapping;
        private System.Windows.Forms.TabPage tabPageDataVisualisation;
        private System.Windows.Forms.GroupBox groupBoxDataCollectorKegg;
        private System.Windows.Forms.Button buttonCollectAllKeggEntryFiles;
        private System.Windows.Forms.Button buttonCollectAllOrganism;
        private System.Windows.Forms.GroupBox groupBoxDataCollectorNcbi;
        private System.Windows.Forms.Button buttonGetGenbankFiles;
        private System.Windows.Forms.SplitContainer splitContainerDataCollector;
        private System.Windows.Forms.TextBox textBoxAccessionCutOff;
        private System.Windows.Forms.Button buttonReadDataInMemory;
        private System.Windows.Forms.Button buttonVisualizeMappedDataToPieChart;
    }
}

