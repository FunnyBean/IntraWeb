import {Component, Input, Output, OnInit} from '@angular/core';
import {OfficeService} from '../../../../../services/office.service';
import {Equipment} from '../../../../../models/equipment.admin.model';

@Component({
  selector: 'equipment',
  templateUrl: 'app/components/admin/offices/detail/equipment/equipment.component.html',
  styles: ['.control-label { padding-top: 7px; }']
})

export class EquipmentComponent implements OnInit{
  public list;
  public amount:number = 0;
  public itemId:number = 1;

  @Input() equipment;

  constructor(private officeService:OfficeService){ }

  ngOnInit(){
    this.getEquipmentList();
  }

  getEquipmentList(){
    this.officeService.getEquipment().subscribe(
      data => {this.list = data.json()},
      error => console.log(error)
    );
  }

  addEquipment(){
    var desc:string;
    for(var i = 0; i < this.list.length; i++){
      if(this.list[i].id == this.itemId)
        desc = this.list[i].description;
    }
    if(this.equipment)
      this.equipment.push({"equipmentId": this.itemId, "description": desc, "amount": this.amount});
    else this.equipment = JSON.parse('[{"equipmentId": '+this.itemId+', "description": "'+desc+'", "amount": '+this.amount+'}]');
    this.amount = 0;
  }

  removeEquipment(item){
    var lastId = this.equipment.length - 1;
    for(var i = 0; i < this.equipment.length; i++){
      if(this.equipment[i] == item) {
        var helper = this.equipment[lastId];
        this.equipment[lastId] = this.equipment[i];
        this.equipment[i] = helper;
        delete this.equipment[lastId];
        this.equipment.length -= 1;
      }
    }
  }
}
