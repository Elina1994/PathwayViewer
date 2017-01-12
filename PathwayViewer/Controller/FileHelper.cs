
namespace PathwayViewer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;


    /// <summary>
    /// This class contains several methods for reading, writing and mapping data based on files.
    /// </summary>
    public class FileHelper
    {
        #region FIELDS

        private Controller Controller = null;

        #endregion

        #region CONSTRUCTOR

        public FileHelper(Controller controller)
        {
            this.Controller = controller;
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Reformats genbank lines by removing unnecessary characters 
        /// </summary>
        /// <param name="genbankLine">line from a genbank file</param>
        /// <returns>reformatted genbank lines</returns>
        private string ReformatGenbankLine(string genbankLine)
        {
            string output = string.Empty;

            try
            {
                string reformattedGenbankLine = genbankLine.Replace(string.Format("     {0}", (char)47), "@"); // /
                while (reformattedGenbankLine.Contains("  "))
                {
                    reformattedGenbankLine = reformattedGenbankLine.Replace("  ", " ");
                }

                char sep = (char)64;            // @
                char doubleQoute = (char)34;    // "   
                char backSlash = (char)92;      // \
                string[] blocks = reformattedGenbankLine.Split(sep);
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i] != string.Empty)
                    {
                        output += string.Format("[{0}]", blocks[i].Trim().Replace(doubleQoute.ToString(), string.Empty).Replace(backSlash.ToString(), string.Empty).Trim()).Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return output;
        }

        /// <summary>
        /// Gets properties from gene regions
        /// </summary>
        /// <param name="geneLine"></param>
        /// <returns>Gene properties</returns>
        private Gene GetPropertiesFromGene(string geneLine)
        {
            Gene gene = new Gene();

            try
            {
                bool oldLocusTagDone = false;
                bool locusTagDone = false;
                bool geneDone = false;

                string[] lines = geneLine.Split((char)13);
                if (lines != null)
                {
                    for (int h = 0; h < lines.Length; h++)
                    {
                        if (!oldLocusTagDone || !locusTagDone || !geneDone)
                        {
                            char[] sep = { '[', ']' };
                            string[] IdValues = lines[h].Split(sep);

                            if (IdValues != null)
                            {
                                for (int i = 0; i < IdValues.Length; i++)
                                {
                                    if (!oldLocusTagDone || !locusTagDone || !geneDone)
                                    {
                                        if (IdValues[i] != string.Empty)
                                        {
                                            string[] parts = IdValues[i].Split('=');
                                            if (parts != null)
                                            {
                                                if (parts.Length >= 2)
                                                {
                                                    if (parts[0].ToUpper().Contains("LOCUS") && parts[0].ToUpper().Contains("TAG") && parts[0].ToUpper().Contains("OLD"))
                                                    {
                                                        // example -> /old_locus_tag="NAL212_0906"      
                                                        gene.OldLocusTag = parts[1];
                                                        oldLocusTagDone = true;
                                                    }

                                                    if (parts[0].ToUpper().Contains("LOCUS") && parts[0].ToUpper().Contains("TAG") && !parts[0].ToUpper().Contains("OLD"))
                                                    {
                                                        // example -> /locus_tag="NAL212_0906"      
                                                        gene.LocusTag = parts[1];
                                                        locusTagDone = true;
                                                    }

                                                    if (parts[0].ToUpper().Contains("GENE"))
                                                    {
                                                        // example -> /gene="pyrG"      
                                                        gene.GeneName = parts[1];
                                                        geneDone = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return gene;
        }

        /// <summary>
        /// (c) M.Burger.
        /// Reads content from JavaScript file for the creation of Krona
        /// </summary>
        /// <param name="graphType">Type of graph</param>
        /// <returns></returns>
        private string ReadCircularGraphJsContent(string graphType)
        {
            string circularGraphScript = string.Empty;

            try
            {
                string filePath = Path.Combine(this.Controller.CompressedHtmlDir, "maims_circular_graph.js");

                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(filePath);
                    string content = reader.ReadToEnd();
                    reader.Close();
                    string tmp = content.Replace("VARLOGOTEXT", string.Format("{0}", graphType));
                    circularGraphScript = tmp;
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return circularGraphScript;
        }

        /// <summary>
        /// (c) M.Burger.
        /// Generates the content of the Krona plot
        /// </summary>
        /// <param name="accessionFile">Object accessionFile</param>
        /// <param name="uniqueLocustagPathwayDic">Dictionary containing unique locus_tags per pathway</param>
        /// <returns>Content of the nodes within Krona</returns>
        private string GenerateNodeContentExpression(AccessionFile accessionFile, Dictionary<string, Pathway> uniqueLocustagPathwayDic)
        {
            string nodeContent = string.Empty;

            try
            {
                int sumTotalReadAmount = 0;

                // Pathways : classes, subclasses, names
                Dictionary<string, Dictionary<string, Dictionary<string, List<Accession>>>> pathwayClassesDic = new Dictionary<string, Dictionary<string, Dictionary<string, List<Accession>>>>();
                Dictionary<string, Dictionary<string, List<Accession>>> pathwaySubClassesDic = new Dictionary<string, Dictionary<string, List<Accession>>>();
                Dictionary<string, List<Accession>> pathwayNamesDic = new Dictionary<string, List<Accession>>();
                List<Accession> accessions = new List<Accession>();

                // Annotate
                foreach (KeyValuePair<string, List<Accession>> accessionList in accessionFile.AccessionListDic)
                {
                    foreach (Accession accession in accessionList.Value)
                    {
                        foreach (Dictionary<string, List<Pathway>> locustags in accession.WholeGenomeLocusTagsPathWaysDicList)
                        {
                            foreach (KeyValuePair<string, List<Pathway>> locustag in locustags)
                            {
                                // Filter on unique pathways
                                Pathway pathwayUnique = null;
                                if (uniqueLocustagPathwayDic.TryGetValue(locustag.Key, out pathwayUnique))
                                {
                                    foreach (Pathway pathway in locustag.Value)
                                    {
                                        // Categorize
                                        if (pathwayClassesDic.TryGetValue(pathway.Class, out pathwaySubClassesDic))
                                        {
                                            // Check if subclass exists
                                            if (pathwaySubClassesDic.TryGetValue(pathway.SubClass, out pathwayNamesDic))
                                            {
                                                // Check if pathway name exists
                                                if (pathwayNamesDic.TryGetValue(pathway.Name, out accessions))
                                                {
                                                    bool isPresent = false;
                                                    foreach (Accession item in accessions)
                                                    {
                                                        if (item.Name == accession.Name)
                                                        {
                                                            isPresent = true;
                                                            break;
                                                        }
                                                    }

                                                    if (!isPresent)
                                                    {
                                                        sumTotalReadAmount += accession.ReadTotalAmount;

                                                        pathwayClassesDic[pathway.Class][pathway.SubClass][pathway.Name].Add(accession);
                                                    }
                                                }
                                                else
                                                {
                                                    sumTotalReadAmount += accession.ReadTotalAmount;

                                                    // Create accession list 
                                                    accessions = new List<Accession>();
                                                    // Add the accession to the accession list
                                                    accessions.Add(accession);
                                                    // Add the accession list to the pathway names dic
                                                    pathwayNamesDic.Add(pathway.Name, accessions);
                                                    // Add the pathway subclasses dic to the pathway class dic
                                                    pathwayClassesDic[pathway.Class][pathway.SubClass] = pathwayNamesDic;
                                                }
                                            }
                                            else
                                            {
                                                sumTotalReadAmount += accession.ReadTotalAmount;

                                                // Create accession list 
                                                accessions = new List<Accession>();
                                                // Add the accession to the accession list
                                                accessions.Add(accession);
                                                // Create new pathway name Dic
                                                pathwayNamesDic = new Dictionary<string, List<Accession>>();
                                                // Add the accession list to the pathway names dic
                                                pathwayNamesDic.Add(pathway.Name, accessions);
                                                // Create new pathway subclass Dic
                                                //pathwaySubClassesDic = new Dictionary<string, Dictionary<string, List<Accession>>>();
                                                // Add the pathway names dic to the pathway subclas dic
                                                pathwaySubClassesDic.Add(pathway.SubClass, pathwayNamesDic);
                                                // Add the pathway subclass dic to the pathway class dic
                                                pathwayClassesDic[pathway.Class] = pathwaySubClassesDic;
                                            }
                                        }
                                        else
                                        {
                                            if (pathway.Class != string.Empty)
                                            {
                                                sumTotalReadAmount += accession.ReadTotalAmount;
                                                
                                                // Create accession list 
                                                accessions = new List<Accession>();
                                                // Add the accession to the accession list
                                                accessions.Add(accession);
                                                // Create new pathwaynameDic
                                                pathwayNamesDic = new Dictionary<string, List<Accession>>();
                                                // Add accession list to pathway name dic
                                                pathwayNamesDic.Add(pathway.Name, accessions);
                                                // Create new pathwaySubclassesDic
                                                pathwaySubClassesDic = new Dictionary<string, Dictionary<string, List<Accession>>>();
                                                // Add the pathway names dic to the pathway subclas dic
                                                pathwaySubClassesDic.Add(pathway.SubClass, pathwayNamesDic);
                                                // Add the pathway subclass dic to the pathway class dic
                                                pathwayClassesDic.Add(pathway.Class, pathwaySubClassesDic);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Generate the NodeContent
                int newTotalReadAmount = 0;
                string endNodeTemplate = string.Format("</node>{0}", (char)10);
                nodeContent += GetNode(accessionFile.ExtractCode, sumTotalReadAmount, 0);
                foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<Accession>>>> pathwayClass in pathwayClassesDic)
                {
                    int pathwayClassReadAmount = GetPathwayClassNodeReadAmount(pathwayClass.Value);
                    nodeContent += GetNode(pathwayClass.Key, pathwayClassReadAmount, 0.0f);
                    foreach (KeyValuePair<string, Dictionary<string, List<Accession>>> pathwaySubClass in pathwayClass.Value)
                    {
                        int pathwaySubClassReadAmount = GetPathwaySubClassNodeReadAmount(pathwaySubClass.Value);
                        nodeContent += GetNode(pathwaySubClass.Key, pathwaySubClassReadAmount, 0.0f);
                        foreach (KeyValuePair<string, List<Accession>> pathwayName in pathwaySubClass.Value)
                        {
                            int pathwayNameReadAmount = GetPathwayNameReadAmount(pathwayName.Value);
                            nodeContent += GetNode(pathwayName.Key, pathwayNameReadAmount, 0.0f);
                            foreach (Accession accession in pathwayName.Value)
                            {
                                newTotalReadAmount += accession.ReadTotalAmount;

                                nodeContent += GetNode(accession.MRT_Taxonomy.ScientificName, accession.ReadTotalAmount, 0.0f);

                                nodeContent += endNodeTemplate;
                            }
                            nodeContent += endNodeTemplate;
                        }
                        nodeContent += endNodeTemplate;
                    }
                    nodeContent += endNodeTemplate;
                }
                nodeContent += endNodeTemplate;
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return nodeContent;
        }

        /// <summary>
        /// Gets read amount of the pathway classes nodes
        /// </summary>
        /// <param name="pathwayNames">Names of the pathways</param>
        /// <returns>integer with read amount</returns>
        private int GetPathwayClassNodeReadAmount(Dictionary<string, Dictionary<string, List<Accession>>> pathwayNames)
        {
            int pathwayClassReadAmount = 0;

            try
            {
                foreach (KeyValuePair<string, Dictionary<string, List<Accession>>> pathwayName in pathwayNames)
                {
                    pathwayClassReadAmount += GetPathwaySubClassNodeReadAmount(pathwayName.Value);
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return pathwayClassReadAmount;
        }

        /// <summary>
        /// Gets read amount of the pathway subclasses nodes
        /// </summary>
        /// <param name="pathwayNames">names of the pathways</param>
        /// <returns>integer with read amount</returns>
        private int GetPathwaySubClassNodeReadAmount(Dictionary<string, List<Accession>> pathwayNames)
        {
            int pathwayClassReadAmount = 0;

            try
            {
                foreach (KeyValuePair<string, List<Accession>> pathwayName in pathwayNames)
                {
                    pathwayClassReadAmount += GetPathwayNameReadAmount(pathwayName.Value);
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return pathwayClassReadAmount;
        }

        /// <summary>
        /// Gets read amount of the pathway name nodes
        /// </summary>
        /// <param name="accessions">List with accession numbers</param>
        /// <returns>integer with read amount</returns>
        private int GetPathwayNameReadAmount(List<Accession> accessions)
        {
            int pathwayNameReadAmount = 0;

            try
            {
                foreach (Accession accession in accessions)
                {
                    pathwayNameReadAmount += accession.ReadTotalAmount;
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return pathwayNameReadAmount;
        }
        
        private string GetNode(string nodeName, int magnitude, float score)
        {
            string node = string.Empty;

            try
            {
                string nodeTemplate = string.Format("<node name=\"VARNODENAME\"><magnitude><val> VARMAGNITUDE</val></magnitude><score><val> VARSCORE</val></score>{0}", (char)10);
                node = nodeTemplate.Replace("VARNODENAME", nodeName).Replace("VARMAGNITUDE", magnitude.ToString()).Replace("VARSCORE", score.ToString());
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return node;
        }

        #endregion

        #region PUBLIC METHODS

        public Dictionary<string, Setting> ReadConfigurationFile()
        {
            Dictionary<string, Setting> settings = new Dictionary<string, Setting>();

            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "PathwayViewer.conf");

                if (File.Exists(filePath))
                {
                    bool headerCollected = false;
                    
                    int keyId = -1;
                    int keyTypeId = -1;
                    int valueId = -1;
                    int valueTypeId = -1;
                    int dirId = -1;
                    
                    StreamReader reader = new StreamReader(filePath);
                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] lineParts = line.Split((char)9);

                        switch (headerCollected)
                        {
                            case false:
                                if (line.ToUpper().StartsWith("#KEY"))
                                {
                                    for (int i = 0; i < lineParts.Length; i++)
                                    {
                                        if (lineParts[i].ToUpper().Contains("KEY") && !lineParts[i].ToUpper().Contains("TYPE")) { keyId = i; }
                                        if (lineParts[i].ToUpper().Contains("KEYTYPE")) { keyTypeId = i; }
                                        if (lineParts[i].ToUpper().Contains("VALUE") && !lineParts[i].ToUpper().Contains("TYPE")) { valueId = i; }
                                        if (lineParts[i].ToUpper().Contains("VALUETYPE")) { valueTypeId = i; }
                                        if (lineParts[i].ToUpper().Contains("DIR")) { dirId = i; }
                                    }
                                    headerCollected = true;
                                }
                                break;

                            case true:
                                // Data from file
                                if (!line.StartsWith("#") && line.Trim() != string.Empty)
                                {
                                    Setting setting = new Setting();
                                    if (keyId != -1 && lineParts.Length > keyId) { setting.Key = lineParts[keyId]; }
                                    if (keyTypeId != -1 && lineParts.Length > keyTypeId) { setting.KeyType = lineParts[keyTypeId]; }
                                    if (valueId != -1 && lineParts.Length > valueId) { setting.Value = lineParts[valueId]; }
                                    if (valueTypeId != -1 && lineParts.Length > valueTypeId) { setting.ValueType = lineParts[valueTypeId]; }
                                    if (dirId != -1 && lineParts.Length > dirId) { setting.Dir = lineParts[dirId]; }

                                    Setting settingTmp = null;
                                    if (!settings.TryGetValue(setting.Key, out settingTmp))
                                    {
                                        settings.Add(setting.Key, setting);
                                    }
                                }

                                break;

                            default:
                                break;
                        }
                    }
                    reader.Close();
                }
                else
                {
                    throw new Exception("Configuration file could not be found.");
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return settings;
        }

        /// <summary>
        /// Writes error message to a log file.
        /// </summary>
        /// <param name="message">Used to indicate an error</param>
        /// <returns>succes</returns>
        public bool WriteError(string message)
        {
            bool succes = false;

            try
            {
                string fileName = string.Format("error_{0}_{1}.log", Environment.UserName, this.Controller.GetCustomDate());
                string filePath = Path.Combine(this.Controller.LogDir, fileName);

                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine(message);
                writer.Close();

                succes = true;
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return succes;
        }
        
        /// <summary>
        /// Gets file name from a complete file path 
        /// </summary>
        /// <param name="filePath">Indicates a path a file</param>
        /// <returns>name of file</returns>
        public string GetFileNameFromFilePath(string filePath)
        {
            string fileName = string.Empty;

            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fi = new FileInfo(filePath);
                    fileName = fi.Name;
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return fileName;
        }
        
        /// <summary>
        ///  Gets the directory of a file
        /// </summary>
        /// <param name="filePath">Indidicates a path to a file</param>
        /// <returns>Directory of a file</returns>
        public string GetFileDirectory(string filePath)
        {
            string fileDir = string.Empty;

            try
            {
                FileInfo file = new FileInfo(filePath);
                DirectoryInfo dir = file.Directory;
                fileDir = dir.Name;
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return fileDir;
        }
        
        /// <summary>
        /// Saves all collected organisms from KEGG to one single file,
        /// </summary>
        /// <param name="content">Contains the content of all the collected organisms</param>
        /// <returns>path to output file</returns>
        public string WriteKeggOrganismFile(string content)
        {
            string outputPath = string.Empty;

            try
            {
                string fileName = string.Format("KeggOrganisms_{0}.kegg", this.Controller.GetCustomDate());
                string filePath = Path.Combine(this.Controller.KeggDir, fileName);
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.Write(content);
                writer.Close();

                outputPath = filePath;
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return outputPath;
        }
        
        /// <summary>
        /// Reads genbank files and gets gene regions
        /// </summary>
        /// <param name="filePath">Path to genbank files</param>
        /// <returns>A list with gene properties (Name, locus_tags etc)</returns>
        public List<Gene> ReadGenbankFile(string filePath)
        {
            List<Gene> accessionGenes = new List<Gene>();

            try
            {
                List<string> lines = new List<string>();

                // get genelines from file
                if (File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);

                    int lineIndex = 0;
                    List<int> geneLineIds = new List<int>();

                    StreamReader reader = new StreamReader(file.FullName);
                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        lines.Add(line);

                        if (line.ToUpper().StartsWith("     GENE   "))
                        {
                            geneLineIds.Add(lineIndex);
                        }

                        lineIndex += 1;
                    }
                    reader.Close();

                    for (int i = 0; i < geneLineIds.Count; i++)
                    {
                        string geneLine = lines[geneLineIds[i]];
                        geneLineIds[i] += 1;
                        if (i < geneLineIds.Count - 1)
                        {
                            while (lines[geneLineIds[i]].StartsWith("              "))
                            {
                                geneLine += string.Format("{0}", lines[geneLineIds[i]]);
                                geneLineIds[i] += 1;
                            }
                        }

                        if (i == geneLineIds.Count - 1)
                        {
                            while (geneLineIds[i] < lines.Count)
                            {
                                if (lines[geneLineIds[i]].StartsWith("              "))
                                {
                                    geneLine += string.Format("{0}", lines[geneLineIds[i]]);
                                    geneLineIds[i] += 1;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        geneLine = ReformatGenbankLine(geneLine);
                        Gene gene = GetPropertiesFromGene(geneLine);

                        accessionGenes.Add(gene);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return accessionGenes;
        }
        
        /// <summary>
        /// Writes all accessions with their locus_tags to a file.
        /// </summary>
        /// <param name="accessionName">Accession number from NCBI</param>
        /// <param name="accessionGenes">Locus_tags from genbank files</param>
        /// <param name="folderName">Used to seperate files by rank</param>
        /// <returns>succes</returns>
        public bool WriteAccessionLocustagMapping(string accessionName, List<Gene> accessionGenes, string folderName)
        {
            bool succes = false;

            try
            {
                StreamWriter writer = null;
                string fileName = string.Format("AccessionLocustags_{0}.mapping", folderName);
                string filePath = Path.Combine(this.Controller.NcbiDir, fileName);

                if (!File.Exists(filePath))
                {
                    writer = new StreamWriter(filePath, true);
                    writer.WriteLine(string.Format("Accession:{0}LocusTag:{0}OldLocusTag:", (char)9));
                }
                else
                {
                    writer = new StreamWriter(filePath, true);
                }

                foreach (Gene gene in accessionGenes)
                {
                    string line = string.Format("{1}{0}{2}{0}{3}", (char)9, accessionName.Substring(0, accessionName.LastIndexOf(".")), gene.LocusTag, gene.OldLocusTag);
                    if (gene.LocusTag != string.Empty)
                    {
                        writer.WriteLine(line);
                    }
                }
                writer.Close();

                succes = true;
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return succes;
        }

        /// <summary>
        ///  <para>Reads all collected kegg files.</para> 
        ///  <para>And stores the locus_tags and pathways of the file into a dictionary.</para>
        /// </summary>
        /// <param name="keggFilePath">Path to all the collected kegg files</param>
        /// <param name="locusTagDict">Stores locus_tags and pathways</param>
        /// <returns>A dictionary with locus_tags and pathways</returns>
        public Dictionary<string, List<Pathway>> ReadKeggFile(string keggFilePath, Dictionary<string, List<Pathway>> locusTagDict)
        {
            try
            {
                if (File.Exists(keggFilePath))
                {
                    FileInfo file = new FileInfo(keggFilePath);
                    StreamReader reader = new StreamReader(file.FullName);

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] lineParts = line.Split((char)9);

                        if (lineParts.Length >= 2)
                        {
                            Pathway pathway = new Pathway();
                            pathway.Code = lineParts[0].Split(':')[1];
                            string locustagValue = lineParts[1].Split(':')[1].ToUpper().Trim();

                            List<Pathway> pathways = null;
                            if (locusTagDict.TryGetValue(locustagValue, out pathways))
                            {
                                locusTagDict[locustagValue].Add(pathway);
                            }
                            else
                            {
                                pathways = new List<Pathway>();
                                pathways.Add(pathway);

                                locusTagDict.Add(locustagValue, pathways);
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return locusTagDict;
        }

        /// <summary>
        /// <para>Reads mapping file with accession numbers and locus_tags.</para>
        /// <para>And maps locus_tags with pathways from KEGG.</para>
        /// </summary>
        /// <param name="ncbiFilePath">Path to all mapping files from NCBI</param>
        /// <param name="locusTagDict">Dictionary containing accession(key) and locus_tags(value)</param>
        /// <param name="accessionLocustagPathwaysDic">Dictionary containing accession(key) with a Dictionary(value) containing: locus_tag(key) and pathways(value) </param>
        /// <returns>accessionLocustagPathwaysDic</returns>
        public Dictionary<string, Dictionary<string, List<Pathway>>> ReadNcbiMappingFileAndMapLocusTags(string ncbiFilePath, Dictionary<string, List<Pathway>> locusTagDict, Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic)
        {
            try
            {
                if (File.Exists(ncbiFilePath))
                {
                    FileInfo file = new FileInfo(ncbiFilePath);
                    StreamReader reader = new StreamReader(file.FullName);

                    while (reader.Peek() > 0)
                    {
                        Gene gene = new Gene();
                        Accession accession = new Accession();

                        string line = reader.ReadLine();
                        string header = string.Empty;

                        // Skip header and get content
                        if (!line.Contains(":"))
                        {
                            string[] lineParts = line.Split((char)9);
                            if (lineParts.Length >= 2)
                            {
                                accession.Name = lineParts[0].ToUpper().Trim();
                                gene.LocusTag = lineParts[1].ToUpper().Trim();

                                if (lineParts.Length >= 3)
                                {
                                    gene.OldLocusTag = lineParts[2];
                                }

                                List<Pathway> pathwayList = null;
                                if (locusTagDict.TryGetValue(gene.LocusTag, out pathwayList))
                                {
                                    Dictionary<string, List<Pathway>> newLocusTagDict = null;
                                    // Checks if accession id already exists if not create new locustag dictionary
                                    if (!accessionLocustagPathwaysDic.TryGetValue(accession.Name, out newLocusTagDict))
                                    {
                                        newLocusTagDict = new Dictionary<string, List<Pathway>>();
                                        newLocusTagDict.Add(gene.LocusTag, pathwayList);
                                        accessionLocustagPathwaysDic.Add(accession.Name, newLocusTagDict);
                                    }
                                    else
                                    {   // if accession exists add locusTag plus pathway
                                        List<Pathway> pathwayListTmp = null;
                                        if (!accessionLocustagPathwaysDic[accession.Name].TryGetValue(gene.LocusTag, out pathwayListTmp))
                                        {
                                            accessionLocustagPathwaysDic[accession.Name].Add(gene.LocusTag, pathwayList);
                                        }
                                        else
                                        {
                                            //pathway list cannot be empty
                                            if (pathwayListTmp == null)
                                            {
                                                pathwayListTmp = new List<Pathway>();
                                            }
                                            pathwayListTmp.AddRange(pathwayList);
                                            accessionLocustagPathwaysDic[accession.Name][gene.LocusTag] = pathwayListTmp;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return accessionLocustagPathwaysDic;
        }

        /// <summary>
        /// Writes the content of the accessionLocustagPathwaysDic to a file.
        /// </summary>
        /// <param name="accessionLocustagPathwaysDic">Dictionary with accessions locustags and pathways</param>
        /// <returns>boolean succes</returns>
        public bool WriteAccessionLocustagPathwayMapping(Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic)
        {
            bool succes = false;

            try
            {
                StreamWriter writer = null;
                string fileName = string.Format("AccessionLocustagPathwayMapping_{0}.mapping", this.Controller.GetCustomDate());
                string filePath = Path.Combine(this.Controller.KeggNcbiDir, fileName);

                if (!File.Exists(filePath))
                {
                    writer = new StreamWriter(filePath, false);
                    writer.WriteLine(string.Format("Accession:{0}LocusTag:{0}Pathway:", (char)9));
                }
                else
                {
                    writer = new StreamWriter(filePath, false);
                }

                foreach (KeyValuePair<string, Dictionary<string, List<Pathway>>> locusTagMapping in accessionLocustagPathwaysDic)
                {
                    foreach (KeyValuePair<string, List<Pathway>> locustagPathway in locusTagMapping.Value)
                    {
                        foreach (Pathway pathway in locustagPathway.Value)
                        {
                            if (locusTagMapping.Key != string.Empty && locustagPathway.Key != string.Empty && pathway.Code != string.Empty)
                            {
                                string line = string.Format("{1}{0}{2}{0}{3}", (char)9, locusTagMapping.Key, locustagPathway.Key, pathway.Code);
                                writer.WriteLine(line);
                            }
                        }
                    }
                }
                writer.Close();
            }

            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return succes;
        }

        /// <summary>
        /// Reads the accessionLocustagPathway mapping file into memory
        /// </summary>
        /// <param name="filePath">path to accessionLocustagPathway mapping file</param>
        /// <param name="accessionLocustagPathwaysDic">Dictionary containing accessions,locus_tags and pathways</param>
        /// <returns>accessionLocustagPathwaysDic</returns>
        public Dictionary<string, Dictionary<string, List<Pathway>>> ReadaccessionLocustagPathwayFile(string filePath, Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic)
        {

            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    StreamReader reader = new StreamReader(file.FullName);

                    int lineCounter = 0;
                    while (reader.Peek() > 0)
                    {
                        lineCounter += 1;

                        Gene gene = new Gene();
                        Accession accession = new Accession();
                        Pathway pathway = new Pathway();

                        string line = reader.ReadLine();
                        if (!line.Contains(":"))
                        {
                            string[] lineParts = line.Split((char)9);
                            if (lineParts.Length >= 3)
                            {
                                if (lineParts[0].ToUpper().Trim() != string.Empty &&
                                    lineParts[1].ToUpper().Trim() != string.Empty &&
                                    lineParts[2].ToUpper().Trim() != string.Empty)
                                {
                                    accession.Name = lineParts[0].ToUpper().Trim();
                                    gene.LocusTag = lineParts[1].ToUpper().Trim();
                                    pathway.Code = lineParts[2].ToUpper().Trim();
                                }
                            }

                            Dictionary<string, List<Pathway>> locusTagDic = null;
                            //check if accession already exists 
                            if (accessionLocustagPathwaysDic.TryGetValue(accession.Name, out locusTagDic))
                            {
                                List<Pathway> pathwayList = null;
                                //check if locus tag already exists
                                if (accessionLocustagPathwaysDic[accession.Name].TryGetValue(gene.LocusTag, out pathwayList))
                                {
                                    //if locus tag exists only add pathway
                                    if (locusTagDic.TryGetValue(gene.LocusTag, out pathwayList))
                                    {
                                        locusTagDic[gene.LocusTag].Add(pathway);
                                    }

                                }
                                else
                                {
                                    //add locus tag plus pathways
                                    pathwayList = new List<Pathway>();
                                    pathwayList.Add(pathway);
                                    accessionLocustagPathwaysDic[accession.Name].Add(gene.LocusTag, pathwayList);
                                }

                            }
                            else
                            {
                                //if accession does not yet exist add it
                                List<Pathway> pathwayList = new List<Pathway>();
                                locusTagDic = new Dictionary<string, List<Pathway>>();
                                pathwayList.Add(pathway);
                                locusTagDic.Add(gene.LocusTag, pathwayList);
                                accessionLocustagPathwaysDic.Add(accession.Name, locusTagDic);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return accessionLocustagPathwaysDic;
        }

        //TODO: summary
        public Dictionary<int, Taxonomy> ReadTaxonomies()
        {
            Dictionary<int, Taxonomy> taxonomies = new Dictionary<int, Taxonomy>();

            try
            {
                string filePath = this.Controller.TaxonomyFilePath;
                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(filePath);
                    bool headerCollected = false;

                    int taxId = -1;
                    int scientificNameId = -1;

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] lineParts = line.Split((char)9);

                        if (lineParts.Length >= 6)
                        {
                            switch (headerCollected)
                            {
                                case false:
                                    if (line.ToUpper().StartsWith("ID"))
                                    {
                                        // Get headerline and determine column indexes
                                        // Id:	ScientificName:	PhylumId:	FamilyId:	GenusId:	SpeciesId:	
                                        for (int i = 0; i < lineParts.Length; i++)
                                        {
                                            if (lineParts[i].ToUpper() == ("ID:")) { taxId = i; }
                                            if (lineParts[i].ToUpper().Contains("SCIENTIFICNAME")) { scientificNameId = i; }
                                        }

                                        headerCollected = true;
                                    }
                                    break;

                                case true:

                                    // Get object
                                    Taxonomy record = new Taxonomy();
                                    int.TryParse(lineParts[taxId], out record.Id);
                                    record.ScientificName = lineParts[scientificNameId];

                                    taxonomies.Add(record.Id, record);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return taxonomies;
        }

        /// <summary>
        /// Maps the taxonomy identity to accession numbers
        /// </summary>
        /// <param name="filePath">Path to taxonomy files</param>
        /// <param name="accessionLocustagPathwaysDic">Dictionary with accessions, locus_tags and pathways</param>
        /// <param name="taxonomyAccessionsDic">Dictionary with taxonomy ID (key) and accession numbers(value)</param>
        /// <returns>Dictionary with a taxonomy ID and accession numbers</returns>
        public Dictionary<int, List<string>> MapTaxonomiesToAccessions(string filePath, Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic, Dictionary<int, List<string>> taxonomyAccessionsDic)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(filePath);

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] lineParts = line.Split((char)9);

                        // accession    accession.version   taxid   gi
                        int accessionIdId = 0;
                        int taxIdId = 2;

                        if (lineParts.Length >= 3)
                        {
                            if (!lineParts[accessionIdId].ToUpper().StartsWith("ACCESSION"))
                            {
                                string accessionId = lineParts[accessionIdId];
                                int taxonomyId = -1;
                                if (int.TryParse(lineParts[taxIdId], out taxonomyId))
                                {
                                    Dictionary<string, List<Pathway>> accessionLocusTagsPathways = null;
                                    if (accessionLocustagPathwaysDic.TryGetValue(accessionId, out accessionLocusTagsPathways))
                                    {
                                        List<string> accessions = null;
                                        if (!taxonomyAccessionsDic.TryGetValue(taxonomyId, out accessions))
                                        {
                                            accessions = new List<string>();
                                            accessions.Add(accessionId);
                                            taxonomyAccessionsDic.Add(taxonomyId, accessions);
                                        }
                                        else
                                        {
                                            taxonomyAccessionsDic[taxonomyId].Add(accessionId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return taxonomyAccessionsDic;
        }

        /// <summary>
        /// Writes the taxonomy ID with the right accession numbers to a file
        /// </summary>
        /// <param name="taxonomyAccessionDic">Dictionary with taxonomy ID and accession numbers</param>
        /// <returns>boolean succes</returns>
        public bool WriteTaxonomyAccessionsMappingToFile(Dictionary<int, List<string>> taxonomyAccessionDic)
        {
            bool succes = false;

            try
            {
                string filePath = Path.Combine(this.Controller.KeggNcbiDir, "TaxId_Accession_mapping.tab");
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine(string.Format("TaxonomyId:{0}AccessionId:", (char)9));
                    foreach (KeyValuePair<int, List<string>> taxonomyId in taxonomyAccessionDic)
                    {
                        foreach (string accession in taxonomyId.Value)
                        {
                            string line = string.Format("{1}{0}{2}", (char)9, taxonomyId.Key, accession);
                            writer.WriteLine(line);
                        }
                    }

                    writer.Close();

                    succes = true;
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return succes;
        }

        /// <summary>
        /// Reads taxonomyIdAccession mapping file into memory
        /// </summary>
        /// <returns>Dictionary taxonomyAccessionsDic</returns>
        public Dictionary<int, List<string>> ReadTaxIdAccessionMappingFile()
        {
            Dictionary<int, List<string>> taxonomyAccessionsDic = new Dictionary<int, List<string>>();

            try
            {
                string filePath = Path.Combine(this.Controller.KeggNcbiDir, "TaxId_Accession_mapping.tab");
                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(filePath);

                    // TaxonomyId    AccessionId
                    int taxIdId = 0;
                    int accessionIdId = 1;

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] lineParts = line.Split((char)9);
                        if (lineParts.Length >= 2)
                        {
                            if (!lineParts[0].ToUpper().Contains("TAXONOMYID"))
                            {
                                int taxonomyId = -1;
                                if (int.TryParse(lineParts[taxIdId], out taxonomyId))
                                {
                                    string accessionId = lineParts[accessionIdId];

                                    List<string> accessions = null;
                                    if (!taxonomyAccessionsDic.TryGetValue(taxonomyId, out accessions))
                                    {
                                        accessions = new List<string>();
                                        accessions.Add(accessionId);
                                        taxonomyAccessionsDic.Add(taxonomyId, accessions);
                                    }
                                    else
                                    {
                                        taxonomyAccessionsDic[taxonomyId].Add(accessionId);
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return taxonomyAccessionsDic;
        }

        /// <summary>
        /// <para>Reads the entire silva database into memory.</para>
        /// <para>And maps the accession to the right sequence and silvaType </para>
        /// </summary>
        /// <returns>Dictionary silvaTypeAccessionsSequenceDic</returns>
        public Dictionary<string, Dictionary<string, string>> ReadSilvaDataset()
        {
            Dictionary<string, Dictionary<string, string>> silvaTypeAccessionsSequencesDic = new Dictionary<string, Dictionary<string, string>>();

            StreamReader reader = null;

            try
            {
                if (File.Exists(this.Controller.SilvaDatasetFilePath))
                {
                    reader = new StreamReader(this.Controller.SilvaDatasetFilePath);
                    string silvaType = string.Empty;
                    string accession = string.Empty;
                    string sequence = string.Empty;

                    Dictionary<string, string> accessionsSequenceLsuDic = new Dictionary<string, string>();
                    Dictionary<string, string> accessionsSequenceSsuDic = new Dictionary<string, string>();

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();

                        if (line.StartsWith(">"))
                        {
                            string[] lineParts = line.Split('|');
                            if (lineParts.Length > 4)
                            {
                                silvaType = lineParts[2];
                                if (accession != string.Empty)
                                {
                                    string sequenceTmp = string.Empty;
                                    Dictionary<string, string> accessionsSequenceDic = null;
                                    if (silvaTypeAccessionsSequencesDic.TryGetValue(silvaType, out accessionsSequenceDic))
                                    {
                                        if (!accessionsSequenceDic.TryGetValue(accession, out sequenceTmp))
                                        {
                                            accessionsSequenceDic.Add(accession, sequence);
                                            silvaTypeAccessionsSequencesDic[silvaType] = accessionsSequenceDic;
                                            accession = string.Empty;
                                            sequence = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        accessionsSequenceDic = new Dictionary<string, string>();
                                        accessionsSequenceDic.Add(accession, sequence);
                                        silvaTypeAccessionsSequencesDic.Add(silvaType, accessionsSequenceDic);
                                        accession = string.Empty;
                                        sequence = string.Empty;
                                    }
                                }

                                accession = lineParts[3];
                            }
                        }
                        else
                        {
                            sequence += line;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                reader.Close();
            }

            return silvaTypeAccessionsSequencesDic;
        }

        /// <summary>
        /// Reads pathway file and gets pathway information.
        /// </summary>
        /// <returns>Dictionary pathways with pathwayCode, pathwayName and pathwayClass</returns>
        public Dictionary<string, Pathway> ReadPathways()
        {
            Dictionary<string, Pathway> pathways = new Dictionary<string, Pathway>();

            try
            {
                string filePath = Path.Combine(this.Controller.KeggDir, "pathways.tab");
                if (File.Exists(filePath))
                {
                    StreamReader reader = new StreamReader(filePath);
                    bool headerCollected = false;

                    int codeId = -1;
                    int nameId = -1;
                    int classId = -1;
                    int subClassId = -1;

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] lineParts = line.Split((char)9);

                        if (lineParts.Length >= 6)
                        {
                            switch (headerCollected)
                            {
                                case false:
                                    if (line.ToUpper().StartsWith("CODE"))
                                    {
                                        // Get headerline and determine column indexes
                                        // Code:	Name:	Class:	SubClass:	Enzymes:	Reactions:
                                        for (int i = 0; i < lineParts.Length; i++)
                                        {
                                            if (lineParts[i].ToUpper().Contains("CODE")) { codeId = i; }
                                            if (lineParts[i].ToUpper().Contains("NAME")) { nameId = i; }
                                            if (lineParts[i].ToUpper().Contains("CLASS") && !lineParts[i].ToUpper().Contains("SUB")) { classId = i; }
                                            if (lineParts[i].ToUpper().Contains("SUBCLASS")) { subClassId = i; }
                                        }

                                        headerCollected = true;
                                    }
                                    break;

                                case true:

                                    // Get object
                                    Pathway record = new Pathway();
                                    record.Code = lineParts[codeId];
                                    record.Name = lineParts[nameId];
                                    record.Class = lineParts[classId];
                                    record.SubClass = lineParts[subClassId];

                                    pathways.Add(record.Code, record);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return pathways;
        }
              
        /// <summary>
        /// Maps the name of the pathway to the accessions locus_tags and pathways.
        /// </summary>
        /// <param name="pathways">Dictionary with pathwayCode(key) pathwayName,PathwayClass and PathwayCode(value)</param>
        /// <param name="accessionLocustagPathwaysDic">Dictionary with accessions locus_tags and pathways</param>
        /// <returns>Dictionary accessionLocustagPathwaysDic</returns>
        public Dictionary<string, Dictionary<string, List<Pathway>>> MapPathwayNamesToAccessionLocustagPathways(Dictionary<string, Pathway> pathways,
                                                                                                                Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic)
        {
            try
            {
                foreach (KeyValuePair<string, Dictionary<string, List<Pathway>>> accessions in accessionLocustagPathwaysDic)
                {
                    foreach (KeyValuePair<string, List<Pathway>> locusTag in accessions.Value)
                    {
                        foreach (Pathway pathway in locusTag.Value)
                        {
                            string pathwayCode = string.Empty;
                            
                            //Remove letter codes
                            StringBuilder stringbuilder = new StringBuilder();
                            foreach (char character in pathway.Code)
                            {
                                if (!char.IsUpper(character))
                                {
                                    char integer = character;
                                    stringbuilder.Append(integer);
                                }
                            }
                            //convert characters to one string
                            pathwayCode = stringbuilder.ToString();

                            // Annotate pathway with Name etc...
                            Pathway pathwayTmp = null;
                            if (pathways.TryGetValue(pathwayCode, out pathwayTmp))
                            {
                                pathway.Code = pathwayCode;
                                pathway.Name = pathwayTmp.Name;
                                pathway.Class = pathwayTmp.Class;
                                pathway.SubClass = pathwayTmp.SubClass;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return accessionLocustagPathwaysDic;
        }
        /// <summary>
        /// Collects the unique locus_tags per pathway and stores them in a dictionary.
        /// </summary>
        /// <param name="accessionLocustagPathwaysDic">Dictionary containing all accessions,locus_tags and pathways</param>
        /// <returns>Dictionary with unique locus_tags per pathway</returns>
        public Dictionary<string, Pathway> CollectUniqueLocustagPathways(Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic)
        {
            Dictionary<string, Pathway> uniqueLocustagPathways = new Dictionary<string, Pathway>();

            try
            {
                foreach (KeyValuePair<string, Dictionary<string, List<Pathway>>> accessions in accessionLocustagPathwaysDic)
                {
                    foreach (KeyValuePair<string, List<Pathway>> locusTag in accessions.Value)
                    {
                        if (locusTag.Value.Count == 1)
                        {
                            Pathway uniquePathway = null;
                            // collect all unique locus_tags per pathway 
                            if (uniqueLocustagPathways.TryGetValue(locusTag.Key, out uniquePathway))
                            {
                                if (uniquePathway.Name != locusTag.Value[0].Name)
                                {

                                    // if another pathway for the same locus_tag was found remove the locus_tag
                                    uniqueLocustagPathways.Remove(locusTag.Key);
                                }
                            }
                            else
                            {
                                
                                uniqueLocustagPathways.Add(locusTag.Key, locusTag.Value[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return uniqueLocustagPathways;
        }

        /// <summary>
        ///  <para>Reads the accession file (output from MAIMS), </para>
        ///  <para>Collects all information of an accession into an object.</para>
        ///  <para>Achieves collecting information by mapping different parameters from different dictionaries </para>
        /// </summary>
        /// <param name="filePath">Path to accession file</param>
        /// <param name="accessionLocustagPathwaysDic">Dictionary with accession locus_tags and pathways</param>
        /// <param name="taxonomyAccessionDic">Dictionary with taxonomy ID's and accession numbers</param>
        /// <param name="accessionsSequenceDic">Dictionary with accessions and sequences</param>
        /// <param name="accessionCutOff">Used to cut of accessions to "n" numbers from the accession file</param>
        /// <returns>AccessionFile with all information</returns>
        public AccessionFile ReadAccessionFile(string filePath,
                                                 Dictionary<string, Dictionary<string, List<Pathway>>> accessionLocustagPathwaysDic,
                                                 Dictionary<int, List<string>> taxonomyAccessionDic,
                                                 Dictionary<string, Dictionary<string, string>> accessionsSequenceDic,
                                                 int accessionCutOff
                                                )
        {
            List<Accession> accessions = new List<Accession>();
            AccessionFile accessionsFile = new AccessionFile();

            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    string[] fileInfoParts = fileInfo.Name.Split('_');
                    if (fileInfoParts.Length > 3)
                    {
                        accessionsFile.ExtractCode = string.Format("{0}_{1}_{2}", fileInfoParts[0], fileInfoParts[1], fileInfoParts[2]);

                        StreamReader reader = new StreamReader(filePath);
                        int sumTotalReadAmount = 0;

                        bool headerCollected = false;

                        int nameId = -1;
                        int totalreadAmountid = -1;
                        int ncbiTaxId = -1;
                        int scientificNameId = -1;
                        int mrtAccession = -1;
                        int mrtTaxId = -1;
                        int mrtScientificNameId = -1;

                        while (reader.Peek() > 0)
                        {
                            string line = reader.ReadLine();
                            string[] lineParts = line.Split((char)9);

                            if (lineParts.Length >= 21)
                            {
                                switch (headerCollected)
                                {
                                    case false:
                                        if (line.ToUpper().StartsWith("ACCESSION"))
                                        {
                                            for (int i = 0; i < lineParts.Length; i++)
                                            {
                                                //extract data based on header name
                                                if (lineParts[i].ToUpper().Contains("ACCESSION") && !lineParts[i].ToUpper().Contains("MRT")) { nameId = i; }
                                                if (lineParts[i].ToUpper().Contains("READTOTALAMOUNT")) { totalreadAmountid = i; }
                                                if (lineParts[i].ToUpper().Contains("NCBI_TAXONOMYID")) { ncbiTaxId = i; }
                                                if (lineParts[i].ToUpper().Contains("NCBI_ORGANISM") && !lineParts[i].ToUpper().Contains("MRT")) { scientificNameId = i; }
                                                if (lineParts[i].ToUpper().Contains("MRT_ACCESSION")) { mrtAccession = i; }
                                                if (lineParts[i].ToUpper().Contains("MRT_TAXONOMYID")) { mrtTaxId = i; }
                                                if (lineParts[i].ToUpper().Contains("MRT_ORGANISM")) { mrtScientificNameId = i; }

                                            }
                                            headerCollected = true;
                                        }
                                        break;

                                    case true:
                                        // Data from file
                                        Accession accession = new Accession();

                                        accession.Name = lineParts[nameId];
                                        int.TryParse(lineParts[totalreadAmountid], out accession.ReadTotalAmount);
                                        int.TryParse(lineParts[ncbiTaxId], out accession.NCBI_Taxonomy.Id);
                                        accession.NCBI_Taxonomy.ScientificName = lineParts[scientificNameId];
                                        accession.MRT_Accession = lineParts[mrtAccession];
                                        int.TryParse(lineParts[mrtTaxId], out accession.MRT_Taxonomy.Id);
                                        accession.MRT_Taxonomy.ScientificName = lineParts[mrtScientificNameId];
                                        //Replace characters that can conflict with JavaScript
                                        accession.MRT_Taxonomy.ScientificName = accession.MRT_Taxonomy.ScientificName.Replace("(", string.Empty)
                                                                                                                     .Replace(")", string.Empty)
                                                                                                                     .Replace(";", string.Empty)
                                                                                                                     .Replace(";", string.Empty)
                                                                                                                     .Replace("'", string.Empty)
                                                                                                                     .Replace(".", string.Empty)
                                                                                                                     .Replace(" ", "_");

                                        sumTotalReadAmount += accession.ReadTotalAmount;
                                 
                                        // Data from collections
                                        List<string> wholeGenomeAccessions = null;
                                        if (taxonomyAccessionDic.TryGetValue(accession.NCBI_Taxonomy.Id, out wholeGenomeAccessions))
                                        {
                                            foreach (string wholeGenomeAccession in wholeGenomeAccessions)
                                            {
                                                Dictionary<string, List<Pathway>> locustagsPathways = null;
                                                if (accessionLocustagPathwaysDic.TryGetValue(wholeGenomeAccession, out locustagsPathways))
                                                {
                                                    accession.WholeGenomeLocusTagsPathWaysDicList.Add(locustagsPathways);
                                                }
                                            }
                                        }

                                        string sequence = string.Empty;
                                        foreach (KeyValuePair<string, Dictionary<string, string>> accessionSequences in accessionsSequenceDic)
                                        {
                                            if (accessionsSequenceDic[accessionSequences.Key].TryGetValue(accession.Name, out sequence))
                                            {
                                                accession.Sequence = sequence;
                                                if (accessionSequences.Key.ToUpper().Contains("SSU"))
                                                {
                                                    accession.silvaType = "SSU";
                                                }
                                                else
                                                {
                                                    accession.silvaType = "LSU";
                                                }
                                            }
                                        }

                                        accessions.Add(accession);
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        reader.Close();

                        // Sort the accessions on ReadTotalAmount descending
                        accessions.Sort((x, y) => y.ReadTotalAmount.CompareTo(x.ReadTotalAmount));
                        accessionsFile.AccessionListDic.Add("SSU", new List<Accession>());
                        accessionsFile.AccessionListDic.Add("LSU", new List<Accession>());
                        accessionsFile.SumTotalReadAmount = sumTotalReadAmount;
                        
                        foreach (Accession accession in accessions)
                        {
                            if (accessionsFile.AccessionListDic["SSU"].Count < accessionCutOff ||
                                accessionsFile.AccessionListDic["LSU"].Count < accessionCutOff)
                            {
                                switch (accession.silvaType.ToUpper())
                                {
                                    case "SSU":
                                        if (accessionsFile.AccessionListDic["SSU"].Count < accessionCutOff)
                                        {
                                            accessionsFile.AccessionListDic["SSU"].Add(accession);
                                        }
                                        break;

                                    case "LSU":
                                        if (accessionsFile.AccessionListDic["LSU"].Count < accessionCutOff)
                                        {
                                            accessionsFile.AccessionListDic["LSU"].Add(accession);
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return accessionsFile;
        }


        /// <summary>
        ///  Writes the sequence with accession number to a fasta file
        /// </summary>
        /// <param name="accessionFile">Used to retrieve information of accessions</param>
        /// <returns></returns>
        public bool WriteAccessionSequenceToFasta(AccessionFile accessionFile)
        {
            //returns a boolean when writing of the data fails
            bool succes = false;
            
            try
            {
                foreach (KeyValuePair<string, List<Accession>> accessionList in accessionFile.AccessionListDic)
                {
                    string percentage = string.Empty;
                    string fileName = string.Format("{0}_{1}_Accession_File.fasta", accessionList.Key, accessionFile.ExtractCode);
                    string filePath = Path.Combine(this.Controller.AlignmentSetsDir, fileName);
                    StreamWriter writer = new StreamWriter(filePath, false);

                    foreach (Accession accession in accessionList.Value)
                    {
                        //calculate percentage
                        float readTotalAmount = float.Parse(accession.ReadTotalAmount.ToString());
                        float totalReadAmount = float.Parse(accessionFile.SumTotalReadAmount.ToString());
                        percentage = ((readTotalAmount / totalReadAmount) * 100).ToString("N2").Replace(",", ".");

                        //write data to file
                        string fastaHeader = string.Format(">{0}{1}{2}{1}{3}{1}{4}{5}{1}", accession.Name, (char)124, accession.MRT_Taxonomy.ScientificName, accession.ReadTotalAmount, percentage, (char)37);
                        writer.WriteLine(fastaHeader);
                        string sequence = accession.Sequence;
                        writer.WriteLine(sequence);
                    }
                                       
                    writer.Close();

                    succes = true;
                }
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return succes;
        }

        /// <summary>
        ///  Reads file which contains a newick string
        /// </summary>
        /// <param name="filePath">Path to newick file</param>
        /// <returns>A newick string</returns>
        public string ReadNewickTreeOutputFile(string filePath)
        {
            string newickString = string.Empty;
            StreamReader reader = null;

            try
            {


                if (File.Exists(filePath))
                {
                    reader = new StreamReader(filePath);

                    while (reader.Peek() > 0)
                    {
                        //replace all newlines and other empty spaces
                        string line = reader.ReadToEnd();
                        line = line.Replace("\n", string.Empty)
                                   .Replace("\r", string.Empty)
                                   .Replace("\t", string.Empty);
                        newickString = line;
                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return newickString;
        }

         /// <summary>
        ///  Reads HTML file and pasts newick string at the right posisition in this file
        /// </summary>
        /// <param name="newickString">String in newick format as input for a phylogenetic tree</param>
        /// <returns></returns>
        public string ReadCompressedHtmlFile(string newickString)
        {
            List<string> htmlFileList = new List<string>();
            string htmlFile = string.Empty;
            string organismTreeLine = string.Empty;

            StreamReader reader = null;

            try
            {
                if (File.Exists(this.Controller.CompressedHtmlFilePath))
                {
                    reader = new StreamReader(this.Controller.CompressedHtmlFilePath);

                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();


                        if (line.Contains("var") && line.Contains("organism_tree"))
                        {
                            organismTreeLine = line;
                            organismTreeLine = organismTreeLine.Replace("=", string.Format("= {1}{0}{1}", newickString, '"'));
                            htmlFileList.Add(organismTreeLine);

                        }
                        else
                        {
                            htmlFileList.Add(line);
                        }
                        htmlFile = string.Join("\n", htmlFileList);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return htmlFile;
        }

        /// <summary>
        /// Write a html file with newick string to a file.
        /// </summary>
        /// <param name="htmlFile">html file which generate a phylogenetic tree</param>
        /// <returns></returns>
        public string WriteHtmlTreeFile(string htmlFile, string extractCode)
        {
            //define path to output file
            string outputPath = string.Empty;

            try
            {
                
                string fileName = string.Format("{0}_phylogenetic_tree.html", extractCode);
                string filePath = Path.Combine(this.Controller.CompressedHtmlFileOutputPath, fileName);

                //write data to file
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.Write(htmlFile);
                writer.Close();
                outputPath = filePath;
            }

            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return outputPath;
        }
        
        /// <summary>
        /// (c) M.Burger.
        /// Writes genereted html file for Krona to a .html file
        /// </summary>
        /// <param name="accessionFile">Object accessionFile</param>
        /// <param name="uniqueLocustagPathwayDic">Dictionary containing unieque locus_tags and pathways</param>
        /// <param name="title">Defines the title of the Krona plot</param>
        /// <returns>Path to html output file</returns>
        public string WriteCirculairGraphExpressionHtmlFile(AccessionFile accessionFile, Dictionary<string, Pathway> uniqueLocustagPathwayDic, string title)
        {
            //path to output html file
            string kronaOutputPath = string.Empty;

            try
            {
                float scoreStart = 0.0f;
                float scoreEnd = 0.0f;
                int hueStart = 0;
                int hueEnd = 120;

                //define html header
                string htmlHeadHeader = string.Format(
                                                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">{0}" +
                                                "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">{0}" +
                                                "<head>{0}" +
                                                "<meta charset=\"utf-8\"/>{0}",
                                                (char)10); // LF
                //define html script
                string htmlHeadScript = string.Format("<script>{0}{1}{0}</script>{0}</head>{0}", (char)10, ReadCircularGraphJsContent(title));

                //define html body
                string htmlBodyHeader = string.Format(
                                                    "<body>{0}" +
                                                    "<noscript>Javascript must be enabled to view this page.</noscript>{0}" +
                                                    "<div style=\"display:none\"><PathwayViewer>{0}" +
                                                    "<attributes magnitude=\"magnitude\">{0}" +
                                                    "<attribute display=\"Read amount\">magnitude</attribute>{0}" +
                                                    "<attribute display=\"Risk prediction score\">score</attribute>{0}" +
                                                    "</attributes>{0}" +
                                                    "<color attribute=\"score\" valueStart=\"{1}\" valueEnd=\"{2}\" hueStart=\"{3}\" hueEnd=\"{4}\"></color>{0}",
                                                    (char)10, scoreStart, scoreEnd, hueStart, hueEnd);

                //get node content
                string nodeContent = GenerateNodeContentExpression(accessionFile, uniqueLocustagPathwayDic);

                string htmlTotal = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}</PathwayViewer></div>{0}</body>{0}</html>", (char)10, htmlHeadHeader, htmlHeadScript, htmlBodyHeader, nodeContent);

                string fileName = string.Format("{0}_circular_graph.html", title);
                string filePath = Path.Combine(this.Controller.CompressedHtmlFileOutputPath, fileName);

                //write data to file
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.Write(htmlTotal);
                writer.Close();

                kronaOutputPath = filePath;
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += string.Format("Function: {1}.{2}{0}ERROR: {3}{0}", Environment.NewLine, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return kronaOutputPath;
        }

        #endregion
    }
}

