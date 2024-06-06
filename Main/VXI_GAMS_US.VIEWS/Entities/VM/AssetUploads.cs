namespace VXI_GAMS_US.VIEWS.Entities.VM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssetUploads
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string SerialNo { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Manufacturer { get; set; }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public string Site { get; set; }

        public string Vendor { get; set; }

        public string PurchaseOrder { get; set; }

        public string CostValue { get; set; }

        public string Ram { get; set; }

        public string HdCapacity { get; set; }

        public string MonitorSize { get; set; }

        public string YearModel { get; set; }

        public string SM_AssetTag { get; set; }

        public string ItemDesc { get; set; }

        public string Quantity { get; set; }

        public string Currency { get; set; }

        public string UnitPrice { get; set; }

        public string TotalValue { get; set; }

        public string VendorAddress { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public string PurchaseOrderDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string InvoiceDate { get; set; }

        public string DeliveryReceiptNumber { get; set; }

        public string DeliveryReceiptDate { get; set; }

        public string ReceivedBy { get; set; }

        public string WarrantyStartDate { get; set; }

        public string WarrantyEndDate { get; set; }

        public string PezaForm8105Number { get; set; }

        public string PezaPermitNumber { get; set; }

        public string PezaApprovalDate { get; set; }

        public string ImportationPermitNumber { get; set; }

        public string ValidUntil { get; set; }

        public string BillOfLandingNumber { get; set; }

        public string TaxesPaid { get; set; }

        public string BondIssuer { get; set; }

        public string SuretyBondPolicyNumber { get; set; }

        public string ValidityPeriod { get; set; }

        public string SuretyBondOfficialReceieptAmountPaid { get; set; }

        public string Remarks { get; set; }
    }
}
