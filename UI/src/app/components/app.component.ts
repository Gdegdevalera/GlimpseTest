import { Component, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { filter, load } from '../actions/app.actions';
import { colors } from './app.colors';
import { IState } from '../reducers/app.reducer';
import { barSelector, pieSelector } from '../selectors/app.selectors';

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
  dataPie$ = this.store$.pipe(select(pieSelector));
  dataBar$ = this.store$.pipe(select(barSelector));
  itemColors: ItemColor[] = [];

  constructor(private store$: Store<IState>) {    
    this.dataPie$.subscribe(c => {
      this.itemColors = (c || []).map((x, i) => { 
        return { name: x.name, value: colors[i % colors.length] };
      })
    });
  }

  ngOnInit(): void {
    this.store$.dispatch(load());
  }

  onSelectPie(data: ChartItem): void {
    this.store$.dispatch(filter(data));
  }

  customColors() {
    return this.itemColors;
  }
}