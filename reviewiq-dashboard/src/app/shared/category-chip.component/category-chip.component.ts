import { Component, Input } from '@angular/core';
import { Category } from '../../core/models';

@Component({
  selector: 'app-category-chip.component',
  imports: [],
  templateUrl: './category-chip.component.html',
  styleUrl: './category-chip.component.scss',
})
export class CategoryChipComponent {
  @Input() category!: Category;

  get chipClass(): string {
    switch (this.category) {
      case 'Security':
        return 'riq-chip riq-chip--security';
      case 'Bug':
        return 'riq-chip riq-chip--bug';
      case 'Performance':
        return 'riq-chip riq-chip--performance';
      case 'Style':
        return 'riq-chip riq-chip--style';
    }
  }
}
