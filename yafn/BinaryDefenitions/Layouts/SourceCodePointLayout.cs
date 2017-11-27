using System.IO;

namespace yafn.BinaryDefenitions {
	public struct SourceCodePointLayout : IBinaryReadable {
		public long address;
		public int sourceOperationRangeIndex;

		public void Read(BinaryReader reader) {
			address = reader.ReadInt64();
			sourceOperationRangeIndex = reader.ReadInt32();
		}
	}
}