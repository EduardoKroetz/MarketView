import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { AssetComponent } from './features/asset/asset.component';
import { BaseLayoutComponent } from './core/layout/base-layout/base-layout.component';

export const routes: Routes = [
  {
    path: "", component: BaseLayoutComponent, 
    children: [
    {path: "", component: HomeComponent},
    {path: "assets/:symbol", component: AssetComponent }
  ] },
];
