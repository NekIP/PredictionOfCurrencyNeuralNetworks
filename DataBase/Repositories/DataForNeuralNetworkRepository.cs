using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IDataForNeuralNetworkRepository : IRepository<DataForNeuralNetwork> { }
    public class DataForNeuralNetworkRepository : Repository<DataForNeuralNetwork>, IDataForNeuralNetworkRepository {
        public override DbSet<DataForNeuralNetwork> Table() => DataForNeuralNetwork;
        public DbSet<DataForNeuralNetwork> DataForNeuralNetwork { get; set; }
        public DataForNeuralNetworkRepository(IConfiguration configuration) : base(configuration) { }
    }
}
