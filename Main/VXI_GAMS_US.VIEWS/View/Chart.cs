using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming

namespace VXI_GAMS_US.VIEWS.View
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class Chart
    {
        public IList<categoriescategory> categories { get; set; }
        public IList<seriesdata> dataset { get; set; }


        public class categoriescategory
        {
            public IList<categorylabel> category { get; set; }

            public class categorylabel
            {
                public string label { get; set; }
            }

        }

        public class seriesdata
        {
            public string seriesname { get; set; }
            public IList<datavalue> data { get; set; }

            public class datavalue
            {
                public int value { get; set; }
            }

        }

        public class LabelValue
        {
            public string label { get; set; }

            public string value { get; set; }
        }

    }
}