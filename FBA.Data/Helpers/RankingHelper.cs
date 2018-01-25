using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;

using FBA.Shared.Entities;
using FBA.Shared.Interfaces;

namespace FBA.Data.Helpers {
	public static class RankingHelper {
	    private static DbCommand AddParameter(this DbCommand command, string name, object value) {
	        var param = command.CreateParameter();
	        param.ParameterName = name;
	        param.Value = value ?? DBNull.Value;
	        command.Parameters.Add(param);
	        return command;
	    }

	    private static IEnumerable<IRanking> GetRankingForOrg(this IDataContext context, int orgId, string nace5, int? adminLevel, int? beforeCount, int? afterCount, out OrgInfo orgInfo, out IEnumerable<IRanking> tops) {
	        var cmd = context.Database.Connection.CreateCommand();
	        cmd.CommandText = "GetRankingForOrg";
            cmd.CommandType = CommandType.StoredProcedure;
	        cmd.AddParameter("@OrgId", orgId)
	            .AddParameter("@Nace5", nace5)
	            .AddParameter("@NumBefore", beforeCount)
	            .AddParameter("@NumAfter", afterCount)
                .AddParameter("@administrativeLevel", adminLevel);

	        var rankList = new List<Ranking>();
            tops = new List<Ranking>();

	        const int orgInfoResultId = 1;
	        const int orgCountsResultId = 2;
	        const int revenueResultId = 3;
            const int revenueTopResultId = 4;
	        const int marketShareResultId = 5;
            const int marketShareTopResultId = 6;
	        const int ebitdaResultId = 7;
	        const int ebitdaTopResultId = 8;
	        const int operatingMarginResultId = 9;
	        const int operatingMarginTopResultId = 10;
	        const int costPerEmployeeResultId = 11;
	        const int costPerEmployeeTopResultId = 12;
	        const int numOfEmployeesResultId = 13;
	        const int numOfEmployeesTopResultId = 14;
	        const int revenuePerEmployeeResultId = 15;
	        const int revenuePerEmployeeTopResultId = 16;

	        orgInfo = null;

	        try {
	            context.Database.Connection.Open();

	            var reader = cmd.ExecuteReader();
	            bool hasResult = true;
	            int resultId = 1;
	            while (hasResult) {
	                if (resultId == orgInfoResultId) {
	                    var spOrgInfo = ((IObjectContextAdapter) context).ObjectContext.Translate<SpOrgInfo>(reader);
	                    orgInfo = spOrgInfo.First().ToOrgInfo();
	                }
                    else if (resultId == orgCountsResultId) {
                        var spOrgCounts = ((IObjectContextAdapter)context).ObjectContext.Translate<SpOrgCount>(reader);
                        if (orgInfo != null) {
                            orgInfo.Counts = new OrgCounts();
                            foreach (var spOrgCount in spOrgCounts) {
                                if (spOrgCount.Category == orgInfo.Municipality) {
                                    orgInfo.Counts.PerMunicipality = spOrgCount.companyCount;
                                }
                                else if (spOrgCount.Category == orgInfo.Region) {
                                    orgInfo.Counts.PerRegion = spOrgCount.companyCount;
                                } else {
                                    orgInfo.Counts.PerNation = spOrgCount.companyCount;
                                }
                            }
                        }
                    } else if (resultId == revenueResultId ||
                        resultId == ebitdaResultId ||
                        resultId == operatingMarginResultId ||
                        resultId == costPerEmployeeResultId ||
                        resultId == revenuePerEmployeeResultId) {
	                    rankList.AddRange(((IObjectContextAdapter)context).ObjectContext.Translate<LongRanking>(reader));
                    }
                    else if (resultId == revenueTopResultId ||
                        resultId == ebitdaTopResultId ||
                        resultId == operatingMarginTopResultId ||
                        resultId == costPerEmployeeTopResultId ||
                        resultId == revenuePerEmployeeTopResultId) {
                        ((List<Ranking>)tops).AddRange(((IObjectContextAdapter)context).ObjectContext.Translate<LongRanking>(reader));
                    }
                    if (resultId == marketShareResultId) {
                        var ranks = ((IObjectContextAdapter)context).ObjectContext.Translate<DecimalRanking>(reader);
                        rankList.AddRange(ranks);
                    } else if (resultId == marketShareTopResultId) {
                        ((List<Ranking>)tops).AddRange(((IObjectContextAdapter)context).ObjectContext.Translate<DecimalRanking>(reader));
                    } else if (resultId == numOfEmployeesResultId) {
                        rankList.AddRange(((IObjectContextAdapter)context).ObjectContext.Translate<IntRanking>(reader));
                    } else if (resultId == numOfEmployeesTopResultId) {
                        ((List<Ranking>)tops).AddRange(((IObjectContextAdapter)context).ObjectContext.Translate<IntRanking>(reader));
                    }

	                hasResult = reader.NextResult();
	                resultId++;
	            }
	        } finally {
	            context.Database.Connection.Close();
	        }

	        return rankList.AsReadOnly();
	    }

