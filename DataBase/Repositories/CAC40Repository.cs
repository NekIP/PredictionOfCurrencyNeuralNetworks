using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface ICAC40Repository : IRepository<Product> { }
    public class CAC40Repository : Repository<Product>, ICAC40Repository {
        public override DbSet<Product> Table() => CSI200;
        public DbSet<Product> CSI200 { get; set; }
        public CAC40Repository(IConfiguration configuration) : base(configuration) { }
    }
}
