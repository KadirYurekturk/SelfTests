using Microsoft.EntityFrameworkCore;
using SelfTest.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfTest.Data.Contexts
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\Mssqllocaldb;Initial Catalog=SelfTest;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;

    }
}
