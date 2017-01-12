namespace PathwayViewer
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Controller
    {
        #region FIELDS

        private Dictionary<string, Setting> Settings = null;

        public string CatchContent = string.Empty;

        // Ininitialize program file/folder paths
        public string Server = string.Empty;
        public string MainDir = string.Empty;
        public string KeggDir = string.Empty;
        public string KeggDatasetDir = string.Empty;
        public string KeggPathwaysDir = string.Empty;
        public string LogDir = string.Empty;

        public string NcbiDir = string.Empty;
        public string NcbiGenomesDir = string.Empty;
        public string KeggNcbiDir = string.Empty;
        public string InputAccessionDir = string.Empty;
        public string AlignmentSetsDir = string.Empty;
        public string KeggEntryScriptPath = string.Empty;
        public string NcbiGenbankScriptPath = string.Empty;
        public string AccessionsCollectingScriptPath = string.Empty;
        public string MultipleSequenceAlignmentMusclePath = string.Empty;

        public string SilvaDatasetFilePath = string.Empty;
        public string MultipleSequenceAlignmentOutputPath = string.Empty;
        public string NewickTreeOutputPath = string.Empty;

        public string TaxonomyFilePath = string.Empty;
        public string CompressedHtmlDir = string.Empty;
        public string CompressedHtmlFilePath = string.Empty;
        public string CompressedHtmlFileOutputPath = string.Empty;

        // Initialize different helper classes
        public WebHelper WebHelper = null;
        public FileHelper FileHelper = null;
        
        #endregion

        #region CONSTRUCTOR

        public Controller()
        {
            this.WebHelper = new WebHelper(this);
            this.FileHelper = new FileHelper(this);
            SetSettings();
        }

        #endregion 

        #region PUBLIC METHODS

        /// <summary>
        ///  Gets current Date
        /// </summary>
        /// <returns>Current date(year,month,day)</returns>
        public string GetCustomDate()
        {
            string dateTime = string.Empty;

            try
            {
                DateTime now = DateTime.Now;
                dateTime = string.Format("{0}{1}{2}", now.Year, now.Month.ToString().PadLeft(2, '0'), now.Day.ToString().PadLeft(2, '0'));
            }
            catch (Exception ex)
            {
                this.CatchContent += ex.Message;
            }

            return dateTime;
        }
        
        #endregion

        #region PRIVATE METHODs

        
        private void SetSettings()
        {
            try
            {
                this.Settings = this.FileHelper.ReadConfigurationFile();
                
                Setting serverSetting = GetSetting("Server");
                Setting mainDirSetting = GetSetting("MainDir");

                if (serverSetting != null && mainDirSetting != null)
                {
                    this.MainDir = Path.Combine(mainDirSetting.Value.Split(';'));
                    this.MainDir = Path.Combine(serverSetting.Value, this.MainDir);

                    LogDir = GetSettingVariable(LogDir, "LogDir");
                    KeggDir = GetSettingVariable(KeggDir, "KeggDir");
                    KeggPathwaysDir = GetSettingVariable(KeggPathwaysDir, "KeggPathwaysDir");
                    NcbiDir = GetSettingVariable(NcbiDir, "NcbiDir");
                    NcbiGenomesDir = GetSettingVariable(NcbiGenomesDir, "NcbiGenomesDir");
                    KeggNcbiDir = GetSettingVariable(KeggNcbiDir, "KeggNcbiDir");

                    InputAccessionDir = GetSettingVariable(InputAccessionDir, "InputAccessionDir");
                    AlignmentSetsDir = GetSettingVariable(AlignmentSetsDir, "AlignmentSetsDir");
                    TaxonomyFilePath = GetSettingVariable(TaxonomyFilePath, "TaxonomyFile");

                    KeggEntryScriptPath = GetSettingVariable(KeggEntryScriptPath, "KeggEntryScript");
                    NcbiGenbankScriptPath = GetSettingVariable(NcbiGenbankScriptPath, "NcbiGenbankScript");
                    AccessionsCollectingScriptPath = GetSettingVariable(AccessionsCollectingScriptPath, "AccessionsCollectingScript");
                    SilvaDatasetFilePath = GetSettingVariable(SilvaDatasetFilePath, "SilvaDatasetFile");

                    MultipleSequenceAlignmentMusclePath = GetSettingVariable(MultipleSequenceAlignmentMusclePath, "MSAMuscleToolFile");
                    string msaSettingVar = GetSettingVariable(MultipleSequenceAlignmentOutputPath, "MSAOutputDir");
                    MultipleSequenceAlignmentOutputPath = string.Format("{0}{1}", msaSettingVar, Path.DirectorySeparatorChar);
                    string newickSettingvar = GetSettingVariable(NewickTreeOutputPath, "NewickTreeOutputDir");
                    NewickTreeOutputPath = string.Format("{0}{1}", newickSettingvar, Path.DirectorySeparatorChar);

                    CompressedHtmlDir = GetSettingVariable(CompressedHtmlDir, "PhylogeneticTreeDir");
                    CompressedHtmlFilePath = GetSettingVariable(CompressedHtmlFilePath, "CompressedHtmlFile");
                    CompressedHtmlFileOutputPath = GetSettingVariable(CompressedHtmlFileOutputPath, "HtmlOutputDir");

                    //if (System.Environment.OSVersion.Platform)


                    // Replace global variables
                    //MainDir = @"\\172.16.6.53\StageStorage\PathwayViewer\";
                    //KeggDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Kegg";

                    //KeggDatasetDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Kegg\KeggOrganisms_20161128.kegg";
                    //KeggPathwaysDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Kegg\Pathways\";
                    //LogDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Logs";

                    //NcbiDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Ncbi";
                    //NcbiGenomesDir = @"\\172.16.6.53\StageStorage\Genomes\";
                    //KeggNcbiDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Kegg_Ncbi_Mapping";
                    //InputAccessionDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Accession\";
                    //AlignmentSetsDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Alignment\Input_Files";

                    //KeggEntryScriptPath = @"\\172.16.6.53\StageStorage\PathwayViewer\Scripts\keggdatacollector.py";
                    //NcbiGenbankScriptPath = @"\\172.16.6.53\StageStorage\PathwayViewer\Scripts\ncbi_datacollector.py";
                    //AccessionsCollectingScriptPath = @"\\172.16.6.53\StageStorage\PathwayViewer\Scripts\accessionscollector.py";
                    //MultipleSequenceAlignmentMusclePath = @"\\172.16.6.53\StageStorage\PathwayViewer\Tools\muscle3.8.31_i86win32.exe";

                    //SilvaDatasetFilePath = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Silva\silva.fasta";
                    //MultipleSequenceAlignmentOutputPath = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Alignment\Output_Files\";
                    //NewickTreeOutputPath = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Alignment\Output_Files\NewickTree\";

                    //TaxonomyFilePath = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Taxonomy\Taxonomy.tab";

                    //CompressedHtmlDir = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Tree";
                    //CompressedHtmlFilePath = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Tree\compressedHtmlScript.html";
                    //CompressedHtmlFileOutputPath = @"\\172.16.6.53\StageStorage\PathwayViewer\Datasets\Tree\OutPutHtmlFiles";
                }
            }
            catch (Exception ex)
            {
                this.CatchContent += ex.Message;
            }
        }

        private string GetSettingVariable(string variable, string settingKey)
        {
            try
            { 
                Setting setting = GetSetting(settingKey);
                if (setting != null)
                {
                    if (setting.Dir != string.Empty && setting.ValueType.ToUpper() != "LIST")
                    {
                        // Gebruik deze folder variable voor de value
                        string settingDirVariable = string.Empty;
                        settingDirVariable = GetSettingVariable(settingDirVariable, setting.Dir);
                        variable = Path.Combine(this.MainDir, settingDirVariable, setting.Value);
                    }
                    else if (setting.Dir == string.Empty && setting.ValueType.ToUpper() == "LIST")
                    {
                        // Bouw op uit verschillende folders
                        variable = Path.Combine(setting.Value.Split(';'));
                        variable = Path.Combine(this.MainDir, variable);
                    }
                    else if (setting.Dir != string.Empty && setting.ValueType.ToUpper() == "LIST")
                    {
                        string settingDirVariable = string.Empty;
                        settingDirVariable = GetSettingVariable(settingDirVariable, setting.Dir);
                        variable = Path.Combine(setting.Value.Split(';'));
                        variable = Path.Combine(this.MainDir, settingDirVariable, variable);
                    }
                    else
                    {
                        variable = Path.Combine(this.MainDir, setting.Value);
                    }
                    
                    if (setting.KeyType.ToUpper() == "DIR" || setting.KeyType.ToUpper() == "ROOT")
                    {
                        if (!Directory.Exists(variable))
                        {
                            throw new Exception(string.Format("{0} does not exist.", variable));
                        }
                    }

                    if (setting.KeyType.ToUpper() == "FILE")
                    {
                        if (!File.Exists(variable))
                        {
                            throw new Exception(string.Format("{0} does not exist.", variable));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.CatchContent += ex.Message;
            }

            return variable;
        }

        private Setting GetSetting(string key)
        {
            Setting setting = null;

            try
            {
                if (!this.Settings.TryGetValue(key, out setting))
                {
                    throw new Exception(string.Format("Unknown key: {0}", key));
                }
            }
            catch (Exception ex)
            {
                this.CatchContent += ex.Message;
            }

            return setting;
        }
        
        #endregion
    }
}
