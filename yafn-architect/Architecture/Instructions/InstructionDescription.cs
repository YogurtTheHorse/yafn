using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Instructions {
	public class InstructionDescription : INamed {
		public string Name { get; set; }
		public string ShortName;
		public Sequence Sequence;
	}
}
