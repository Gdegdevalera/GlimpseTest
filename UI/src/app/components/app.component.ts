import { Component, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { filter, load } from '../actions/app.actions';
import { IState } from '../reducers/app.reducer';
import { barSelector, colorsSelector, pieSelector } from '../selectors/app.selectors';

export interface ChartItem {
  name: string;
  value: number;
}

export interface BarItem {
  name: string | null;
  series: ChartItem[];
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  dataPie$ = this.store$.pipe(select(pieSelector));
  dataBar$ = this.store$.pipe(select(barSelector));
  itemColors$ = this.store$.pipe(select(colorsSelector));

  constructor(private store$: Store<IState>) {   
  }

  ngOnInit(): void { 
    this.store$.dispatch(load());
  }

  onSelectPie(data: ChartItem): void {
    this.store$.dispatch(filter(data));
  }
}