﻿// <auto-generated />
using System;
using Eumel.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Eumel.Persistance.Migrations
{
    [DbContext(typeof(EumelGameContext))]
    [Migration("20210515190115_AddPersistedSeriesEvents")]
    partial class AddPersistedSeriesEvents
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Eumel.Persistance.GameEvents.PersistedEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("GameUuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Payload")
                        .HasColumnType("jsonb");

                    b.Property<int>("RoundIndex")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("GameUuid");

                    b.HasIndex("RoundIndex");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Eumel.Persistance.GameEvents.PersistedSeriesEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("GameUuid")
                        .HasColumnType("text");

                    b.Property<string>("Payload")
                        .HasColumnType("jsonb");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("GameUuid");

                    b.ToTable("SeriesEvents");
                });

            modelBuilder.Entity("Eumel.Persistance.Games.PersistedEumelGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Eumel.Persistance.Games.PersistedGameRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<int>("NumTricks")
                        .HasColumnType("integer");

                    b.Property<int?>("PersistedEumelGameId")
                        .HasColumnType("integer");

                    b.Property<int>("StartingPlayerIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersistedEumelGameId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Eumel.Persistance.Games.PersistedPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PersistedEumelGameId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PersistedEumelGameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Eumel.Persistance.Games.PersistedGameRound", b =>
                {
                    b.HasOne("Eumel.Persistance.Games.PersistedEumelGame", null)
                        .WithMany("Rounds")
                        .HasForeignKey("PersistedEumelGameId");
                });

            modelBuilder.Entity("Eumel.Persistance.Games.PersistedPlayer", b =>
                {
                    b.HasOne("Eumel.Persistance.Games.PersistedEumelGame", null)
                        .WithMany("Players")
                        .HasForeignKey("PersistedEumelGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eumel.Persistance.Games.PersistedEumelGame", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Rounds");
                });
#pragma warning restore 612, 618
        }
    }
}
