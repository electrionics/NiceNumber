import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';


@Component({
  selector: 'app-game',
  templateUrl: './game.component.html'
})
export class GameComponent {
  public startGame: StartModel;
  public game: ProgressModel;
  public endGame: EndModel;

  public difficultyLevel: number;

  public number: { value: number; selected: boolean; }[];
  public positions: { value: number; selected: boolean; }[];

  public regularityTypes: { type: number; label: string; shortLabel: string; regularityNumberHint: string; }[];
  public difficultyLevels: { type: number; label: string; }[];

  public timerSet: boolean;

  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    this.initLists();

    this.difficultyLevel = 3;
    this.timerSet = false;

    this.http = http;
    this.baseUrl = baseUrl;
  }

  public start(){
    this.http.get<StartModel>(this.baseUrl + 'Game/Start?difficultyLevel=' + this.difficultyLevel).subscribe(result => {
      this.endGame = null;
      this.startGame = result;
      this.game = new ProgressModel();
      this.game.ProgressRegularityInfos = {};
      this.game.Score = 0;

      this.number = [];

      let tempNumber = result.Number;
      while(tempNumber > 0){
        let currentDigit = tempNumber % 10;
        tempNumber = (tempNumber - currentDigit) / 10;

        this.number.push({value: currentDigit, selected: false});
      }
      this.number = this.number.reverse();

      this.positions = [];
      for(let pos = 0; pos < this.startGame.Length; pos++){
        this.positions.push({value: pos, selected: false});
      }

      this.regularityTypes.map(x => x.type).forEach(type => {
        this.game.ProgressRegularityInfos[type] = [];
        this.startGame.ExistRegularityInfos.filter(x => x.Type == type).forEach(regularityInfo =>{
          let progressInfo = new ProgressRegularityInfo();
          progressInfo.Found = false;
          progressInfo.Positions = [];
          progressInfo.RegularityNumber = regularityInfo.RegularityNumber;

          this.game.ProgressRegularityInfos[type].push(progressInfo);
        });
      });

      this.game.TimerSeconds = 300;
      if (!this.timerSet){
        setInterval(() => {
          if (this.game.TimerSeconds > 0 && !this.endGame){
            if (--this.game.TimerSeconds == 0){
              this.end(false);
            }
          }
        }, 1000);
        this.timerSet = true;
      }
    }, error => console.error(error));
  }

  public toggleSelected(digit){
    if (this.endGame) return;

    digit.selected = !digit.selected;
    for(let pos = 0; pos < this.number.length; pos++){
      if (this.number[pos] === digit){
        this.positions[pos].selected = digit.selected;
      }
    }
  }

  public check(regularityType){
    var selectedPositions = this.positions.filter(x => x.selected).map(x => x.value);
    if (selectedPositions.length > 1){
      this.http.post<CheckResultModel>(this.baseUrl + 'Game/Check', {
        GameId: this.startGame.GameId,
        Type: regularityType,
        Positions: selectedPositions
      }).subscribe(result => {
        // popup with appearing and hiding animation
        if (result.Match){
          let progressInfos = this.game.ProgressRegularityInfos[regularityType];
          let info = progressInfos.find(x => x.RegularityNumber == result.RegularityNumber && !x.Found);

          info.Positions = selectedPositions;
          info.Found = true;

          alert(result.PointsAdded + ' очков заработано!');

          this.game.Score = result.NewTotalPoints;
          this.game.TimerSeconds += 5;

          this.clearSelections();
        }
        else {
          let hintMessage = GameComponent.composeHintMessage(result);

          if (hintMessage){
            alert(hintMessage);
          }
        }
      }, error => console.error(error));
    }
    else {
      alert("Выберите минимум 2 цифры.");
    }
  }

  public end(needConfirm){
    if (!this.endGame){
      if (!needConfirm || confirm("Вы уверены, что хотите завершить игру?")){
        this.http.post<EndModel>(this.baseUrl + 'Game/End?gameId=' + this.startGame.GameId, null).subscribe(result => {
          this.endGame = result;
          // TODO: popup with appearing animation
          this.clearSelections();
          this.confirmEndGameWithResults();
        }, error => console.error(error));
      }
    }
    else {
      this.confirmEndGameWithResults();
    }
  }

  public filterByFound(progressRegularityInfos, type){
    let infos = progressRegularityInfos[type];
    if (!infos) {
      return [];
    }

    return infos.filter(x => x.Found);
  }

  public some(collection, value){
    return collection.some(x => x == value);
  }

  private static composeHintMessage(result){
    let digitsToAddMessage = result.AddHint == CheckHint.No
      ? '' :
      result.AddHint == CheckHint.AddOneDigit
        ? 'добавьте одну цифру'
        : result.AddHint == CheckHint.AddMoreThanOneDigit
        ? 'добавьте несколько цифр'
        : '';
    let digitsToRemoveMessage = result.RemoveHint == CheckHint.No
      ? '' :
      result.RemoveHint == CheckHint.RemoveOneDigit
        ? 'удалите одну цифру'
        : result.RemoveHint == CheckHint.RemoveMoreThanOneDigit
        ? 'удалите несколько цифр'
        : '';

    return digitsToAddMessage == ''
      ? digitsToRemoveMessage
      : digitsToAddMessage + (digitsToRemoveMessage == ''
      ? ''
      : ', ' + digitsToRemoveMessage);
  }

  private initLists(){
    this.regularityTypes = [];
    this.regularityTypes.push({type: 1, label: "Одинаковые цифры", regularityNumberHint: "Количество цифр", shortLabel: "ОЦ"});
    this.regularityTypes.push({type: 2, label: "Одинаковые числа", regularityNumberHint: "Количество чисел", shortLabel: "ОЧ"});
    this.regularityTypes.push({type: 3, label: "Зеркальные цифры", regularityNumberHint: "Количество цифр между крайними цифрами", shortLabel: "ЗЦ"});
    this.regularityTypes.push({type: 4, label: "Кратные числа", regularityNumberHint: "Кратность", shortLabel: "КЧ"});
    this.regularityTypes.push({type: 5, label: "Арифметическая прогрессия", regularityNumberHint: "Шаг прогрессии", shortLabel: "АП"});
    this.regularityTypes.push({type: 6, label: "Геометрическая прогрессия", regularityNumberHint: "Шаг прогрессии", shortLabel: "ГП"});

    this.difficultyLevels = [];
    this.difficultyLevels.push({type: 1, label: "Лёгкий"});
    this.difficultyLevels.push({type: 2, label: "Средний"});
    this.difficultyLevels.push({type: 3, label: "Тяжелый"});
  }

  private clearSelections(){
    this.positions.forEach(pos => {
      pos.selected = false;
    });
    this.number.forEach(num => {
      num.selected = false;
    });
  }

  private confirmEndGameWithResults(){
    if (confirm("Игра окончена! Набрано " + this.endGame.TotalScore + ' очков за ' + this.endGame.SpentMinutes + ' минуты ' + this.endGame.SpentSeconds + ' секунды. Завершить сеанс?')){
      this.startGame = null;
      this.game = null;
      this.number = null;
      this.positions = null;
    }
  }
}

interface StartModel {
  GameId: string;
  Number: number;
  Length: number;
  DifficultyLevel: number;
  ExistRegularityInfos: StartRegularityInfo[];
  ExistRegularityTypeCounts: { [key: number]: number };
}

interface StartRegularityInfo {
  Type: number;
  RegularityNumber: number;
}

class ProgressModel
{
  ProgressRegularityInfos: { [type: number]: ProgressRegularityInfo[] }
  Score: number;
  TimerSeconds: number;
}

class ProgressRegularityInfo
{
  RegularityNumber: number;
  Found: boolean;
  Positions: number[];
}

interface EndModel {
  TotalScore: number;
  SpentMinutes: number;
  SpentSeconds: number;
  FoundRegularityInfos: EndRegularityInfo[];
}

interface EndRegularityInfo {
  Type: number;
  Count: number;
}

interface CheckResultModel {
  RegularityNumber: number;
  Match: boolean;
  PointsAdded: number;
  NewTotalPoints: number;
  AddHint: CheckHint;
  RemoveHint: CheckHint;
}

enum CheckHint{
  No = 0,
  AddOneDigit = 11,
  AddMoreThanOneDigit = 12,
  RemoveOneDigit = 21,
  RemoveMoreThanOneDigit = 22
}
