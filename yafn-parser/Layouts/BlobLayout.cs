using System.IO;

namespace Yafn.Parser.Layouts {
	public struct BlobLayout {
		public byte[] data;

		public void Read(BinaryReader reader, int length) {
			data = reader.ReadBytes(length);
		}
	}
}