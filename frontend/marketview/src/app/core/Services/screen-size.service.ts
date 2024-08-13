// screen-size.service.ts
import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ScreenSizeService {
  width: number = 0;
  height: number = 0;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    if (isPlatformBrowser(this.platformId)) {
      this.width = window.innerWidth;
      this.height = window.innerHeight;
      window.addEventListener('resize', this.onResize.bind(this));
    }
  }

  private onResize(): void {
    this.width = window.innerWidth;
    this.height = window.innerHeight;
  }

}
