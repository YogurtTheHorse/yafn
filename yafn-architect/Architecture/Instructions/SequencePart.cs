using System.Collections;

namespace Yafn.Architect.Architecture.Instructions {
	public class SequencePart {
		public ESequencePartType PartType;
		
		public BitArray Bits;
		public string Tag;

		public SequenceFieldDescription FieldType;
		public string FieldName;
	}
}