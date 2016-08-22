/**
 * Created by Tibor Poštek on 14.07.2016.
 */
import { UserService }        from '../services/user.service';
import {AdminGuard} from "./AdminGuard";
import {AuthGuard} from "./AuthGuard";


export const authProviders = [AuthGuard, AdminGuard, UserService];
