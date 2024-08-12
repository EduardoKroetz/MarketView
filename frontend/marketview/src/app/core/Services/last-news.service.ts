import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LastNewsService {

  constructor(private http: HttpClient) { }

  getLastNews() {
    return this.http.get(`${environment.api_url}/assets/last-news`)
  }
}
