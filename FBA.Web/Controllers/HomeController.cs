using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Text;

using FBA.Data;
using FBA.Data.Helpers;
using FBA.Models;
using FBA.Shared.Entities;
using FBA.Web.Helpers;

namespace FBA.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFBADataContext ctx;

        public HomeController(IFBADataContext dataContext)
        {
            ctx = dataContext;
        }

		[HttpGet]
        public ActionResult Index() {
		    var model = new OrgSearchModel();
            model.Naces = GetNaceCriteriaModel();

            return View(model);
        }

		[HttpPost]
	    public ActionResult Index(OrgSearchModel model) {
		    if (ModelState.IsValid) {
		        var rankings = new RankingInfo();
		        if (model.OrgId != null) {
		            var nace5 = model.Nace5 != null ? model.Nace5.Value.ToString(CultureInfo.InvariantCulture) : null;
		            var adminLevel = model.AdministrativeLevel == null ? 2 : model.AdministrativeLevel.Value;
		            rankings = ctx.RankOrg(model.OrgId.Value, nace5, adminLevel);
		        }

		        model.Nace5 = model.Nace5 == null && rankings.Info != null ? rankings.Info.Nace5 : (int?)null;
		        model.Rankings = rankings;
		        model.Naces = GetNaceCriteriaModel();

			    return View(model);
		    }

			return new HttpStatusCodeResult(500);
	    }

        private NaceCodesCriteriaModel GetNaceCriteriaModel() {
            var model = new NaceCodesCriteriaModel();

            var naceCodes = GetNaceCodes() ?? new List<Nace>();
            naceCodes = (from n in naceCodes orderby n.NaceCode ascending select n).ToList();

            var addedNodes = new List<TreeViewNode>(naceCodes.Count);

            foreach (var code in naceCodes)
            {
                if (code.Level.HasValue)
                {
                    var node = new TreeViewNode()
                    {
                        Id = code.NaceCode,
                        Name = String.Format("({0}) {1}", code.NaceCode, code.ClassificationText),
                        Level = code.Level.Value
                    };
                    model.NaceCodes.Construct(node, (int)code.Level, addedNodes);
                }
            }

            return model;
        }

        private List<Nace> GetNaceCodes() {
            return ctx.Nace.ToList();
        }
    }
}
