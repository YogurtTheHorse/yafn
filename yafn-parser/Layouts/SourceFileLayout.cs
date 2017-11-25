using System.IO;

namespace Yafn.Parser.Layouts {
	public struct SourceFileLayout : ISimpleLayout {
		public int fileNameIndex;
		public int sha256hashBytesIndex;

		public void Read(BinaryReader reader) {
			fileNameIndex = reader.ReadInt32();
			sha256hashBytesIndex = reader.ReadInt32();
		}

		public void Write(BinaryWriter writer) {
			writer.Write(fileNameIndex);
			writer.Write(sha256hashBytesIndex);
		}
	}
}