import {Directive, forwardRef, OnInit} from '@angular/core';
import {NG_VALIDATORS, FormControl} from '@angular/forms';
import * as moment from 'moment';

@Directive({
  selector: '[validateTime][ngModel],[validateTime][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => TimeValidator), multi: true }
  ]
})

export class TimeValidator{
  validate(c:FormControl){
  //  if(c.value % 30 == 0 && c.value != 0)
      if(c.value % 0.5 == 0 && c.value != 0 && c.value <= 11)
      return null;
    else return {validateTime: true}
  }
}

@Directive({
  selector: '[validateDate][ngModel],[validateDate][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => DateValidator), multi: true }
  ]
})

export class DateValidator{
  validate(c:FormControl){
    var date = moment(c.value);
    if(date.minute() % 30 == 0 && ((date.hour() >= 7 && date.hour() <= 18) || date.hour() == 0)  && date.day() != 6 && date.day() != 0)
      return null;
    else return {validateDate: true}
  }
}
