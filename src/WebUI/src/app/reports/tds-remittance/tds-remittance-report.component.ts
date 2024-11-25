import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormGroupDirective,FormControl } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { fuseAnimations } from '@fuse/animations';
import * as Xlsx from 'xlsx';
import { PropertyService } from '../../property/property.service';
import { ToastrService } from 'ngx-toastr';
import * as _ from 'lodash';
import { TdsRemittanceReportService } from '../tds-remittance/tds-remittance-report.service';
import { SellerService } from '../../seller/seller.service';
import * as moment from 'moment';
import * as fileSaver from 'file-saver';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'tds-remittance-report',
  templateUrl: './tds-remittance-report.component.html',
  styleUrls: ['./tds-remittance-report.component.scss'],
  animations: fuseAnimations
})
export class TdsRemittanceReportComponent implements OnInit, OnDestroy {
  reportform: FormGroup;

  reportRowData: any[] = [];
  reportColumnDef: any[] = [];
  premisesDDl: any[] = [];
  unitNoDDl: any[] = [];
  sellerDDl: any[] = [];
  activeProperty: any[] = [];
   //Property Filter
   public propertyFilterCtrl: FormControl = new FormControl();
   @ViewChild('PropertyFilterSelect', { static: true }) PropertyFilterSelect: MatSelect;
   /** Subject that emits when the component has been destroyed. */
   protected _onDestroy = new Subject<void>();
   public filteredProperty: ReplaySubject<any[]> = new ReplaySubject<any[]>();
  constructor(private _formBuilder: FormBuilder, private sellerReportSvc: TdsRemittanceReportService, private sellerSvc: SellerService, private propertySvc: PropertyService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reportform = this._formBuilder.group({
      lotNo: [''],
      premisesId: [''],
      unitNo: [''],
      customerName: [''],
      sellerId: ['']
    });

    this.reportColumnDef = [
      { 'header': 'Lot', 'field': 'lotNo', 'type': 'label', 'width': 50 },
      { 'header': 'Client name', 'field': 'customerName', 'type': 'label','width':220 },
      { 'header': 'Premises', 'field': 'premises', 'type': 'label', 'width': 220},     
      { 'header': 'Unit No', 'field': 'unitNo', 'type': 'label', 'width': 80 },
      { 'header': 'Challan Date', 'field': 'challandate', 'type': 'label','width':120 },
      { 'header': 'Challan Sl. No', 'field': 'challanID', 'type': 'label', 'width': 130},
      { 'header': 'TDS Ack No', 'field': 'challanAckNo', 'type': 'label', 'width': 100 },
      { 'header': 'Challan Amount', 'field': 'challanAmount', 'type': 'label', 'width': 130 },
      { 'header': 'Date of Request', 'field': 'requestDate', 'type': 'label', 'width': 140},
      { 'header': 'Request No', 'field': 'f16BRequestNo', 'type': 'label', 'width': 100 }, 
      { 'header': 'TDS Certificate Date', 'field': 'tdsCertificateDate', 'type': 'label', 'width': 180 },
      { 'header': 'TDS Certificate No', 'field': 'f16BCertificateNo', 'type': 'label', 'width': 160},
      { 'header': 'Amount', 'field': 'f16CreditedAmount', 'type': 'label', 'width': 80 },
      { 'header': 'Date Of Payment', 'field': 'paymentDate', 'type': 'label', 'width': 160 },
      { 'header': 'Amount Paid', 'field': 'amountPaid', 'type': 'label', 'width': 80 },
      { 'header': 'Gross Amount', 'field': 'grossAmount', 'type': 'label', 'width': 80 },
      { 'header': 'GST', 'field': 'gst', 'type': 'label', 'width': 80 },
      { 'header': 'TDS %', 'field': 'tdsRate', 'type': 'label', 'width': 80 }

    ];

    this.getProperties();   
    this.getSellers();

    this.propertyFilterCtrl.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterProperty();
    });
  }

  getProperties() {
    this.propertySvc.getProperties().subscribe(response => {
      this.premisesDDl = response;
      this.premisesDDl.splice(0, 0, { 'propertyID': '', 'addressPremises': '' });
      this.activeProperty = _.filter(response, o => { return o.isActive == null || o.isActive == true; });
    });
  }

  getSellers() {
    this.sellerSvc.getSellers().subscribe(response => {
      this.sellerDDl = response;
      this.sellerDDl.splice(0, 0, { 'sellerID': '', 'sellerName': '' });
    });
  }

  getReportList() {
    var filters = this.reportform.value;

    this.sellerReportSvc.getReportList(filters.customerName, filters.premisesId, filters.sellerId, filters.unitNo, filters.lotNo).subscribe(response => {
      _.forEach(response, obj => {
        obj.requestDate = obj.f16BDateOfReq == null ? "" : moment(obj.f16BDateOfReq).local().format("DD-MMM-YYYY");
        obj.challandate = obj.challanDate == null ? "" : moment(obj.challanDate).local().format("DD-MMM-YYYY");
        obj.tdsCertificateDate = obj.f16UpdateDate == null ? "" : moment(obj.f16UpdateDate).local().format("DD-MMM-YYYY");
        obj.paymentDate = obj.dateOfPayment == null ? "" : moment(obj.dateOfPayment).local().format("DD-MMM-YYYY");
      });
      this.reportRowData = response;
    });
  }

  /**
     * On destroy
     */
  ngOnDestroy(): void {
  }

  downloadExcel() {
    var filters = this.reportform.value;

    this.sellerReportSvc.downloadtoExcel(filters.customerName, filters.premisesId, filters.sellerId, filters.unitNo, filters.lotNo).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms-excel' });
      fileSaver.saveAs(blob, 'TdsRemittanceReport.xls');
    });
  }

  search() {
    this.getReportList();
  }

  reset() {
    this.reportform.reset();
  }

     //property Filter functionality for active property
     protected filterProperty() {
      if (!this.activeProperty) {
        return;
      }
      // get the search keyword
      let search = this.propertyFilterCtrl.value;
      if (!search) {
        this.filteredProperty.next(this.activeProperty.slice());
        return;
      } else {
        search = search.toLowerCase();
      }
      // filter the banks
      this.filteredProperty.next(this.filterProFun(search));
    }
    filterProFun(search) {
      var list = this.activeProperty.filter(prop => prop.addressPremises.toLowerCase().indexOf(search) > -1);
      return list;
    }
}
