import { createAction, props } from '@ngrx/store';
import { CategoriesModel } from '../models/category.model';

export const load = createAction('[App Component] Load');
export const loadSuccess = createAction('[App Component] Load Success', props<CategoriesModel>());
export const loadError = createAction('[App Component] Load Error');

export const filter = createAction('[App Component] Filter', props<{name: string}>());
