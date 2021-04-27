import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  private datePipe = new DatePipe('en-US');

  constructor(api: ApiService) {
    api.getCategories()
        .subscribe(x => {
          this.dataPie = x.map(c => { return { name: c.name, value: c.total }});
        });
    api.getCategoriesByHours()
        .subscribe(x => {
          this.dataBar = x.map(c => { return { name: this.datePipe.transform(c.hour, 'HH:mm'), value: c.total }});
        });
  }

  title = 'UI';
  view: [number, number] = [700, 400];
  dataPie: {
      name: string;
      value: number;
    }[] = [];
    
  dataBar: {
    name: string | null;
    value: number;
  }[] = [];

  onSelectPie(data: any): void {
    console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }
}
