using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Instructions {
	public class SequenceFieldDescription : INamed {
		public string Name { get; set; }
		public string Interpretation;
		public int Size;
	}
}