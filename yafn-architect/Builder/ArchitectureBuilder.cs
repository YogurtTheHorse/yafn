using Irony.Parsing;
using System.Collections.Generic;
using System;
using Yafn.Architect.Architecture;
using System.Linq;
using Yafn.Architect.Architecture.Registers;
using Yafn.Architect.Ext;
using Yafn.Architect.Architecture.Memory;
using Yafn.Architect.Architecture.Instructions;
using System.Collections;

namespace Yafn.Architect.Builder {
	public class ArchitectureBuilder {
		public List<ArchitectureDescription> Build(string input) {
			LanguageData language = new LanguageData(new ArchitectureGrammar());
			language.CanParse();

			Parser parser = new Parser(language);
			ParseTree syntaxTree = parser.Parse(input);

			return ParseTree(syntaxTree);
		}

		public static List<ArchitectureDescription> ParseTree(ParseTree tree) {
			List<ArchitectureDescription> archs = new List<ArchitectureDescription>();
			foreach (ParseTreeNode node in tree.Root.ChildNodes) {
				archs.Add(ParseArchitecture(node));
			}

			return archs;
		}

		private static List<ParseTreeNode> FindSectionsNode(ParseTreeNode root, string name) {
			List<ParseTreeNode> sectionsNodes = new List<ParseTreeNode>();
			foreach (ParseTreeNode sectionNode in root.ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes) {
				if (sectionNode.ChildNodes[0].Term.Name == name) {
					sectionsNodes.Add(sectionNode.ChildNodes[0]);
				}
			}

			return sectionsNodes;
		}

		private static ArchitectureDescription ParseArchitecture(ParseTreeNode node) {
			string name = node.ChildNodes[1].Token.ValueString;
			ArchitectureDescription architecture = new ArchitectureDescription(name);

			AddStorages(architecture, node);
			AddMemoryRanges(architecture, node);
			AddInstruucitions(architecture, node);

			return architecture;
		}

		private static void AddInstruucitions(ArchitectureDescription architecture, ParseTreeNode node) {
			var instructionSectionDeclarations =
				from sectionNode in FindSectionsNode(node, "instructions-section")
				from instructionSectionDeclaration in sectionNode.ChildNodes[2].ChildNodes
				select instructionSectionDeclaration.ChildNodes[0].ChildNodes[0];

			var instructions =
				from declaration in instructionSectionDeclarations
				where declaration.Term.Name == "instruction-declaration"
				select declaration;

			var encodings =
				from declaration in instructionSectionDeclarations
				where declaration.Term.Name == "encoding-declaration"
				select declaration;

			var fields =
				from e in encodings
				where e.ChildNodes[0].Term.Name == "encoding-field-declaration"
				select e.ChildNodes[0];

			var cases =
				from e in encodings
				where e.ChildNodes[0].Term.Name == "encoding-case-declaration"
				select e.ChildNodes[0];

			foreach (var f in fields) {
				architecture.AddFieldType(new SequenceFieldDescription() {
					Name = f.ChildNodes[1].Token.ValueString,
					Size = (int)f.ChildNodes[6].Token.Value,
					Interpretation = f.ChildNodes[8].ChildNodes.FirstOrDefault()?.ChildNodes?.FirstOrDefault()?.ChildNodes?.First()?.Token?.ValueString ?? ""
				});
			}

			foreach (var c in cases) {
				throw new NotImplementedException("Cases not implemented");
			}

			foreach (var i in instructions) {
				foreach (InstructionDescription id in GenerateInstructions(architecture, i)) {
					architecture.AddInstruction(id);
				}
			}
		}

		private static List<InstructionDescription> GenerateInstructions(ArchitectureDescription architecture, ParseTreeNode i) {
			List<InstructionDescription> res = new List<InstructionDescription>();
			List<List<SequencePart>> partCases = new List<List<SequencePart>>();
			List<List<SequencePart>> currentParts = new List<List<SequencePart>>();
			string name = i.ChildNodes[1].Token.ValueString;


			foreach (ParseTreeNode partNode in i.ChildNodes[4].SeperatedList("instruction-content", "instruction-part")) {
				if (partNode.ChildNodes[0].Term.Name == "bit-pattern") {
					List<bool> bits = new List<bool>();

					foreach (var b in partNode.ChildNodes[0].ChildNodes) {
						int len = b.Token.Length,
							v = (int)b.Token.Value;


						string sv = Convert.ToString(v, 2);

						if (sv.Length > len) {
							sv = sv.Substring(sv.Length - len);
						} else if (sv.Length < len) {
							sv = sv.PadLeft(len, '0');
						}

						sv.ToList().ForEach(c => {
							bits.Add(c == '1');
						});
					}
					partCases.Add(new[] { new SequencePart() {
						PartType = ESequencePartType.BitPattern,
						Bits = new BitArray(bits.ToArray())
					} }.ToList());
				} else {
					string n = partNode.ChildNodes[0].ChildNodes[0].Term.Name;

					switch (n) {
						case "argument-field":
							string field_name = partNode.ChildNodes[0].ChildNodes[0].ChildNodes[2].Token.ValueString,
								field_type = partNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.ValueString;

							if (!architecture.ContainsFieldType(field_type)) {
								throw new KeyNotFoundException($"Field type {field_type} not found for {name}.");
							}

							partCases.Add(new List<SequencePart>(new[] {new SequencePart() {
								PartType = ESequencePartType.Field,
								FieldName = field_name,
								FieldType = architecture.FieldsTypes[field_type]
							} }));
							break;

						default:
							throw new NotImplementedException(n + "is not implemented.");
					}
				}
			}

			return new List<InstructionDescription>();
			//throw new NotImplementedException();
		}

