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
    public static class Nace5Helper
    {
        public static Nace5 Nace5_New(this IDataContext ctx, string nace5, string nace5_tekst)
        {
            var obj = new Nace5()
            {
                nace5 = nace5,
                nace5_tekst = nace5_tekst
            };

            ctx.Nace5.Add(obj);
//            ctx.SaveChanges();

            return obj;
        }

        public static IEnumerable<Nace5> Nace5_List(this IDataContext ctx) //, int Organization_Id, bool? enabled)
        {
            var obj = ctx.Nace5.OfType<Nace5>(); //.Where(r => r.Organization_Id == Organization_Id);
            //if (enabled != null)
            //    obj = obj.Where(r => r.Enabled == enabled);

            return obj;
        }

        public static Nace5 Nace5_Get(this IDataContext ctx, int id)
        {
            var obj = ctx.Nace5.OfType<Nace5>().FirstOrDefault(x => x.Id == id);
            return obj;
        }

        public static Nace5 Nace5_Find(this IDataContext ctx, string nace5, string nace5_tekst)
        {
            var obj = ctx.Nace5.OfType<Nace5>().FirstOrDefault(x => x.nace5 == nace5 && x.nace5_tekst == nace5_tekst);
            return obj;
        }

        public static bool Nace5_Exists(this IDataContext ctx, int id)
        {
            return ctx.Nace5.OfType<Nace5>().Count(e => e.Id == id) > 0;
        }

        public static bool Nace5_Delete(this IDataContext ctx, int id)
        {
            var obj = ctx.Nace5.OfType<Nace5>().FirstOrDefault(x => x.Id == id);
            if (obj == null)
                return false;

            ctx.Entry(obj).State = EntityState.Deleted;
            ctx.SaveChanges();

            return true;
        }

        public static bool Nace5_Update(this IDataContext ctx, int id, string Name, string OrganizationNumber, bool enabled)
        {
            var obj = ctx.Nace5.OfType<Nace5>().FirstOrDefault(x => x.Id == id);
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
