import { Component } from '@angular/core';
import { SearchAssetComponent } from "../../core/search-asset/search-asset.component";
import { HeaderComponent } from "../../core/Components/header/header.component";
import { LastNewsComponent } from "../../core/Components/last-news/last-news.component";
import { MostTradedComponent } from "../../core/Components/most-traded/most-traded.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SearchAssetComponent, HeaderComponent, LastNewsComponent, MostTradedComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
