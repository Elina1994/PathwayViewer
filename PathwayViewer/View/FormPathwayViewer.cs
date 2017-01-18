namespace PathwayViewer
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing.Drawing2D;

    public partial class FormPathwayViewer : Form
    {
        #region PROPERTIES

        public enum MessageType { Info, Warning, Error }
        private Controller Controller = null;
        private Style Style = null;

        private ToolTip ToolTipCurrent = null;

        private Dictionary<string, Dictionary<string, List<Pathway>>> AccessionLocustagPathwaysDic = null;
        private Dictionary<int, List<string>> TaxIdAccessionsDic = null;
        private Dictionary<string, Dictionary<string, string>> AccessionsSequenceDic = null;
        private Dictionary<string, Pathway> PathwaysDic = null;
        private Dictionary<string, Pathway> UniqueLocustagPathwayDic = null;

        private Dictionary<int, Taxonomy> TaxonomyDicId = null;

        private AccessionFile InputAccessions = null;

        private bool DataInMemory = false;

        #endregion

        #region CONSTRUCTOR

        public FormPathwayViewer()
        {
            try
            {
                InitializeComponent();

                this.Controller = new Controller();
                this.Style = new Style(this);
                this.Style.ApplyStyle(this);
                this.SetButtonProperties();

                this.Text = string.Format("Pathway Viewer v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

                if (this.Controller.CatchContent != string.Empty)
                {
                    throw new Exception(this.Controller.CatchContent);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while initializing the application.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void ShowMessage(MessageType messageType, string message)
        {
            try
            {
                Cursor = Cursors.Default;

                Color foreColor = new Color();

                switch (messageType)
                {
                    case MessageType.Info:
                        foreColor = Color.ForestGreen;
                        break;

                    case MessageType.Warning:
                        foreColor = Color.Orange;
                        break;

                    case MessageType.Error:
                        foreColor = Color.Red;
                        this.Controller.FileHelper.WriteError(message);
                        break;

                    default:
                        break;
                }

                this.richTextBoxLog.SelectionColor = foreColor;
                this.richTextBoxLog.AppendText(string.Format("{0}{1}", Environment.NewLine, message));
                CleanupLog();
                this.richTextBoxLog.SelectionStart = richTextBoxLog.TextLength;
                this.richTextBoxLog.ScrollToCaret();
                this.richTextBoxLog.Refresh();

                this.Controller.CatchContent = string.Empty;
            }
            catch (Exception ex)
            {
                string error = ex.Message;// TODO
            }
        }

        #endregion

        #region PRIVATE METHODS


        /// <summary>
        /// Sets hover text and picture for each button
        /// </summary>
        private void SetButtonProperties()
        {
            try
            {
                // Set default control tags
                this.buttonGetGenbankFiles.Tag = "Collect genbank files";
                this.buttonCollectAllKeggEntryFiles.Tag = "Collect KEGG entry files";
                this.buttonCollectAllOrganism.Tag = "Collect KEGG organism file";
                this.buttonMapAccessionsToLocustag.Tag = "Map accessions to locus_tags";
                this.buttonMapPathwayData.Tag = "Map pathway data";
                this.buttonMapAccessionToTaxIds.Tag = "Map taxonomy data";
                this.buttonReadDataInMemory.Tag = "Read needed visualization data into memory";
                this.buttonGenerateSsuLsuFiles.Tag = "Generate SSU and LSU fasta file";
                this.buttonMultipleSequenceAlignment.Tag = "Generate Multiple Sequence Alignment";
                this.buttonVisualizeMappedDataToTree.Tag = "Generate a phylogenetic tree";
                this.buttonVisualizeMappedDataToPieChart.Tag = "Generate a pie chart";

                // Set button images
                int border = 8;
                this.buttonGetGenbankFiles.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.Collect_96_96, new Size(this.buttonGetGenbankFiles.Width - border, this.buttonGetGenbankFiles.Height - border));
                this.buttonCollectAllKeggEntryFiles.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.CollectKegg_96_96, new Size(this.buttonCollectAllKeggEntryFiles.Width - border, this.buttonCollectAllKeggEntryFiles.Height - border));
                this.buttonCollectAllOrganism.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.Organisms_96_96, new Size(this.buttonCollectAllOrganism.Width - border, this.buttonCollectAllOrganism.Height - border));
                this.buttonMapAccessionsToLocustag.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.MapGene_96_96, new Size(this.buttonMapAccessionsToLocustag.Width - border, this.buttonMapAccessionsToLocustag.Height - border));
                this.buttonMapPathwayData.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.MapPathway_96_96, new Size(this.buttonMapPathwayData.Width - border, this.buttonMapPathwayData.Height - border));
                this.buttonMapAccessionToTaxIds.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.MapTaxonomy_96_64, new Size(this.buttonMapAccessionToTaxIds.Width - border, this.buttonMapAccessionToTaxIds.Height - border));
                this.buttonReadDataInMemory.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.Memory_95_95, new Size(this.buttonReadDataInMemory.Width - border, this.buttonReadDataInMemory.Height - border));
                this.buttonGenerateSsuLsuFiles.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.CreateMsaData_96_96, new Size(this.buttonGenerateSsuLsuFiles.Width - border, this.buttonGenerateSsuLsuFiles.Height - border));
                this.buttonMultipleSequenceAlignment.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.Alignment_96_89, new Size(this.buttonMultipleSequenceAlignment.Width - border, this.buttonMultipleSequenceAlignment.Height - border));
                this.buttonVisualizeMappedDataToTree.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.Visualize_96_52, new Size(this.buttonVisualizeMappedDataToTree.Width - border, this.buttonVisualizeMappedDataToTree.Height - border));
                this.buttonVisualizeMappedDataToPieChart.BackgroundImage = ResizeImage(PathwayViewer.Properties.Resources.pieChart_95_77, new Size(this.buttonVisualizeMappedDataToPieChart.Width - border, this.buttonVisualizeMappedDataToPieChart.Height - border));
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while setting module button properties.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }


        /// <summary>
        ///  Resizes an image when changing the button size. (c) Marcel Burger
        /// </summary>
        /// <param name="imgToResize">Image to resize</param>
        /// <param name="newSize">Used for the new size of an image</param>
        /// <returns></returns>
        private Image ResizeImage(Image imgToResize, Size newSize)
        {
            Bitmap b = null;

            try
            {
                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)newSize.Width / (float)sourceWidth);
                nPercentH = ((float)newSize.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                b = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while resizing an image.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }

            return (Image)b;
        }

        /// <summary>
        /// Shows the message assigned to the image. (c) Marcel Burger
        /// </summary>
        /// <param name="tip"></param>
        /// <param name="toolTipIcon"></param>
        /// <param name="control"></param>
        private void ShowToolTip(string tip, ToolTipIcon toolTipIcon, Control control)
        {
            try
            {
                if (this.ToolTipCurrent != null)
                {
                    this.ToolTipCurrent.Dispose();
                }

                this.ToolTipCurrent = new ToolTip();
                this.ToolTipCurrent.OwnerDraw = true;
                this.ToolTipCurrent.Draw += new DrawToolTipEventHandler(toolTip_Draw);
                this.ToolTipCurrent.Show(tip, control, 0, -30, 1500);
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while showing a tooltip.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }
        
        /// <summary>
        /// Cleans up the log of the tool when more than 500 lines present
        /// </summary>
        private void CleanupLog()
        {
            try
            {
                // cleans up log when over 500 lines are displayed
                while (this.richTextBoxLog.Lines.Length > 500)
                {
                    this.richTextBoxLog.ReadOnly = false;
                    this.richTextBoxLog.SelectionStart = 0;
                    this.richTextBoxLog.SelectionLength = this.richTextBoxLog.Text.IndexOf("\n", 0) + 1;
                    this.richTextBoxLog.SelectedText = string.Empty;
                    this.richTextBoxLog.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while cleaning the message log textbox.{0}Method: {1}.{2}{0}Exception: {3}", 
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Calls Pathwayviewer's DownloadKeggOrganismFile method.
        /// </summary>
        private void CollectAllOrganisms()
        {
            try
            {
                string keggDownloadFile = this.Controller.WebHelper.DownloadKeggOrganismFile(string.Empty);

                // start downloading if url is not empty
                if (keggDownloadFile != string.Empty)
                {
                    ShowMessage(MessageType.Info, "Downloading of Kegg file is finished.");
                }
                else
                {
                    ShowMessage(MessageType.Warning, "Downloading of Kegg file has failed.");
                }

                if (this.Controller.CatchContent != string.Empty)
                {
                    throw new Exception(this.Controller.CatchContent);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Executes a Python script which collects all KEGG entry files.
        /// </summary>
        private void CollectKeggEntryFiles()
        {
            try
            {
                string scriptPath = this.Controller.KeggEntryScriptPath;

                // open new file dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.ShowDialog();

                if (dialog.FileName.Length > 0)
                {
                    string keggFilePath = dialog.FileName;
                    string keggPathwayDir = this.Controller.KeggPathwaysDir;
                    // script arguments
                    string arguments = string.Format("{0} {1} {2}", scriptPath, keggFilePath, keggPathwayDir);

                    // start python process
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("python");
                    processStartInfo.Arguments = arguments;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.RedirectStandardOutput = false;

                    ShowMessage(MessageType.Info, "Starting to fetch kegg entry files ...");
                    Process process = new Process();
                    process.StartInfo = processStartInfo;

                    Console.WriteLine("Calling Python script {0}", scriptPath);

                    process.Start();

                    // wait exit signal from the app we called and then close it. 
                    process.WaitForExit();
                    process.Close();

                    if (this.Controller.CatchContent != string.Empty)
                    {
                        throw new Exception(this.Controller.CatchContent);
                    }

                    ShowMessage(MessageType.Info, "Finished fetching kegg entry files ...");
                }
                else
                {
                    ShowMessage(MessageType.Warning, "No file selected, please select one ...");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while collecting the Kegg entry files.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Executes a Python script which collects all genbank files from NCBI.
        /// </summary>
        private void CollectNcbiGenbankFiles()
        {
            try
            {
                // path to python script
                string scriptPath = this.Controller.NcbiGenbankScriptPath;
                string genomeFolderPath = this.Controller.NcbiGenomesDir;
                string accessionFolderPath = this.Controller.MainDir;
                // script arguments
                string arguments = string.Format("{0} {1} {2}", scriptPath, genomeFolderPath, accessionFolderPath);

                ShowMessage(MessageType.Info, "Starting to collect Genbank files....");

                // start python process
                ProcessStartInfo processStartInfo = new ProcessStartInfo("python");
                processStartInfo.Arguments = arguments;
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardOutput = false;

                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();

                process.WaitForExit();
                process.Close();

                if (this.Controller.CatchContent != string.Empty)
                {
                    throw new Exception(this.Controller.CatchContent);
                }

                ShowMessage(MessageType.Info, "Collecting of Genbank files was finished...");
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while collecting NCBI genbank files.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Shows dialog and calls PathwayViewer's : ReadGenbankFile, GetFileNamefromFilePath and WriteAccessionLocustagMapping methods.
        /// </summary>
        private void MapAccessionsToLocustag()
        {
            try
            {
                // boolean to detect errors
                bool detectedErrors = false;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.ShowDialog();

                ShowMessage(MessageType.Info, "Mapping of accessions to locus_tags. Please wait ...");
                if (dialog.FileNames.Length > 0)
                {
                    foreach (string filePath in dialog.FileNames)
                    {
                        List<Gene> accessionGenes = this.Controller.FileHelper.ReadGenbankFile(filePath);
                        string accessionName = this.Controller.FileHelper.GetFileNameFromFilePath(filePath);

                        // Write the accession
                        if (!this.Controller.FileHelper.WriteAccessionLocustagMapping(accessionName, accessionGenes, this.Controller.FileHelper.GetFileDirectory(dialog.FileNames[0])))
                        {
                            detectedErrors = true;
                        }
                    }
                }

                else
                {
                    ShowMessage(MessageType.Warning, "No files selected! Please select at least one ...");
                    detectedErrors = true;
                }

                if (!detectedErrors)
                {
                    ShowMessage(MessageType.Info, "Mapping of accessions to locus_tags succeeded ...");
                }
                else
                {
                    ShowMessage(MessageType.Error, "Mapping of accessions to locus_tags failed ...");
                }

                if (this.Controller.CatchContent != string.Empty)
                {
                    throw new Exception(this.Controller.CatchContent);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while mapping accessions with genes.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Shows dialogs and calls PathwayViewer's : ReadKeggFile,ReadNcbiMappingFileAndMapLocusTags 
        /// <para>and WriteAccessionLocustagPathwayMapping methods.</para>
        /// </summary>
        private void MapAccessionLocustagToPathways()
        {
            try
            {
                Dictionary<string, List<Pathway>> keggPathways = new Dictionary<string, List<Pathway>>();
                Dictionary<string, Dictionary<string, List<Pathway>>> ncbiGeneKeggPathways = new Dictionary<string, Dictionary<string, List<Pathway>>>();

                // open file dialog to select kegg mapping files
                OpenFileDialog dialogKeggFile = new OpenFileDialog();
                dialogKeggFile.Multiselect = true;
                dialogKeggFile.ShowDialog();

                ShowMessage(MessageType.Info, "Reading KEGG mapping files. Please wait ...");
                if (dialogKeggFile.FileNames.Length > 0)
                {
                    foreach (string fileName in dialogKeggFile.FileNames)
                    {
                        keggPathways = this.Controller.FileHelper.ReadKeggFile(fileName, keggPathways);
                    }
                    ShowMessage(MessageType.Info, "Reading of KEGG mapping files succeeded");
                }

                else
                {
                    ShowMessage(MessageType.Error, "Reading of KEGG mapping files failed");
                }

                // open file dialog to select ncbi mapping files
                OpenFileDialog dialogNcbiFile = new OpenFileDialog();
                dialogNcbiFile.Multiselect = true;
                dialogNcbiFile.ShowDialog();

                ShowMessage(MessageType.Info, "Reading NCBI mapping files. Please wait ...");
                if (dialogNcbiFile.FileNames.Length > 0)
                {
                    foreach (string fileName in dialogNcbiFile.FileNames)
                    {
                        // map accession to locus_tags pathways
                        ncbiGeneKeggPathways = this.Controller.FileHelper.ReadNcbiMappingFileAndMapLocusTags(fileName, keggPathways, ncbiGeneKeggPathways);
                        this.Controller.FileHelper.WriteAccessionLocustagPathwayMapping(ncbiGeneKeggPathways);
                    }
                    ShowMessage(MessageType.Info, "Reading of NCBI mapping files succeeded");

                }
                else
                {
                    ShowMessage(MessageType.Error, "Reading of NCBI mapping files failed");
                }

                if (this.Controller.CatchContent != string.Empty)
                {
                    throw new Exception(this.Controller.CatchContent);
                }

                ShowMessage(MessageType.Info, "Mapping of accession locus_tags to pathways succeeded");

            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while mapping Kegg locustag pathway with NCBI accession locustags.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Reads all taxonomy to accession id files and maps the taxonomy id to an accession of a complete genome.
        /// </summary>
        private void MapTaxonomyToAccessionIds()
        {
            try
            {
            
                Dictionary<int, List<string>> taxonomyAccessionDic = new Dictionary<int, List<string>>();
                Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic = ReadKeggNcbiMappingFile();

                // open new file dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.ShowDialog();

                if (dialog.FileNames.Length > 0)
                {
                    int counter = 0;
                    foreach (string file in dialog.FileNames)
                    {
                        // counter to count amount of files
                        counter += 1;
                        ShowMessage(MessageType.Info, string.Format("Reading Accession to TaxId mapping file {0} [{1} of {2}]. Please wait ...", file, counter, dialog.FileNames.Length));
                        taxonomyAccessionDic = this.Controller.FileHelper.MapTaxonomiesToAccessions(file, accessionLocustagPathwaysDic, taxonomyAccessionDic);
                    }
                    ShowMessage(MessageType.Info, "Mapping of Taxonomy to accessions has finished ...");

                    // write mapping file to directory
                    ShowMessage(MessageType.Info, "Writing Taxonomy to Accessions mapping file. Please wait ...");
                    if (this.Controller.FileHelper.WriteTaxonomyAccessionsMappingToFile(taxonomyAccessionDic))
                    {
                        ShowMessage(MessageType.Info, "Writing Taxonomy to Accessions mapping has finished ...");
                    }
                    else
                    {
                        ShowMessage(MessageType.Warning, "Writing Taxonomy to Accessions mapping has failed ...");
                    }
                }
                else
                {
                    ShowMessage(MessageType.Error, "No files selected ...");
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while mapping taxonomies to accessions.{0}Method: {1}.{2}{0}Exception: {3}",
                   Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Reads accessionLocustagPathwaysDic, taxIdAccessionsDic, accessionsSequenceDic into memory
        /// </summary>
        private void ReadDataInMemory()
        {
            try
            {
                if (!DataInMemory)
                {
                    // read all mapped data into memory
                    this.AccessionLocustagPathwaysDic = ReadKeggNcbiMappingFile();
                    
                    ShowMessage(MessageType.Info, "Reading taxonomy accession mapping file. Please wait ...");
                    this.TaxIdAccessionsDic = this.Controller.FileHelper.ReadTaxIdAccessionMappingFile();
                    ShowMessage(MessageType.Info, "Reading taxonomy accession mapping file was finished ...");

                    ShowMessage(MessageType.Info, "Reading the silva database. Please wait ...");
                    this.AccessionsSequenceDic = this.Controller.FileHelper.ReadSilvaDataset();
                    ShowMessage(MessageType.Info, "Reading of the silva database was finished ...");
                    
                    ShowMessage(MessageType.Info, "Reading the pathway dataset. Please wait ...");
                    this.PathwaysDic = this.Controller.FileHelper.ReadPathways();
                    ShowMessage(MessageType.Info, "Reading of the pathway dataset was finished ...");
                    
                    ShowMessage(MessageType.Info, "Mapping the pathway dataset to the AccessionLocustagPathways. Please wait ...");
                    this.AccessionLocustagPathwaysDic = this.Controller.FileHelper.MapPathwayNamesToAccessionLocustagPathways(this.PathwaysDic, this.AccessionLocustagPathwaysDic);
                    ShowMessage(MessageType.Info, "Mapping of the pathway dataset to the AccessionLocustagPathways was finished ...");

                    ShowMessage(MessageType.Info, "Collecting the unique Locustag Pathways. Please wait ...");
                    this.UniqueLocustagPathwayDic = this.Controller.FileHelper.CollectUniqueLocustagPathways(this.AccessionLocustagPathwaysDic);
                    ShowMessage(MessageType.Info, "Collection of the unique Locustag Pathway was finished ...");
                    
                    this.DataInMemory = true;
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while reading visualization data into memory.{0}Method: {1}.{2}{0}Exception: {3}",
                   Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }
             
        /// <summary>
        /// Reads mapping file with accessions locus_tags and pathways. And calls PathwayViewer's : ReadaccessionLocustagPathwayFile
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, List<Pathway>>> ReadKeggNcbiMappingFile()
        {
            Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic = new Dictionary<string, Dictionary<string, List<Pathway>>>();

            try
            {
                // open new file dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.ShowDialog();

                ShowMessage(MessageType.Info, "Reading KEGG/NCBI mapping file. Please wait ...");
                if (dialog.FileName.Length > 0)
                {
                    accessionLocustagPathwaysDic = this.Controller.FileHelper.ReadaccessionLocustagPathwayFile(dialog.FileName, accessionLocustagPathwaysDic);
                    ShowMessage(MessageType.Info, "Reading KEGG/NCBI mapping file was finished ...");
                }
                else
                {
                    ShowMessage(MessageType.Error, "No files selected ...");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while reading KEGG/NCBI mapping file.{0}Method: {1}.{2}{0}Exception: {3}",
                   Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }

            return accessionLocustagPathwaysDic;
        }


        /// <summary>
        /// Get all taxonomic information. Calls PathwayViewer's : ReadTaxonomies
        /// </summary>
        private void GetTaxonomies()
        {
            try
            {
                this.ShowMessage(MessageType.Info, "Reading Taxonomy database. Please wait ...");

                this.TaxonomyDicId = this.Controller.FileHelper.ReadTaxonomies();
                
                this.ShowMessage(MessageType.Info, "Finished reading Taxonomy database.");

                if (this.Controller.CatchContent != string.Empty)
                {
                    throw new Exception(this.Controller.CatchContent);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while retrieving taxonomies.{0}Method: {1}.{2}{0}Exception: {3}",
                   Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        ///  Calls PathwayViewers's : ReadAccessionFile and WriteAccessionSequenceToFasta
        /// </summary>
        /// <param name="accessionLocustagPathwaysDic">Dictionary containing accession,locus_tags pathways</param>
        /// <param name="taxonomyAccessionDic">Dictionary containing accession and taxonomies</param>
        /// <param name="accessionsSequenceDic">Dictionary containing accession and sequences</param>
        private void ReadAccessionFile(Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic, 
                                       Dictionary<int, List<string>> taxonomyAccessionDic,
                                       Dictionary<string, Dictionary<string, string>> accessionsSequenceDic)
        {
            try
            {
                // determine max number of accessions to process
                int accessionCutOff = 0;
                if (int.TryParse(this.textBoxAccessionCutOff.Text, out accessionCutOff))
                {
                    // open file select dialog
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.ShowDialog();

                    ShowMessage(MessageType.Info, "Reading accession file. Please wait ...");
                    if (dialog.FileName.Length > 0)
                    {
                        this.InputAccessions = this.Controller.FileHelper.ReadAccessionFile(dialog.FileName, accessionLocustagPathwaysDic, taxonomyAccessionDic, accessionsSequenceDic, accessionCutOff);
                        ShowMessage(MessageType.Info, "Reading of accession file was finished ...");
                        // writes sequences to separate fasta files
                        this.Controller.FileHelper.WriteAccessionSequenceToFasta(this.InputAccessions);
                    }
                    else
                    {
                        ShowMessage(MessageType.Error, "Reading of accession file failed ...");
                    }
                }
                else
                {
                    this.ShowMessage(MessageType.Warning, "Please assign a integer value for the accession cutoff.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while reading accession file.{0}Method: {1}.{2}{0}Exception: {3}",
                 Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Generates fasta files for the SSU and the LSU and calls PathwayViewer's: ReadaccessionLocustagPathwayFile.
        /// </summary>
        private void GenerateDataForMultipleSequenceAlignment()
        {
            try
            {
                int accessionCutOff = 0;
                // apply cut off value
                if (this.textBoxAccessionCutOff.Text != string.Empty)
                {
                    if (!int.TryParse(this.textBoxAccessionCutOff.Text, out accessionCutOff))
                    {
                        ShowMessage(MessageType.Warning, "Please enter a valid accession cut off value");
                    }
                    else
                    {
                        ReadAccessionFile(this.AccessionLocustagPathwaysDic, this.TaxIdAccessionsDic, this.AccessionsSequenceDic);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while visualizing accession mappings.{0}Method: {1}.{2}{0}Exception: {3}",
                   Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Generates a Multiple Sequence Alignment with MUSCLE
        /// </summary>
        /// <param name="extractCode">extraction code from project</param>
        private void CreateMultipleSequenceAlignment(string extractCode)
        {
            try
            {
                string muscleFilePath = this.Controller.MultipleSequenceAlignmentMusclePath;
                string multipleSequenceAlignmentOutputPath = this.Controller.MultipleSequenceAlignmentOutputPath;
              
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.ShowDialog();

                if (dialog.FileName.Length > 0)

                {
                    string fileName = string.Format("muscle_SSU_alignment_output_{0}.fasta", extractCode);
                    string filePath = multipleSequenceAlignmentOutputPath + fileName;
                    // MUSCLE arguments
                    string arguments = string.Format("-in {0} {1} -out {2} {3}", dialog.FileName, "-fasta", filePath, "-group");

                    ShowMessage(MessageType.Info, "Starting to create a multiple sequence alignment file....");

                    // start process 
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(muscleFilePath);
                    processStartInfo.Arguments = arguments;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.RedirectStandardOutput = false;

                    Process process = new Process();
                    process.StartInfo = processStartInfo;
                    process.Start();

                    // wait exit signal from the app we called and then close it. 
                    process.WaitForExit();
                    process.Close();
                    ShowMessage(MessageType.Info, "Finished with creating a multiple sequence alignment file....");

                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while creating a multiple sequence alignment.{0}Method: {1}.{2}{0} Exception: {3}",
                Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Generates a newick string from the previous generated multiple sequence alignment.
        /// </summary>
        /// <param name="extractCode">extraction code from project</param>
        private void GenerateNewickTreefromMultipleSequenceAlignment(string extractCode)
        {
            try
            {
                string newickTreeOutputFilePath = this.Controller.NewickTreeOutputPath;

                // open new file dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.ShowDialog();

                if (dialog.FileName.Length > 0)
                {
                    // determine variables for MUSCLE
                    string muscleFilePath = this.Controller.MultipleSequenceAlignmentMusclePath;
                    string outputFileName = string.Format("newick_tree_SSU_{0}.newick", extractCode);
                    string outputFilePath = newickTreeOutputFilePath + outputFileName;
                    string arguments = string.Format("{0} -in {1} -out {2} {3}", "-maketree", dialog.FileName, outputFilePath, "-cluster neighborjoining");

                    ShowMessage(MessageType.Info, "Starting to create a tree file based on the selected multiple sequence alignment file....");

                    // start process
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(muscleFilePath);
                    processStartInfo.Arguments = arguments;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.RedirectStandardOutput = false;

                    Process process = new Process();
                    process.StartInfo = processStartInfo;
                    process.Start();

                    // wait exit signal from the app we called and then close it. 
                    process.WaitForExit();
                    process.Close();
                    ShowMessage(MessageType.Info, "Finished creating a newick tree file ....");

                }
            }
            catch (Exception ex )
            {
              ShowMessage(MessageType.Error, string.Format("An error occurred while creating a multiple sequence alignment.{0}Method: {1}.{2}{0} Exception: {3}",
              Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Generates a phylogenetic tree.
        /// </summary>
        /// <param name="extractCode">extraction code from project</param>
        private void GeneratePhyloGeneticTreefromNewick(string extractCode)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.ShowDialog();

                string outputFilePath = string.Empty;
                if (dialog.FileName.Length > 0)
                {
                    ShowMessage(MessageType.Info, "Generating HTML phylogenetic tree output file ...");
                    string newickString = this.Controller.FileHelper.ReadNewickTreeOutputFile(dialog.FileName);
                    string htmlFile = this.Controller.FileHelper.ReadCompressedHtmlFile(newickString);
                    outputFilePath = this.Controller.FileHelper.WriteHtmlTreeFile(htmlFile, extractCode);
                    
                }
                ShowMessage(MessageType.Info, "Finished generating HTML phylogenetic tree output file ...");
                // open output in standard browser
                Process.Start(outputFilePath);
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while generating a tree.{0}Method: {1}.{2}{0} Exception: {3}",
                Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }

        }

        /// <summary>
        /// Generates a Krona Plot.
        /// </summary>
        private void GeneratePieChart()
        {
            try
            {
                if (this.InputAccessions != null)
                {
                    // Determine the unique locustags per pathway
                    ShowMessage(MessageType.Info, "Generating HTML circular graph output file ...");
                    this.Controller.FileHelper.WriteCirculairGraphExpressionHtmlFile(this.InputAccessions, this.UniqueLocustagPathwayDic, this.InputAccessions.ExtractCode);
                    ShowMessage(MessageType.Info, "Finished generating HTML circular graph output file ...");
                }
                else
                {
                    this.ShowMessage(MessageType.Warning, "No accessionfile selected.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while generating a piechart.{0}Method: {1}.{2}{0} Exception: {3}",
                Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion

        #region EVENTS


        /// <summary>
        ///  This section contains all the events for this tool.
        /// </summary>

        private void button_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    Button b = (Button)sender;
                    if (b.Tag.GetType().ToString() == "System.String")
                    {
                        string tip = (string)b.Tag;
                        ShowToolTip(tip, ToolTipIcon.None, b);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while hovering above a button.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            try
            {
                this.ToolTipCurrent.ForeColor = this.Style.White;
                this.ToolTipCurrent.BackColor = this.Style.Black;
                e.DrawBackground();
                e.DrawBorder();
                e.DrawText();
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, string.Format("An error occurred while drawing a tooltip.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        private void buttonCollectAllOrganism_Click(object sender, EventArgs e)
        {
            CollectAllOrganisms();
        }
       
        private void buttonCollectAllKeggEntryFiles_Click(object sender, EventArgs e)
        {
            CollectKeggEntryFiles();
        }

        private void buttonGetGenbankFiles_Click(object sender, EventArgs e)
        {
            CollectNcbiGenbankFiles();
        }

        private void buttonMapAccessionsToLocustag_Click(object sender, EventArgs e)
        {
            MapAccessionsToLocustag();
        }

        private void buttonMapPathwayData_Click(object sender, EventArgs e)
        {
            MapAccessionLocustagToPathways();
        }

        private void buttonMapAccessionToTaxIds_Click(object sender, EventArgs e)
        {
            MapTaxonomyToAccessionIds();
        }

        private void buttonReadDataInMemory_Click(object sender, EventArgs e)
        {
            ReadDataInMemory();
        }

        private void buttonGenerateSsuLsuFiles_Click(object sender, EventArgs e)
        {
            GenerateDataForMultipleSequenceAlignment();
        }

        private void buttonMultipleSequenceAlignment_Click(object sender, EventArgs e)
        {
            CreateMultipleSequenceAlignment(this.InputAccessions.ExtractCode);
           
        }

        private void buttonVisualizeMappedData_Click(object sender, EventArgs e)
        {
            GenerateNewickTreefromMultipleSequenceAlignment(this.InputAccessions.ExtractCode);
            GeneratePhyloGeneticTreefromNewick(this.InputAccessions.ExtractCode);
        }

        private void buttonVisualizeMappedDataToPieChart_Click(object sender, EventArgs e)
        {
            GeneratePieChart();
        }

        #endregion

    }
}
