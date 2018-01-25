using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBA.Models
{
    public class NaceCodesCriteriaModel
    {
        public NaceCodesCriteriaModel()
        {
            NaceCodes = new List<TreeViewNode>();
        }

        public int Id { get; set; }
        public IList<TreeViewNode> NaceCodes { get; private set; }
        public int SelectedValue { get; set; }
    }
}