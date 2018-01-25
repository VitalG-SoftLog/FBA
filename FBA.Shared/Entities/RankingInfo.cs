using System;
using System.Collections.Generic;

namespace FBA.Shared.Entities {
    public class RankingInfo {
        public OrgInfo Info { get; set; }

        public IEnumerable<OrgRankingInfo> OrgRankings { get; set; }

        public IEnumerable<OrgRankingInfo> TopTankings { get; set; }
    }
}
