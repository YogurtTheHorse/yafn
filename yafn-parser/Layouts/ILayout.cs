using System.IO;

namespace Yafn.Parser.Layouts {
	public interface ILayout {
		void Write(BinaryWriter writer);
	}
}
