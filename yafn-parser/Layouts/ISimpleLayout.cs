using System.IO;

namespace Yafn.Parser.Layouts {
	public interface ISimpleLayout : ILayout {
		void Read(BinaryReader reader);
	}
}