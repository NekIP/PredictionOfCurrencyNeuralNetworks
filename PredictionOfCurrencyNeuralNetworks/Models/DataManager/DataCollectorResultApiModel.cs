using DataBase;
using System.Collections.Generic;
using System.Linq;

namespace PredictionOfCurrencyNeuralNetworks.Models.DataManager {
    public class DataCollectorResultApiModel {
        public IList<EntityApiModel> Values { get; set; }
        public double ExpectedValue { get; set; }
        public double Dispersion { get; set; }
        public static DataCollectorResultApiModel Map(IList<Entity> entity, double expectedValue, double dispersion) =>
            new DataCollectorResultApiModel {
                Values = entity.Select(EntityApiModel.Map).ToList(),
                ExpectedValue = expectedValue,
                Dispersion = dispersion
            };
    }
}
