using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Entities {
    public class DataForNeuralNetwork : Entity {
        public double D1 { get; set; }
        public double D2 { get; set; }
        public double D3 { get; set; }
        public double D4 { get; set; }
        public double D5 { get; set; }
        public double D6 { get; set; }
        public double D7 { get; set; }
        public double D8 { get; set; }
        public double D9 { get; set; }
        public double D10 { get; set; }
        public double D11 { get; set; }
        public double D12 { get; set; }
        public double D13 { get; set; }
        public double D14 { get; set; }
        public double D15 { get; set; }
        public double D16 { get; set; }
        public double D17 { get; set; }
        public double D18 { get; set; }
        public double D19 { get; set; }
        public double D20 { get; set; }
        public double D21 { get; set; }
        public double D22 { get; set; }
        public double D23 { get; set; }
        public double D24 { get; set; }
        public double D25 { get; set; }
        public double D26 { get; set; }
        public double D27 { get; set; }
        public double D28 { get; set; }
        public double D29 { get; set; }
        public double D30 { get; set; }
        public double D31 { get; set; }
        public double D32 { get; set; }

        public int Count { get; set; }

        [NotMapped]
        public double[] Data {
            get {
                var result = new double[Count];
                for (var i = 0; i < Count; i++) {
                    var property = GetType().GetProperty($"D{ i + 1 }");
                    result[i] = (double)property.GetValue(this);
                }
                return result;
            }
            set {
                if (value.Length > 32) {
                    throw new Exception("Length of array must be less or equals than 32");
                }
                Count = value.Length;
                for (var i = 0; i < Count; i++) {
                    var property = GetType().GetProperty($"D{ i + 1 }");
                    property.SetValue(this, value[i]);
                }
            }
        }

        public DataForNeuralNetwork() { }
        public DataForNeuralNetwork(int count) {
            Count = count;
        }
    }
}
