using System.IO;

namespace yafn.BinaryDefenitions {
	public struct BlobLayout {
		public byte[] data;

		public void Read(BinaryReader reader, int length) {
			data = reader.ReadBytes(length);
		}
	}
}