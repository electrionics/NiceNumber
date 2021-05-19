import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];
  public test: number;

  constructor(http: HttpClient, @Inject('BASE_API_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + 'WeatherForecast/Get').subscribe(result => {
      this.forecasts = result;
      http.get<number>(baseUrl + 'Game/Test').subscribe(result => {
        this.test = result;
      }, error => console.error(error));
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
