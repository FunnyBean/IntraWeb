import {Injectable} from '@angular/core';
import * as moment from 'moment';

@Injectable()
export class HolidayService {
  private holidays = ['01/01','06/01', '', '', '01/05','08/05','05/07','29/08','01/09','15/09', '01/11','17/11','24/12','25/12','26/12'];
  private holidayNames = ['Deň vzniku Slovenskej republiky',
                        'Zjavenie Pána (Traja králi)',
                        'Veľký piatok',
                        'Veľkonočný pondelok',
                        'Sviatok práce',
                        'Deň víťazstva nad fašizmom',
                        'Sviatok svätého Cyrila a Metoda',
                        'Výročie SNP',
                        'Deň Ústavy Slovenskej republiky',
                        'Sedembolestná Panna Mária',
                        'Sviatok všetkých svätých ',
                        'Deň boja za slobodu a demokraciu',
                        'Štedrý deň',
                        'Prvý sviatok vianočný',
                        'Druhý sviatok vianočný'];
private year:any = 2016;

  constructor()
  {
    this.buildEaster();
  }

  buildEaster()
  {
    let a = this.year % 19;
    let b = this.year % 4;
    let c = this.year % 7;
    let k = Math.floor(this.year / 100);
    let p = Math.floor((13 + 8*k)/25);
    let q = Math.floor(k / 4);
    let M = (15 - p + k - q) % 30;
    let N = (4 + k - q) % 7;
    let d = (19*a + M) % 30;
    let e = (2*b + 4*c + 6*d + N) % 7;
    if(d == 29 && e == 6){
      this.holidays[2] = "24/04";
      this.holidays[3] = "27/04";
    }
    else if(d == 28 && e == 6 && (11*M + 11) % 30 < 19){
      this.holidays[2] = "16/04";
      this.holidays[3] = "19/04";
    }
    else if(22 + d + e <= 31){
      var sunday = moment((22+d+e)+".03."+this.year,"DD.MM.YYYY");
      this.holidays[2] = sunday.add(-2, 'days').format("DD/MM");
      this.holidays[3] = sunday.add(3, 'days').format("DD/MM");
    }
    else if(d + e - 9 > 0) {
      var sunday = moment((d+e-9)+".04."+this.year,"DD.MM.YYYY");
      this.holidays[2] = sunday.add(-2, 'days').format("DD/MM");
      this.holidays[3] = sunday.add(3, 'days').format("DD/MM");
    }
  }

  isHoliday(date:any, year:any)
  {
    if(this.year != year)
    {
      this.year = year;
      this.buildEaster();
    }
    if(this.holidays.indexOf(date)== -1){
      return null;
    }  
    else{
      return this.holidayNames[this.holidays.indexOf(date)];
    }   
  }  
}



