namespace Yafn.Instructions {
	public static class Extenstions {
		public static InstructionGroup GetGroup(this Instruction instruction) {
			return (InstructionGroup)(((int)instruction) >> 4);
		}

		public static int GetCode(this Instruction instruction) {
			return 15 & (int)instruction;
		}

		public static bool HasArgument(this Instruction instruction) {
			return
				instruction == Instruction.ipush ||
				instruction == Instruction.mpush ||
				instruction == Instruction.cpush ||
				instruction == Instruction.sav ||
				instruction == Instruction.jmp ||
				instruction == Instruction.jez ||
				instruction == Instruction.jnz ||
				instruction == Instruction.call;
		}
	}
}
