import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {HttpClient} from "@angular/common/http";
import {parseErrors, Result} from "../../main";
import {Form, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-update-record-dialog',
  templateUrl: './updateRecordDialog.component.html',
})
export class UpdateRecordDialogComponent {
  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string, public dialogRef: MatDialogRef<UpdateRecordDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: {
    gameId: string,
    totalScore: number,
    spentMinutes: number,
    spentSeconds: number
  }) {
    this.model = new UpdateEndedModel();
    this.model.GameId = data.gameId;
    this.http = http;
    this.baseUrl = baseUrl;
    this.dialogReference = dialogRef;

    this.Name = new FormControl(this.model.Name, [Validators.required, Validators.maxLength(50)]);
    this.Link = new FormControl(this.model.Link, Validators.maxLength(100));
  }

  public model: UpdateEndedModel;

  public Name: FormControl;
  public Link: FormControl;

  private http: HttpClient;
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
