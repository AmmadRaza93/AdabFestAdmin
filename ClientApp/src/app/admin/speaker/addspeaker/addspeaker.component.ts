import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ImageuploadComponent } from 'src/app/imageupload/imageupload.component';
import { Router, ActivatedRoute } from '@angular/router';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { ToastService } from 'src/app/_services/toastservice';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SpeakerComponent } from '../speaker.component';
import { SpeakerService } from '../../../_services/speaker.service';
//import { ToolbarService, LinkService, ImageService, HtmlEditorService } from '@syncfusion/ej2-angular-richtexteditor';
import { AngularEditorConfig } from '@kolkov/angular-editor';
@Component({
  selector: 'app-addspeaker',
  templateUrl: './addspeaker.component.html',
  styleUrls: ['./addspeaker.component.css'],
  //providers: [ToolbarService, LinkService, ImageService, HtmlEditorService]
})
export class AddSpeakerComponent implements OnInit {
 
  editorConfig: AngularEditorConfig = {
    editable: true,
    enableToolbar: true,
    showToolbar: true,
  };

  submitted = false;
  speakerForm: FormGroup;
  loading = false;
  loadingSpeaker = false;
  EventList = [];
  selectedEventIds: string[];
  @ViewChild(ImageuploadComponent, { static: true }) imgComp;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    public ts: ToastService,
    private ls: LocalStorageService,
    private speakerService: SpeakerService

  ) {
    this.createForm();
    this.loadEvents();
  }

  ngOnInit() {
    this.setSelectedaddon();
  }

  get f() { return this.speakerForm.controls; }

  private createForm() {
    this.speakerForm = this.formBuilder.group({
      name: ['', Validators.required],
      designation: [''],
      events: [],
      company: [''],
      about: [''],
      statusID: [true],
      speakerID: 0,
      image: [''],
      locationID: null
    });
  }

  private editForm(obj) { 
    debugger   
    this.f.name.setValue(obj.name);
    this.f.designation.setValue(obj.designation);
    this.f.company.setValue(obj.company);
    this.f.about.setValue(obj.about);
    this.f.speakerID.setValue(obj.speakerID);
    this.f.image.setValue(obj.image);
    this.f.statusID.setValue(obj.statusID === 1 ? true : false);
    this.imgComp.imageUrl = obj.image;

    if (obj.events != "") {
      debugger
      var stringToConvert = obj.events;
      this.selectedEventIds = stringToConvert.split(',').map(Number);
      this.f.events.setValue(obj.events);
    }

  }

  setSelectedaddon() {
    
    this.route.paramMap.subscribe(param => {
      const sid = +param.get('id');
      if (sid) {
        this.loadingSpeaker = true;
        this.f.speakerID.setValue(sid);
        this.speakerService.getById(sid).subscribe(res => {
          //Set Forms
          this.editForm(res);
          this.loadingSpeaker = false;
        });
      }
    })
  }
  private loadEvents() {
    this.speakerService.loadEvent().subscribe((res: any) => {

      this.EventList = res;
    });
  }
  onSubmit() {
    debugger
    this.speakerForm.markAllAsTouched();
    this.submitted = true;
    if (this.speakerForm.invalid) { return; }
    this.loading = true;
    this.f.statusID.setValue(this.f.statusID.value === true ? 1 : 2);
    this.f.image.setValue(this.imgComp.imageUrl);
    this.f.events.setValue(this.selectedEventIds == undefined ? "" : this.selectedEventIds.toString());
    if (parseInt(this.f.speakerID.value) === 0) {
      
      //Insert modifier
      console.log(JSON.stringify(this.speakerForm.value));
      this.speakerService.insert(this.speakerForm.value).subscribe(data => {
        if (data != 0) {
          this.ts.showSuccess("Success", "Record added successfully.")
          this.router.navigate(['/admin/speaker']);
        }
        // this.alertService.success("Item has been created");
        this.loading = false;
      }, error => {
        this.ts.showError("Error", "Failed to insert record.")
        this.loading = false;
      });

    } else {
      //Update modifier
      this.speakerService.update(this.speakerForm.value).subscribe(data => {
        this.loading = false;
        if (data != 0) {
          this.ts.showSuccess("Success", "Record updated successfully.")
          this.router.navigate(['/admin/speaker']);
        }
      }, error => {
        this.ts.showError("Error", "Failed to update record.")
        this.loading = false;
      });
    }
  }
}

