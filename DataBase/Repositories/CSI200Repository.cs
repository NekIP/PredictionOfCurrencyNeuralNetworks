using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface ICSI200Repository : IRepository<Product> { }
    public class CSI200Repository : Repository<Product>, ICSI200Repository {
        public override DbSet<Product> Table() => CSI200;
        public DbSet<Product> CSI200 { get; set; }
        public CSI200Repository(IConfiguration configuration) : base(configuration) { }
    }
}
