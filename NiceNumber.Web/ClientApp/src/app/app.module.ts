import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { AuthorComponent } from './author/author.component';
import { StartGameComponent } from "./game/startGame.component";
import { GameComponent } from "./game/game.component";
import { RecordsComponent } from "./records/records.component";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTooltipModule } from "@angular/material/tooltip";
import { UpdateRecordDialogComponent } from "./game/updateRecordDialog.component";
import { StartGameDialogComponent } from "./game/startGameDialog.component";
import { StartGameRedirectComponent } from "./game/startGameRedirect.component";
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { ConfirmDialogComponent } from "./common/confirm.component";
import { AlertDialogComponent } from "./common/alert.component";
import { CookieService } from "./common/cookieService.component";
import { PassGameParametersService } from "./game/passGameParametersService";
import { TutorialComponent } from "./game/tutorial.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FooterComponent,
    HomeComponent,
    AboutComponent,
    AuthorComponent,
    StartGameComponent,
    GameComponent,
    TutorialComponent,
    UpdateRecordDialogComponent,
    StartGameDialogComponent,
    StartGameRedirectComponent,
    RecordsComponent,
    ConfirmDialogComponent,
    AlertDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: StartGameComponent, pathMatch: 'full'},
      {path: 'game', component: GameComponent},
      {path: 'gameRedirect', component: StartGameRedirectComponent},
      {path: 'tutorial', component: TutorialComponent},
      {path: 'records', component: RecordsComponent},
      {path: 'about', component: AboutComponent},
      {path: 'author', component: AuthorComponent},
      {path: 'home', component: HomeComponent},
    ]),
    BrowserAnimationsModule,
    MatTooltipModule,
    MatDialogModule,
    MatButtonModule
  ],
  entryComponents: [
    UpdateRecordDialogComponent,
    StartGameDialogComponent,
    ConfirmDialogComponent,
    AlertDialogComponent
  ],
  providers: [
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: true, disableClose: true}},
    CookieService, PassGameParametersService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
