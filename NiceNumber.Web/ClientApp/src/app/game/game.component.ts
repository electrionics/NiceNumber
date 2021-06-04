import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html'
})
export class GameComponent {
  public startGame: StartModel;
  public difficultyLevel: number;
  public number: number[];
  public timer: number;
  public positions: number[];
  public regularityTypes: { type: number; label: string; shortLabel: string; }[];
  public difficultyLevels: { type: number; label: string; }[];
  public endGame: EndModel;

  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    this.regularityTypes = [];
    this.regularityTypes.push({type: 1, label: "Одинаковые цифры", shortLabel: "ОЦ"});
    this.regularityTypes.push({type: 2, label: "Одинаковые числа", shortLabel: "ОЧ"});
    this.regularityTypes.push({type: 3, label: "Зеркальные цифры", shortLabel: "ЗЦ"});
    this.regularityTypes.push({type: 4, label: "Кратные числа", shortLabel: "КЧ"});
    this.regularityTypes.push({type: 5, label: "Арифметическая прогрессия", shortLabel: "АП"});
    this.regularityTypes.push({type: 6, label: "Геометрическая прогрессия", shortLabel: "ГП"});

    this.difficultyLevels = [];
    this.difficultyLevels.push({type: 1, label: "Лёгкий"});
    this.difficultyLevels.push({type: 2, label: "Средний"});
    this.difficultyLevels.push({type: 3, label: "Тяжелый"});

    this.difficultyLevel = 3;

    this.http = http;
    this.baseUrl = baseUrl;
  }

  public start(){
    this.http.get<StartModel>(this.baseUrl + 'Game/Start?difficultyLevel=' + this.difficultyLevel).subscribe(result => {
      this.startGame = result;

      this.number = [];

      let tempNumber = result.number;
      while(tempNumber > 0){
        let currentDigit = tempNumber % 10;
        tempNumber = (tempNumber - currentDigit) / 10;

        this.number.push(currentDigit);
      }
      this.number = this.number.reverse();

      this.positions = [];
      for(let pos = 0; pos < this.startGame.length; pos++){
        this.positions.push(pos);
      }

    }, error => console.error(error));
  }
}

interface StartModel {
  gameId: string;
  number: number;
  length: number;
  difficultyLevel: number;
  existRegularityInfos: StartRegularityInfo[];
  existRegularityTypeCounts: { [key: number]: number };
}

interface StartRegularityInfo {
  type: number;
  regularityNumber: number;
}

interface EndModel {
  totalScore: number;
  spentMinutes: number;
  spentSeconds: number;
  foundRegularityInfos: EndRegularityInfo[];
}

interface EndRegularityInfo {
  type: number;
  count: number;
}
