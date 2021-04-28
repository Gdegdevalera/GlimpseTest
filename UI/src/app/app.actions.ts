import { createAction, props } from '@ngrx/store';
import { CategoriesModel } from './services/api.service';

export const loadAction = createAction('[App Component] Load');
export const loadSuccessAction = createAction('[App Component] Load Success', props<CategoriesModel>());
export const loadErrorAction = createAction('[App Component] Load Error');

export const filterAction = createAction('[App Component] Filter', props<{name: string}>());
