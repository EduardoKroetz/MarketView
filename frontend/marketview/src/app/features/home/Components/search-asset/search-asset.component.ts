import { Component } from '@angular/core';
import { SearchAssetService } from '../../../../core/Services/search-asset.service';
import SearchAsset from '../../../../core/Models/SearchAsset';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SearchResultsComponent } from "../../../../core/Components/search-results/search-results.component";

@Component({
  selector: 'app-search-asset',
  standalone: true,
  imports: [CommonModule, FormsModule, SearchResultsComponent],
  templateUrl: './search-asset.component.html',
  styleUrl: './search-asset.component.css'
})
export class SearchAssetComponent {
  assets: SearchAsset[] = [];
  symbol: string = "";
  resultsIsOpen = false;

  constructor (private searchAssetService: SearchAssetService) {}

  search(event: Event): void {
    event.preventDefault();
    this.searchAssetService.searchAssetsAsync(this.symbol).subscribe(
      (response: any) => {
        this.assets = response.data;
        this.resultsIsOpen = true;
      },
      (error) => {
        console.log("Erro ocorreu: "+ error)
      }
    );
      
  }

  closeResults() {
    this.resultsIsOpen = false;
  }
}
