import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { NgbdSortableHeader, SortEvent } from 'src/app/_directives/sortable.directive';

import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/_services/toastservice';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CorporateClient } from 'src/app/_models/CorporateClient';
import { CorporateClientService } from 'src/app/_services/corporateclient.service';

@Component({
  selector: 'app-CorporateClient',
  templateUrl: './corporateclient.component.html',
  providers: []
})

export class CorporateClientComponent implements OnInit {
  data$: Observable<CorporateClient[]>;  
  oldData: CorporateClient[];
  total$: Observable<number>;
  loading$: Observable<boolean>;
  private selectedBrand;
  
  locationSubscription: Subscription;
  submit: boolean;
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
    closeResult: string;

  constructor(public service: CorporateClientService,
    public ls :LocalStorageService,
    public ts :ToastService,
    public router: Router,
    private modalService: NgbModal) {
/*     this.selectedBrand =this.ls.getSelectedBrand().brandID;*/

    this.loading$ = service.loading$;
    this.submit = false;
    
  }

  ngOnInit() {
    this.getData();
  }

  getData() {    
    this.service.getAllData();    
    this.data$ = this.service.data$;
    this.total$ = this.service.total$;
    this.loading$ = this.service.loading$;
  }

  onSort({ column, direction }: SortEvent) {

    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });
    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }

  Edit(corporateclient) {
        this.router.navigate(["admin/settings/corporateclient/edit", corporateclient]);
  }

  Delete(obj) {
    this.service.delete(obj).subscribe((res: any) => {
      if(res!=0){
        this.ts.showSuccess("Success","Record deleted successfully.")
        this.getData();
      }
      else
      this.ts.showError("Error","Failed to delete record.")

    }, error => {
      this.ts.showError("Error","Failed to delete record.")
    });
  }
  open(content, obj) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      if (result === 'yes') {
        this.Delete(obj);
      }
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
