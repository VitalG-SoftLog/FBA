using System;

namespace FBA.Shared.Entities
{
    public class OrgInfo
    {
        public OrgInfo(int orgNr, string name) {
            Name = name;
            OrgNr = orgNr;
        }

        public string Name { get; private set; }

        public int OrgNr { get; private set; }

        public int MunicipalityId { get; set; }

        public string Municipality { get; set; }

        public int RegionId { get; set; }

        public string Region { get; set; }

        public int Nace2 { get; set; }

        public string Nace2Text { get; set; }

        public int Nace5 { get; set; }

        public string Nace5Text { get; set; }

        public OrgCounts Counts { get; set; }
    }
}
