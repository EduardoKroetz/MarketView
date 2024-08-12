import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MostTradedComponent } from './most-traded.component';

describe('MostTradedComponent', () => {
  let component: MostTradedComponent;
  let fixture: ComponentFixture<MostTradedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MostTradedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MostTradedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
