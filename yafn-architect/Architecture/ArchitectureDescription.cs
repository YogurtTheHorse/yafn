using Yafn.Architect.Architecture.Memory;
using Yafn.Architect.Architecture.Registers;
using Yafn.Architect.Architecture.Instructions;
using Yafn.Architect.Ext;

using System;

namespace Yafn.Architect.Architecture {
	public class ArchitectureDescription {
		public NamedDictionary<StorageDescription> Storages;
		public NamedDictionary<RegisterDescription> Registers;

		public NamedDictionary<MemoryDescription> MemoryRanges;

		public NamedDictionary<Sequence> Sequences;
		public NamedDictionary<MultiSequence> MultiSequences;
		public NamedDictionary<SequenceFieldDescription> FieldsTypes;
		public NamedDictionary<InstructionDescription> Instructions;

		public string Name { get; private set; }

		public ArchitectureDescription(string name) {
			Name = name;

			Storages = new NamedDictionary<StorageDescription>();
			Registers = new NamedDictionary<RegisterDescription>();
			
			MemoryRanges = new NamedDictionary<MemoryDescription>();

			Sequences = new NamedDictionary<Sequence>();
			MultiSequences = new NamedDictionary<MultiSequence>();
			FieldsTypes = new NamedDictionary<SequenceFieldDescription>();
			Instructions = new NamedDictionary<InstructionDescription>();
		}

		public bool ContainsStorage(string name) {
			return Storages.ContainsKey(name);
		}

		public bool ContainsFieldType(string field_type) {
			return FieldsTypes.ContainsKey(field_type);
		}

		public void AddRegister(RegisterDescription reg) {
			if (Registers.ContainsKey(reg.Name)) {
				throw new ArgumentException($"Dublicate register: {reg.Name}");
			}

			Registers.Add(reg);
		}

		public void AddStorage(StorageDescription storageDescription) {
			if (Storages.ContainsKey(storageDescription.Name)) {
				throw new ArgumentException($"Dublicate storage: {storageDescription.Name}");
			}

			Storages.Add(storageDescription);
		}

		public void AddMemoryRange(MemoryDescription memoryDescription) {
			if (MemoryRanges.ContainsKey(memoryDescription.Name)) {
				throw new ArgumentException($"Dublicate memory range: {memoryDescription.Name}");
			}

			MemoryRanges.Add(memoryDescription);
		}

		public void AddFieldType(SequenceFieldDescription sequenceFieldDescription) {
			if (FieldsTypes.ContainsKey(sequenceFieldDescription.Name)) {
				throw new ArgumentException($"Dublicate field: {sequenceFieldDescription.Name}");
			}

			FieldsTypes.Add(sequenceFieldDescription);
		}

		public void AddInstruction(InstructionDescription id) {
			if (Instructions.ContainsKey(id.Name)) {
				throw new ArgumentException($"Dublicate instruction: {id.Name}");
			}

			Instructions.Add(id);
		}
	}
}