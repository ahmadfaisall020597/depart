﻿// <auto-generated />
using API5.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API5.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API5.Models.Account", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("API5.Models.Department", b =>
                {
                    b.Property<string>("Dept_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Dept_Initial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dept_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Dept_Id");

                    b.ToTable("Departments", (string)null);
                });

            modelBuilder.Entity("API5.Models.Employee", b =>
                {
                    b.Property<string>("Employee_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Dept_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Employee_Id");

                    b.HasIndex("Dept_Id");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("API5.Models.Employee", b =>
                {
                    b.HasOne("API5.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("Dept_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });
#pragma warning restore 612, 618
        }
    }
}
