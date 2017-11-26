using System.Collections.Generic;
using Yafn.Architect.Ext;

namespace Yafn.Architect.Architecture.Registers {
	public class RegisterDescription : INamed {
		public string Name { get; set; }
		public List<StoragePartDescription> Parts;

		public RegisterDescription() {
			Parts = new List<StoragePartDescription>();
		}
	}
}