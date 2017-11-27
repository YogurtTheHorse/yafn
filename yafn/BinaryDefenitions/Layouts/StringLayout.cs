﻿using System;
using System.IO;

namespace yafn.BinaryDefenitions {
	public struct StringLayout : IBinaryReadable {
		public int strLength;
		public string str;

		public void Read(BinaryReader reader) {
			strLength = reader.ReadInt32();
			for (int i = 0; i < strLength; i++) {
				str += (char)reader.ReadByte();
			}
		}

		public static implicit operator string(StringLayout s) {
			return s.str;
		}

		public override string ToString() {
			return str;
		}
	}
}