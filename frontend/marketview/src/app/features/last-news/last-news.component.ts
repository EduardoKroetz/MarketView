import { Component, OnInit } from '@angular/core';
import NewArticle from '../../core/Models/NewArticle';
import { LastNewsService } from '../../core/Services/last-news.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-last-news',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './last-news.component.html',
  styleUrl: './last-news.component.css'
})
export class LastNewsComponent implements OnInit {
  lastNews : NewArticle[] = [];

  constructor (private lastNewsService: LastNewsService) {}


  ngOnInit(): void {
    this.lastNewsService.lastNews$.subscribe(news => 
      this.lastNews = news
    )
  }

}
