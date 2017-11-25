using System;
using System.IO;
using Yafn.Parser.Layouts;

namespace Yafn.Parser {
	public static class Utils {
		public static T[] ReadArray<T>(BinaryReader reader, int count) where T : ISimpleLayout {
			T[] res = new T[count];

			for (int i = 0; i < count; i++) {
				res[i].Read(reader);
			}

			return res;
		}

		public static void WriteArray(BinaryWriter writer, ILayout[] layouts) {
			foreach (ILayout l in layouts) {
				l.Write(writer);
			}
		}
	}
}
