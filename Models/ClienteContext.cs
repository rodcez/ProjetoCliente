using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Models
{
    public class ClienteContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Data Source=.\sqlexpress;Initial Catalog=ClienteDB;Integrated Security=True");
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigCliente(builder);
            base.OnModelCreating(builder);
        }

        private void ConfigCliente(ModelBuilder builder)
        {
            builder.Entity<Cliente>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.Nome).HasMaxLength(100);
            });
        }
    }
}
