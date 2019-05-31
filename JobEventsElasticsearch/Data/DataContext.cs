using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JobEventsElasticsearch.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmployerProfileDb39> EmployerProfileDb39 { get; set; }
        public virtual DbSet<JobDb39> JobDb39 { get; set; }
        public virtual DbSet<ZStatJobEvent> ZStatJobEvent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=jobcentre_production;Integrated Security=SSPI;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<EmployerProfileDb39>(entity =>
            {
                entity.HasKey(e => e.EmployerId)
                    .HasName("PK_EmployerProfile");

                entity.ToTable("EmployerProfileDB39");

                entity.HasIndex(e => e.Deleted);

                entity.HasIndex(e => e.Suspended);

                entity.Property(e => e.EmployerId).HasColumnName("EmployerID");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<JobDb39>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK_Job");

                entity.ToTable("JobDB39");

                entity.HasIndex(e => e.EmployerId);

                entity.HasIndex(e => e.StatusId);


                entity.HasIndex(e => new { e.EmployerId, e.StatusId })
                    .HasName("IX_JobDB39_EmployerId_StatusId");

                entity.HasIndex(e => new { e.JobId, e.StatusId, e.EmployerId })
                    .HasName("IX_JobDB39_StatusId_EmployerID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.EmployerId).HasColumnName("EmployerID");

                entity.Property(e => e.JobTitle).HasMaxLength(256);

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.JobDb39)
                    .HasForeignKey(d => d.EmployerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobDB39_EmployerProfileDB39");
            });

            modelBuilder.Entity<ZStatJobEvent>(entity =>
            {
                entity.HasKey(e => e.StatId)
                    .HasName("PK_Stat_JobModification");

                entity.ToTable("z_Stat_JobEvent");

                entity.HasIndex(e => new { e.EmployerId, e.StatDate, e.StatTypeId })
                    .HasName("IX_Stat_JobEvent_EmployerId_StatDate_StatTypeID");

                entity.HasIndex(e => new { e.JobId, e.StatDate, e.StatTypeId })
                    .HasName("IX_Stat_JobEvent_JobId_StatDate_StatTypeID");

                entity.HasIndex(e => new { e.JobId, e.StatTypeId, e.StatDate })
                    .HasName("IX_Stat_JobEvent_StatTypeID_StatDate");

                entity.Property(e => e.StatId).HasColumnName("StatID");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.Campaign).HasMaxLength(100);

                entity.Property(e => e.EmployerId).HasColumnName("EmployerID");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.JobseekerId).HasColumnName("JobseekerID");

                entity.Property(e => e.Medium).HasMaxLength(100);

                entity.Property(e => e.PositionTypeId).HasColumnName("PositionTypeID");

                entity.Property(e => e.Source2).HasMaxLength(100);

                entity.Property(e => e.StatDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StatTypeId).HasColumnName("StatTypeID");
            });
        }
    }
}
