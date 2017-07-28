using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCHomework_20170703.Models.ViewModels
{
    public class QueryCustomerViewModel
    {
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
    }

    public class QueryCustomerReportViewModel
    {
        public string CustomerName { get; set; } 
    }
}