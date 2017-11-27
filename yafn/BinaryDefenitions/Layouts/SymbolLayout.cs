using System;
using System.IO;

namespace yafn.BinaryDefenitions {
	public struct SymbolLayout : IBinaryReadable {
		public int sectionIndex;
		public long blobEntryIndex;
		public int nameIndex;

		public void Read(BinaryReader reader) {
			sectionIndex = reader.ReadInt32();
			blobEntryIndex = reader.ReadInt64();
			nameIndex = reader.ReadInt32();
		}
	}
}