import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrendingAssetsComponent } from './trending-assets.component';

describe('TrendingAssetsComponent', () => {
  let component: TrendingAssetsComponent;
  let fixture: ComponentFixture<TrendingAssetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrendingAssetsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrendingAssetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
