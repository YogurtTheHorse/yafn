using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Registers {
	public class StorageDescription : INamed {
		public string Name { get; set; }
		public int Length;
	}
}