using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

using FBA.Shared.Interfaces;

namespace FBA.Data {
	public interface IFBADataContext : IDataContext {
		DbSet<bransjefakta> bransjefakta { get; set; }

        DbSet<Nace> Nace { get; }
	}
}
