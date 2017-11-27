namespace yafn.BinaryDefenitions {
	public class Section {
		public string Name;
		public byte[] Data;
		public Label[] Labels;

		public Section() {
			Name = null;
			Data = new byte[0] { };
		}
	}
}
