using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Memory {
	public class MemoryDescription : INamed {
		public string Name { get; set; }
		public uint StartBit, EndBit;

		public int CellSize;
		public EEndianess Endianess;
		public int Granularity;
	}
}