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
    public static class FylkeHelper
    {
        public static Fylke Fylke_New(this IDataContext ctx, string fylkesnr, string fylke)
        {
            var obj = new Fylke()
            {
                fylke = fylke,
                fylkesnr = fylkesnr  
            };

            ctx.Fylke.Add(obj);
//            ctx.SaveChanges();

            return obj;
        }

        public static IEnumerable<Fylke> Fylke_List(this IDataContext ctx) //, int Organization_Id, bool? enabled)
        {
            var obj = ctx.Fylke.OfType<Fylke>(); //.Where(r => r.Organization_Id == Organization_Id);
            //if (enabled != null)
            //    obj = obj.Where(r => r.Enabled == enabled);

            return obj;
        }

        public static Fylke Fylke_Get(this IDataContext ctx, int id)
        {
            var obj = ctx.Fylke.OfType<Fylke>().FirstOrDefault(x => x.Id == id);
            return obj;
        }
        public static Fylke Fylke_Find(this IDataContext ctx, string fylkesnr, string fylke)
        {
            var obj = ctx.Fylke.OfType<Fylke>().FirstOrDefault(x => x.fylke == fylke && x.fylkesnr == fylkesnr);
            return obj;
        }

        public static bool Fylke_Exists(this IDataContext ctx, int id)
        {
            return ctx.Fylke.OfType<Fylke>().Count(e => e.Id == id) > 0;
        }

        public static bool Fylke_Delete(this IDataContext ctx, int id)
        {
            var obj = ctx.Fylke.OfType<Fylke>().FirstOrDefault(x => x.Id == id);
            if (obj == null)
                return false;

            ctx.Entry(obj).State = EntityState.Deleted;
            ctx.SaveChanges();

            return true;
        }

        public static bool Fylke_Update(this IDataContext ctx, int id, string Name, string OrganizationNumber, bool enabled)
        {
            var obj = ctx.Fylke.OfType<Fylke>().FirstOrDefault(x => x.Id == id);
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
