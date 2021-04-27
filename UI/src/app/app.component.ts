import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'UI';
  view: [number, number] = [700, 400];
  data = [
    {
      "name": "Germany",
      "value": 40632
    },
    {
      "name": "United States",
      "value": 50000
    },
    {
      "name": "France",
      "value": 36745
    },
    {
      "name": "United Kingdom",
      "value": 36240
    },
    {
      "name": "Spain",
      "value": 33000
    },
    {
      "name": "Italy",
      "value": 35800  
    }
  ]
}
