import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ImageuploadComponent } from 'src/app/imageupload/imageupload.component';
import { Router, ActivatedRoute } from '@angular/router';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { AppsettingService } from 'src/app/_services/appsetting.service';
import { ToastService } from 'src/app/_services/toastservice';
import { Appsetting } from '../../../../_models/Appsetting';
import { HttpClient, HttpErrorResponse, HttpEventType, HttpResponse } from '@angular/common/http';
//import { ToolbarService, LinkService, ImageService, HtmlEditorService } from '@syncfusion/ej2-angular-richtexteditor';
import { AngularEditorConfig } from '@kolkov/angular-editor';

 
@Component({
  selector: 'app-Addsettings',
  templateUrl: './Addsettings.component.html',
  //providers: [ToolbarService, LinkService, ImageService, HtmlEditorService]
})
export class AddsettingsComponent implements OnInit {
  
  formData = {
    appName: '',
    appVersion: '',
    about: '',
    image: '',
    splashScreen: '',   
    facebookUrl: '',
    instagramUrl: '',
    twitterUrl: '',
    youtubeUrl: '',
    statusID: 1,
    settingID: 1,
    pdfFile: File = null
  };
  selectedFile: File = null;
  editorConfig: AngularEditorConfig = {
    editable: true,
    enableToolbar: true,
    showToolbar: true,
  };
 
 
  onFileChange(files: File[]) {
    debugger
    this.selectedFile = files[0];
    this.formData.pdfFile = '';
  }

  submitted = false;
  settingForm: FormGroup;
  loading = false;
  loadingSetting = false;
  ButtonText = "Save"; 
  selectedSubCategoriesIds: string[];
  selectedLocationIds: string[];
  selectedgroupModifierIds: string[];

  // @ViewChild(ImageuploadComponent, { static: true }) imgComp;

  @ViewChild('splashImageUpload', { static: true }) splashImageUpload: ImageuploadComponent;
  // @ViewChild('chairImageUpload', { static: true }) chairImageUpload: ImageuploadComponent;
  // @ViewChild('conferenceChairImageUpload', { static: true }) conferenceChairImageUpload: ImageuploadComponent;
  

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private ls: LocalStorageService,
    public ts: ToastService,
    private settingService: AppsettingService

  ) {
    
    
  }

  ngOnInit() {
    this.setSelectedSetting();
    
  }

 

  get f() { return this.settingForm.controls; }

  
  private editForm(obj) {
    debugger
    
    this.formData.settingID = obj.settingID;    
    this.formData.appName = obj.appName;
    this.formData.pdfFile = obj.pdfUrl.replace('/ClientApp/dist/assets/', '');
    this.formData.appVersion = obj.appVersion;
    this.formData.about = obj.about;
    this.formData.splashScreen = obj.splashScreen;
    this.formData.facebookUrl = obj.facebookUrl;
    this.formData.instagramUrl = obj.instagramUrl;
    this.formData.twitterUrl = obj.twitterUrl;
    this.formData.youtubeUrl = obj.youtubeUrl;
    this.formData.statusID = obj.statusID;
    this.splashImageUpload.imageUrl = obj.splashScreen;
     
  }

  setSelectedSetting() {
    debugger
    this.loadingSetting = true;
    this.settingService.getById(1).subscribe(res => {
      //Set Forms
      this.editForm(res);
      this.loadingSetting = false;
    });
     
  }

  // onSubmit() {
  //   debugger
  //   this.settingForm.markAllAsTouched();
  //   this.submitted = true;
  //   if (this.settingForm.invalid) { return; }
  //   this.loading = true;
  //   this.f.statusID.setValue(this.f.statusID.value === true ? 1 : 2);

  //   this.f.splashScreen.setValue(this.splashImageUpload.imageUrl);
    
  //   if (parseInt('1') === 0) {
  //     //Insert banner
  //     console.log(JSON.stringify(this.settingForm.value));
  //     this.settingService.insert(this.settingForm.value).subscribe(data => {
  //       if (data != 0) {
  //         this.ts.showSuccess("Success","Record added successfully.")
  //         this.router.navigate(['/admin/settings/appsettings']);
  //       }
  //       this.loading = false;
  //     }, error => {
  //       this.ts.showError("Error","Failed to insert record.")
  //       this.loading = false;
  //     });
  //   } 
  //   else {
  //     //Update 
  //     this.settingService.update(this.settingForm.value).subscribe(data => {
  //       this.loading = false;
  //       if (data != 0) {
  //         this.ts.showSuccess("Success","Record updated successfully.")
  //         this.setSelectedSetting();
  //         this.router.navigate(['/admin/settings/appsettings/add']);
  //       }
  //     }, error => {
  //       this.ts.showError("Error","Failed to update record.")
  //       this.loading = false;
  //     });
  //   }
  // }

  onSubmit() {
    debugger;  
    if (this.formData.settingID === 1) {
      
      const formData1 = new FormData();
    
      formData1.append('settingID', this.formData.settingID.toString());
      formData1.append('appName', this.formData.appName);
      formData1.append('appVersion', this.formData.appVersion);
      formData1.append('about', this.formData.about);
      //formData1.append('image', this.formData.image);  
      formData1.append('splashScreen', this.formData.splashScreen);
      formData1.append('facebookUrl', this.formData.facebookUrl);
      formData1.append('instagramUrl', this.formData.instagramUrl);
      formData1.append('twitterUrl', this.formData.twitterUrl);
      formData1.append('youtubeUrl', this.formData.youtubeUrl);   
      formData1.append('statusID', this.formData.statusID.toString());      
      formData1.append('file', this.selectedFile); 
     
      // Log the contents of formData1 for debugging
     console.log(formData1);
  
    //  this.settingService.update(this.formData,this.selectedFile).subscribe(res => {
    //   //Set Forms
    //   this.ts.showSuccess("Success", "Record added successfully.");
          
    // });
      
      this.http.post('api/appsetting/update', formData1).subscribe(
        response => {
          this.ts.showSuccess("Success", "Record updated successfully.");
          // this.router.navigate(['/admin/laboratory/uploadreport']);
        },
        error => {
          console.error('Error occurred:', error);
        }
      );
    }  
    }
  }
  

 
