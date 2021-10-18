import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-alert-dialog',
  templateUrl: './alert.component.html',
})
export class AlertDialogComponent {
  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string, public dialogRef: MatDialogRef<AlertDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: {
    content: string
  }) {
    this.content = data.content;
    this.dialogReference = dialogRef;
  }

  public content: string;
  private dialogReference: MatDialogRef<AlertDialogComponent>;

  public ok(){
    this.dialogReference.close(true);
  }
}
