using System.IO;

namespace Yafn.Parser.Layouts {
	public struct BlobLayout : ILayout {
		public byte[] data;

		public void Read(BinaryReader reader, int length) {
			data = reader.ReadBytes(length);
		}

		public void Write(BinaryWriter writer) {
			writer.Write(data);
		}
	}
}