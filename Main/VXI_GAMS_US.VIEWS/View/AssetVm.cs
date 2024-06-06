using System;
using VXI_GAMS_US.VIEWS.Entities;

namespace VXI_GAMS_US.VIEWS.View
{
    public class AssetVm
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; }
        public string Description { get; set; }
        public string SerialNo { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Vendors Vendors { get; set; }
        public AssignAssetEmployee AssetEmployee { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmployeeTitle { get; set; }
        public Status Status { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public string Site { get; set; }
        public string Ram { get; set; }
        public string HdCapacity { get; set; }
        public string MonitorSize { get; set; }
        public string YearModel { get; set; }
        public string Classification { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public class AssetVm2
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Floors { get; set; }
        public string AreaWorkStation { get; set; }
        public string SerialNo { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Status { get; set; }
        public string Manufacturer { get; set; }
        public string Site { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmployeeTitle { get; set; }
        public string Hrid { get; set; }
        public string EmployeeName { get; set; }
        public string WorkType { get; set; }
        public string Floor { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string ContactNo { get; set; }
        public string Vendor { get; set; }
        public string PurchaseOrder { get; set; }
        public string CostValue { get; set; }
        public string Email { get; set; }
        public string TrackingNumber { get; set; }
        public string ItTicketNumber { get; set; }
        public string Ram { get; set; }
        public string HdCapacity { get; set; }
        public string MonitorSize { get; set; }
        public string YearModel { get; set; }
        public string Classification { get; set; }
        public string DeployedDate { get; set; }
        public string RetrievedDate { get; set; }
        public string StatusCode { get; set; }
        public string TrackingNo { get; set; }
        public string TicketNo { get; set; }
        public DateTime DateCreated { get; set; }
        public string ItemDescription { get; set; }
        public string Quantity { get; set; }
        public string Currency { get; set; }
        public string UnitPrice { get; set; }
        public string TotalValue { get; set; }
        public string VendorAddress { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryReceiptNumber { get; set; }
        public string DeliveryReceivedDate { get; set; }
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
        public string SuretyBondOfficialReceiept { get; set; }
        public string AmountPaid { get; set; }
        public string Remarks { get; set; }
        
    }

    public class TrackTicketVm
    {
        //public string TrackingNumber { get; set; }
        //public string ItTicketNumber { get; set; }
        public string TrackingNo { get; set; }
        public string TicketNo { get; set; }
    }

    public class TransferVm : TrackTicketVm
    {
        public string Site { get; set; }
    }
}