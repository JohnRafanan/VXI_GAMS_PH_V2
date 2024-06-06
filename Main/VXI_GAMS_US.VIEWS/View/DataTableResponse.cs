namespace VXI_GAMS_US.VIEWS.View
{
    public class DataTableResponse
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }
        public string error { get; set; }
    }
}