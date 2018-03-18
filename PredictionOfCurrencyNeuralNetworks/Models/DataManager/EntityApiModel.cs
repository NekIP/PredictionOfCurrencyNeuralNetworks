using DataBase;
using System;

namespace PredictionOfCurrencyNeuralNetworks.Models.DataManager {
    public class EntityApiModel {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public static EntityApiModel Map(Entity entity) => 
            new EntityApiModel {
                Id = entity.Id,
                Date = entity.Date,
                Value = entity.Selector()
            };
    }
}
