using System;
using System.Collections.Generic;

using FBA.Shared.Entities;

namespace FBA.Models {
    public class SubsetRankingModel {
        public string Subset { get; set; }

        public IEnumerable<OrgRankingInfo> Rankings { get; set; }
    }
}