import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AssetPageService {

  constructor(private http: HttpClient) { }

  getCompanyInfo(symbol: string) {
    return this.http.get(`${environment.api_url}/assets/${symbol}`)
  }

  getCompanyNews(symbol: string) {
    return this.http.get(`${environment.api_url}/assets/news/${symbol}`)
  }
}
