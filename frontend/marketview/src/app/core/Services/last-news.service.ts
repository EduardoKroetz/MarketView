import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import NewArticle from '../Models/NewArticle';

@Injectable({
  providedIn: 'root'
})
export class LastNewsService {
  private lastNewsSubject = new BehaviorSubject<NewArticle[]>([]);
  lastNews$ = this.lastNewsSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadLastNews();
  }

  private loadLastNews(): void {
    this.http.get(`${environment.api_url}/assets/last-news`).subscribe(
      (data: any) => {
        this.lastNewsSubject.next(data.data);
      }
    );
  }
}
