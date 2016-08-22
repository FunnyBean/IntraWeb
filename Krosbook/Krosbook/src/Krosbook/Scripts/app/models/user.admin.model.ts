import {Role} from "./role.admin.model";
/**
 * Created by Tibor Poštek on 19.07.2016.
 */
export class User{
  public id;
  public name;
  public surname;
  public email;
  public photoBase64;
  public isLocked;
  public roles= JSON.parse("[]"); //= JSON.parse("[]"); //:
}
