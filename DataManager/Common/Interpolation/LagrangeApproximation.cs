using System.Collections.Generic;
using System.Linq;

namespace DataManager {
    public class LagrangeApproximation {
        public List<KeyValuePair<double, double>> Values { get; set; }
        public LagrangeApproximation(List<KeyValuePair<double, double>> values) {
            Values = values;
        }

        public double GetValue(double x) {
            var value = 0d;
            for (var i = 0; i < Values.Count; i++) {
                var basic = Values.Where((t, j) => j != i).Aggregate(1d, (current, t) => current * ((x - t.Key) / (Values[i].Key - t.Key)));
                value += basic * Values[i].Value;
            }
            return value;
        }
    }
}
