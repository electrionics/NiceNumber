import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {HttpClient} from "@angular/common/http";
import {parseErrors, Result} from "../../main";
import {Form, FormControl, FormGroup, Validators} from "@angular/forms";
import {CookieService} from "../common/cookieService.component";

@Component({
  selector: 'app-update-record-dialog',
  templateUrl: './updateRecordDialog.component.html',
})
export class UpdateRecordDialogComponent {
  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string, cookies: CookieService, public dialogRef: MatDialogRef<UpdateRecordDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: {
    gameId: string,
    totalScore: number,
    spentMinutes: number,
    spentSeconds: number
  }) {
    this.model = new UpdateEndedModel();
    this.model.GameId = data.gameId;
    this.http = http;
    this.cookies = cookies;
    this.baseUrl = baseUrl;
    this.dialogReference = dialogRef;

    this.model.Name = cookies.getCookie("lastRecord_name");
    this.model.Link = cookies.getCookie("lastRecord_link");

    const nameRegexp = '[a-zA-Zа-яА-Я0-9_-]{0,50}';
    const urlRegexp = '(https?:\\/\\/)?(www\\.)?[_\\-a-zA-Zа-яА-Я0-9\\.]{1,100}\\.[a-zA-Zа-яА-Я0-9]{1,6}([-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)';

    this.Name = new FormControl(this.model.Name, [Validators.required, Validators.maxLength(50), Validators.pattern(nameRegexp)]);
    this.Link = new FormControl(this.model.Link, [Validators.maxLength(100), Validators.pattern(urlRegexp)]);
  }

  public model: UpdateEndedModel;

  public Name: FormControl;
  public Link: FormControl;

  private http: HttpClient;
  private cookies: CookieService;
  private baseUrl: string;
  private dialogReference: MatDialogRef<UpdateRecordDialogComponent>;

  public updateControls(){
    this.Name.setValue(this.model.Name);
    this.Link.setValue(this.model.Link);
    this.Name.updateValueAndValidity();
    this.Link.updateValueAndValidity();
  }

  public updateRecord(){

    if (!this.Name.invalid && !this.Link.invalid){
      var obj = {
        GameId: this.model.GameId,
        Name: this.model.Name,
        Link: this.model.Link
      };

      this.http.post<Result>(this.baseUrl + 'Game/UpdateEnded', obj).subscribe(result => {
        if (result.Success){
          this.cookies.setCookie("lastRecord_name", this.model.Name, 7);
          this.cookies.setCookie("lastRecord_link", this.model.Link, 7);

          this.dialogReference.close();
        }
        else {
          parseErrors(result);
        }
      }, error => console.error(error));
    }
  }
}

class UpdateEndedModel{
  GameId: string;
  Name: string;
  Link: string;
}
