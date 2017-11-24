using System.IO;

namespace Yafn.Parser.Layouts {
	public interface ISimpleLayout {
		void Read(BinaryReader reader);
	}
}