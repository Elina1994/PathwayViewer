namespace PathwayViewer
{
    using System;
    using System.Net;
    using System.IO;

    public class WebHelper
    {
        #region FIELDS

        private Controller Controller = null;

        #endregion

        #region CONSTRUCTOR

        public WebHelper(Controller controller)
        {
            this.Controller = controller;
        }

        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Downloads all files with kegg organisms.
            /// </summary>
        /// <param name="url">Link to location of data</param>
        /// <returns>An output path to write data to</returns>
        public string DownloadKeggOrganismFile(string url)
        {
            string outputPath = string.Empty;

            try
            {
                // url to kegg organism file
                url = "http://rest.kegg.jp/list/organism";
                
                WebRequest request = WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // get data from url
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                response.Close();
                // write content to file
                outputPath = this.Controller.FileHelper.WriteKeggOrganismFile(responseFromServer);
            }
            catch (Exception ex)
            {
                this.Controller.CatchContent += ex.Message;
            }

            return outputPath;
        }
        
        #endregion

        #region PRIVATE METHODS
                
        #endregion

    }
}
