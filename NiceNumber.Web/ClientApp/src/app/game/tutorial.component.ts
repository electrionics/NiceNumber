import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {MatDialog} from '@angular/material/dialog';
import {PassGameParametersService} from "./passGameParametersService";
import {GameComponent} from "./game.component";

@Component({
  selector: 'app-tutorial',
  templateUrl: './game.component.html'
})
export class TutorialComponent extends GameComponent{
  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string, dialog: MatDialog, router: Router, dataService: PassGameParametersService) {
    super(http, baseUrl, dialog, router, dataService);
  }

  protected getStartUrl(): string {
    let url = super.getStartUrl();

    let nextLevelIndex;
    if (!this.currentLevel){
      nextLevelIndex = 1;
    }
    else{
      nextLevelIndex = this.currentLevel.Level + 1;
    }
    url += ('&tutorialLevel=' + nextLevelIndex);

    return url;
  }


  protected successStart(result) {
    this.initTutorialLevel(result);

    super.successStart(result);
  }


  public toggleSelected(digit, index) {
    super.toggleSelected(digit, index);

    this.increaseTask('digit', index);
  }


  protected successCongratulations(result, regularityType) {
    this.increaseTask('btnCheckSuccess', 0, regularityType) ||
    this.increaseTask('digitsAndBtnCheckSuccess', 0, regularityType);

    super.successCongratulations(result, regularityType);
  }


  protected successEnd(result) {
    super.successEnd(result);

    this.increaseTask('endGame');
  }


  protected successHintMode(result, regularityType) {
    super.successHintMode(result, regularityType);

    this.increaseTask('hintRegNum',0, regularityType) ||
    this.increaseTask('hintRegType',0, regularityType) ||
    this.increaseTask('hintRandom');
  }


  protected successShowNotFound() {
    this.increaseTask('showNotFound');
  }


  public toggleHints(){
    super.toggleHints();

    this.increaseTask('toggleHints');
  }


  public understand(){
    super.understand();

    this.increaseTask('understand');
  }


  public toggleTooltip(tooltip, index){
    super.toggleTooltip(tooltip, index);

    this.increaseTask('tooltip', index);
  }


  public rowClick(regularityType){
    super.rowClick(regularityType);

    this.increaseTask('row', 0, regularityType);
  }


  public showCurrentTask(){
    super.showCurrentTask();

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

      postProcessTasks(this);

      this.showCurrentTask();
    }

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

  public getTutorialClass(controlName, subIndex = 0, type = null){
    return this.isTutorialActiveElement(controlName, subIndex, type)
      ? 'highlighted-control'
      : null;
  }

  public isTutorialActiveElement(controlName, subIndex = 0, type = null){
    if (!this.tasks){
      return false;
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
