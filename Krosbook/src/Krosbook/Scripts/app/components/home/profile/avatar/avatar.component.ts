import {Component,Directive, forwardRef, OnInit} from '@angular/core';
import {User} from "../../../../models/user.admin.model";
import {UserService} from "../../../../services/user.service"
import {NG_VALIDATORS, FormControl} from '@angular/forms';

declare var $:any;

@Component({
  selector: 'avatar',
  templateUrl: 'app/components/home/profile/avatar/avatar.component.html',
  styles: ['.avatarImg{width: 150px; height: auto; display: block; margin: 0 auto;margin-bottom: 15px; margin-top: 15px}'],
})

export class AvatarComponent implements OnInit{
  public userData:User = new User();
  public newImage:string = '';
  public progress:number = 0;
  public error;

  constructor(private userService:UserService) { }

  ngOnInit(){
    this.userService.myProfile().subscribe(
      data => {
        this.userData = data.json();
      },
      error => console.log(error)
    );
  }

  onChange(event){
    var fileToLoad = event.srcElement.files[0];
    if(fileToLoad){
      if(fileToLoad.size > 1048576){
        this.error = "Zvolený obrázok musí byť menší alebo rovný 1MB. Zvoľte obrázok, ktorý túto podmienku spĺňa.";
        return false;
      }
      var fileReader = new FileReader();
      var thisDocument = this;
      fileReader.onload = function(file) {
          thisDocument.newImage = fileReader.result;
          $("#result").attr("src", thisDocument.newImage);
          thisDocument.newImage = thisDocument.newImage.split(',')[1];
      }
      fileReader.onloadstart = function() { thisDocument.progress = 1; }
      fileReader.onloadend = function() { thisDocument.progress = 0; }
      fileReader.readAsDataURL(fileToLoad);
    }
    
  }

  editAvatar(){
    this.userService.updateImage(this.newImage).subscribe(
      data => { 
        $("#profilePicture").attr("src", 'data:image/png;base64,'+this.newImage);
        $("#result").removeAttr("src");
     
        $("#avatarImg").attr("src","data:image/jpeg;base64,"+this.newImage);
     },
      error => console.log(error)
    );

    
  }

  dismiss(){
    this.error = "";
  }
}



