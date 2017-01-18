namespace PathwayViewer
{
    using System.Collections.Generic;

    /// <summary>
    /// This class contains the Accession object properties
    /// </summary>
    public class Accession
    {
        public string Name = string.Empty;
        public Taxonomy NCBI_Taxonomy = new Taxonomy();
        public int ReadTotalAmount = -1;
        public List<Dictionary<string, List<Pathway>>> WholeGenomeLocusTagsPathWaysDicList = new List<Dictionary<string, List<Pathway>>>();
        public string Sequence = string.Empty;
        public string silvaType = string.Empty;
        public int SumTotalReadAmount = -1;
     

        public string MRT_Accession = string.Empty;
        public Taxonomy MRT_Taxonomy = new Taxonomy();
        
    }
}
