import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import SearchAsset from '../Models/SearchAsset';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MostTradedService {
  private mostTraded = new BehaviorSubject<SearchAsset[]>([]);
  mostTraded$ = this.mostTraded.asObservable();

  constructor(private http: HttpClient) {
    this.getMostTraded();
   }

  private getMostTraded() {
    this.http.get(`${environment.api_url}/assets/most-traded`).subscribe(
      (response: any) => {
        this.mostTraded.next(response.data)
      }
    )
  }
}
