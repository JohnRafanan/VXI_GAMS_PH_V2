using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using VXI_GAMS_US.VIEWS.Entities.VM;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public partial class UpdateModel : DbContext
    {
        public UpdateModel()
            : base("name=UpdateModel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Assets> Assets { get; set; }
        public virtual DbSet<AssetStatusHistories> AssetStatusHistories { get; set; }
        public virtual DbSet<AssetStatusUploads> AssetStatusUploads { get; set; }
        public virtual DbSet<AssetUpdateHistories> AssetUpdateHistories { get; set; }
        public virtual DbSet<AssetUpdateUploads> AssetUpdateUploads { get; set; }
        public virtual DbSet<AssetUploads> AssetUploads { get; set; }
        public virtual DbSet<AssetUploadsFarmouts> AssetUploadsFarmouts { get; set; }
        public virtual DbSet<AssignAssetEmployeeHistories> AssignAssetEmployeeHistories { get; set; }
        public virtual DbSet<AssignAssetEmployees> AssignAssetEmployees { get; set; }
        public virtual DbSet<AssignAssetEmployeeUploads> AssignAssetEmployeeUploads { get; set; }
        public virtual DbSet<AssignedAssetChangeHistories> AssignedAssetChangeHistories { get; set; }
        public virtual DbSet<AssignedAssetChangeUploads> AssignedAssetChangeUploads { get; set; }
        public virtual DbSet<Bulks> Bulks { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<EmployeeUpdateHistories> EmployeeUpdateHistories { get; set; }
        public virtual DbSet<EmployeeUpdateStatus> EmployeeUpdateStatus { get; set; }
        public virtual DbSet<LocationTransferHistories> LocationTransferHistories { get; set; }
        public virtual DbSet<LocationTransferUploads> LocationTransferUploads { get; set; }
        public virtual DbSet<LoginLogs> LoginLogs { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<RemoveAssignedAssetEmployeeHistories> RemoveAssignedAssetEmployeeHistories { get; set; }
        public virtual DbSet<RemoveAssignedAssetEmployeeUploads> RemoveAssignedAssetEmployeeUploads { get; set; }
        public virtual DbSet<ReplaceHistories> ReplaceHistories { get; set; }
        public virtual DbSet<ReplaceUploads> ReplaceUploads { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<SubCategories> SubCategories { get; set; }
        public virtual DbSet<TemplateHistories> TemplateHistories { get; set; }
        public virtual DbSet<UserUploadHistories> UserUploadHistories { get; set; }
        public virtual DbSet<tableTemp> tableTemp { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Assets>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.SerialNo)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.Site)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.EmployeeStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.EmployeeTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.Vendor)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.PurchaseOrder)
                .IsUnicode(false);

            modelBuilder.Entity<Assets>()
                .Property(e => e.CostValue)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusHistories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusHistories>()
                .Property(e => e.From)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusHistories>()
                .Property(e => e.To)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusUploads>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusUploads>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<AssetStatusUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateHistories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateHistories>()
                .Property(e => e.Column)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateHistories>()
                .Property(e => e.From)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateHistories>()
                .Property(e => e.To)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateUploads>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateUploads>()
                .Property(e => e.Column)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateUploads>()
                .Property(e => e.From)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateUploads>()
                .Property(e => e.To)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUpdateUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.SerialNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.SubCategory)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Manufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Site)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Vendor)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.PurchaseOrder)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.CostValue)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Ram)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.HdCapacity)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.MonitorSize)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.YearModel)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.SM_AssetTag)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.ItemDesc)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Quantity)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Currency)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.UnitPrice)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.TotalValue)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.VendorAddress)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.PurchaseOrderNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.PurchaseOrderDate)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.InvoiceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.InvoiceDate)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.DeliveryReceiptNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.DeliveryReceivedDate)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.ReceivedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.WarrantyStartDate)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.WarrantyEndDate)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.PezaForm8105Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.PezaPermitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.PezaApprovalDate)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.ImportationPermitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.ValidUntil)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.BillOfLandingNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.TaxesPaid)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.BondIssuer)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.SuretyBondPolicyNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.ValidityPeriod)
                .IsUnicode(false);
            
            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.SuretyBondOfficialReceiept)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.AmountPaid)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.SM_AssetTag)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Transaction_Type)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.iTrack_Ticket_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Purchase_Order_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Quantity)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Originating_Site)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Destination_Site)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.LOA_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Validity)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.LOA_OR_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.LOA_Fee)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Bond_Issuer)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Bondable_Ammount)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Surety_Bond_Policy_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Validity_Period)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Surety_Bond_Official_Receiept)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Amount_Paid)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Peza_Form_8106_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Peza_Permit_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploadsFarmouts>()
                .Property(e => e.Peza_Approval_Date)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.WorkType)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.Hrid)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.Floor)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.Area)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeHistories>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.WorkType)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.Hrid)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.Floor)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.Area)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployees>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.WorkType)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.Hrid)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.Floor)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.Area)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignAssetEmployeeUploads>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeHistories>()
                .Property(e => e.CodeFrom)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeHistories>()
                .Property(e => e.CodeTo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeHistories>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeHistories>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeHistories>()
                .Property(e => e.ReturnTrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeUploads>()
                .Property(e => e.FromCode)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeUploads>()
                .Property(e => e.ToCode)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeUploads>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeUploads>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AssignedAssetChangeUploads>()
                .Property(e => e.ReturnTrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.HireDate)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.TeamID)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.LocationDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.TitleName)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.SupervisorID)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Project)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.JobLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.PositionLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.Ranking)
                .IsUnicode(false);

            modelBuilder.Entity<Bulks>()
                .Property(e => e.ProjectID)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferHistories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferHistories>()
                .Property(e => e.From)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferHistories>()
                .Property(e => e.To)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferHistories>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferHistories>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferUploads>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferUploads>()
                .Property(e => e.Site)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferUploads>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<LocationTransferUploads>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<LoginLogs>()
                .Property(e => e.User)
                .IsUnicode(false);

            modelBuilder.Entity<Manufacturers>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Manufacturers>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Manufacturers>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Manufacturers>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeHistories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeHistories>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeHistories>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeUploads>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeUploads>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<RemoveAssignedAssetEmployeeUploads>()
                .Property(e => e.TicketNo)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.FromSerial)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.ToSerial)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.FromManufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.ToManufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.FromVendor)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.ToVendor)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.FromPurchaseOrder)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.ToPurchaseOrder)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.FromCostValue)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceHistories>()
                .Property(e => e.ToCostValue)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.Serial)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.Manufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.Vendor)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.PurchaseOrder)
                .IsUnicode(false);

            modelBuilder.Entity<ReplaceUploads>()
                .Property(e => e.CostValue)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SubCategories>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<SubCategories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SubCategories>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateHistories>()
                .Property(e => e.UploadedTemplateFileDir)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateHistories>()
                .Property(e => e.TemplateBackupFileDir)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateHistories>()
                .Property(e => e.UploadedBy)
                .IsUnicode(false);

            modelBuilder.Entity<UserUploadHistories>()
                .Property(e => e.UploadedTemplateFileDir)
                .IsUnicode(false);

            modelBuilder.Entity<UserUploadHistories>()
                .Property(e => e.UploadedBy)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.HireDate)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.TeamID)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.LocationDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.TitleName)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.SupervisorID)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Project)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.JobLevel)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.PositionLevel)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.Ranking)
                .IsUnicode(false);

            modelBuilder.Entity<tableTemp>()
                .Property(e => e.ProjectID)
                .IsUnicode(false);
        }
    }
}
