/**
 * Created by krosaci on 25.7.2016.
 */
import {Component, AfterContentInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

declare var $:any;

@Component({
  selector: 'profile',
  templateUrl: 'app/components/home/profile/profile.component.html',
  directives: [ROUTER_DIRECTIVES]
})

export class ProfileComponent implements AfterContentInit{

  ngAfterContentInit(){
    $(".nav a").on("click", function(){
      $("li.active").removeClass("active");
      $(this).parent("li").addClass("active");
    });
  }

}
