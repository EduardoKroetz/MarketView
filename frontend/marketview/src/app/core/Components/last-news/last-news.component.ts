import { Component, OnInit } from '@angular/core';
import NewArticle from '../../Models/NewArticle';
import { LastNewsService } from '../../Services/last-news.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-last-news',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './last-news.component.html',
  styleUrl: './last-news.component.css'
})
export class LastNewsComponent implements OnInit {
  lastNews: NewArticle[] = [];

  constructor (private lastNewsService: LastNewsService) {}

  ngOnInit(): void {
    this.lastNewsService.getLastNews()
      .subscribe(
        (data: any) =>{
          this.lastNews = data.data
        }
      )
  }
}
