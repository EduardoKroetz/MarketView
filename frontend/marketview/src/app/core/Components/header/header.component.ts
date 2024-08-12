import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BurgerDropDownComponent } from "../burger-drop-down/burger-drop-down.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, BurgerDropDownComponent, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  burgerIsOpen = false;

  closeBurger(): void {
    this.burgerIsOpen = false;
  }
}
