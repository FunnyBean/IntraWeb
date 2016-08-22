import {Component, Input, Output, EventEmitter, OnInit} from '@angular/core';
import {Office} from '../../../../models/office.admin.model';
import {Equipment} from '../../../../models/equipment.admin.model';
import {OfficeService} from '../../../../services/office.service';
import {EquipmentComponent} from './equipment/equipment.component';
import {OfficeNameValidator} from '../../../../validators/officeName.validator';


@Component({
  selector: "office",
  templateUrl: 'app/components/admin/offices/detail/detail.office.admin.component.html',
  styleUrls: ['lib/css/modalWindow.css'],
  directives: [EquipmentComponent, OfficeNameValidator]
})

export class DetailOfficeAdminComponent implements OnInit{
  public error:string;
  public success:string;
  public equipment:Equipment;
  public formReset:boolean = true;
  public officeData:Office = new Office();

  @Input() officeId:number;
  @Output() windowClose = new EventEmitter<boolean>();
  @Output() updateList = new EventEmitter<boolean>();

  constructor(private officeService:OfficeService) { }

  ngOnInit() {
    if(this.officeId)
      this.getData();
    else this.equipment = JSON.parse('[]');
  }

  getData(){
    this.officeService.getOffice(this.officeId).subscribe(
      data => {this.officeData = data.json(), this.equipment = data.json().equipment},
      error => console.error(error)
    );
  }

  newOffice(){
    let name = this.officeData.name;
    let type = this.officeData.type;
    let description = this.officeData.description;
    let color = this.officeData.color;
    let equipment = this.equipment;
    this.officeService.addOffice(JSON.stringify({name, type, description, color, equipment})).subscribe(
      data => {
      },
        error => {
          this.error = error;
        },
        () => {
          this.success = 'Miestnosť úspešne vytvorená.';
          this.officeData = new Office();
          this.formReset = false;
          setTimeout(() => this.formReset = true, 0);
          this.updateList.emit(true);
          //this.closeWindow();
        }
    );
  }

  editOffice(){
    let id = this.officeData.id;
    let name = this.officeData.name;
    let type = this.officeData.type;
    let description = this.officeData.description;
    let color = this.officeData.color;
    let equipment = this.equipment;
    this.officeService.editOffice(id, JSON.stringify({id, name, type, description, color, equipment})).subscribe(
      data => {
      },
      error => {
        this.error = error;
      },
      () => {
        this.success = 'Miestnosť úspešne upravená.';
        this.updateList.emit(true);
        //this.closeWindow();
      }
    );
  }

  closeWindow(){
    this.windowClose.emit(true);
  }
}
