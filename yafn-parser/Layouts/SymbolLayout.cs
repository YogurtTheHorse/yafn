using System.IO;

namespace Yafn.Parser.Layouts {
	public struct SymbolLayout : ISimpleLayout {
		public int sectionIndex;
		public long blobEntryIndex;
		public int nameIndex;

		public void Read(BinaryReader reader) {
			sectionIndex = reader.ReadInt32();
			blobEntryIndex = reader.ReadInt64();
			nameIndex = reader.ReadInt32();
		}

		public void Write(BinaryWriter writer) {
			writer.Write(sectionIndex);
			writer.Write(blobEntryIndex);
			writer.Write(nameIndex);
		}
	}
}