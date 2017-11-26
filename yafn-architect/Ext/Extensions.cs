using Irony.Parsing;
using System.Collections.Generic;

namespace Yafn.Architect.Ext {
	public static class Extensions {
		public static List<ParseTreeNode> SeperatedList(this ParseTreeNode node, string parentName, string childName) {
			List<ParseTreeNode> res = new List<ParseTreeNode>();

			foreach (ParseTreeNode n in node.ChildNodes) {
				if (n.Term.Name == parentName) {
					res.AddRange(n.SeperatedList(parentName, childName));
				} else if (n.Term.Name == childName) {
					res.Add(n);
				}
			}

			return res;
		}

		public static uint ToUInt(this int n) {
			if (n < 0) {
				return (uint)(int.MaxValue + n);
			} else {
				return (uint)n;
			}
		}
	}
}
