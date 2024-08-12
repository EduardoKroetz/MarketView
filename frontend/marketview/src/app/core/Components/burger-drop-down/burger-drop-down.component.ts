import { Component, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-burger-drop-down',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './burger-drop-down.component.html',
  styleUrl: './burger-drop-down.component.css'
})
export class BurgerDropDownComponent implements OnInit {
  @Input() burgerIsOpen: boolean = false;
  @Output() closeBurgerEvent = new EventEmitter<void>(); 
  private elementRef: ElementRef;

  constructor(private el: ElementRef) {
    this.elementRef = el;
  }

  ngOnInit(): void {
    document.addEventListener('click', this.closeBurger);
  }

  ngOnDestroy(): void {
    document.removeEventListener('click', this.closeBurger);
  }

  closeBurger = (event: MouseEvent) => {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.closeBurgerEvent.emit(); 
    }
  }
}
