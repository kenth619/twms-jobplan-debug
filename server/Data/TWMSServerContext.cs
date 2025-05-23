using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TWMSServer.Model;
using TWMSServer.Model.Enum;

namespace TWMSServer
{
    public class TWMSServerContext(DbContextOptions<TWMSServerContext> options) : DbContext(options)
    {
        // Existing DbSets
        public DbSet<EmailData> Emails { get; set; }
        public DbSet<SystemRoleAssignment> SystemRoleAssignments { get; set; }
        public DbSet<DepartmentRoleAssignment> DepartmentRoleAssignments { get; set; }
        public DbSet<JobSchedule> JobSchedules { get; set; }
        public DbSet<WorkOrder> tblWorkOrders { get; set; }
        public DbSet<Asset> tblAssets { get; set; }

        // New DbSets Added for Assets and Job Plans
        public DbSet<AssetStatus> tblAssets_Statuses { get; set; }
        public DbSet<AssetManufacturer> tblAssets_Manufacturers { get; set; }

        // public DbSet<Department> tbl_Departments { get; set; }
        public DbSet<Substation> tblSubstations { get; set; }
        public DbSet<Feeder> tblFeeders { get; set; }
        public DbSet<Ring> tblRings { get; set; }
        public DbSet<Zone> tblZones { get; set; }
        public DbSet<SegmentType> tblSegment_Types { get; set; }
        public DbSet<SegmentPoint> tblSegment_Points { get; set; }
        public DbSet<TxCircuit> tblTx_Circuits { get; set; }
        public DbSet<AssetClass> tblAssets_Classes { get; set; }
        public DbSet<AssetFailure> tblAssets_Failures { get; set; }
        public DbSet<AssetAttribute> tblAssets_Attributes { get; set; }
        public DbSet<AssetClassAttrMap> tblAssets_Class_Attr_Maps { get; set; }
        // public DbSet<AssetAttrValue> tbl_Asset_Attr_Values { get; set; }
        public DbSet<JobPlansHeader> tblJob_Plans_Headers { get; set; }
        public DbSet<JobPlansLine> tblJob_Plans_Lines { get; set; }
        public DbSet<JobPlanStatus> tblJob_Plans_Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemRoleAssignment>().Property(r => r.Role).HasConversion(new SystemRoleConverter());
            modelBuilder.Entity<DepartmentRoleAssignment>().Property(r => r.Role).HasConversion(new DepartmentRoleConverter());

            #region Email Converters
            var comp = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<HashSet<string>>(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet()
            );

            modelBuilder.Entity<EmailData>()
                .Property(e => e.ToRecipients)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<HashSet<string>>(v, (JsonSerializerOptions?)null) ?? new HashSet<string>(),
                    comp
                );

            modelBuilder.Entity<EmailData>()
                .Property(e => e.CCRecipients)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<HashSet<string>>(v, (JsonSerializerOptions?)null) ?? new HashSet<string>(),
                    comp
                );

            modelBuilder.Entity<EmailData>()
                .Property(e => e.BCCRecipients)
                .HasConversion(
                    static v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<HashSet<string>>(v, (JsonSerializerOptions?)null) ?? new HashSet<string>(),
                    comp
                );
            #endregion
        }
    }
}
