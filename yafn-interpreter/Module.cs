using System;
using Yafn.Parser.Layouts;

namespace Yafn.Interpreter {
	public class Module {
		private ModuleLayout _layout;

		public Section[] Sections;

	
		public Module(ModuleLayout layout) {
			_layout = layout;


			int[] labelsCount = new int[_layout.data.sectionsCount];
			Array.Clear(labelsCount, 0, _layout.data.sectionsCount);

			Label[] labels = new Label[_layout.data.symbolsCount];
			for (int i = 0; i < _layout.data.symbolsCount; i++) {
				SymbolLayout symbol = _layout.data.symbols[i];
				labelsCount[symbol.sectionIndex]++;
				labels[i] = new Label() {
					Name = _layout.data.strings[symbol.nameIndex],
					Offset = symbol.blobEntryIndex,
					SectionId = symbol.sectionIndex
				};
			}

			Sections = new Section[_layout.data.sectionsCount];
			for (int i = 0; i < _layout.data.sectionsCount; i++) {
				SectionLayout section = _layout.data.sections[i];
				int ind = 0;

				Sections[i] = new Section() {
					Data = _layout.data.blobs[section.blobIndex].data,
					Name = _layout.data.strings[section.bankNameIndex],
					Labels = new Label[labelsCount[i]]
				};

				for (int j = 0; j < _layout.data.symbolsCount; j++) {
					if (labels[j].SectionId == i) {
						Sections[i].Labels[ind++] = labels[j];
					}
				}
			}
		}
	}
}
