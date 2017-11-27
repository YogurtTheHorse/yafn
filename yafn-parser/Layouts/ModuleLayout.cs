using System;
using System.IO;

namespace Yafn.Parser.Layouts {
	public struct ModuleLayout : ISimpleLayout {
		public ModuleHeader header;
		public ModuleData data;

		public void Read(BinaryReader reader) {
			header.Read(reader);
			data.Read(reader);
		}

		public static ModuleLayout ReadModuleLayout(string path) {
			ModuleLayout module = new ModuleLayout();

			using (BinaryReader b_reader = new BinaryReader(File.Open(path, FileMode.Open))) {
				module.Read(b_reader);
			}

			return module;
		}

		public void Write(BinaryWriter writer) {
			header.Write(writer);
			data.Write(writer);
		}
	}
}
