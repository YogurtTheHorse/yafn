using System.Collections.Generic;

namespace Yafn.Architect.Ext {
	public class NamedDictionary<T> : Dictionary<string, T> where T : INamed { 
		public void Add(T v) {
			base.Add(v.Name, v);
		}
	}
}
