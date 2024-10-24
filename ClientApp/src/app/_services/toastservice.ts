// import { ToastrService } from 'ngx-toastr';
import { ToastrService, ActiveToast } from 'ngx-toastr';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
 export class ToastService {

  constructor(public toastr: ToastrService) { }

  public showSuccess(_title, _message) {
    this.toastr.success(_title, _message, {
      timeOut: 5000,
    });
  }
  public showError(_title, _message) {
    this.toastr.error(_title, _message, {
      timeOut: 5000,
    });
  }
  public showWarning(_title, _message) {
    this.toastr.warning(_title, _message, {
      timeOut: 5000,
    });
  }
  public showToast(_title, _message) {
    this.toastr.warning(_title, _message, { disableTimeOut: true, closeButton:true });
  }
  
}
