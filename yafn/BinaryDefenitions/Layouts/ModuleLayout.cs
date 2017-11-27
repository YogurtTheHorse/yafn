using System;
using System.IO;

namespace yafn.BinaryDefenitions {
	public struct ModuleLayout : IBinaryReadable {
		public ModuleHeader header;
		public ModuleData data;
		public Section[] sections;

		public void Read(BinaryReader reader) {
			header.Read(reader);
			data.Read(reader);


			int[] labelsCount = new int[data.sectionsCount];
			Array.Clear(labelsCount, 0, data.sectionsCount);
			
			Label[] labels = new Label[data.symbolsCount];
			for (int i = 0; i < data.symbolsCount; i++) {
				labelsCount[data.symbols[i].sectionIndex]++;
				labels[i] = new Label() {
					name = data.strings[data.symbols[i].nameIndex],
					offset = data.symbols[i].blobEntryIndex,
					sectionId = data.symbols[i].sectionIndex
				};
			}

			sections = new Section[data.sectionsCount];
			for (int i = 0; i < data.sectionsCount; i++) {
				int ind = 0;

				sections[i] = new Section() {
					Data = data.blobs[data.sections[i].blobIndex].data,
					Name = data.strings[data.sections[i].bankNameIndex],
					Labels = new Label[labelsCount[i]]
				};
				
				for (int j = 0; j < data.symbolsCount; j++) {
					if (labels[j].sectionId == i) {
						sections[i].Labels[ind++] = labels[j];
					}
				}
			}
		}

		public Section FindSectionByName(string str) {
			foreach (Section sec in sections) {
				if (sec.Name == str) {
					return sec;
				}
			}

			return new Section() {
				Name = str
			};
		}
	}
}
