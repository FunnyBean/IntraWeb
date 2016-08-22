import {Directive, forwardRef, OnInit} from '@angular/core';
import {NG_VALIDATORS, FormControl} from '@angular/forms';
import {OfficeService} from '../services/office.service';

@Directive({
  selector: '[validateOfficeName][ngModel],[validateOfficeName][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => OfficeNameValidator), multi: true }, OfficeService
  ]
})

export class OfficeNameValidator{

  private offices;
  private initialCValue = null;

  constructor(private officeService:OfficeService) {
    this.officeService.getOffices().subscribe(
      data => {this.offices = data.json()},
      error => console.log(error)
    );
  }

  validate(c:FormControl){
    //nastavenia pre správne fungovanie editovania - nahratie prvej hodnoty do premennej - ak je nová miestnosť tak NULL
    if(this.initialCValue == null && c.value != null)
      this.initialCValue = c.value;
    if(c.value == null)
      this.initialCValue = null;


    if(this.initialCValue == c.value)
      return null;
    if(this.offices)
    {
      for (var i = 0; i < this.offices.length; i++) {
        if (this.offices[i].name == c.value)
          return {validateOfficeName: true}
      }
    }
    return null;
  }

}
