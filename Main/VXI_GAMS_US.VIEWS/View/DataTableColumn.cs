﻿namespace VXI_GAMS_US.VIEWS.View
{
    public class DataTableColumn
    { 
        public string Data { get; set; } 
        public string Name { get; set; } 
        public bool Searchable { get; set; } 
        public bool Orderable { get; set; } 
        public DataTableSearch Search { get; set; }
    }
}