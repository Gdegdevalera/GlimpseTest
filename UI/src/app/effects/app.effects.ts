import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, switchMap, catchError } from 'rxjs/operators';
import { load, loadError, loadSuccess } from '../actions/app.actions';
import { ApiService } from '../services/api.service';
 
@Injectable()
export class AppEffects {
 
  loadCategories$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      switchMap(() => this.apiService
        .getCategories()
        .pipe(map(data => loadSuccess(data)))
      ),
      catchError(() => of(loadError()))
    )
  );
 
  constructor(
    private actions$: Actions,
    private apiService: ApiService
  ) {}
}