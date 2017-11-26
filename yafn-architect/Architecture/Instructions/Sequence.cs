using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Instructions {
	public class Sequence : INamed {
		public string Name { get; set; }
		public string[] Tiggers;
		public SequencePart[] Parts;
	}
}