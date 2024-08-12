import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MostTradedService {

  constructor(private http: HttpClient) { }

  getMostTraded() {
    return this.http.get(`${environment.api_url}/assets/most-traded`)
  }
}
