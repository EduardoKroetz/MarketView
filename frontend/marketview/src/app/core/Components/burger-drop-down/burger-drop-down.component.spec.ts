import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BurgerDropDownComponent } from './burger-drop-down.component';

describe('BurgerDropDownComponent', () => {
  let component: BurgerDropDownComponent;
  let fixture: ComponentFixture<BurgerDropDownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BurgerDropDownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BurgerDropDownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
