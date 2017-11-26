using System.Collections.Generic;
using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Instructions {
	public class MultiSequence : INamed {
		public string Name { get; set; }
		public List<Sequence> Sequences;
	}
}
