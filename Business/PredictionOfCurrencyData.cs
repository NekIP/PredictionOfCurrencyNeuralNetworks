using DataAssistants.Structs;
using Newtonsoft.Json;
using System;

namespace Business {
    public class PredictionOfCurrencyData {
        [JsonProperty]
        public DateTime Date { get; set; }
        [JsonProperty]
        public Vector Vector { get; set; }
    }
}
