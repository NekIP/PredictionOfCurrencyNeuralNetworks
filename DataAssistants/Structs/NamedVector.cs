namespace DataAssistants.Structs {
    public class NamedVector {
        public Vector Value { get; set; }
        public string[] Names { get; set; }

        public NamedVector(Vector value, string[] names) {
            Value = value;
            Names = names;
        }
    }
}
