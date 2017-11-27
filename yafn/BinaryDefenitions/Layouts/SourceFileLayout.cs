using System;
using System.IO;

namespace yafn.BinaryDefenitions {
	public struct SourceFileLayout : IBinaryReadable {
		public int fileNameIndex;
		public int sha256hashBytesIndex;

		public void Read(BinaryReader reader) {
			fileNameIndex = reader.ReadInt32();
			sha256hashBytesIndex = reader.ReadInt32();
		}
	}
}