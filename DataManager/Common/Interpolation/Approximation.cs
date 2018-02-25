using System;
using System.Collections.Generic;
using System.Text;

namespace DataManager {
    public abstract class Approximation {
        public abstract double Approximate(double x, IDictionary<double, double> nodes);
    }
}
