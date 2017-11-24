namespace Yafn.Parser.Layouts {
	public enum SectionAccessMode {
		None = 0,
		Read = 1 << 0,
		Write = 1 << 1,
		Execute = 1 << 2
	}
}