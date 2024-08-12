import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, ElementRef, Input, Output, EventEmitter, Inject, PLATFORM_ID } from '@angular/core';
import { RouterLink } from '@angular/router';
import SearchAsset from '../../Models/SearchAsset';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './search-results.component.html',
  styleUrl: './search-results.component.css'
})
export class SearchResultsComponent {
  @Input() assets: SearchAsset[] = [];
  @Input() resultsIsOpen = false;
  @Output() closeResultsEvent = new EventEmitter<void>();
  private elementRef: ElementRef;
  private isBrowser: boolean;

  constructor(private el: ElementRef, @Inject(PLATFORM_ID) private platformId: Object) {
    this.elementRef = el;
    this.isBrowser = isPlatformBrowser(this.platformId);
  }
  
  ngOnInit(): void {
    if (this.isBrowser) {
      document.addEventListener('click', this.closeResults);
    }
  }

  ngOnDestroy(): void {
    if (this.isBrowser) {
      document.removeEventListener('click', this.closeResults);
    }
  }

  closeResults = (event: MouseEvent) => {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.closeResultsEvent.emit(); 
    }
  }
}
