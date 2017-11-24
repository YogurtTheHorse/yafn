using System.IO;

namespace Yafn.Parser.Layouts {
	public struct SectionLayout : ISimpleLayout {
		public int blobIndex;
		public int bankNameIndex;
		public long startAddress;
		public SectionKind kind;
		public int customSectionNameIndex;
		public SectionAccessMode accessMode;

		public void Read(BinaryReader reader) {
			blobIndex = reader.ReadInt32();
			bankNameIndex = reader.ReadInt32();
			startAddress = reader.ReadInt64();
			kind = (SectionKind)reader.ReadInt16();
			customSectionNameIndex = reader.ReadInt32();
			accessMode = (SectionAccessMode)reader.ReadInt16();
		}
	}
}