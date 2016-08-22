import {Directive, forwardRef, OnInit} from '@angular/core';
import {NG_VALIDATORS, FormControl} from '@angular/forms';
import {CarService} from '../services/car.service';

@Directive({
  selector: '[validatePlateUniqueness][ngModel],[validatePlateUniqueness][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => PlateUniquenessValidator), multi: true }, CarService
  ]
})

export class PlateUniquenessValidator{

  private cars;
  private initialCValue = null;

  constructor(private carService:CarService) {
    this.carService.getCars().subscribe(
      data => {this.cars = data.json()},
      error => console.log(error)
    );
  }

  validate(c:FormControl){
    var plate:string;
    if(c.value != null)
      plate = c.value.toUpperCase();

    //nastavenia pre správne fungovanie editovania - nahratie prvej hodnoty do premennej - ak je nová miestnosť tak NULL
    if(this.initialCValue == null && plate != null)
      this.initialCValue = plate;
    if(plate == null)
      this.initialCValue = null;

    if(this.initialCValue == plate)
      return null;
    if(this.cars)
    {
      for (var i = 0; i < this.cars.length; i++) {
        if (this.cars[i].plate == plate)
          return {validatePlateUniqueness: true}
      }
    }
    return null;
  }

}

@Directive({
  selector: '[validatePlate][ngModel],[validatePlate][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => PlateValidator), multi: true }, CarService
  ]
})

export class PlateValidator{

  validate(c:FormControl){
    var plate:string;
    if(c.value != null)
      plate = c.value.toUpperCase();
    if(/^[A-Z]{2}?-?\d{3}[A-Z]{2}$/.test(plate))
      return null;
    else return {validatePlate: true}

  }
}
