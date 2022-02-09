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
  public personals: PersonalRecordModel[];
  public nickName: string;
  public selectedDays: number;
  public difficultyLevel: number;
  public difficultyLevels: { type: number; label: string; }[];
  public requestProgress: boolean;

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.records = [];
    this.personals = [];

    this.difficultyLevels = [];
    this.difficultyLevels.push({type: 1, label: "Лёгкий"});
    this.difficultyLevels.push({type: 2, label: "Средний"});
    this.difficultyLevels.push({type: 3, label: "Тяжелый"});

    this.difficultyLevel = 1;
    this.getRecordsByDays(1);
  }

  public getRecordsByLevel(level){
    this.difficultyLevel = level;

    this.getRecordsBody(false);
  }

  public getRecordsByDays(days) {
    this.selectedDays = days;

    this.getRecordsBody(true);
    this.getPersonalRecordsBody();
  }

  private getRecordsBody(dayCouldBeChanged){
    let urlRecords = this.baseUrl + 'Game/Records';
    let urlPersonal = this.baseUrl + 'Game/PersonalRecords';
    if (this.selectedDays) {
      urlRecords += '?days=' + this.selectedDays;
      urlPersonal += '?days=' + this.selectedDays;
    }
    if (this.difficultyLevel){
      let prependChar = this.selectedDays ? '&' : '?';
      urlRecords += prependChar + 'difficultyLevel=' + this.difficultyLevel;
    }
    if (!this.requestProgress) {
      this.requestProgress = true;
      let requestCounter = 1;
      if (dayCouldBeChanged) {
        requestCounter += 1;
      }

      this.http.get<RecordModel[]>(urlRecords).subscribe(result => {
        result.forEach((item) => {
          if (item.Link && !item.Link.startsWith('http')) {
            item.Link = '//' + item.Link;
          }
        });

        this.records = result;
      }, error => console.error(error), () => {
        requestCounter--;
        if (requestCounter == 0) {
          this.requestProgress = false
        }
      });

      if (dayCouldBeChanged) {
        this.http.get<PersonalRecordModel[]>(urlPersonal).subscribe(result => {
          result.forEach((item) => {
            item.DifficultyLevelStr = this.difficultyLevels.find(x => x.type == item.DifficultyLevel).label;
          });

          this.nickName = result[0].Player;
          this.personals = result;
        }, error => console.error(error), () => {
          requestCounter--;
          if (requestCounter == 0) {
            this.requestProgress = false
          }
        });
      }
    }
  }

  private getPersonalRecordsBody() {

  }
}

interface RecordModel {
  Position: number;
  Score: number;
  CurrentPlayer: boolean;
  Player: string;
  Link:string;
}

interface PersonalRecordModel {
  Score: number;
  Player: string;
  DifficultyLevel: number;
  DifficultyLevelStr: string;
}
