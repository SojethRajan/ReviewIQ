import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CodeReview } from '../../../core/models';
import { getScoreClass, getCriticalCount, getWarningCount, getSuggestionCount, formatReviewDate, getScoreLabel, getScoreLabelClass } from '../../../core/utils/review-utils';

@Component({
    selector: 'app-summary-card',
    imports: [CommonModule],
    templateUrl: './summary-card.component.html',
    styleUrl: './summary-card.component.scss',
})
export class SummaryCardComponent {
    @Input() review!: CodeReview;

    protected getScoreClass = getScoreClass;
    protected getScoreLabelClass = getScoreLabelClass;
    protected getScoreLabel = getScoreLabel;
    protected getCriticalCount = getCriticalCount;
    protected getWarningCount = getWarningCount;
    protected getSuggestionCount = getSuggestionCount;
    protected formatReviewDate = formatReviewDate;
}