import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { AssetComponent } from './features/asset/asset.component';
import { BaseLayoutComponent } from './core/layout/base-layout/base-layout.component';
import { LastNewsComponent } from './features/last-news/last-news.component';
import { TrendingAssetsComponent } from './features/trending-assets/trending-assets.component';

export const routes: Routes = [
  {
    path: "", component: BaseLayoutComponent, 
    children: [
    {path: "", component: HomeComponent},
    {path: "assets/:symbol", component: AssetComponent },
    {path: "last-news", component: LastNewsComponent},
    {path: "most-traded", component: TrendingAssetsComponent}
  ] },
];
