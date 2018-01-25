using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FBA.Models;

namespace FBA.Web.Helpers {
    public static class TreeViewStructureHelper {
        public static void Construct(this ICollection<TreeViewNode> nodes, TreeViewNode node, int level, List<TreeViewNode> workingList) {
            const int border = 3;

            if (level < border) {
                nodes.Add(node);
                workingList.Add(node);
            } else {
                //there should always be a parent for a entry at this level
                TreeViewNode parent = null;
                int i = 1;
                while (parent == null && level - i >= 0) {
                    int length = level - 1;
                    string substring = node.Id.Substring(0, length);
                    parent = workingList.LastOrDefault(p => p.Level < level && substring == p.Id.Substring(0, length));
                    i++;
                }


                if (parent != null) {
                    parent.ChildNodes.Add(node);
                    workingList.Add(node);
                }
            }
        }
    }
}