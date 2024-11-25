﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.SellerComplianceReport;
using ReProServices.Application.SellerComplianceReport.Queries;
using WeihanLi.Npoi;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Authorize(Roles ="SellerComplianceReport_View")]
    public class SellerComplianceReportController : ApiController
    {
        [HttpGet()]
        public async Task<IList<SellerComplianceDto>> Get([FromQuery] SellerComplianceReportFilter statusReportFilter)
        {
            return await Mediator.Send(new GetSellerComplianceReportQuery() { Filter = statusReportFilter });
        }

        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport([FromQuery] SellerComplianceReportFilter statusReportFilter)
        {
            var result = await Mediator.Send(new GetSellerComplianceReportQuery() { Filter = statusReportFilter });
            var settings = FluentSettings.For<SellerComplianceDto>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
            settings.Property(x => x.SellerName)
               .HasColumnTitle("Seller Name")
               .HasColumnIndex(0);

            settings.Property(x => x.PropertyCode)
                .HasColumnTitle("Project Code")
                .HasColumnWidth(36)
                .HasColumnIndex(1);

            settings.Property(x => x.Premises)
            .HasColumnTitle("Project Name")
            .HasColumnWidth(36)
            .HasColumnIndex(2);

            settings.Property(x => x.CustomerNo)
                .HasColumnTitle("Customer ID")
                .HasColumnIndex(3);

            settings.Property(x => x.CustomerName)
               .HasColumnTitle("Client Name")
               .HasColumnIndex(4);
            settings.Property(x => x.Material)
                .HasColumnTitle("Material")
                .HasColumnWidth(20)
                .HasColumnIndex(5);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnWidth(14)
                .HasColumnIndex(6);
            settings.Property(x => x.TransactionId)
                .HasColumnTitle("Transaction ID")
                .HasColumnWidth(14)
                .HasColumnIndex(7);

            settings.Property(x => x.TdsCertificateDate)
                .HasColumnTitle("TDS Certificate Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(8);

            settings.Property(x => x.TdsCertificateNo)
               .HasColumnTitle("TDS Certificate No")
               .HasColumnWidth(21)
               .HasColumnIndex(9);

            settings.Property(x => x.Amount)
              .HasColumnTitle("Amount")
              .HasColumnWidth(21)
              .HasColumnIndex(10);

            settings.Property(x => x.Form16BFileName)
             .HasColumnTitle("Form 16B File Name")
             .HasColumnWidth(150)
             .HasColumnIndex(11);

            settings.Property(x => x.LotNo)
                .HasColumnTitle("Lot No")
                .HasColumnWidth(15)
                .HasColumnIndex(12);
            settings.Property(x => x.SellerID)
                .HasColumnTitle("Seller ID")
                .HasColumnWidth(15)
                .HasColumnIndex(13);
            settings.Property(x => x.AssessmentYear)
                .HasColumnTitle("Assessment Year")
                .HasColumnWidth(15)
                .HasColumnIndex(14);
            settings.Property(x => x.TaxDepositDate)
                .HasColumnTitle("Tax Deposit Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(15);

            settings.Property(_ => _.PropertyID).Ignored();
           

            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SellerComplianceReport.xls");

        }

    }
}
