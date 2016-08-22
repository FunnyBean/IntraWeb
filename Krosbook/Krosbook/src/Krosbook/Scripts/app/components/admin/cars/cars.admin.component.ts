import {Component, OnInit} from  '@angular/core';
import {CarService} from '../../../services/car.service';
import {DetailCarAdminComponent} from './detail/detail.car.admin.component';
import {Car} from '../../../models/car.model';

@Component({
  selector: 'cars-admin',
  templateUrl:'app/components/admin/cars/cars.admin.component.html',
  directives: [DetailCarAdminComponent]
})

export class CarsAdminComponent implements OnInit{
  public cars:Array<Car>;
  public carId:number;
  private showCarWindow:boolean = false;

  constructor(private carService:CarService){ }

  ngOnInit(){
    this.getCars();
  }

  getCars() {
    this.carService.getCars()
      .subscribe(
        data => {this.cars = data.json()},
        error => console.error(error)
      );
  }

  deleteCar(id:string){
    if(confirm("Naozaj chcete vymazať vozidlo?")) {
      this.carService.removeCar(id).subscribe(
        data => { },
        error => { alert(error) },
        () => { this.getCars(); }
      )
    }
  }

  newCar() {
    this.carId = null;
    this.windowOpen();
  }

  editCar(id:number){
    for(var i = 0; i < this.cars.length; i++){
      if(this.cars[i].id == id){
        this.carId = this.cars[i].id;
        break;
      }
    }
    this.windowOpen();
  }

  //opens detail.office.admin.component window
  windowOpen(){
    this.showCarWindow = true;
  }

  //closes detail.office.admin.component window
  windowClose(action: boolean){
    this.showCarWindow = action;
  }
}



