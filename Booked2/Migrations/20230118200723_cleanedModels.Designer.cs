﻿// <auto-generated />
using System;
using Booked2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Booked2.Migrations
{
    [DbContext(typeof(Booked2Context))]
    [Migration("20230118200723_cleanedModels")]
    partial class cleanedModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Booked2.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ConferenceRoomId")
                        .HasColumnType("int");

                    b.Property<int>("DayId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConferenceRoomId");

                    b.HasIndex("DayId");

                    b.HasIndex("PersonId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Booked2.Models.ConferenceRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NrOfSeats")
                        .HasColumnType("int");

                    b.Property<bool>("Projector")
                        .HasColumnType("bit");

                    b.Property<bool>("WhiteBoard")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ConferenceRooms");
                });

            modelBuilder.Entity("Booked2.Models.Day", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Days");
                });

            modelBuilder.Entity("Booked2.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Booked2.Models.Booking", b =>
                {
                    b.HasOne("Booked2.Models.ConferenceRoom", "ConferenceRoom")
                        .WithMany("Bookings")
                        .HasForeignKey("ConferenceRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Booked2.Models.Day", "Day")
                        .WithMany("Bookings")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Booked2.Models.Person", "Person")
                        .WithMany("Bookings")
                        .HasForeignKey("PersonId");

                    b.Navigation("ConferenceRoom");

                    b.Navigation("Day");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Booked2.Models.ConferenceRoom", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("Booked2.Models.Day", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("Booked2.Models.Person", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}