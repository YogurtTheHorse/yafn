using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Yafn.Instructions;
using Yafn.Interpreter;

namespace Yafn {
	public static class Runner {
		private static Section codeSection, dataSection, constants;
		private static byte[] data, code;

		private static bool haltOnError;

		private static int err;
		private static int cursor;
		private static bool was_brp = false;

		private static Random random;


		private static Stack<int> stack;

		public static void Prepare(Module module) {
			Console.WriteLine("Looking for sections");

			codeSection = module.FindSectionByName("__code");
			dataSection = module.FindSectionByName("__data");
			constants = module.FindSectionByName("__constants");

			Console.WriteLine("Preapring memory structures.");
			stack = new Stack<int>();

			data = new byte[Math.Max(dataSection.Data.Length, 256)];
			Array.Copy(dataSection.Data, data, dataSection.Data.Length);

			code = new byte[codeSection.Data.Length];
			Array.Copy(codeSection.Data, code, code.Length);

			random = new Random();
		}

		public static void Run(bool halt_on_error = true) {
			Console.WriteLine("Running instructions...");
			Debug.WriteLine("Running instructions...");

			haltOnError = halt_on_error;
			err = 0;
			cursor = 0;

			foreach (Label l in codeSection.Labels) {
				if (l.Name == "__start") {
					cursor = (int)l.Offset;
					break;
				}
			}

			while (cursor < code.Length) {
				Instruction ins = ReadInstruction();

				DoInstruction(ins);
			}
		}

		private static void DoInstruction(Instruction ins) {
			InstructionGroup grp = ins.GetGroup();

			Debug.Write(ins.ToString() + " ");
			if (ins.HasArgument()) {
				Debug.WriteLine(ReadInt32(cursor, code));
			} else {
				Debug.WriteLine("");
			}

			switch (grp) {
				case InstructionGroup.Base:
					BaseInstruction(ins);
					break;

				case InstructionGroup.Stack:
					StackInstruction(ins, ins.HasArgument() ? ReadInt32() : 0);
					break;

				case InstructionGroup.Aritmetics:
					ArithmeticInstruction(ins);
					break;

				case InstructionGroup.Bitwise:
					BitwiseCommand(ins);
					break;

				case InstructionGroup.ConditionsAndJumps:
					CondAndJumpInstruction(ins, ins.HasArgument() ? ReadInt32() : 0);
					break;
			}

			#if DEBUG
			//Dump();
			#endif
		}

		private static void Dump() {
			using (BinaryWriter b_writer = new BinaryWriter(File.Open("data.bin", FileMode.Create))) {
				b_writer.Write(data);
			}
		}

		private static void BitwiseCommand(Instruction ins) {
			int bit_type = (((int)ins) & 12) >> 2,
				bit_code = (((int)ins) & 3);

			int a = stack.Pop();

			switch (bit_type) {
				case 0: // usual
				case 4: // xor
					int b = stack.Pop();
					if (bit_type == 0) {
						switch (bit_code) {
							case 0:
								stack.Push(a | b);
								break;

							case 1:
								stack.Push(a & b);
								break;

							case 2:
								stack.Push(a << b);
								break;

							case 3:
								stack.Push(a >> b);
								break;
						}
					} else {
						stack.Push(a ^ b);
					}
					break;

				case 1:
					stack.Push(a & (1 << bit_code));
					break;

				case 2:
					stack.Push(a << bit_code);
					break;

				case 3:
					stack.Push(a >> bit_code);
					break;
			}
		}

		private static void CondAndJumpInstruction(Instruction ins, int v) {
			switch (ins) {
				case Instruction.cmp0:
				case Instruction.cmp: {
						int a = stack.Pop(),
							b = ins == Instruction.cmp0 ? 0 : stack.Pop();

						int res =
							Convert.ToInt32(a == b) << 0 |
							Convert.ToInt32(a > b) << 1 |
							Convert.ToInt32(a < b) << 2;

						stack.Push(res);
					}
					break;

				case Instruction.eq: {
						int a = stack.Pop(),
							b = stack.Pop();

						stack.Push(a == b ? 1 : 0);
					}
					break;

				case Instruction.jmp:
					cursor = v;
					break;

				case Instruction.jez:
					if (stack.Pop() == 0) {
						cursor = v;
					}
					break;

				case Instruction.jnz:
					if (stack.Pop() != 0) {
						cursor = v;
					}
					break;

				case Instruction.call:
					stack.Push(cursor);
					cursor = v;
					break;

				case Instruction.ret:
					cursor = stack.Pop();
					break;
			}
		}

