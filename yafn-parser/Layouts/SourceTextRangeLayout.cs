using System.IO;

namespace Yafn.Parser.Layouts {
	public struct SourceTextRangeLayout : ISimpleLayout {
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

		public void Write(BinaryWriter writer) {
			writer.Writer(sourceFileIndex);
			writer.Writer(position);
			writer.Writer(length);
			writer.Writer(line);
			writer.Writer(column);
		}
	}
}