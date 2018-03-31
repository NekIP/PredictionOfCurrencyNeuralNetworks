using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiment {
	public class UnsafeReferenceMatrix : Experiment {
		public class Ref<T> {
			public T V;

			public Ref(T value) {
				V = value;
			}

			public override string ToString() {
				return V.ToString();
			}
		}

		public override void Run() {

		}
	}
}
