import { Component, OnInit } from '@angular/core';
import SearchAsset from '../../Models/SearchAsset';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MostTradedService } from '../../Services/most-traded.service';

@Component({
  selector: 'app-most-traded',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './most-traded.component.html',
  styleUrl: './most-traded.component.css'
})
export class MostTradedComponent implements OnInit {
  assets : SearchAsset[] = [];

  constructor (private mostTradedService: MostTradedService ) {}

  ngOnInit(): void {
    this.mostTradedService.mostTraded$.subscribe(assets => {
      this.assets = assets;
    });
  }
}
