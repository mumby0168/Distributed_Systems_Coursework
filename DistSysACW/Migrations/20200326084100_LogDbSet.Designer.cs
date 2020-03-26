﻿// <auto-generated />
using System;
using DistSysACW.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DistSysACW.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20200326084100_LogDbSet")]
    partial class LogDbSet
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DistSysACW.Models.Log", b =>
                {
                    b.Property<string>("LogId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LogDateTime");

                    b.Property<string>("LogString");

                    b.Property<string>("UserId");

                    b.HasKey("LogId");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("DistSysACW.Models.User", b =>
                {
                    b.Property<string>("ApiKey")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Role");

                    b.Property<string>("Username");

                    b.HasKey("ApiKey");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DistSysACW.Models.Log", b =>
                {
                    b.HasOne("DistSysACW.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}