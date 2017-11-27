using System.IO;

namespace yafn.BinaryDefenitions {
	public struct SourceTextRangeLayout : IBinaryReadable {
		public int sourceFileIndex;
		public int position;
		public int length;
		public int line;
		public int column;

		public void Read(BinaryReader reader) {
			sourceFileIndex = reader.ReadInt32();
			position = reader.ReadInt32();
			length = reader.ReadInt32();
			line = reader.ReadInt32();
			column = reader.ReadInt32();

		}
	}
}