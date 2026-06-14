import { CodeReview, PrFeedItem, Severity } from '../models';

export function getScoreClass(qualityScore: number, status: string): string {
    if (status === 'InProgress') return 'riq-score--analysing';
    if (qualityScore >= 85) return 'riq-score--high';
    if (qualityScore >= 65) return 'riq-score--mid';
    return 'riq-score--low';
}

export function getScoreLabelClass(qualityScore: number, status: string): string {
    if (status === 'InProgress') return 'label--analysing';
    if (qualityScore >= 85) return 'label--high';
    if (qualityScore >= 65) return 'label--mid';
    return 'label--low';
}

// Used in summary-card — shows text label below the circle
export function getScoreLabel(qualityScore: number, status: string): string {
    if (status === 'InProgress') return 'Analysing';
    if (qualityScore >= 85) return 'Good';
    if (qualityScore >= 65) return 'Fair';
    return 'Poor';
}

export function countBySeverity(
    review: CodeReview,
    severity: Severity,
): number {
    return review.comments?.filter((c) => c.severity === severity).length ?? 0;
}

export function formatReviewDate(date: string | Date | undefined): string {
    if (!date) return '—';
    return new Date(date).toLocaleString('en-IN', {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
    });
}

export function getCriticalCount(item: CodeReview): number {
    return item.comments.filter((c) => c.severity === 'Critical').length;
}

export function getWarningCount(item: CodeReview): number {
    return item.comments.filter((c) => c.severity === 'Warning').length;
}

export function getSuggestionCount(item: CodeReview): number {
    return item.comments.filter((c) => c.severity === 'Suggestion').length;
}
