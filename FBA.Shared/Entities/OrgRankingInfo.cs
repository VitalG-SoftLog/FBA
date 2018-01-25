using System;

namespace FBA.Shared.Entities {
	public class OrgRankingInfo {
		public OrgRankingInfo(long refOrgNr, long orgNr, string subset) {
		    RefOrgNumber = refOrgNr;
			OrgNumber = orgNr;
			Subset = subset;
		}

        public long RefOrgNumber { get; private set; }

		public long OrgNumber { get; private set; }

		public string Subset { get; private set; }

		public long OverallRanking { get; set; }

		public decimal MetricValue { get; set; }

	    public string CompanyName { get; set; }
	}
}
