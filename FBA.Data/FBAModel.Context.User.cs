using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBA.Shared.Interfaces;

namespace FBA.Data
{
    public partial class Entities : IFBADataContext
    {
        public Entities(DbConnection connection)
            : this(connection, true)
        {
        }

        public Entities(DbConnection connection, bool ownsConnection)
            : base(connection, ownsConnection)
        {
        }

        public Entities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    }
}
