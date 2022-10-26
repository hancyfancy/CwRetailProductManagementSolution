import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication.service';
import { Settings } from '../../settings';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  protected user: User = new User();

  constructor(private authenticationService: AuthenticationService, private settings: Settings, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.user = new User();
  }

  get(username: string): void {
    this.authenticationService.getUser(new User(0n, username))
      .then((token) => {
        this.goToValidate(token!);
      })
      .catch((error) => {
        console.log("Promise rejected with " + JSON.stringify(error));
      });
  }

  goToValidate(token : string): void {
    const navigationDetails: string[] = ['/validate', this.encrypt(token)];
    this.router.navigate(navigationDetails);
  }

  encrypt(data: string): string {
    try {
      return CryptoJS.AES.encrypt(data, this.settings.secretKey).toString();
    } catch (e) {
      console.log(e);
      throw (e);
    }
  }
}
