using BojoBox.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BojoBox.EntityFramework
{
    public class BojoBoxContext : DbContext
    {
        public static string ConnectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Manually updating databases
            ConnectionString = "Server=tcp:bojoboxdbserver.database.windows.net,1433;Initial Catalog=BojoBoxDb;Persist Security Info=False;User ID=bojobiscuit;Password=omgCAT123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //ConnectionString = "Data Source=localhost;Database=bojoboxdb;Initial Catalog=bojoboxdb;User ID=sa;Password=Passw0rd123;";

            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<League>()
                .HasMany(a => a.Franchises)
                .WithOne(b => b.League)
                .HasForeignKey(b => b.LeagueId);

            modelBuilder.Entity<League>()
                .HasMany(a => a.SkaterSeasons)
                .WithOne(b => b.League)
                .HasForeignKey(b => b.LeagueId);

            modelBuilder.Entity<League>()
                .HasMany(a => a.GoalieSeasons)
                .WithOne(b => b.League)
                .HasForeignKey(b => b.LeagueId);

            modelBuilder.Entity<Franchise>()
                .HasMany(a => a.Teams)
                .WithOne(b => b.Franchise)
                .HasForeignKey(b => b.FranchiseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Franchise>()
                .HasOne(a => a.CurrentTeam)
                .WithOne()
                .HasForeignKey<Franchise>(a => a.CurrentTeamId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Team>()
                .HasMany(a => a.SkaterSeasons)
                .WithOne(b => b.Team)
                .HasForeignKey(b => b.TeamId);

            modelBuilder.Entity<Team>()
                .HasMany(a => a.GoalieSeasons)
                .WithOne(b => b.Team)
                .HasForeignKey(b => b.TeamId);

            modelBuilder.Entity<Skater>()
                .HasMany(a => a.Seasons)
                .WithOne(b => b.Skater)
                .HasForeignKey(b => b.SkaterId);

            modelBuilder.Entity<Goalie>()
                .HasMany(a => a.Seasons)
                .WithOne(b => b.Goalie)
                .HasForeignKey(b => b.GoalieId);

            modelBuilder.Entity<SkaterSeason>()
                .HasMany(a => a.SubTotals)
                .WithOne(b => b.SubtotalFor)
                .HasForeignKey(b => b.SubtotalForId);

            modelBuilder.Entity<GoalieSeason>()
                .HasMany(a => a.SubTotals)
                .WithOne(b => b.SubtotalFor)
                .HasForeignKey(b => b.SubtotalForId);
        }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Skater> Skaters { get; set; }
        public DbSet<SkaterSeason> SkaterSeasons { get; set; }

        public DbSet<Goalie> Goalies { get; set; }
        public DbSet<GoalieSeason> GoalieSeasons { get; set; }
    }
}