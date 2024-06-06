namespace VXI_GAMS_US.VIEWS.View
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataTableOrder[] Order { get; set; }
        public DataTableColumn[] Columns { get; set; }
        public DataTableSearch Search { get; set; }
    }
}