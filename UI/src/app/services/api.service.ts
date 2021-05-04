import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CategoriesModel } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  
  constructor(private http: HttpClient) { }

  public getCategories(): Observable<CategoriesModel> {
    return this.http.get<CategoriesModel>(environment.apiUrl + 'categories/');
  }
}
