using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VXI_GAMS_US.VIEWS.Entities;

namespace VXI_GAMS_US.DATA.Data
{
    public class DataContext : DbContext
    {

        public DataContext() : base("DefaultConnection")
        {
            Database.CommandTimeout = 600;
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>().Configure(c => c.IsUnicode(false));
        }

        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<EmployeeUpdateStatus> EmployeeUpdateStatus { get; set; }
        public virtual DbSet<EmployeeUpdateHistory> EmployeeUpdateHistory { get; set; }
        public virtual DbSet<Bulk> BulkApiEmployee { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Assets> Assets { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Vendors> Vendors { get; set; }
        public virtual DbSet<AssetUploads> AssetUploads { get; set; }
        public virtual DbSet<AssetUploadsFarmouts> AssetUploadsFarmouts { get; set; }
        public virtual DbSet<AssetUploadsAssetDetails> AssetUploadsAssetDetails { get; set; }
        public virtual DbSet<AssetUploadsAssetAssignments> AssetUploadsAssetAssignments { get; set; }
        public virtual DbSet<AssetUploadsAssetUpdates> AssetUploadsAssetUpdates { get; set; }
        public virtual DbSet<AssetUploadsAssetReplacements> AssetUploadsAssetReplacements { get; set; }
        public virtual DbSet<LocationTransferHistory> LocationTransferHistory { get; set; }
        public virtual DbSet<LocationTransferUploads> LocationTransferUploads { get; set; }
        public virtual DbSet<AssetStatusHistory> AssetStatusHistory { get; set; }
        public virtual DbSet<AssetStatusUploads> AssetStatusUploads { get; set; }
        public virtual DbSet<ReplaceHistory> ReplaceHistory { get; set; }
        public virtual DbSet<ReplaceUploads> ReplaceUploads { get; set; }
        public virtual DbSet<AssignAssetEmployeeHistory> AssignAssetEmployeeHistory { get; set; }
        public virtual DbSet<AssignAssetEmployee> AssignAssetEmployee { get; set; }
        public virtual DbSet<AssignAssetEmployeeUploads> AssignAssetEmployeeUploads { get; set; }
        public virtual DbSet<RemoveAssignedAssetEmployeeHistory> RemoveAssignedAssetEmployeeHistory { get; set; }
        public virtual DbSet<RemoveAssignedAssetEmployeeUploads> RemoveAssignedAssetEmployeeUploads { get; set; }
        public virtual DbSet<TemplateHistory> TemplateHistory { get; set; }
        public virtual DbSet<UserUploadHistory> UserUploadHistory { get; set; }
        public virtual DbSet<AssignedAssetChangeHistory> AssignedAssetChangeHistory { get; set; }
        public virtual DbSet<AssignedAssetChangeUploads> AssignedAssetChangeUploads { get; set; }
        public virtual DbSet<AssetUpdateHistory> AssetUpdateHistory { get; set; }
        public virtual DbSet<AssetUpdateUploads> AssetUpdateUploads { get; set; }
    }
}
