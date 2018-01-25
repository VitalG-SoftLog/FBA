using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FBA.Shared.Interfaces;
using FBA.Shared.Entities;
using System.Data.Entity;

namespace HOK.Data.Helpers
{
    public static class KommuneHelper
    {
        public static Kommune Kommune_New(this IDataContext ctx, string kommunenr, string kommune)
        {
            var obj = new Kommune()
            {
                kommune = kommune,
                kommunenr = kommunenr
            };

            ctx.Kommune.Add(obj);
//            ctx.SaveChanges();

            return obj;
        }

        public static IEnumerable<Kommune> Kommune_List(this IDataContext ctx) //, int Organization_Id, bool? enabled)
        {
            var obj = ctx.Kommune.OfType<Kommune>(); //.Where(r => r.Organization_Id == Organization_Id);
            //if (enabled != null)
            //    obj = obj.Where(r => r.Enabled == enabled);

            return obj;
        }

        public static Kommune Kommune_Get(this IDataContext ctx, int id)
        {
            var obj = ctx.Kommune.OfType<Kommune>().FirstOrDefault(x => x.Id == id);
            return obj;
        }

        public static Kommune Kommune_Find(this IDataContext ctx, string kommunenr, string kommune)
        {
            var obj = ctx.Kommune.OfType<Kommune>().FirstOrDefault(x => x.kommune == kommune && x.kommunenr == kommunenr);
            return obj;
        }

        public static bool Kommune_Exists(this IDataContext ctx, int id)
        {
            return ctx.Kommune.OfType<Kommune>().Count(e => e.Id == id) > 0;
        }

        public static bool Kommune_Delete(this IDataContext ctx, int id)
        {
            var obj = ctx.Kommune.OfType<Kommune>().FirstOrDefault(x => x.Id == id);
            if (obj == null)
                return false;

            ctx.Entry(obj).State = EntityState.Deleted;
            ctx.SaveChanges();

            return true;
        }

        public static bool Kommune_Update(this IDataContext ctx, int id, string Name, string OrganizationNumber, bool enabled)
        {
            var obj = ctx.Kommune.OfType<Kommune>().FirstOrDefault(x => x.Id == id);
            if (obj == null)
                return false;

            //obj.Name = Name;
            //obj.Enabled = enabled;
            //obj.OrganizationNumber = OrganizationNumber;

            ctx.Entry(obj).State = EntityState.Modified;
            ctx.SaveChanges();

            return true;
        }

    }
}
