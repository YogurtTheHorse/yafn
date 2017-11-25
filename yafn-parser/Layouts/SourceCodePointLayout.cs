using System.IO;

namespace Yafn.Parser.Layouts {
	public struct SourceCodePointLayout : ISimpleLayout {
		public long address;
		public int sourceOperationRangeIndex;

		public void Read(BinaryReader reader) {
			address = reader.ReadInt64();
			sourceOperationRangeIndex = reader.ReadInt32();
		}

		public void Write(BinaryWriter writer) {
			writer.Write(address);
			writer.Write(sourceOperationRangeIndex);
		}
	}
}