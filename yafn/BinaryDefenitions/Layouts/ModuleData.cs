﻿using System;
using System.IO;

namespace yafn.BinaryDefenitions {
	public struct ModuleData : IBinaryReadable {
		public int sectionsCount;
		public int symbolsCount;
		public int sourceFilesCount;
		public int sourceTextRangesCount;
		public int sourceCodePointsCount;
		public int blobsCount;
		public int stringsCount;

		public SectionLayout[] sections;
		public SymbolLayout[] symbols;
		public SourceFileLayout[] sourceFiles;
		public SourceTextRangeLayout[] sourceTextRanges;
		public SourceCodePointLayout[] sourceCodePoints;

		public int[] blobsLengths;
		public BlobLayout[] blobs;

		public StringLayout[] strings;

		public void Read(BinaryReader reader) {
			sectionsCount = reader.ReadInt32();
			symbolsCount = reader.ReadInt32();
			sourceFilesCount = reader.ReadInt32();
			sourceTextRangesCount = reader.ReadInt32();
			sourceCodePointsCount = reader.ReadInt32();
			blobsCount = reader.ReadInt32();
			stringsCount = reader.ReadInt32();

			sections = Utils.ReadArray<SectionLayout>(reader, sectionsCount);
			symbols = Utils.ReadArray<SymbolLayout>(reader, symbolsCount);
			sourceFiles = Utils.ReadArray<SourceFileLayout>(reader, sourceFilesCount);
			sourceTextRanges = Utils.ReadArray<SourceTextRangeLayout>(reader, sourceTextRangesCount);
			sourceCodePoints = Utils.ReadArray<SourceCodePointLayout>(reader, sourceCodePointsCount);

			blobsLengths = new int[blobsCount];
			byte[] blobsLengthBytes = reader.ReadBytes(blobsCount * 4);
			Buffer.BlockCopy(blobsLengthBytes, 0, blobsLengths, 0, blobsCount * 4);

			blobs = new BlobLayout[blobsCount];
			for (int i = 0; i < blobsCount; i++) {
				blobs[i].Read(reader, blobsLengths[i]);
			}

			strings = Utils.ReadArray<StringLayout>(reader, stringsCount);
		}
	}
}