import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface CategoriesModel
{
  categories: Category[]
}

export interface Category {
  id: number;
  name: string;
  total: number;
  hour: string;
} 

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  
  constructor(private http: HttpClient) { }

  public getCategories(): Observable<CategoriesModel> {
    return this.http.get<CategoriesModel>(environment.apiUrl + 'categories/');
  }
}
