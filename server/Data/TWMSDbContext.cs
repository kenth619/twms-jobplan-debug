using Microsoft.EntityFrameworkCore;
using TWMSServer.Model;

namespace TWMSServer.Data
{
    public class TWMSDbContext : DbContext
    {
        public TWMSDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Asset> tbl_Assets { get; set; }
        public DbSet<AssetStatus> tbl_Assets_Statuses { get; set; }
        public DbSet<AssetManufacturer> tbl_Assets_Manufacturers { get; set; }
        public DbSet<Department> tbl_Departments { get; set; }
        public DbSet<Substation> tbl_Substations { get; set; }
        public DbSet<Feeder> tbl_Feeders { get; set; }
        public DbSet<Ring> tbl_Rings { get; set; }
        public DbSet<Zone> tbl_Zones { get; set; }
        public DbSet<SegmentType> tbl_Segment_Types { get; set; }
        public DbSet<SegmentPoint> tbl_Segment_Points { get; set; }
        public DbSet<TxCircuit> tbl_Tx_Circuits { get; set; }
        public DbSet<AssetClass> tbl_Assets_Classes { get; set; }
        public DbSet<AssetFailure> tbl_Assets_Failures { get; set; }
        public DbSet<AssetAttribute> tbl_Assets_Attributes { get; set; }
        public DbSet<AssetClassAttrMap> tbl_Assets_Class_Attr_Maps { get; set; }
        public DbSet<AssetAttrValue> tbl_Asset_Attr_Values { get; set; }
        public DbSet<JobPlanHeader> tbl_Job_Plans_Headers { get; set; }
        public DbSet<JobPlanLine> tbl_Job_Plans_Lines { get; set; }
        public DbSet<JobPlanStatus> tbl_Job_Plans_Statuses { get; set; }
    }
}
