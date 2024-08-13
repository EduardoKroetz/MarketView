import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import SearchAsset from '../../core/Models/SearchAsset';
import { MostTradedService } from '../../core/Services/most-traded.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-trending-assets',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './trending-assets.component.html',
  styleUrl: './trending-assets.component.css'
})
export class TrendingAssetsComponent {
  trendingAssets: SearchAsset[] = [];

  constructor(private mostTradedService: MostTradedService) { }

  ngOnInit(): void {
    this.mostTradedService.mostTraded$.subscribe(assets => {
      this.trendingAssets = assets;
    });
  }
}
