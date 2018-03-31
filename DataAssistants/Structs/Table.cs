using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAssistants.Structs {
    public class Table {
        public Dictionary<string, int> Names { get; set; }
        public Matrix Matrix { get; set; }

        public double this[int i, int j] {
            get {
                return Matrix[i, j];
            }
            set {
                Matrix[i, j] = value;
            }
        }

        public Vector this[int i] {
            get {
                return Matrix[i];
            }
            set {
                Matrix[i] = value;
            }
        }

        public Vector this[string i] {
            get {
                return Matrix.Vectors.Select(x => x[Names[i]]).ToArray();
            }
        }
    }
}
