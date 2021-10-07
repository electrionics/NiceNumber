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
  public hintsEnabled: boolean;
  public understandDescription: boolean;

  public number: { value: number; selected: boolean; disabled: boolean; }[];
  public positions: { value: number; selected: boolean; }[];

  public regularityTypes: { type: number; enabled: boolean; label: string; shortLabel: string; regularityNumberHint: string; }[];
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
      this.hintsEnabled = true;
      this.understandDescription = false;
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

        this.number.push({value: currentDigit, selected: false, disabled: false });
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
          progressInfo.FoundStatus = FoundStatus.NotFound;
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
    let selectedPositions = this.positions.filter(x => x.selected).map(x => x.value);
    if (selectedPositions.length > 1){
      this.http.post<CheckResultModel>(this.baseUrl + 'Game/Check', {
        GameId: this.startGame.GameId,
        Type: regularityType,
        Positions: selectedPositions
      }).subscribe(result => {
        // popup with appearing and hiding animation
        if (result.Match){
          let progressInfos = this.game.ProgressRegularityInfos[regularityType];
          let info = progressInfos.find(x => x.RegularityNumber == result.RegularityNumber && x.FoundStatus == FoundStatus.NotFound);

          info.Positions = selectedPositions;
          info.FoundStatus = result.Hinted ? FoundStatus.Hinted : FoundStatus.Found;

          alert(result.PointsAdded + ' очков заработано!');

          this.game.Score = result.NewTotalPoints;

          if (!result.Hinted){
            this.game.TimerSeconds += 5;
          }

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
          alert("Игра окончена! Набрано " + this.endGame.TotalScore + ' очков за ' + this.endGame.SpentMinutes + ' минуты ' + this.endGame.SpentSeconds + ' секунды.');
          this.showNotFound();
        }, error => console.error(error));
      }
    }
  }

  public endSession(){
    if (confirm('Завершить сеанс? (текущий прогресс будет утерян)')){
      this.startGame = null;
      this.game = null;
      this.number = null;
      this.positions = null;
    }
  }

  public showNotFound(){
    if (confirm("Показать ненайденные закономерности?")){
      this.endGame.NotFoundRegularityInfos.forEach(hint => {
        let progressInfos = this.game.ProgressRegularityInfos[hint.Type];
        let info = progressInfos.find(x => x.RegularityNumber == hint.RegularityNumber && x.FoundStatus == FoundStatus.NotFound);

        if (info) {
          info.Positions = hint.Positions;
          info.FoundStatus = FoundStatus.Hinted;
        }
      });
      this.endGame.HintsIterated = true;
    }
  }

  public toggleHints(){
    this.hintsEnabled = !this.hintsEnabled;
  }

  public startHintMode(regularityType, regularityNumber, needConfirm){
    if (!needConfirm || confirm("Вам нужна подсказка?")){
      this.http.post<HintResultModel>(this.baseUrl + 'Game/Hint', {
        GameId: this.startGame.GameId,
        Type: regularityType,
        RegularityNumber: regularityNumber
      }).subscribe(result => {
        if (result) {
          this.clearSelections();

          for (let pos = 0; pos < this.startGame.Length; pos++) {
            let selected = result.Positions.some(x => x == pos);

            this.positions[pos].selected = selected;
            this.number[pos].selected = selected;
            this.number[pos].disabled = true;
          }

          this.regularityTypes.forEach(regType => {
            regType.enabled = result.Type == regType.type;
          });
        }
      }, error => console.error(error));
    }
  }

  public filterByFound(progressRegularityInfos, type){
    let infos = progressRegularityInfos[type];
    if (!infos) {
      return [];
    }

    return infos.filter(x => x.FoundStatus == FoundStatus.Hinted || x.FoundStatus == FoundStatus.Found);
  }

  public someFoundProgressInfo(){
    if (!this.game){
      return false;
    }

    var dictionary = this.game.ProgressRegularityInfos;
    return this.regularityTypes.some(t => this.filterByFound(dictionary, t.type).length);
  }

  public some(collection, value){
    return collection.some(x => x == value);
  }

  public getHintClass(status){
    return status == FoundStatus.Found
      ? 'plus'
      : status == FoundStatus.NotFound
        ? 'minus'
        : 'hint';
  }

  public getHintChar(status){
    return status == FoundStatus.NotFound ? '-' : '+';
  }

  public getHintDisabled(status) {
    return status != FoundStatus.NotFound;
  }

  public getAnyDisabled(regularityTypes){
    return regularityTypes.some(x => !x.enabled);
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
    this.regularityTypes.push({type: 1, enabled: true, label: "Одинаковые цифры", regularityNumberHint: "Количество цифр в закономерности. Например, для числа '373177' будет два числа-подсказки, равные 2 для троек и 3 для семерок.", shortLabel: "ОЦ"});
    this.regularityTypes.push({type: 2, enabled: true, label: "Одинаковые числа (минимум 2-значные)", regularityNumberHint: "Количество чисел в закономерности. Например, для числа '404040' будет одно число-подсказка, равное 3: три числа, равных 40.", shortLabel: "ОЧ"});
    this.regularityTypes.push({type: 3, enabled: true, label: "Зеркальные цифры", regularityNumberHint: "Количество цифр между внутренними крайними цифрами. Например, для числа '133221' будет два числа-подсказки, равных 0, так как зеркальные будут '1331' и '1221', а между двойками и тройками в исходном числе нет цифр.", shortLabel: "ЗЦ"});
    this.regularityTypes.push({type: 4, enabled: true, label: "Кратные числа (минимум 2-значные)", regularityNumberHint: "Кратность чисел (большее число разделить на меньшее), всегда целое число. Например, для числа '8421' будет одно число-подсказка, равное 84/21 = 4.", shortLabel: "КЧ"});
    this.regularityTypes.push({type: 5, enabled: true, label: "Арифметическая прогрессия", regularityNumberHint: "Шаг (разность) прогрессии. Например, для числа '23420' будет два числа-подсказки, 4-3 = 3-2 = 1 и 0-2 = 2-4 = -2.", shortLabel: "АП"});
    this.regularityTypes.push({type: 6, enabled: true, label: "Геометрическая прогрессия", regularityNumberHint: "Знаменатель прогрессии. Например, для числа '141684' будет два числа-подсказки, 16/4 = 4/1 = 4 и 4/8 = 8/16 = 0.5.", shortLabel: "ГП"});

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
      num.disabled = false;
    });
    this.regularityTypes.forEach(regType => {
      regType.enabled = true;
    });
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
  FoundStatus: FoundStatus;
  Positions: number[];
}

interface EndModel {
  TotalScore: number;
  SpentMinutes: number;
  SpentSeconds: number;
  NotFoundRegularityInfos: HintResultModel[];
  FoundRegularityInfos: EndRegularityInfo[];
  HintsIterated: boolean;
}

interface EndRegularityInfo {
  Type: number;
  Count: number;
}

interface HintResultModel{
  Positions: number[];
  Type: number;
  RegularityNumber: number;
}

interface CheckResultModel {
  RegularityNumber: number;
  Match: boolean;
  Hinted: boolean;
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

enum FoundStatus{
  Found = 1,
  Hinted = 2,
  NotFound = 3
}
