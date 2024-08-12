import { Component } from '@angular/core';
import { SearchAssetComponent } from "./Components/search-asset/search-asset.component";
import { HeaderComponent } from "../../core/Components/header/header.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SearchAssetComponent, HeaderComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
