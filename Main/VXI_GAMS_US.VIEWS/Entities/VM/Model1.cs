using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace VXI_GAMS_US.VIEWS.Entities.VM
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<AssetUploads> AssetUploads { get; set; }
        public virtual DbSet<AssetUploads_FarmOut> AssetUploads_FarmOut { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
                .Property(e => e.DeliveryReceiptDate)
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
                .Property(e => e.SuretyBondOfficialReceieptAmountPaid)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.SM_AssetTag)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Transaction_Type)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.iTrack_Ticket_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Purchase_Order_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Quantity)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Originating_Site)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Destination_Site)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.LOA_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Validity)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.LOA_OR_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.LOA_Fee)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Bond_Issuer)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Bondable_Ammount)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Surety_Bond_Policy_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Validity_Period)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Surety_Bond_Official_Receiept)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Amount_Paid)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Peza_Form_8106_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Peza_Permit_Number)
                .IsUnicode(false);

            modelBuilder.Entity<AssetUploads_FarmOut>()
                .Property(e => e.Peza_Approval_Date)
                .IsUnicode(false);
        }
    }
}
