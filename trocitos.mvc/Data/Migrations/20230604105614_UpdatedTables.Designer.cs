﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace trocitos.Data.Migrations
{
    [DbContext(typeof(TrocitosDbContext))]
    [Migration("20230604105614_UpdatedTables")]
    partial class UpdatedTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("trocitos.mvc.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Cancellation")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<int>("PartySize")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNo")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeOnly>("RsvEnd")
                        .HasColumnType("time(6)");

                    b.Property<TimeOnly>("RsvStart")
                        .HasColumnType("time(6)");

                    b.Property<string>("Surname")
                        .HasColumnType("longtext");

                    b.Property<int>("TableNo")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.HasIndex("TableNo");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("trocitos.mvc.Models.Table", b =>
                {
                    b.Property<int>("TableNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Booked")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.HasKey("TableNo");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("trocitos.mvc.Models.Reservation", b =>
                {
                    b.HasOne("trocitos.mvc.Models.Table", "Table")
                        .WithMany("Reservations")
                        .HasForeignKey("TableNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Table");
                });

            modelBuilder.Entity("trocitos.mvc.Models.Table", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}