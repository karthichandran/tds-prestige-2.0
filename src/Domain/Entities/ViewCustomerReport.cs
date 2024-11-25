﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    [Table("ViewCustomerReport")]
    public class ViewCustomerReport
    {
        [Key]
        public int CustomerPropertyID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string PAN { get; set; }
        public string PropertyPremises { get; set; }
        public int PropertyID { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public string UnitNo { get; set; }
        public string Remarks { get; set; }
        public int StatusTypeID { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal? TotalUnitCost { get; set; }
        public DateTime? DateOfAgreement { get; set; }
        public Guid OwnershipID { get; set; }
        public string UnitStatus { get; set; }
        public decimal? StampDuty { get; set; }
        public string TracesPassword { get; set; }
        public string CustomerAlias { get; set; }
        public string IsPanVerified { get; set; }

        public string CustomerStatus { get; set; }
        public DateTime? CustomerOptingOutDate { get; set; }
        public string CustomerOptingOutRemarks { get; set; }
        public DateTime? InvalidPanDate { get; set; }
        public string InvalidPanRemarks { get; set; }
        public string IncomeTaxPassword { get; set; }
        public DateTime? ITpwdMailStatus { get; set; }
        public DateTime? CoOwnerITpwdMailStatus { get; set; }

    }
}
