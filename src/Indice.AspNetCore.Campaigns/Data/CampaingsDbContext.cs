﻿using Indice.AspNetCore.Features.Campaigns.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Indice.AspNetCore.Features.Campaigns.Data
{
    public class CampaingsDbContext : DbContext
    {
        public CampaingsDbContext(DbContextOptions<CampaingsDbContext> options) : base(options) { }

        public DbSet<DbCampaign> Campaigns { get; set; }
        public DbSet<DbCampaignAttachment> CampaignAttachments { get; set; }
        public DbSet<DbCampaignUser> CampaignUsers { get; set; }
        public DbSet<DbCampaignVisit> CampaignVisits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) => builder.ApplyConfigurationsFromAssembly(typeof(DbCampaign).Assembly);
    }
}