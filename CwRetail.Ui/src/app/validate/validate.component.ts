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

  decrypt(data: string): string {
    try {
      const bytes = CryptoJS.AES.decrypt(data, this.settings.secretKey);
      return JSON.parse(bytes.toString(CryptoJS.enc.Utf8)) as string;
    } catch (e) {
      console.log(e);
      throw (e);
    }
  }
}
