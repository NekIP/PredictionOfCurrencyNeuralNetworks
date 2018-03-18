using DataBase.Entities;
using System;

namespace PredictionOfCurrencyNeuralNetworks.Models {
    public class DataForNeuralNetworkApiModel {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public double[] Data { get; set; }

        public static DataForNeuralNetworkApiModel Map(DataForNeuralNetwork entity) => new DataForNeuralNetworkApiModel() {
            Id = entity.Id,
            Date = entity.Date,
            Data = entity.Data
        };
    }
}
