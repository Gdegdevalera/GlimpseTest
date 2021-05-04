import { Action, createReducer, on } from '@ngrx/store';
import { filter, loadSuccess } from '../actions/app.actions';
import { Category } from '../models/category.model';

export interface IState {
  app: IAppState;
}

export interface IAppState {
    selected: string;
    categories: Category[];
}

const initialState: IAppState = {
    selected: '',
    categories: []
};
 
const _appReducer = createReducer(
  initialState,
  
  on(loadSuccess, (state, props) => { 
      return { ...state, categories: props.categories };
  }),
  
  on(filter, (state, props) => { 
    if (state.selected === props.name) {
      return { ...state, selected: '' };
    } else {
      return { ...state, selected: props.name };
    }
  }),
);

export function appReducer(state: IAppState = initialState, action: Action): IAppState {
  return _appReducer(state, action);  
}