﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.SitemapsModule.Data.Models;

namespace VirtoCommerce.SitemapsModule.Data.Repositories
{
    public class SitemapRepository : EFRepositoryBase, ISitemapRepository
    {
        public SitemapRepository()
        {
        }

        public SitemapRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<SitemapEntity>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<SitemapEntity>().Property(x => x.Filename)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            modelBuilder.Entity<SitemapEntity>().ToTable("Sitemap");

            modelBuilder.Entity<SitemapItemEntity>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<SitemapItemEntity>().HasRequired(x => x.Sitemap).WithMany(x => x.Items)
                .HasForeignKey(x => x.SitemapId).WillCascadeOnDelete(true);
            modelBuilder.Entity<SitemapItemEntity>().ToTable("SitemapItem");

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<SitemapEntity> Sitemaps
        {
            get
            {
                return GetAsQueryable<SitemapEntity>();
            }
        }

        public IQueryable<SitemapItemEntity> SitemapItems
        {
            get
            {
                return GetAsQueryable<SitemapItemEntity>();
            }
        }
    }
}