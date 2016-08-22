import {Component, OnInit} from '@angular/core';
import {UserService} from '../../services/user.service';
import {ROUTER_DIRECTIVES} from '@angular/router';
import {UserInfoComponent} from './userInfo/userInfo.component';
import {User} from '../../models/user.admin.model';

declare var $:any;

@Component({
  selector: 'home',
  templateUrl: 'app/components/home/home.component.html',
  styleUrls: ['app/components/home/home.component.css'],
  directives: [ROUTER_DIRECTIVES, UserInfoComponent]
})

export class HomeComponent implements OnInit{

  public openUserInfo:boolean = false;
  private isAdmin:boolean = false;

  public openReservations:boolean = true;
  public userData:User = new User();
  private height;

  constructor(public userService:UserService) { }

  ngOnInit(){
    this.userService.myProfile().subscribe(
      data =>{
        this.userData = data.json();
        for(var i = 0; i < this.userData.roles.length; i++)
        {
          if(this.userData.roles[i].roleId == 1)
            this.isAdmin = true;
        }
      },
      error =>{console.log(error)}
    )
  }

  ngAfterContentInit(){
    this.height = window.innerHeight - document.getElementById("header").clientHeight;
    $(window).on("resize", function(){
      this.height = window.innerHeight - document.getElementById("header").clientHeight;
      $("#content").css({"height": this.height});
    });
  }


  showUserInfo()
  {
    //var str = (<HTMLTextAreaElement>document.getElementById("contentWindow"));
   // alert(str.toString()+"" + this.isViewed);
    //this.isViewed=!this.isViewed;
    document.getElementById('userInfo').style.display='block';

  }

  toggleReservations(){
    this.openReservations = this.openReservations ? false : true;
  }



}
