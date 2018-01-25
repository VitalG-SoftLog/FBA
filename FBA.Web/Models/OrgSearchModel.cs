using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FBA.Shared.Entities;

namespace FBA.Models {
	public class OrgSearchModel {
	    public OrgSearchModel() {
	        AdministrativeLevel = 2;
	    }

		public int? OrgId { get; set; }

        public int? Nace5 { get; set; }

		public int? AdministrativeLevel { get; set; }

		public int? BeforeCount { get; set; }

		public int? AfterCount { get; set; }

		public RankingInfo Rankings { get; set; }

        public NaceCodesCriteriaModel Naces { get; set; }

	    public SelectList GetAdministrativeLevels() {
	        return new SelectList(new SelectListItem[] {
	            new SelectListItem {Value = "1", Text = String.Format("AS Norge {0}", Rankings.Info.Counts.PerNation)},
	            new SelectListItem {
	                Value = "2",
	                Text = String.Format("{0} {1}", Rankings.Info.Region, Rankings.Info.Counts.PerRegion),
	                Selected = true
	            },
	            new SelectListItem {
	                Value = "3",
	                Text = String.Format("{0} {1}", Rankings.Info.Municipality, Rankings.Info.Counts.PerMunicipality)
	            },
	        },
	            "Value",
	            "Text");
	    }

	    public SubsetRankingModel RankingForSubset(string subset) {
		    return new SubsetRankingModel {
		        Rankings = Rankings.OrgRankings.Where(x => x.RefOrgNumber == OrgId && x.Subset == subset),
                Subset = subset
		    };
		}

	    public SubsetRankingModel TopRankingForSubset(string subset) {
            return new SubsetRankingModel
            {
                Rankings = Rankings.TopTankings.Where(x => x.Subset == subset),
                Subset = subset
            };	        
	    }
	}
}