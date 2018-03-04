using System;

namespace DataBase.Entities {
    public class EconomicIndicator : Entity {
        public double Indicator { get; set; }
        public override double Value() => Indicator;
    }
}
