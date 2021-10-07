import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';


@Component({
  selector: 'app-records',
  templateUrl: './records.component.html'
})
export class RecordsComponent {
  private http: HttpClient;
  private baseUrl: string;
  public records: RecordModel[];
  public selectedDays: number;

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.records = [];

    this.getRecords(1);
  }

  public getRecords(days){
    this.selectedDays = days;

    var url = this.baseUrl + 'Game/Records';
    if (this.selectedDays){
      url += '?days=' + this.selectedDays;
    }

    this.http.get<RecordModel[]>(url).subscribe(result => {
      this.records = result;
    }, error => console.error(error));
  }
}

interface RecordModel {
  Position: number;
  Score: number;
  CurrentPlayer: boolean;
  Player: string;
  Link:string;
}
