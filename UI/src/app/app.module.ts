import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BarChartModule, PieChartModule } from '@swimlane/ngx-charts';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    BarChartModule,
    PieChartModule    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
