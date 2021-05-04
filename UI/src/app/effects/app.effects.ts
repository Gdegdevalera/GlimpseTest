import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { ApiService } from '../services/api.service';
 
@Injectable()
export class AppEffects {
 
  loadMovies$ = createEffect(() =>
    this.actions$.pipe(
      ofType('[App Component] Load'),
      mergeMap(() => this.apiService.getCategories()
        .pipe(
          map(data => ({ type: '[App Component] Load Success', ...data })),
          catchError(() => of({ type: '[App Component] Load Error' }))
        )
      )
    )
  );
 
  constructor(
    private actions$: Actions,
    private apiService: ApiService
  ) {}
}