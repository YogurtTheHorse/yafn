using System;
using System.IO;
using Yafn.Architect.Builder;
using Yafn.Interpreter;
using Yafn.Parser.Layouts;

namespace Yafn {
	public class Program {
		public static ModuleLayout modLayout;

		public static ModuleLayout ReadBin(string path) {
			ModuleLayout module = new ModuleLayout();
			
			using (BinaryReader b_reader = new BinaryReader(File.Open(path, FileMode.Open))) {
				module.Read(b_reader);
			}

			return module;
		}
		
		public static void Main(string[] args) {
			Console.WriteLine("Reading module file...");
			modLayout = ReadBin("content\\out.ptptb");
			Console.WriteLine($"Done. Read {modLayout.header.signature}.");
			Module mod = new Module(modLayout);

			Runner.Prepare(mod);
			Runner.Run();
			Console.ReadLine();

			//ArchitectureBuilder builder = new ArchitectureBuilder();
			//var a = builder.Build(File.ReadAllText("content/yafn.arch"))[0];
		}
	}
}
