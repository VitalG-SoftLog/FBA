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
    public static class Nace2Helper
    {
        public static Nace2 Nace2_New(this IDataContext ctx, string nace2, string nace2_tekst)
        {
            var obj = new Nace2()
            {
                nace2 = nace2,
                nace2_tekst = nace2_tekst
            };

            ctx.Nace2.Add(obj);
//            ctx.SaveChanges();

            return obj;
        }

        public static IEnumerable<Nace2> Nace2_List(this IDataContext ctx) //, int Organization_Id, bool? enabled)
        {
            var obj = ctx.Nace2.OfType<Nace2>(); //.Where(r => r.Organization_Id == Organization_Id);
            //if (enabled != null)
            //    obj = obj.Where(r => r.Enabled == enabled);

            return obj;
        }

        public static Nace2 Nace2_Get(this IDataContext ctx, int id)
        {
            var obj = ctx.Nace2.OfType<Nace2>().FirstOrDefault(x => x.Id == id);
            return obj;
        }

        public static Nace2 Nace2_Find(this IDataContext ctx, string nace2, string nace2_tekst)
        {
            var obj = ctx.Nace2.OfType<Nace2>().FirstOrDefault(x => x.nace2 == nace2 && x.nace2_tekst == nace2_tekst);
            return obj;
        }

        public static bool Nace2_Exists(this IDataContext ctx, int id)
        {
            return ctx.Nace2.OfType<Nace2>().Count(e => e.Id == id) > 0;
        }

        public static bool Nace2_Delete(this IDataContext ctx, int id)
        {
            var obj = ctx.Nace2.OfType<Nace2>().FirstOrDefault(x => x.Id == id);
            if (obj == null)
                return false;

            ctx.Entry(obj).State = EntityState.Deleted;
            ctx.SaveChanges();

            return true;
        }

        public static bool Nace2_Update(this IDataContext ctx, int id, string Name, string OrganizationNumber, bool enabled)
        {
            var obj = ctx.Nace2.OfType<Nace2>().FirstOrDefault(x => x.Id == id);
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
