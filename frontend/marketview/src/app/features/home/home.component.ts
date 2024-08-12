import { Component } from '@angular/core';
import { SearchAssetComponent } from "./Components/search-asset/search-asset.component";
import { HeaderComponent } from "../../core/Components/header/header.component";
import { LastNewsComponent } from "../../core/Components/last-news/last-news.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SearchAssetComponent, HeaderComponent, LastNewsComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
