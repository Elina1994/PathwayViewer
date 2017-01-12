namespace PathwayViewer
{
    using System.Collections.Generic;

    public class AccessionFile
    {
        public Dictionary<string, List<Accession>> AccessionListDic = new Dictionary<string, List<Accession>>();
        public string ExtractCode = string.Empty;
        public int SumTotalReadAmount = 0;
    }
}
