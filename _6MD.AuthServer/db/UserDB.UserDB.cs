﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 06.03.2025 17:32:34
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace _6MD.AuthServer.DB
{

    public partial class UserDB : DbContext
    {

        public UserDB() :
            base()
        {
            OnCreated();
        }

        public UserDB(DbContextOptions<UserDB> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<Groups> Groups
        {
            get;
            set;
        }

        public virtual DbSet<UserPremission> UserPremissions
        {
            get;
            set;
        }

        public virtual DbSet<OAuthAccount> OAuthAccounts
        {
            get;
            set;
        }

        public virtual DbSet<Ranks> Ranks
        {
            get;
            set;
        }

        public virtual DbSet<GroupPremission> GroupPremissions
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.UserMapping(modelBuilder);
            this.CustomizeUserMapping(modelBuilder);

            this.GroupsMapping(modelBuilder);
            this.CustomizeGroupsMapping(modelBuilder);

            this.UserPremissionMapping(modelBuilder);
            this.CustomizeUserPremissionMapping(modelBuilder);

            this.OAuthAccountMapping(modelBuilder);
            this.CustomizeOAuthAccountMapping(modelBuilder);

            this.RanksMapping(modelBuilder);
            this.CustomizeRanksMapping(modelBuilder);

            this.GroupPremissionMapping(modelBuilder);
            this.CustomizeGroupPremissionMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region User Mapping

        private void UserMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(@"Users");
            modelBuilder.Entity<User>().Property(x => x.Guid).HasColumnName(@"Guid").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName(@"Email").ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName(@"Password").ValueGeneratedNever();
            modelBuilder.Entity<User>().Property<Guid?>(@"RanksGuid").HasColumnName(@"RanksGuid").ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.Retired).HasColumnName(@"Retired").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<User>().HasKey(@"Guid");
        }

        partial void CustomizeUserMapping(ModelBuilder modelBuilder);

        #endregion

        #region Groups Mapping

        private void GroupsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Groups>().ToTable(@"Groups");
            modelBuilder.Entity<Groups>().Property(x => x.Guid).HasColumnName(@"Guid").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Groups>().Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Groups>().Property(x => x.Color).HasColumnName(@"Color").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Groups>().HasKey(@"Guid");
        }

        partial void CustomizeGroupsMapping(ModelBuilder modelBuilder);

        #endregion

        #region UserPremission Mapping

        private void UserPremissionMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPremission>().ToTable(@"UserPremissions");
            modelBuilder.Entity<UserPremission>().Property(x => x.Guid).HasColumnName(@"Guid").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserPremission>().Property(x => x.Key).HasColumnName(@"Key").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserPremission>().Property(x => x.Power).HasColumnName(@"Power").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserPremission>().Property<Guid>(@"UserGuid").HasColumnName(@"UserGuid").ValueGeneratedNever();
            modelBuilder.Entity<UserPremission>().HasKey(@"Guid");
        }

        partial void CustomizeUserPremissionMapping(ModelBuilder modelBuilder);

        #endregion

        #region OAuthAccount Mapping

        private void OAuthAccountMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OAuthAccount>().ToTable(@"OAuthAccounts");
            modelBuilder.Entity<OAuthAccount>().Property(x => x.Guid).HasColumnName(@"Guid").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.Provider).HasColumnName(@"Provider").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.provider_id).HasColumnName(@"provider_id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.access_token).HasColumnName(@"access_token").ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.refresh_token).HasColumnName(@"refresh_token").ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.expires_at).HasColumnName(@"expires_at").ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.CreatedAt).HasColumnName(@"CreatedAt").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<OAuthAccount>().Property(x => x.UpdatedAt).HasColumnName(@"UpdatedAt").IsRequired().ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<OAuthAccount>().Property<Guid>(@"UserGuid").HasColumnName(@"UserGuid").ValueGeneratedNever();
            modelBuilder.Entity<OAuthAccount>().HasKey(@"Guid");
        }

        partial void CustomizeOAuthAccountMapping(ModelBuilder modelBuilder);

        #endregion

        #region Ranks Mapping

        private void RanksMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ranks>().ToTable(@"Ranks");
            modelBuilder.Entity<Ranks>().Property(x => x.Guid).HasColumnName(@"Guid").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Ranks>().Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Ranks>().Property(x => x.ImagePath).HasColumnName(@"ImagePath").ValueGeneratedNever();
            modelBuilder.Entity<Ranks>().Property(x => x.Color).HasColumnName(@"Color").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Ranks>().Property(x => x.Prefix).HasColumnName(@"Prefix").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Ranks>().HasKey(@"Guid");
        }

        partial void CustomizeRanksMapping(ModelBuilder modelBuilder);

        #endregion

        #region GroupPremission Mapping

        private void GroupPremissionMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupPremission>().ToTable(@"GroupPremissions");
            modelBuilder.Entity<GroupPremission>().Property(x => x.Guid).HasColumnName(@"Guid").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<GroupPremission>().Property(x => x.Key).HasColumnName(@"Key").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<GroupPremission>().Property(x => x.Power).HasColumnName(@"Power").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<GroupPremission>().Property<Guid>(@"GroupGuid").HasColumnName(@"GroupGuid").ValueGeneratedNever();
            modelBuilder.Entity<GroupPremission>().HasKey(@"Guid");
        }

        partial void CustomizeGroupPremissionMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.Premissions).WithOne(op => op.User).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserGuid").IsRequired(true);
            modelBuilder.Entity<User>().HasMany(x => x.OAuthAccounts).WithOne(op => op.User).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserGuid").IsRequired(true);
            modelBuilder.Entity<User>().HasMany(x => x.Groups).WithMany(op => op.Users);
            modelBuilder.Entity<User>().HasOne(x => x.Ranks).WithMany(op => op.Users).OnDelete(DeleteBehavior.SetNull).HasForeignKey(@"RanksGuid").IsRequired(false);

            modelBuilder.Entity<Groups>().HasMany(x => x.Premissions).WithOne(op => op.Groups).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"GroupGuid").IsRequired(true);

            modelBuilder.Entity<UserPremission>().HasOne(x => x.User).WithMany(op => op.Premissions).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserGuid").IsRequired(true);

            modelBuilder.Entity<OAuthAccount>().HasOne(x => x.User).WithMany(op => op.OAuthAccounts).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserGuid").IsRequired(true);

            modelBuilder.Entity<Ranks>().HasMany(x => x.Users).WithOne(op => op.Ranks).OnDelete(DeleteBehavior.SetNull).HasForeignKey(@"RanksGuid").IsRequired(false);

            modelBuilder.Entity<GroupPremission>().HasOne(x => x.Groups).WithMany(op => op.Premissions).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"GroupGuid").IsRequired(true);
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
