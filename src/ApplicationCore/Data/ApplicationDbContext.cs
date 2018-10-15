using System;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<LogError> LogErrors { get; set; }
        public DbSet<LogEmail> LogEmails { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SubscribeArticle> SubscribeArticles { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>(ConfigureArticle);
            modelBuilder.Entity<Contact>(ConfigureContact);
            modelBuilder.Entity<LogError>(ConfigureLogError);
            modelBuilder.Entity<LogEmail>(ConfigureLogEmail);
            modelBuilder.Entity<Tag>(ConfigureLogTag);
            modelBuilder.Entity<SubscribeArticle>(ConfigureSubscribeArticle);

            modelBuilder.Entity<TagArticle>(entity =>
            {
                entity.ToTable("TagArticles");
                entity.HasKey(e => new { e.TagId, e.ArticleId });

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagArticles)
                    .HasForeignKey(d => d.TagId);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.TagArticles)
                    .HasForeignKey(d => d.ArticleId);
            });
        }

        private void ConfigureArticle(EntityTypeBuilder<Article> modelBuilder)
        {
            modelBuilder.ToTable("Articles");
            modelBuilder.HasKey(c => c.Id);

            modelBuilder.HasIndex(a => a.Slug).IsUnique();
            modelBuilder.Property(e => e.Slug).IsRequired();

            modelBuilder.HasIndex(a => a.Title).IsUnique();
            modelBuilder.Property(e => e.Title).IsRequired();

            modelBuilder.Property(e => e.Body).IsRequired();

            modelBuilder.HasOne(d => d.DefaultTag)
                .WithMany(p => p.Articles)
                .HasForeignKey(d => d.DefaultTagId);

            modelBuilder.HasOne(d => d.Owner)
                .WithMany(p => p.Articles)
                .HasForeignKey(d => d.OwnerId);
        }

        private void ConfigureContact(EntityTypeBuilder<Contact> modelBuilder)
        {
            modelBuilder.ToTable("Contacts");
            modelBuilder.HasKey(c => c.Id);

            modelBuilder.Property(c => c.EmailFrom).IsRequired();
            modelBuilder.Property(c => c.Message).IsRequired();
            modelBuilder.Property(c => c.SendAt).IsRequired();

            modelBuilder.Property(c => c.Subject)
                .HasMaxLength(255)
                .IsRequired();
        }

        private void ConfigureLogError(EntityTypeBuilder<LogError> modelBuilder)
        {
            modelBuilder.ToTable("LogErrors");
            modelBuilder.HasKey(l => l.Id);

            modelBuilder.Property(l => l.Message).IsRequired();
            modelBuilder.Property(l => l.Path).IsRequired();
            modelBuilder.Property(l => l.StackTrace).IsRequired();

            modelBuilder.Property(l => l.Username)
                .IsRequired()
                .HasMaxLength(255);
        }

        private void ConfigureLogEmail(EntityTypeBuilder<LogEmail> modelBuilder)
        {
            modelBuilder.ToTable("LogEmails");
            modelBuilder.HasKey(r => r.Id);

            modelBuilder.Property(r => r.From).IsRequired();
            modelBuilder.Property(r => r.To).IsRequired();
            modelBuilder.Property(r => r.Subject).IsRequired();
            modelBuilder.Property(r => r.Message).IsRequired();
        }

        private void ConfigureLogTag(EntityTypeBuilder<Tag> modelBuilder)
        {
            modelBuilder.ToTable("Tags");
            modelBuilder.HasKey(c => c.Id);

            modelBuilder.HasIndex(t => t.Slug).IsUnique();
            modelBuilder.Property(e => e.Slug).IsRequired();

            modelBuilder.HasIndex(e => e.Name).IsUnique();
            modelBuilder.Property(e => e.Name).IsRequired();
        }

        private void ConfigureSubscribeArticle(EntityTypeBuilder<SubscribeArticle> modelBuilder)
        {
            modelBuilder.ToTable("SubscribeArticles");
            modelBuilder.HasKey(s => s.Id);

            modelBuilder.HasIndex(s => s.Email).IsUnique();
            modelBuilder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.HasIndex(s => s.Token).IsUnique();
            modelBuilder.Property(s => s.Token).IsRequired();

            modelBuilder.Property(s => s.SubscribeAt).IsRequired();

            modelBuilder.HasOne(s => s.User)
                .WithOne(u => u.SubscribeArticle)
                .HasForeignKey<AppUser>(u => u.SubscribeArticleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
