using Irony.Parsing;

namespace Yafn.Architect.Builder {
	[Language("architecture", "1.0", "abc")]
	public class ArchitectureGrammar : Grammar {
		public ArchitectureGrammar() : base(true) {
			#region Declare Terminals Here
			CommentTerminal blockComment = new CommentTerminal("block-comment", "/*", "*/");
			CommentTerminal lineComment = new CommentTerminal("line-comment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
			NonGrammarTerminals.Add(blockComment);
			NonGrammarTerminals.Add(lineComment);

			StringLiteral str = new StringLiteral("string", "\"");
			NumberLiteral number = TerminalFactory.CreateCSharpNumber("number");
			NumberLiteral bits = new NumberLiteral("bits", NumberOptions.Binary | NumberOptions.IntOnly);

			IdentifierTerminal identifier = new IdentifierTerminal("identifier");
			#endregion


			NonTerminal program = new NonTerminal("program");
			NonTerminal architectures = new NonTerminal("architectures");
			NonTerminal architecture = new NonTerminal("architecture");
			NonTerminal architectureContent = new NonTerminal("architecture-content");

			NonTerminal indexRange = new NonTerminal("index-range");

			#region Sections
			NonTerminal sections = new NonTerminal("sections");
			NonTerminal section = new NonTerminal("section");
			NonTerminal sectionType = new NonTerminal("section-type");

			#region Registers
			NonTerminal registersSection = new NonTerminal("registers-section");

			NonTerminal registersSectionDeclarations = new NonTerminal("registers-section-declarations");
			NonTerminal registerSectionDeclaration = new NonTerminal("registers-section-declaration");
			NonTerminal storageDeclaration = new NonTerminal("storage-declaration");
			NonTerminal viewDeclaration = new NonTerminal("view-declaration");
			NonTerminal viewPart = new NonTerminal("view-part");
			NonTerminal viewParts = new NonTerminal("view-parts");
			#endregion

			#region Memory
			NonTerminal memorySection = new NonTerminal("memory-section");
			NonTerminal rangeDefenitions = new NonTerminal("range-defenitions");
			NonTerminal rangeDefenition = new NonTerminal("range-defenition");
			NonTerminal rangeVariables = new NonTerminal("range-variables");

			NonTerminal rangeVariable = new NonTerminal("range-variable");
			NonTerminal cellVariable = new NonTerminal("cell-variable");
			NonTerminal endianessVariable = new NonTerminal("endianess-variable");
			NonTerminal granularityVariable = new NonTerminal("granularity-variable");
			#endregion

			NonTerminal stacksSection = new NonTerminal("stacks-section");
			
			#region Instructions
			NonTerminal instructionsSection = new NonTerminal("instructions-section");

			NonTerminal instructionsSectionDeclarations = new NonTerminal("instructions-section-declarations");
			NonTerminal instructionsSectionDeclaration = new NonTerminal("instructions-section-declaration");

			NonTerminal instructionDeclaration = new NonTerminal("instruction-declaration");
			NonTerminal instructionContent = new NonTerminal("instruction-content");
			
			NonTerminal instructionPart = new NonTerminal("instruction-part");
			NonTerminal bitPattern = new NonTerminal("bit-pattern");
			NonTerminal instructionField = new NonTerminal("instruction-field");

			NonTerminal argumentField = new NonTerminal("argument-field");
			NonTerminal caseField = new NonTerminal("case-field");
			NonTerminal sequenceField = new NonTerminal("sequence-field");

			NonTerminal encodingDeclaration = new NonTerminal("encoding-declaration");
			NonTerminal encodingFieldDeclaration = new NonTerminal("encoding-field-declaration");
			NonTerminal encodingFieldDescriptionDeclaration = new NonTerminal("encoding-field-description-declaration");

			NonTerminal sequenceDeclaration = new NonTerminal("sequence-declaration");
			NonTerminal sequencesAlternatives = new NonTerminal("sequences-alternatives");
			NonTerminal sequencesAlternative = new NonTerminal("sequences-alternative");

			NonTerminal caseDeclaration = new NonTerminal("case-declaration");
			NonTerminal caseType = new NonTerminal("case-type");
			NonTerminal caseContent = new NonTerminal("case-content");
			#endregion

			#region Mnemonics
			NonTerminal mnemonicsSection = new NonTerminal("mnemonics-section");
			NonTerminal mnemonicsSectionDeclarations = new NonTerminal("mnemonics-section-declarations");
			NonTerminal mnemonicsSectionDeclaration = new NonTerminal("mnemonics-section-declaration");

			NonTerminal mnemonicDeclaration = new NonTerminal("mnemonic-declaration");
			NonTerminal mnemonicFormats = new NonTerminal("mnemonic-formats");
			NonTerminal mnemonicFormatDeclaration = new NonTerminal("mnemonic-declaration");

			NonTerminal formatDeclaration = new NonTerminal("format-declaration");
			NonTerminal formatCaseDeclarations = new NonTerminal("format-declarations");
			NonTerminal formatCaseDeclaration = new NonTerminal("format-declaration");
			NonTerminal formatArguments = new NonTerminal("format-arguments");
			NonTerminal formatArgumentsList = new NonTerminal("format-arguments-list");
			#endregion
			#endregion

			Root = program;
			program.Rule = architectures;
			architectures.Rule = MakePlusRule(architectures, architecture);
			architecture.Rule = ToTerm("architecture") + identifier + (ToTerm("{") + architectureContent + "}");
			architectureContent.Rule = sections;

			indexRange.Rule = ToTerm("[") + number + ".." + number + "]";

			sections.Rule = MakeStarRule(sections, section);
			section.Rule = registersSection | memorySection | stacksSection | instructionsSection | mnemonicsSection;

			#region Registers
			registersSection.Rule = ToTerm("registers") + ":" + registersSectionDeclarations;
			registersSectionDeclarations.Rule = MakeStarRule(registersSectionDeclarations, registerSectionDeclaration);
			registerSectionDeclaration.Rule = (storageDeclaration | viewDeclaration) + ";";

			storageDeclaration.Rule = ToTerm("storage") + identifier + "[" + number + "]";
			viewDeclaration.Rule = ToTerm("view") + identifier + "=" + (viewPart | (ToTerm("{") + viewParts + "}"));

			viewPart.Rule =
					(identifier) |
					(identifier + "[" + number + "]") |
					(identifier + indexRange);
			viewParts.Rule = viewPart + "," + viewParts | viewPart;
			#endregion

			#region Memory
			memorySection.Rule = ToTerm("memory") + ":" + rangeDefenitions;
			rangeDefenitions.Rule = MakeStarRule(rangeDefenitions, rangeDefenition);

			rangeDefenition.Rule = ToTerm("range") + identifier + indexRange + "{" + rangeVariables + "}";
			rangeVariables.Rule = MakeStarRule(rangeVariables, rangeVariable);

			rangeVariable.Rule = cellVariable | endianessVariable | granularityVariable;
			cellVariable.Rule = ToTerm("cell") + "=" + number + ";";
			endianessVariable.Rule = ToTerm("endianess") + "=" + (ToTerm("little-endian") | "big-endian") + ";";
			granularityVariable.Rule = ToTerm("granularity") + "=" + number + ";";
			#endregion

			#region Stacks
			stacksSection.Rule = ToTerm("stacks") + ":";
			#endregion

			#region Instruction
			instructionsSection.Rule = ToTerm("instructions") + ":" + instructionsSectionDeclarations;
			instructionsSectionDeclarations.Rule = MakeStarRule(instructionsSectionDeclarations, instructionsSectionDeclaration);
			instructionsSectionDeclaration.Rule = (instructionDeclaration | encodingDeclaration) + ";";

			instructionDeclaration.Rule = ToTerm("instruction") + identifier + "=" + ToTerm("{") + instructionContent + "}";
			instructionContent.Rule = instructionContent + "," + instructionPart | instructionPart;

			instructionPart.Rule = bitPattern | instructionField;
			bitPattern.Rule = MakePlusRule(bitPattern, bits);
			instructionField.Rule = argumentField | caseField | sequenceField;

			argumentField.Rule = identifier + "as" + identifier;
			caseField.Rule = identifier + "." + identifier;
			sequenceField.Rule = ToTerm("sequence") + identifier;

			encodingDeclaration.Rule = (encodingFieldDeclaration | sequenceDeclaration | caseDeclaration);

			encodingFieldDeclaration.Rule = ToTerm("encode") + identifier + ToTerm("field") + "=" + ToTerm("immediate") + "[" + number + "]" + 
				encodingFieldDescriptionDeclaration.Q();
			encodingFieldDescriptionDeclaration.Rule = ToTerm("data") | "offset" | "displacement";
			
			sequenceDeclaration.Rule = ToTerm("enccode") + identifier + "sequence" + ToTerm("=") +
				((ToTerm("{") + instructionContent + "}") | (ToTerm("alternatives") + "{" + sequencesAlternatives + "}"));
			sequencesAlternatives.Rule = sequencesAlternative | (sequencesAlternative + "," + sequencesAlternatives);
			sequencesAlternative.Rule = identifier + "=" + ToTerm("{") + instructionContent + "}";

			caseDeclaration.Rule = ToTerm("encode") + identifier + ToTerm("field") + "=" + caseType + "{" + caseContent + "}";
			caseType.Rule = ToTerm("register") | "cases";
			caseContent.Rule = (identifier + "=" + bitPattern) | (caseContent + "," + (identifier + "=" + bitPattern));
			#endregion

			#region Mnemonics
			mnemonicsSection.Rule = ToTerm("mnemonics") + ":" + mnemonicsSectionDeclarations;
			mnemonicsSectionDeclarations.Rule = MakeStarRule(mnemonicsSectionDeclarations, mnemonicsSectionDeclaration);
			mnemonicsSectionDeclaration.Rule = (mnemonicDeclaration | formatDeclaration) + ";";

			formatDeclaration.Rule = ToTerm("format") + identifier +
				((ToTerm("of") + formatCaseDeclarations) | (ToTerm("is") + str));
			formatCaseDeclarations.Rule = formatCaseDeclaration | (formatCaseDeclarations + "," + formatCaseDeclaration);
			formatCaseDeclaration.Rule = ((ToTerm("(") + ")") | (formatArguments + (str | identifier))) + ("when" + identifier).Q();
			formatArguments.Rule =
				(ToTerm("(") + formatArgumentsList + ")");
			formatArgumentsList.Rule = (formatArgumentsList + "," + identifier) | identifier;

			mnemonicDeclaration.Rule = ToTerm("mnemonic") + identifier + mnemonicFormats;

			mnemonicFormats.Rule = (mnemonicFormats + "," + mnemonicFormatDeclaration) | mnemonicFormatDeclaration;
			mnemonicFormatDeclaration.Rule = (ToTerm("for") + identifier).Q() + formatCaseDeclaration;
			#endregion

			Root = architectures;

			#region Define Keywords
			MarkReservedWords("registers", "storage", "view", "memory", "range", "endianess", 
				"little-endian", "big-endian", "granularity", "stacks", "isntructions", "isntruction", 
				"sequence");
			#endregion
			LanguageFlags |= LanguageFlags.NewLineBeforeEOF;

		}
	}
}