	    public static RankingInfo RankOrg(this IFBADataContext context,
	        int orgId,
	        string nace5,
	        int adminLevel,
	        int? beforeCount = 10,
	        int? afterCount = 5) {

	        IEnumerable<IRanking> topRankings;
	        OrgInfo orgInfo;
	        var result = context.GetRankingForOrg(orgId, nace5, adminLevel, beforeCount, afterCount, out orgInfo, out topRankings);

	        var obj = new RankingInfo {
                Info = orgInfo,
	            OrgRankings = result.Select(forOrgResult => new OrgRankingInfo(orgId, (long) forOrgResult.OtherOrgNr, forOrgResult.Subset) {
	                CompanyName = forOrgResult.companyName,
	                OverallRanking = forOrgResult.OtherOrgRank,
	                MetricValue = Convert.ToDecimal(forOrgResult.OtherOrgValue)
	            }).ToList().AsReadOnly(),
	            TopTankings = topRankings.Select(ranking => new OrgRankingInfo(orgId, (long) ranking.CurrentOrgNr, ranking.Subset) {
	                CompanyName = ranking.companyName,
	                OverallRanking = ranking.CurrentOrgRank,
	                MetricValue = Convert.ToDecimal(ranking.Value)
	            }).ToList().AsReadOnly()
	        };

	        return obj;
	    }

        #region IRanking

	    interface IRanking {
            string Subset { get; }

            string companyName { get; }

            decimal CurrentOrgNr { get; }

            decimal OtherOrgNr { get; }

            long CurrentOrgRank { get; }

            long OtherOrgRank { get; }

            object Value { get; }

            object OtherOrgValue { get; }
	    }

        #endregion

        #region Ranking

        private abstract class Ranking : IRanking {
            object IRanking.Value {
                get { return RankingValue; }
            }

            protected abstract object RankingValue { get; }

            object IRanking.OtherOrgValue {
                get { return OtherOrgRankingValue; }
            }

            protected abstract object OtherOrgRankingValue { get; }

            public string companyName { get; set; }

            public decimal CurrentOrgNr { get; set; }

	        public long CurrentOrgRank { get; set; }

	        public string Subset { get; set; }

	        public decimal OtherOrgNr { get; set; }

            public long OtherOrgRank { get; set; }
	    }

	    #endregion

        #region IntRanking

        class IntRanking : Ranking
        {
            public int Value { get; set; }

            public int OtherOrgValue { get; set; }

            protected override object RankingValue
            {
                get { return Value; }
            }

            protected override object OtherOrgRankingValue
            {
                get { return OtherOrgValue; }
            }
        }
        

        #endregion

        #region LongRanking

	    class LongRanking : Ranking {
            public long Value { get; set; }

            public long OtherOrgValue { get; set; }

            protected override object RankingValue
            {
                get { return Value; }
            }

            protected override object OtherOrgRankingValue
            {
                get { return OtherOrgValue; }
            }
	    }

        #endregion

        #region DecimalRanking

        class DecimalRanking : Ranking {
            public decimal Value { get; set; }

            public decimal OtherOrgValue { get; set; }

	        protected override object RankingValue {
	            get { return Value; }
	        }

	        protected override object OtherOrgRankingValue {
	            get { return OtherOrgValue; }
	        }
	    }

        #endregion

        #region SPOrgInfo

	    class SpOrgInfo {
            public decimal orgnr { get; set; }
            public int dunsnr { get; set; }
            public string kommunenr { get; set; }
            public string kommune { get; set; }
            public string fylkesnr { get; set; }
            public string fylke { get; set; }
            public string nace2 { get; set; }
            public string nace2_tekst { get; set; }
            public string nace5 { get; set; }
            public string nace5_tekst { get; set; }
            public int regnskapsår { get; set; }
            public long omsetning { get; set; }
            public string juridisk_navn { get; set; }

	        public OrgInfo ToOrgInfo() {
	            return new OrgInfo((int) orgnr, juridisk_navn) {
                    MunicipalityId = Int32.Parse(kommunenr),
                    Municipality = kommune,
                    RegionId = Int32.Parse(fylkesnr),
                    Region = fylke,
                    Nace2 = Int32.Parse(nace2),
                    Nace2Text = nace2_tekst,
                    Nace5 = Int32.Parse(nace5),
                    Nace5Text = nace5_tekst
	            };
	        }
	    }

        #endregion

        #region SpOrgCount

        class SpOrgCount {
            public int companyCount { get; set; }
            public string Category { get; set; }
        }

        #endregion
    }
}