		private static void AddMemoryRanges(ArchitectureDescription architecture, ParseTreeNode node) {
			var memoryRanges =
				from sectionNode in FindSectionsNode(node, "memory-section")
				from memoryRange in sectionNode.ChildNodes[2].ChildNodes
				select memoryRange;

			memoryRanges.ToList().ForEach(r => AddMemoryRange(architecture, r));
		}

		private static void AddStorages(ArchitectureDescription architecture, ParseTreeNode node) {
			var registerSectionDeclarations =
				from sectionNode in FindSectionsNode(node, "registers-section")
				from registerSectionDeclaration in sectionNode.ChildNodes[2].ChildNodes
				select registerSectionDeclaration.ChildNodes[0].ChildNodes[0];

			registerSectionDeclarations.Where(d => d.Term.Name == "storage-declaration").ToList().ForEach(d => {
				AddStorage(architecture, d);
			});

			registerSectionDeclarations.Where(d => d.Term.Name == "view-declaration").ToList().ForEach(d => {
				AddRegister(architecture, d);
			});
		}

		private static void AddMemoryRange(ArchitectureDescription architecture, ParseTreeNode r) {
			string name = r.ChildNodes[1].Token.ValueString;
			IndexRange range = ReadIndexRange(r.ChildNodes[2]);

			Dictionary<string, object> variables = new Dictionary<string, object>();
			foreach (ParseTreeNode node in r.ChildNodes[4].ChildNodes) {
				string var_name = node.ChildNodes[0].ChildNodes[0].Token.ValueString;

				if (variables.ContainsKey(var_name)) {
					throw new ArgumentException($"Duplicated variable {var_name} for {name} range.");
				}

				if (var_name == "endianess") {
					variables.Add(var_name, node.ChildNodes[0].ChildNodes[2].ChildNodes[0].Token.ValueString);
				} else {
					variables.Add(var_name, (int)node.ChildNodes[0].ChildNodes[2].Token.Value);
				}
			}

			if (variables.Count != 3) {
				throw new ArgumentException($"Not enough variables for memory range {name}.");
			}

			architecture.AddMemoryRange(new MemoryDescription() {
				Name = name,
				CellSize = (int)variables["cell"],
				Endianess = (variables["endianess"].ToString() == "big-endian") ? EEndianess.BigEndian : EEndianess.LittleEndian,
				Granularity = (int)variables["granularity"],

				StartBit = range.Start,
				EndBit = range.End
			});
		}

		private static IndexRange ReadIndexRange(ParseTreeNode node) {
			uint s = ((int)node.ChildNodes[1].Token.Value).ToUInt(),
				 e = ((int)node.ChildNodes[3].Token.Value).ToUInt();

			if (s >= e) {
				throw new IndexOutOfRangeException("Index range start bit must be less than end bit");
			}

			return new IndexRange(s, e);
		}

		private static void AddStorage(ArchitectureDescription architecture, ParseTreeNode parseTreeNode) {
			architecture.AddStorage(new StorageDescription() {
				Name = parseTreeNode.ChildNodes[1].Token.ValueString,
				Length = (int)parseTreeNode.ChildNodes[3].Token.Value
			});
		}

		private static void AddRegister(ArchitectureDescription architecture, ParseTreeNode parseTreeNode) {
			RegisterDescription reg = new RegisterDescription() {
				Name = parseTreeNode.ChildNodes[1].Token.ValueString
			};

			List<ParseTreeNode> parts = parseTreeNode.ChildNodes[3].SeperatedList("view-parts", "view-part");
			foreach (var part in parts) {
				string name = part.ChildNodes[0].Token.ValueString;
				IndexRange range = ReadIndexRange(part.ChildNodes[1]);

				if (architecture.ContainsStorage(name)) {
					StorageDescription storage = architecture.Storages[name];
					StoragePartDescription stPart = new StoragePartDescription() {
						Storage = storage,
						StartBit = range.Start,
						EndBit = range.End
					};

					if (range.Start < 0) {
						throw new IndexOutOfRangeException("View range must have positive indexes.");
					} else if (range.End > storage.Length) {
						throw new IndexOutOfRangeException("View is out of storage bounds.");
					}

					reg.Parts.Add(stPart);
				} else {
					throw new KeyNotFoundException($"Storage {name} not found in {architecture.Name}.");
				}
			}

			architecture.AddRegister(reg);
		}
	}
}
