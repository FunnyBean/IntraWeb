/**
 * Created by Ondrej on 25.07.2016.
 */
import {Component, OnInit, AfterViewInit, ViewChildren, QueryList} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {CarService} from '../../../services/car.service';
import {OfficeService} from '../../../services/office.service';
import {UserService} from '../../../services/user.service';
import {Office} from '../../../models/office.admin.model';
import {User} from '../../../models/user.admin.model';
import {TableReservationComponent} from './table/table.reservations.component';
import * as moment from 'moment';
import {TimeValidator, DateValidator} from "../../../validators/time.validator";

declare var $:any;

@Component({
  templateUrl: 'app/components/home/reservations/reservations.component.html',
  directives: [TableReservationComponent, TimeValidator, DateValidator],
  styles: ['.table-arrow {top: 0px;} #filter{background-color: #f2f2f2; padding: 10px}']

})

export class ReservationsComponent implements OnInit  {
  public reservationType:string;
  public name:string;
  public times = JSON.parse('[]');
  public loggedUser:User = new User();
  public data;
  public usersList = [];
  public isShowedFilterInput:boolean;
  public length = 0.5;
  public filterOfficeTypes:string;
  public maxTime:number = 10.5;
  public dateTime;
  public week:number = 0;
  public month:string;
  public days;
  public now = moment();
  public moveDate = this.now.format("YYYY-MM-DD");
  public tableWidth;
  public officeTypes = new Array();

  @ViewChildren(TableReservationComponent) tableReservationComponent: QueryList<TableReservationComponent>;

  constructor(private route:ActivatedRoute, private carService:CarService, private officeService:OfficeService, private userService:UserService) { }

  ngOnInit() {
    this.updateTime();
    this.updateWeek();
    this.loadUsersData();
    this.route.params.subscribe(params => {
      this.reservationType = (params['type'] !== undefined) ? params['type'] : "rooms";
      this.name = (this.reservationType == 'rooms') ? 'miestností' : 'áut';
      if (this.reservationType == 'rooms')
        this.loadOfficesData();
      else this.loadCarsData();
    });   
  }

  ngAfterViewInit(){
    var tables = this.tableReservationComponent.toArray();
    for(var i = 0; i < tables.length; i++){
      tables[i].updateData(this.week);
    }
  }

  ngAfterContentInit(){
    this.tableWidth = document.getElementById("reservationTable").clientWidth;
    
    $(window).on("resize", function(){
      this.tableWidth = document.getElementById("reservationTable").clientWidth;
      $("thead, #month").css({"width": this.tableWidth});
    });
    
    $("#content").scroll(function(){
      var element = $("thead");
      var headHeight = document.getElementById("header").clientHeight;
      if(!element.hasClass("headFixed") && element.offset().top <= headHeight){
        element.addClass("headFixed").css({"top": headHeight});
        $("#contentHolder").show(0).css({"height": element.height()});
      }
      if(element.hasClass("headFixed") && $("tbody h4").offset().top - element.height() > element.offset().top){
        element.removeClass("headFixed");
        $("#contentHolder").hide(0).css({"height": '0px'});
      }
    });
  }

  updateMaxTime() {
    this.maxTime = ((18 - moment(this.dateTime).hour())*60 - moment(this.dateTime).minute())/60;
  }

  loadUsersData() {
    this.userService.getUsers().subscribe(
      data => {
        var usersArray = data.json();
        for (var i = 0; i < usersArray.length; i++)
          this.usersList[usersArray[i].id] = usersArray[i].name + ' ' + usersArray[i].surname;
      },
      error => console.log(error)
    );
    this.userService.myProfile().subscribe(
      data => {
        this.loggedUser = data.json()
      },
      error => console.log(error)
    );
  }

  loadOfficesData() {
    this.officeService.getOffices().subscribe(
      data => {
        this.data = data.json();
        for(var i = 0; i < this.data.length; i++)
        {
          var row = this.data[i];
          if(this.officeTypes.indexOf(row.type) == -1)
            this.officeTypes.push(row.type);
        }
      },
      error => console.log(error)
    );
  }

  loadCarsData() {
    this.carService.getCars().subscribe(
      data => {
        this.data = data.json();
      },
      error => console.log(error)
    );
  }

  updateTime() {
    for (var i = 7; i < 18; i++) {
      for (var j = 0; j < 2; j++) {
        var time = moment().hour(i).minute(j * 30).format('HH:mm');
        this.times.push({'time': time});
      }
    }
  }

  showFilterInput() {
    this.dateTime = moment().minute(0).second(0).format("YYYY-MM-DDTHH:mm:ss");
    this.isShowedFilterInput = !this.isShowedFilterInput;
    if (this.isShowedFilterInput == false) {
      this.data = null;
      this.times = JSON.parse('[]');
      this.ngOnInit();
    }
    
     if(this.isShowedFilterInput){
        document.getElementById("filterButton").innerHTML = "<span class='glyphicon glyphicon-filter'></span> Skryť filter";
     }
     else{
       document.getElementById("filterButton").innerHTML = "<span class='glyphicon glyphicon-filter'></span> Zobraziť filter";
     }
     
  }

  filterReservation(element) {
    if (this.reservationType == 'rooms')
      this.loadFilteredOfficesData();
    else this.loadFilteredCarsData();   
  }

  loadFilteredOfficesData() {
   this.officeService.filterOffices(this.dateTime, this.length * 60, this.filterOfficeTypes).subscribe(
     data => { 
       this.data = data.json();
       this.week = moment(this.dateTime).week() - moment().week() + 52*(moment(this.dateTime).year() - moment().year());
        if(moment(this.dateTime).year() - moment().year() !== 0) this.week++;
       this.updateWeek();
    },
     error => console.log(error)
     );
  }

  loadFilteredCarsData() {
    this.carService.filterCars(this.dateTime, this.length * 60).subscribe(
      data => {
        this.data = data.json();
        this.week = moment(this.dateTime).week() - moment().week() + 52*(moment(this.dateTime).year() - moment().year());
        if(moment(this.dateTime).year() - moment().year() !== 0) this.week++;
        this.updateWeek();
      },
      error => console.log(error)
    );
  }

  moveFor() {
    this.week += 1;
    this.updateWeek();
    this.ngAfterViewInit();
  }

  moveBack() {
    this.week -= 1;
    this.updateWeek();
    this.ngAfterViewInit();
  }

  moveToday(){
    this.week = 0;
    this.updateWeek();
    this.ngAfterViewInit();
  }

  moveToDate(){
    this.week = moment(this.moveDate).week() - moment().week() + 52*(moment(this.moveDate).year() - moment().year());
    if(moment(this.moveDate).year() - moment().year() !== 0) this.week++;
    this.updateWeek();
    this.ngAfterViewInit();
  }

  updateWeek() {  
    this.month = moment().add(this.week, 'weeks').format("MMMM").toUpperCase();
    this.days = JSON.parse('[]');
    for (var i = 1; i < 6; i++) {
      var day = moment().add(this.week, 'weeks').weekday(i).format("dd DD.MM.YY");
      this.days.push(day);
    }
  }
}
