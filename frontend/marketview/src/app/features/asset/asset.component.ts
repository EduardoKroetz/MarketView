import { Component, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChartData, ChartOptions } from 'chart.js';
import { CompanyInfo } from '../../core/Models/CompanyInfo';
import { AssetPageService } from '../../core/Services/asset-page.service';
import NewArticle from '../../core/Models/NewArticle';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ScreenSizeService } from '../../core/Services/screen-size.service';

@Component({
  selector: 'app-asset',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.css']
})
export class AssetComponent implements OnInit {
  companyInfo: CompanyInfo = {historicalDataPrice: [],logoUrl: "", longName: "", summaryProfile: { address1: "", city: "", country: "", industry: "", longBusinessSummary: "", sector: "", state: "", website: "" }, symbol: "", usedInterval: "", usedRange: ""  }
  lastCompanyNews: NewArticle[] = [];
  symbol: string = "";
  loading = true;
  location = this.companyInfo.summaryProfile.city && this.companyInfo.summaryProfile.state && this.companyInfo.summaryProfile.country ? `${this.companyInfo.summaryProfile.city}, ${this.companyInfo.summaryProfile.state},  ${this.companyInfo.summaryProfile.country}` : "Nenhuma informação disponível"
  public isBrowser: boolean;
  screenWidth$ = 0;
  public lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: []
  };
  public lineChartOptions: ChartOptions<'line'> = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
      },
      tooltip: {
        enabled: true,
        mode: "nearest",
        intersect: false,
        backgroundColor: 'gray', 
        titleFont: {
          size: 14, 
          weight: 'bold'
        },
        bodyFont: {
          size: 12 
        }
      }
    }
  };

  constructor(private route: ActivatedRoute, private assetPageService: AssetPageService, @Inject(PLATFORM_ID) platformId: Object, private screenSizeService: ScreenSizeService) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.screenWidth$ = this.screenSizeService.width;
    this.lineChartData.datasets[0]
  }

  ngOnInit(): void {
    this.symbol = this.route.snapshot.paramMap.get('symbol')!;

    this.assetPageService.getCompanyInfo(this.symbol).subscribe((response: any) => {
      this.companyInfo = response.data.assetData;
      this.lastCompanyNews = response.data.assetNews;

      this.lineChartData = {
        labels: this.companyInfo.historicalDataPrice.map(data => new Date(data.date * 1000).toLocaleDateString('pt-BR', {
          day: '2-digit',
          month: '2-digit',
        })),
        datasets: [
          {
            data: this.companyInfo.historicalDataPrice.map(data => data.close),
            borderColor: 'rgba(0, 0, 0, 100)',
            backgroundColor: 'rgb(52, 58, 64)',
            borderWidth: 3,
            pointBackgroundColor: 'gray',
            pointBorderColor: 'gray',
            pointHoverBackgroundColor: 'gray',
            pointHoverBorderColor: 'gray',
            pointRadius:  this.screenWidth$ < 480 ? 1 : this.screenWidth$ < 560 ? 2 : 3,
            pointHoverRadius: 7,
            fill: true
          },
        ]
      };
      
      this.loading = false;
    });
  }

}
