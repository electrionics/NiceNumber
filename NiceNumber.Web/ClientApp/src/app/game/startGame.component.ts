import {Component, Inject, OnDestroy, OnInit} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {MatDialog} from "@angular/material/dialog";
import {Router} from '@angular/router';
import {PassGameParametersService} from "./passGameParametersService";

@Component({
  selector: 'app-start-game',
  templateUrl: './startGame.component.html'
})
export class StartGameComponent implements OnInit, OnDestroy {
  public difficultyLevel: number;
  public difficultyLevels: { type: number; label: string; }[];

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string, public dialog: MatDialog, private router: Router, public dataService: PassGameParametersService) {
    this.difficultyLevels = [];
    //this.difficultyLevels.push({type: 0, label: "Обучение" });
    this.difficultyLevels.push({type: 1, label: "Лёгкий" });
    this.difficultyLevels.push({type: 2, label: "Средний" });
    this.difficultyLevels.push({type: 3, label: "Тяжелый" });
  }

  public navigateStart(){
    this.router.navigateByUrl('/game');
  }

  public navigateTutorial(){
    this.difficultyLevel = 0;
    this.router.navigateByUrl('/tutorial');
  }

  ngOnInit() {
    const defaultDifficultyLevel = 1;

    if (this.dataService.parameter){
      this.difficultyLevel = this.dataService.parameter.difficultyLevel;
      if (!this.difficultyLevel){
        this.difficultyLevel = defaultDifficultyLevel;
      }
    }
    else{
      this.difficultyLevel = defaultDifficultyLevel;
    }
  }

  ngOnDestroy() {
    this.dataService.parameter = {
      difficultyLevel: this.difficultyLevel
    };
  }
}
