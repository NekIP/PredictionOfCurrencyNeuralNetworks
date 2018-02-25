using DataManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Experiment {
    public class InterpolationTest : Experiment {
        public override void Run() {
            var interpolation = new SplineInterpolation(new Dictionary<double, double> {
                { (DateTime.Now - TimeSpan.FromHours(24)).Ticks, 0.5 },
                { (DateTime.Now - TimeSpan.FromHours(23)).Ticks, 0.56 },
                { (DateTime.Now - TimeSpan.FromHours(22)).Ticks, 0.43 },
                { (DateTime.Now - TimeSpan.FromHours(20)).Ticks, 0.78 },
                { (DateTime.Now - TimeSpan.FromHours(18)).Ticks, 0.93 },
                { (DateTime.Now - TimeSpan.FromHours(17)).Ticks, 0.54 },
                { (DateTime.Now - TimeSpan.FromHours(16)).Ticks, 0.23 },
                { (DateTime.Now - TimeSpan.FromHours(15)).Ticks, 0.245 }
            });

            var result = new[] {
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(24)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(23)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(21)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(19)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(15)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(14)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(13)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(12)).Ticks),
                interpolation.GetValue((DateTime.Now - TimeSpan.FromHours(25)).Ticks)
            };
            
        }
    }
}
