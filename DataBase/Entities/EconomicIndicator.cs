namespace DataBase.Entities {
    public class EconomicIndicator : Entity {
        public double Indicator { get; set; }
        public override double Selector() => Indicator;
        public override void Setter(double value) {
            Indicator = value;
        }
    }
}
