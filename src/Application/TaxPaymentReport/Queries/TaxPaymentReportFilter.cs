﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.TaxPaymentReport.Queries
{
    public class TaxPaymentReportFilter
    {
        public int PropertyId { get; set; }
        public string UnitNo { get; set; }
        public int LotNo { get; set; }
    }
}
