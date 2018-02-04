using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IUsdToRubCurrencyRepository : IRepository<Product> { }

    public class UsdToRubCurrencyRepository : Repository<Product>, IUsdToRubCurrencyRepository {
        public override DbSet<Product> Table() => UsdToRub;
        public DbSet<Product> UsdToRub { get; set; }
        public UsdToRubCurrencyRepository(IConfiguration configuration) : base(configuration) { }
    }
}