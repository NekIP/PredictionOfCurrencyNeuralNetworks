using System.Collections.Generic;

namespace DataManager {
    public class LinearInterpolation {
        public KeyValuePair<double, double> A { get; set; }
        public KeyValuePair<double, double> B { get; set; }

        public LinearInterpolation(KeyValuePair<double, double> a, KeyValuePair<double, double> b) {
            A = a;
            B = b;
        }

        public double GetValue(double x) =>
            A.Value + ((B.Value - A.Value) / (B.Key - A.Key)) * (x - A.Key);
    }
}
