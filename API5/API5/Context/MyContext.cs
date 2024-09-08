using API5.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API5.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship between Account and Employee
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithOne()
                .HasForeignKey<Account>(a => a.AccountId);

            // Configure foreign key relationship in Employee
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany() // Assuming Department does not have a collection of Employees
                .HasForeignKey(e => e.Dept_Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