		private static void ArithmeticInstruction(Instruction ins) {
			switch (ins) {
				case Instruction.add: {
						int a = stack.Pop(),
							b = stack.Pop();

						stack.Push(a + b);
					}
					break;

				case Instruction.sub: {
						int a = stack.Pop(),
							b = stack.Pop();

						stack.Push(a - b);
					}
					break;

				case Instruction.mul: {
						int a = stack.Pop(),
							b = stack.Pop();

						stack.Push(a * b);
					}
					break;

				case Instruction.div: {
						int a = stack.Pop(),
							b = stack.Pop();

						stack.Push(a / b);
						stack.Push(a % b);
					}
					break;

				case Instruction.neg:
					stack.Push(-stack.Pop());
					break;

				case Instruction.inc:
					stack.Push(stack.Pop() + 1);
					break;

				case Instruction.dec:
					stack.Push(stack.Pop() - 1);
					break;

			}
		}

		private static void BaseInstruction(Instruction ins) {
			switch (ins) {
				case Instruction.emp:
					break;

				case Instruction.hlt:
					cursor = code.Length;
					break;

				case Instruction.brp:
					was_brp = true;
					break;

				case Instruction.wrt:
					Console.WriteLine(stack.Pop());
					break;
			}
		}

		private static void StackInstruction(Instruction ins, int arg = 0) {
			switch (ins) {
				case Instruction.epush:
					stack.Push(err);
					break;

				case Instruction.ipush:
					stack.Push(arg);
					break;

				case Instruction.mpush:
					stack.Push(ReadInt32(arg, data));
					break;

				case Instruction.cpush:
					stack.Push(ReadInt32(arg, constants.Data));
					break;

				case Instruction.pop:
					stack.Pop();
					break;

				case Instruction.dpl:
					stack.Push(stack.Peek());
					break;

				case Instruction.sav:
					WriteToMemory(stack.Pop(), arg);
					break;

				case Instruction.swp:
					int a = stack.Pop(),
						b = stack.Pop();

					stack.Push(a);
					stack.Push(b);
					break;

				case Instruction.svs:
					WriteToMemory(stack.Pop(), stack.Pop());
					break;

				case Instruction.rnd:
					stack.Push(random.Next());
					break;

				case Instruction.smpsh:
					stack.Push(ReadInt32(stack.Pop(), data));
					break;

				case Instruction.scpsh:
					stack.Push(ReadInt32(stack.Pop(), constants.Data));
					break;

			}
		}

		private static void UnkonwInstruction() {
			Error($"Unkown instruction op-code at {cursor}");
		}

		private static void Error(string v) {
			Console.WriteLine(v);

			if (haltOnError) {
				DoInstruction(Instruction.hlt);
			}
		}

		private static int ReadInt32() {
			return ReadInt32(ref cursor, code);
		}

		private static int ReadInt32(ref int cursor, byte[] arr) {
			int res = ReadInt32(cursor, arr);
			cursor += sizeof(int);

			return res;
		}

		private static int ReadInt32(int cursor, byte[] arr) {
			if (cursor + 4 <= arr.Length) {
				return BitConverter.ToInt32(arr, cursor);
			} else {
				Array.Resize(ref arr, cursor + 4);
				return 0;
			}
		}

		private static void WriteToMemory(int val, int pos) {
			if (pos + 4 > data.Length) {
				Array.Resize(ref data, pos + 4);
			}

			byte[] bytes = BitConverter.GetBytes(val);
			for (int p = pos; p < pos + sizeof(int); p++) {
				data[p] = bytes[p - pos];
			}
		}

		private static Instruction ReadInstruction() {
			return (Instruction)code[cursor++];
		}
	}
}
