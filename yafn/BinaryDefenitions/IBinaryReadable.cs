using System.IO;

namespace yafn.BinaryDefenitions {
	public interface IBinaryReadable {
		void Read(BinaryReader reader);
	}
}