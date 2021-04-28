import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BarChartModule, PieChartModule } from '@swimlane/ngx-charts';
import { HttpClientModule } from '@angular/common/http';
import { StoreModule } from '@ngrx/store';
import { getAppReducer as getAppReducers } from './app.reducer';
import { EffectsModule } from '@ngrx/effects';
import { AppEffects } from './app.effects';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    BarChartModule,
    PieChartModule,
    HttpClientModule,
    StoreModule.forRoot(getAppReducers(), {}),
    EffectsModule.forRoot([ AppEffects ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
