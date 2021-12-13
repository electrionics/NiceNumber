import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {MatDialog} from '@angular/material/dialog';
import {UpdateRecordDialogComponent} from "./updateRecordDialog.component";
import {ConfirmDialogComponent} from "../common/confirm.component";
import {AlertDialogComponent} from "../common/alert.component";
import {PassGameParametersService} from "./passGameParametersService";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html'
})
export class GameComponent implements OnInit, OnDestroy {
  public startGame: StartModel;
  public game: ProgressModel;
  public endGame: EndModel;

  public difficultyLevel: number;
  public hintsEnabled: boolean;
  public understandDescription: boolean;

  public number: { value: number; selected: boolean; disabled: boolean; }[];
  public positions: { value: number; selected: boolean; }[];

  public regularityTypes: { type: number; enabled: boolean; label: string; shortLabel: string; regularityNumberHint: string; }[];
  public difficultyLevels: { type: number; label: string; timer: number; bonusTime: number; }[];

  public timerSet: boolean;

  private http: HttpClient;
  private readonly baseUrl: string;


  public currentLevel: TutorialLevel;
  public countLevels: number;
  public currentTaskIndex: number;
  public tasks: { controlName: string, anySubtask: boolean, subtasks: number[], additionalCondition: Predicate<number, number>, text: string}[]; // if not any - then all

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string, public dialog: MatDialog, private router: Router, public dataService: PassGameParametersService) {
    this.initLists();

    this.timerSet = false;

    this.http = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    if (this.dataService.parameter){
      this.difficultyLevel = this.dataService.parameter.difficultyLevel;

      this.start();
    }
    else{
      this.dataService.parameter = {
        difficultyLevel: null
      };
      this.router.navigateByUrl('/');
    }
  }

  ngOnDestroy() {
    this.dataService.parameter = {
      difficultyLevel: this.difficultyLevel
    };
  }

  public start(){
    var url = this.baseUrl + 'Game/Start?difficultyLevel=' + this.difficultyLevel;
    if (this.difficultyLevel == 0){
      let nextLevelIndex;
      if (!this.currentLevel){
        nextLevelIndex = 1;
      }
      else{
        nextLevelIndex = this.currentLevel.Level + 1;
      }
      url += ('&tutorialLevel=' + nextLevelIndex);
    }

    this.http.get<StartModel>(url).subscribe(result => {
      this.initTutorialLevel(result);

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
          progressInfo.Numbers = null;
          progressInfo.RegularityNumber = regularityInfo.RegularityNumber;
          progressInfo.ReverseRegularityNumber = regularityInfo.ReverseRegularityNumber;

          this.game.ProgressRegularityInfos[type].push(progressInfo);
        });
      });

      this.game.TimerSeconds = this.getCurrentDifficultyLevel().timer;
      if (!this.timerSet){
        setInterval(() => {
          if (this.game && this.game.TimerSeconds && this.game.TimerSeconds > 0 && !this.endGame){
            if (--this.game.TimerSeconds == 0){
              this.end(false);
            }
          }
        }, 1000);
        this.timerSet = true;
      }
    }, error => console.error(error));
  }

  public toggleSelected(digit, index){
    if (this.endGame) return;

    digit.selected = !digit.selected;
    for(let pos = 0; pos < this.number.length; pos++){
      if (this.number[pos] === digit){
        this.positions[pos].selected = digit.selected;
      }
    }

    this.increaseTask('digit', index);
  }

  public check(regularityType){
    let selectedPositions = this.positions.filter(x => x.selected).map(x => x.value);
    if (selectedPositions.length > 1){
      this.http.post<CheckResultModel>(this.baseUrl + 'Game/Check', {
        GameId: this.startGame.GameId,
        Type: regularityType,
        Positions: selectedPositions
      }).subscribe(result => {
        if (result.Match){
          let progressInfos = this.game.ProgressRegularityInfos[regularityType];
          let info = progressInfos.find(x => x.RegularityNumber == result.RegularityNumber && x.FoundStatus == FoundStatus.NotFound);

          info.Numbers = result.FoundNumbers;
          info.FoundStatus = result.Hinted ? FoundStatus.Hinted : FoundStatus.Found;

          this.alertDialog(result.PointsAdded + ' очков заработано!', 'Поздравляем!', 'points-added-dialog', () =>{
            this.increaseTask('btnCheckSuccess', 0, regularityType) ||
            this.increaseTask('digitsAndBtnCheckSuccess', 0, regularityType);

            this.game.Score = result.NewTotalPoints;

            if (!result.Hinted){
              this.game.TimerSeconds += this.getCurrentDifficultyLevel().bonusTime;
            }

            this.clearSelections();

            if (!this.regularityTypes.some(x => this.game.ProgressRegularityInfos[x.type].some(info => info.FoundStatus == FoundStatus.NotFound))){
              this.end(false);
            }
          });
        }
        else {
          let hintMessage = GameComponent.composeHintMessage(result);

          if (hintMessage){
            this.alertDialog(hintMessage, 'Подсказка!');
          }
        }
      }, error => console.error(error));
    }
    else {
      this.alertDialog("Выберите минимум 2 цифры.", 'Внимание!');
    }
  }

  public end(needConfirm){
    let self = this;
    let endBody = function(){
      self.http.post<EndModel>(self.baseUrl + 'Game/End?gameId=' + self.startGame.GameId + '&remainingSeconds=' + self.game.TimerSeconds, null).subscribe(result => {
        self.endGame = result;

        self.game.Score = self.endGame.TotalScore;

        self.clearSelections();

        self.dialog.open(UpdateRecordDialogComponent, {
          data: {
            gameId: self.startGame.GameId,
            totalScore: self.endGame.TotalScore,
            spentMinutes: self.endGame.SpentMinutes,
            spentSeconds: self.endGame.SpentSeconds
          }
        }).afterClosed().subscribe(result => {
          self.showNotFound();
        });

        self.increaseTask('endGame');
      }, error => console.error(error));
    }

    if (!this.endGame){
      if (!needConfirm){
        endBody();
      }
      else{
        this.confirmDialog('Вы уверены, что хотите завершить игру?', () => {
          endBody();
        });
      }
    }
  }

  public endSession(){
    this.confirmDialog('Завершить сеанс и вернуться на главную страницу?', () => {
      this.startGame = null;
      this.game = null;
      this.number = null;
      this.positions = null;

      this.tasks = null;
      this.currentLevel = null;

      this.router.navigateByUrl('/');
    });
  }

  public showNotFound(){
    if (this.endGame.NotFoundRegularityInfos.length){
      this.confirmDialog('Показать ненайденные закономерности?', () =>{
        this.endGame.NotFoundRegularityInfos.forEach(hint => {
          let progressInfos = this.game.ProgressRegularityInfos[hint.Type];
          let info = progressInfos.find(x => x.RegularityNumber == hint.RegularityNumber && x.FoundStatus == FoundStatus.NotFound);

          if (info) {
            info.Numbers = hint.Numbers;
            info.FoundStatus = FoundStatus.Hinted;
          }
        });
        this.endGame.HintsIterated = true;

        this.increaseTask('showNotFound');
      });
    }
    else{
      this.endGame.HintsIterated = true;
    }
  }

  public toggleHints(){
    this.hintsEnabled = !this.hintsEnabled;

    this.increaseTask('toggleHints');
  }

  public startHintMode(regularityType, regularityNumber, needConfirm){
    let self = this;
    let startHintModeBody = function (regularityType, regularityNumber){
      self.http.post<HintResultModel>(self.baseUrl + 'Game/Hint', {
        GameId: self.startGame.GameId,
        Type: regularityType,
        RegularityNumber: regularityNumber
      }).subscribe(result => {
        if (result) {
          self.clearSelections();

          for (let pos = 0; pos < self.startGame.Length; pos++) {
            let selected = result.Numbers.some(n => {
              for (var i = 0; i < n.Length; i++){
                if (n.Position + i == pos){
                  return true;
                }
              }

              return false;
            });

            self.positions[pos].selected = selected;
            self.number[pos].selected = selected;
            self.number[pos].disabled = true;
          }

          self.regularityTypes.forEach(regType => {
            regType.enabled = result.Type == regType.type;
          });

          self.increaseTask('hintRegNum',0, regularityType) ||
          self.increaseTask('hintRegType',0, regularityType) ||
          self.increaseTask('hintRandom');
        }
      }, error => console.error(error));
    }

    if (!needConfirm){
      startHintModeBody(regularityType, regularityNumber);
    }
    else{
      this.confirmDialog('Вам нужна подсказка?', () =>{
        startHintModeBody(regularityType, regularityNumber);
      })
    }
  }

  public understand(){
    this.understandDescription = true;

    this.increaseTask('understand');
  }

  public toggleTooltip(tooltip, index){
    tooltip.toggle();

    this.increaseTask('tooltip', index);
  }

  public filterByFound(progressRegularityInfos, type){
    let infos = progressRegularityInfos[type];
    if (!infos) {
      return [];
    }

    return infos.filter(x => x.FoundStatus == FoundStatus.Hinted || x.FoundStatus == FoundStatus.Found);
  }

  public filterByExists(regularityTypes){
    return regularityTypes.filter(regType => this.startGame.ExistRegularityTypeCounts[regType.type]);
  }

  public getCurrentDifficultyLevel(){
    let result;
    this.difficultyLevels.forEach((item) =>{
      if (item.type == this.difficultyLevel){
        result = item;
      }
    });

    return result;
  }

  public getCurrentLength(numbers: FoundNumber[], position){
    for (let i = 0; i < numbers.length; i++){
      if (numbers[i].Position == position){
        return {
          value: numbers[i].Length,
          exist: true
        }; // colspan cell
      }
      else if (numbers[i].Position < position && numbers[i].Position + numbers[i].Length > position){
        return {
          value: 0,
          exist: true
        }; // skip cell
      }
    }

    return {
      value: 1,
      exist: false
    }; // empty cell
  }

  public range(length){
    let result = [];
    for (let i = 0; i < length; i++){
      result.push(i);
    }
    return result;
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
    this.regularityTypes.push({type: 6, enabled: true, label: "Геометрическая прогрессия", regularityNumberHint: "Знаменатель прогрессии. Например, для числа '141684' будет два числа-подсказки, 16/4 = 4/1 = 4 и 4/8 = 8/16 = 1/2.", shortLabel: "ГП"});

    this.difficultyLevels = [];
    this.difficultyLevels.push({type: 0, label: "Обучение", timer: 600, bonusTime: 10 });
    this.difficultyLevels.push({type: 1, label: "Лёгкий", timer: 180, bonusTime: 10 });
    this.difficultyLevels.push({type: 2, label: "Средний", timer: 240, bonusTime: 9 });
    this.difficultyLevels.push({type: 3, label: "Тяжелый", timer: 300, bonusTime: 8 });
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

  private confirmDialog(message, successCallback){
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        content: message
      },
      autoFocus: true
    }).afterClosed().subscribe(result => {
      if (result){
        successCallback();
      }
    });
  }

  private alertDialog(message, title = null, cssClass = null, successCallback = null){
    this.dialog.open(AlertDialogComponent, {
      data: {
        title: title,
        content: message,
        cssClass: cssClass
      },
      autoFocus: true
    }).afterClosed().subscribe(result => {
      if (successCallback) {
        successCallback();
      }
    });
  }

  /* tutorial */
  public rowClick(regularityType){
    this.increaseTask('row', 0, regularityType);
  }

  public showCurrentTask(){
    if (this.tasks.length > this.currentTaskIndex){
      let text = this.tasks[this.currentTaskIndex].text;
      if (text){
        this.alertDialog(text, 'Задание ' + (this.currentTaskIndex + 1), 'current-task-dialog');
      }
    }
  }

  private initTutorialLevel(startResult){
    if (this.difficultyLevel == 0){
      this.countLevels = 2;

      this.currentTaskIndex = 0;
      this.timerSet = true; // don't start the timer
      this.currentLevel = startResult.TutorialLevel;

      this.tasks = [];
      for (let i = 0; i < this.currentLevel.Tasks.length; i++){
        let currentTask = this.currentLevel.Tasks[i];

        let task = createTask(currentTask.Name, currentTask.Text);

        task.anySubtask = currentTask.AnySubtask;
        task.subtasks = currentTask.Subtasks;

        task.additionalCondition = currentTask.ApplyCondition == null
          ? null
          : currentTask.ApplyCondition == 'fixedType'
            ? fixedTypePredicate(Number.parseInt(currentTask.ConditionParameter))
            : currentTask.ApplyCondition == 'dynamicType' && currentTask.ConditionParameter == 'getEnabled'
              ? dynamicTypePredicate(this.getEnabledRegularityType, this)
              : null;

        this.tasks.push(task);
      }

      // if (this.currentLevel.Level == 1){
      //   initLevel1(this);
      // }
      // if (this.currentLevel.Level == 2){
      //   initLevel2(this);
      // }

      postProcessTasks(this);

      this.showCurrentTask();
    }

    // function initLevel1(self){
    //   let currentTask;
    //
    //   currentTask = createTask('understand', 'Прочитайте описание и нажмите на подсвеченную кнопку "Понятно".');
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('row', 'Подсвеченная строка таблицы содержит числа-подсказки и информацию о прогрессе для выбранного типа закономерности: количество найденных и количество существующих закономерностей в числе. Нажмите на подсвеченную строку таблицы, чтобы перейти к следующему заданию.');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('digit', 'Выделите подсвеченные цифры.');
    //   currentTask.anySubtask = false;
    //   currentTask.subtasks = [0,1,1,1,0,1];
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('btnCheckSuccess', 'Нажмите на подсвеченную кнопку "Проверить", чтобы найти закономерность выбранного типа.');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('tooltip', 'Нажмите на подсвеченные знаки вопроса рядом с названиями в таблице, чтобы посмотреть значение числа-подсказки для каждого из типов закономерностей.');
    //   currentTask.anySubtask = false;
    //   currentTask.subtasks = [0,0,0];
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('toggleHints', 'Нажмите на подсвеченную кнопку "Убрать кнопки-подсказки". Обратите внимание, что все оранжевые кнопки-подсказки исчезают.');
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('toggleHints', 'Нажмите на подсвеченную кнопку "Вернуть кнопки-подсказки". Обратите внимание, что все оранжевые кнопки-подсказки вновь появляются.');
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('hintRegNum', 'Заменить текст');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('btnCheckSuccess', 'Заменить текст');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('hintRegType', 'Заменить текст');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('btnCheckSuccess', 'Заменить текст');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('hintRandom', 'Заменить текст');
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('btnCheckSuccess', 'Заменить текст');
    //   currentTask.additionalCondition = dynamicTypePredicate(self.getEnabledRegularityType, self);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('endGame', 'Заменить текст');
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('showNotFound', 'Заменить текст');
    //   self.tasks.push(currentTask);
    // }
    //
    // function initLevel2(self){
    //   let currentTask;
    //
    //   currentTask = createTask('tooltip');
    //   currentTask.anySubtask = false;
    //   currentTask.subtasks = [0];
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('digit');
    //   currentTask.anySubtask = false;
    //   currentTask.subtasks = [0,1,1,1,0,1];
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('btnCheckSuccess');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    //
    //   currentTask = createTask('digitsAndBtnCheckSuccess');
    //   currentTask.additionalCondition = fixedTypePredicate(1);
    //   self.tasks.push(currentTask);
    // }

    function createTask(controlName, text = null){
      return {
        controlName: controlName,
        anySubtask: null,
        subtasks: null,
        additionalCondition: null,
        text: text
      };
    }

    function postProcessTasks(that){
      if (that.currentLevel.Level < that.countLevels){
        that.tasks.push(createTask('nextLevel', 'Нажмите кнопку "Следующий уровень", чтобы перейти на следующий уровень.'));
      }
      if (that.currentLevel.Level == that.countLevels) {
        that.tasks.push(createTask('endTutorial', 'Нажмите кнопку "Завершить обучение", чтобы завершить обучение.'));
      }

      for (let i = 0; i < that.tasks.length; i++){
        let task = that.tasks[i];
        if (task.anySubtask === null){
          task.anySubtask = true;
        }
        if (task.subtasks === null){
          task.subtasks = [];
          task.subtasks.push(0);
        }
        if (task.additionalCondition === null){
          task.additionalCondition = truePredicate;
        }
      }
    }

    // Predicates
    function truePredicate(param1, param2){
      return true;
    }

    function fixedTypePredicate(comparison){
      function result(param1, type){
        return !type || type == comparison;
      }
      return result;
    }

    function dynamicTypePredicate(getTypeFunction, that){
      function result(param1, type){
        return !type || type == getTypeFunction(that);
      }
      return result;
    }
  }

  public getOverOverlayClass(controlName, subIndex = 0, type = null){
    return this.isTutorialActiveElement(controlName, subIndex, type)
      ? 'over-tutorial-overlay'
      : null;
  }

  public getOverlayedClass(controlName, subIndex = 0, type = null){
    return this.isTutorialActiveElement(controlName, subIndex, type)
      ? 'overlayed-control'
      : null;
  }

  public getTutorialClass(controlName, subIndex = 0, type = null){
    return this.isTutorialActiveElement(controlName, subIndex, type)
      ? 'highlighted-control'
      : null;
  }

  public isTutorialActiveElement(controlName, subIndex = 0, type = null){
    if (!this.tasks){
      return null;
    }

    let indexes = [];
    for (let i = 0; i < this.tasks.length; i++){
      if (this.tasks[i].controlName == controlName){
        indexes.push(i);
      }
    }

    for (let i = 0; i < indexes.length; i++){
      let index = indexes[i];
      let task = this.tasks[index];

      if (task){
        let result = this.isTutorialActiveElementInner(index, subIndex, task.additionalCondition(subIndex, type));
        if (result){
          return result;
        }
      }
    }

    return false;
  }

  public increaseTask(controlName, subIndex = 0, type = null){
    if (!this.tasks){
      return false;
    }

    let indexes = [];
    for (let i = this.tasks.length - 1; i >= 0; i--){
      if (this.tasks[i].controlName == controlName){
        indexes.push(i);
      }
    }

    var result = false;

    for (let i = 0; i < indexes.length; i++){
      let index = indexes[i];
      let task = this.tasks[index];

      if (task){
        result = this.increaseTaskInner(index, subIndex, task.additionalCondition(subIndex, type)) ||
                 result;
      }
    }

    return result;
  }

  private increaseTaskInner(index, subtaskIndex = 0, additionalCondition = true, that = this){
    if (that.difficultyLevel == 0 && additionalCondition && that.currentTaskIndex == index){
      that.tasks[index].subtasks[subtaskIndex]++;

      if (that.tasks[index].anySubtask){
        that.currentTaskIndex++;
        that.showCurrentTask();
      }
      else if (that.tasks[index].subtasks.every(x => x)){
        that.currentTaskIndex++;
        that.showCurrentTask();
      }

      return true;
    }

    return false;
  }

  private isTutorialActiveElementInner(index, subtaskIndex = 0, additionalCondition = true, that = this){
    if (that.difficultyLevel == 0 && additionalCondition && that.currentTaskIndex == index && that.tasks[index].subtasks[subtaskIndex] == 0){
      return true;
    }

    return false;
  }

  private getEnabledRegularityType(that){
    return that.regularityTypes.find(x => x.enabled)?.type;
  }
}

interface StartModel {
  GameId: string;
  Number: number;
  Length: number;
  DifficultyLevel: number;
  ExistRegularityInfos: StartRegularityInfo[];
  ExistRegularityTypeCounts: { [key: number]: number };
  TutorialLevel: TutorialLevel;
}

interface StartRegularityInfo {
  Type: number;
  RegularityNumber: number;
  ReverseRegularityNumber: number;
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
  ReverseRegularityNumber: number;
  FoundStatus: FoundStatus;
  Numbers: FoundNumber[];
}

class FoundNumber{
  Position: number;
  Length: number;
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
  Numbers: FoundNumber[];
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
  FoundNumbers: FoundNumber[];
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

interface TutorialLevel{
  Level: number;
  Title: string;
  Text: string;
  Tasks: TutorialTask[];
}

interface TutorialTask {
  Order: number;
  Text: string;
  Name: string;
  AnySubtask: boolean;
  Subtasks: number[];
  ApplyCondition: string;
  ConditionParameter: string;
}

interface Predicate<T1, T2> {
  (value1: T1, value2: T2): boolean;
}
