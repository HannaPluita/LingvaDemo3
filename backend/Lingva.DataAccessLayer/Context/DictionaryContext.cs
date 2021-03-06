﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lingva.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingva.DataAccessLayer.Context
{
    public class DictionaryContext : DbContext
    {
        public DbSet<DictionaryRecord> Dictionary { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }
        public DbSet<SubtitleRow> SubtitleRows { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ParserWord> ParserWords { get; set; }
        public DbSet<SimpleEnWord> SimpleEnWords { get; set; }
        public DbSet<DictionaryEnWord> DictionaryEnWords { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Event> Events { get; set; }

        public DictionaryContext(DbContextOptions<DictionaryContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Film>().HasData(
                new Film[]
                {
                    new Film{ Id=1, Name="Focus"},
                    new Film{ Id=2, Name="Game"},

                });

            modelBuilder.Entity<Film>().ToTable("Films");
            modelBuilder.Entity<Subtitle>().ToTable("Subtitles");
            modelBuilder.Entity<SubtitleRow>().ToTable("SubtitleRows");

            modelBuilder.Entity<Film>()
                .HasMany(s => s.Subtitles)
                .WithOne(f => f.Film)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Film>()
                .HasOne(s => s.Language)
                .WithMany(f => f.Films)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subtitle>()
                  .HasMany(c => c.SubtitlesRow)
                  .WithOne(t => t.Subtitles)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubtitleRow>()
                  .HasOne(c => c.Subtitles)
                  .WithMany(t => t.SubtitlesRow)
                  .OnDelete(DeleteBehavior.Restrict);
           
            modelBuilder.Entity<Word>()
                  .HasOne(c => c.Language)
                  .WithMany(t => t.Words)
                  .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
