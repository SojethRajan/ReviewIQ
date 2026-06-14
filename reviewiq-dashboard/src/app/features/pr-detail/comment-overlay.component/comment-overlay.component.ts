import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ReviewComment } from '../../../core/models';
import { CategoryChipComponent } from '../../../shared/category-chip.component/category-chip.component';
import { SeverityBadgeComponent } from '../../../shared/severity-badge.component/severity-badge.component';

@Component({
    selector: 'app-comment-overlay',
    imports: [CommonModule, SeverityBadgeComponent, CategoryChipComponent],
    templateUrl: './comment-overlay.component.html',
    styleUrl: './comment-overlay.component.scss',
})
export class CommentOverlayComponent {
    @Input() comments: ReviewComment[] = [];
}
