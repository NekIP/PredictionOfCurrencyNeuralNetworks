using System;

namespace DataBase.Entities {
	public class Product : Entity {
		public double Open { get; set; }
		public double High { get; set; }
		public double Low { get; set; }
		public double Close { get; set; }
		public double ChangeCloseOpen { get; set; }
		public double ChangeHighLow { get; set; }
        public override double Value() => Close;
    }
}
