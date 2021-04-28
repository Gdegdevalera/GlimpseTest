import { Component, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { filterAction, loadAction } from './app.actions';
import { colors } from './app.colors';
import { IState } from './app.reducer';
import { barSelector, pieSelector } from './app.selectors';

export interface ChartItem {
  name: string;
  value: number;
}

export interface BarItem {
  name: string | null;
  series: ChartItem[];
}

interface ItemColor {
  name: string;
  value: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  view: [number, number] = [700, 400];
  dataPie$: Observable<ChartItem[]>;
  dataBar$: Observable<BarItem[]>;
  itemColors: ItemColor[] = [];

  constructor(private store: Store<IState>) {
    this.dataPie$ = store.pipe(select(pieSelector));
    this.dataBar$ = store.pipe(select(barSelector));
    
    this.dataPie$.subscribe(c => {
      this.itemColors = c.map((x, i) => { 
        return { name: x.name, value: colors[i % colors.length] };
      })
    });
  }

  ngOnInit(): void {
    this.store.dispatch(loadAction());
  }

  customColors() {
    return this.itemColors;
  }

  onSelectPie(data: ChartItem): void {
    this.store.dispatch(filterAction(data));
  }
}