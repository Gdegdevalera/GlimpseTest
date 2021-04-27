import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Category {
  id: number;
  name: string;
  total: number;
} 

export interface CategoryByHour {
  total: number;
  hour: string;
} 

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  
  constructor(private http: HttpClient) { }

  public getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(environment.apiUrl + 'categories/');
  }
  
  public getCategoriesByHours(): Observable<CategoryByHour[]> {
    return this.http.get<CategoryByHour[]>(environment.apiUrl + 'categories/by-hours');
  }

  public getCategoriesByHoursFiltered(name: string): Observable<CategoryByHour[]> {
    return this.http.get<CategoryByHour[]>(environment.apiUrl + `categories/by-hours/${encodeURIComponent(name)}`);
  }
}
