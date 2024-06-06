using System;

namespace VXI_GAMS_US.VIEWS.View
{
    public class RetrievalVM
    {
        public long? row  { get; set; }
        public string site  { get; set; }
        public string category  { get; set; }
        public string serialNo  { get; set; }
        public string hrid  { get; set; }
        public DateTime? date  { get; set; }
        public string status  { get; set; }
        public long? id  { get; set; }
        public bool? isActive  { get; set; }
        public string createdBy  { get; set; }
        public int? updatedBy  { get; set; }
        public int? dateUpdated  { get; set; }
        public DateTime? dateCreated  { get; set; }
    }

    public class RetrievalVM2
    {
        public long? row  { get; set; }
        public string site  { get; set; }
        public string qwe  { get; set; }
    }
}