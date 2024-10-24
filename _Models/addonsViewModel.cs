﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdabFest_Admin._Models
{
    public class addonsViewModel
    {
    }

    public class AddonsBLL
    {
        public int AddonID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public int StatusID { get; set; }
        public Nullable<int> BrandID { get; set; }

    }

}
