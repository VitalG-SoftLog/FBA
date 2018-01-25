using System;
using System.Collections.Generic;

namespace FBA.Models {
    public class TreeViewNode
    {
        public TreeViewNode()
        {
            ChildNodes = new List<TreeViewNode>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string CssClass { get; set; }
        public List<TreeViewNode> ChildNodes { get; set; }

        public string Description { get; set; }
    }
}