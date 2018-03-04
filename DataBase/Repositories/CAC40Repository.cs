using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface ICAC40Repository : IRepository<Product> { }
    public class CAC40Repository : Repository<Product>, ICAC40Repository {
        public override DbSet<Product> Table() => CAC40;
        public DbSet<Product> CAC40 { get; set; }
        public CAC40Repository(IConfiguration configuration) : base(configuration) { }
    }
}
