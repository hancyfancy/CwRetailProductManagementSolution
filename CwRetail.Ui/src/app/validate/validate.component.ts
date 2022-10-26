import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserToken } from '../../models/user-token';
import { AuthenticationService } from '../../services/authentication.service';
import { Settings } from '../../settings';

@Component({
  selector: 'app-validate',
  templateUrl: './validate.component.html',
  styleUrls: ['./validate.component.css']
})
export class ValidateComponent implements OnInit {
  protected userToken: UserToken = new UserToken();

  constructor(private authenticationService: AuthenticationService, private settings: Settings, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.userToken = new UserToken();
  }

  validate(token: string): void {
    this.authenticationService.validateUser(new UserToken(0n, token))
      .then((jwtToken) => {
        this.settings.jwtToken = jwtToken!;
        this.goToProducts();
      })
      .catch((error) => {
        console.log("Promise rejected with " + JSON.stringify(error));
      });
  }

  goToProducts(): void {
    const navigationDetails: string[] = ['/products'];
    this.router.navigate(navigationDetails);
  }
}
