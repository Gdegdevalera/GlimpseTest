import { ActionReducerMap, createReducer, on } from '@ngrx/store';
import { filterAction, loadSuccessAction } from './app.actions';
import { ChartItem, BarItem } from './app.component';
import { dateToBarItemName } from './app.helpers';

export interface IState {
  app: IAppState;
}

export interface IAppState {
    selected: string;
    categories: ChartItem[];
    categoriesByHour: BarItem[];
}

export const initialState: IAppState = {
    selected: '',
    categories: [],
    categoriesByHour: []
};
 
const _appReducer = createReducer(
  initialState,
  
  on(loadSuccessAction, (state, props) => { 
      const categories = props.categories
          .map(c => { return { name: c.name, value: c.total }});

      const categoriesByHour = props.categoriesByHour
          .reduce((result, curr) => {
            const hour = dateToBarItemName(curr.hour);
            const groupItem = result.find(x => x.name === hour);
            const chartItem = { name: curr.name, value: curr.total };

            if(!groupItem) {
              result.push({ name: hour, series: [ chartItem ]});
            } else {
              groupItem.series.push(chartItem);
              groupItem.series.sort((a, b) => compare(a.name, b.name));
            }
            
            return result;
          }, [] as BarItem[]);

      return { ...state, categories, categoriesByHour };
  }),
  
  on(filterAction, (state, props) => { 
    if (state.selected === props.name) {
      return { ...state, selected: '' };
    } else {
      return { ...state, selected: props.name };
    }
  }),
);
 
function compare(a: string, b: string) {
    if (a < b) { return -1; }
    if (a > b) { return 1; }
    return 0;
}


export function getAppReducer(): ActionReducerMap<IState, any> {
  return { app: _appReducer };
}