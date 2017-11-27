using System;
using System.IO;
using yafn.BinaryDefenitions;
using Yafn.Architect.Builder;

namespace yafn {
	public class Program {
		public static ModuleLayout mod;

		public static ModuleLayout ReadBin(string path) {
			ModuleLayout module = new ModuleLayout();
			
			using (BinaryReader b_reader = new BinaryReader(File.Open(path, FileMode.Open))) {
				module.Read(b_reader);
			}

			return module;
		}
		
		public static void Main(string[] args) {
			Console.WriteLine("Reading module file...");
			mod = ReadBin("content\\out.ptptb");
			Console.WriteLine($"Done. Read {mod.header.signature}.");

			Runner.Prepare(mod);
			Runner.Run();
			Console.ReadLine();

			//ArchitectureBuilder builder = new ArchitectureBuilder();
			//var a = builder.Build(File.ReadAllText("content/yafn.arch"))[0];
		}
	}
}
