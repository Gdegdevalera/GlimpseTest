import { ActionReducerMap, createReducer, on } from '@ngrx/store';
import { filterAction, loadSuccessAction } from './app.actions';
import { Category } from './services/api.service';

export interface IState {
  app: IAppState;
}

export interface IAppState {
    selected: string;
    categories: Category[];
}

export const initialState: IAppState = {
    selected: '',
    categories: []
};
 
const _appReducer = createReducer(
  initialState,
  
  on(loadSuccessAction, (state, props) => { 
      return { ...state, categories: props.categories };
  }),
  
  on(filterAction, (state, props) => { 
    if (state.selected === props.name) {
      return { ...state, selected: '' };
    } else {
      return { ...state, selected: props.name };
    }
  }),
);

export function getAppReducer(): ActionReducerMap<IState, any> {
  return { app: _appReducer };
}