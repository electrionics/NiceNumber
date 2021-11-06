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
  public difficultyLevel: number;
  public difficultyLevels: { type: number; label: string; }[];

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.records = [];

    this.difficultyLevels = [];
    this.difficultyLevels.push({type: 1, label: "Лёгкий"});
    this.difficultyLevels.push({type: 2, label: "Средний"});
    this.difficultyLevels.push({type: 3, label: "Тяжелый"});

    this.difficultyLevel = 1;
    this.getRecordsByDays(1);
  }

  public getRecordsByLevel(level){
    this.difficultyLevel = level;

    this.getRecordsBody();
  }

  public getRecordsByDays(days) {
    this.selectedDays = days;

    this.getRecordsBody();
  }

  private getRecordsBody(){
    let url = this.baseUrl + 'Game/Records';
    if (this.selectedDays) {
      url += '?days=' + this.selectedDays;
    }
    if (this.difficultyLevel){
      let prependChar = this.selectedDays ? '&' : '?';
      url += prependChar + 'difficultyLevel=' + this.difficultyLevel;
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
