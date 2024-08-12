import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SearchAssetService {

  constructor(private http: HttpClient) { }

  searchAssetsAsync(symbol: string)
  {
    return this.http.get(`${environment.api_url}/assets/search?symbol=${symbol}&pageSize=10`)
  }
}
