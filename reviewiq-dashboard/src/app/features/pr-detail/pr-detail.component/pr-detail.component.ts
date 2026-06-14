import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CommentOverlayComponent } from '../comment-overlay.component/comment-overlay.component';
import { SummaryCardComponent } from '../summary-card.component/summary-card.component';
import { CodeReview } from '../../../core/models';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-pr-detail',
    imports: [CommonModule, SummaryCardComponent, CommentOverlayComponent],
    templateUrl: './pr-detail.component.html',
    styleUrl: './pr-detail.component.scss',
})
export class PrDetailComponent {
    reviewId: string = '';
    review!: CodeReview;

    private stubReviews: CodeReview[] =
        [
            {
                id: '2',
                repositoryId: 'r1',
                incomingEventId: 'e2',
                pullRequestNumber: 20,
                commitSha: 'def456abc123',
                qualityScore: 98,
                status: 'Completed',
                totalComments: 1,
                gitHubReviewId: 'gh-001',
                startedOn: new Date().toISOString(),
                completedOn: new Date().toISOString(),
                comments: [
                    {
                        id: 'c1',
                        codeReviewId: '2',
                        filePath: 'src/Program.cs',
                        lineNumber: 42,
                        category: 'Style',
                        severity: 'Suggestion',
                        comment: 'Consider extracting this into a helper method.',
                        suggestion: 'Extract to a private helper.',
                        createdOn: new Date().toISOString(),
                    },
                ],
            },
            {
                id: '3',
                repositoryId: 'r1',
                incomingEventId: 'e3',
                pullRequestNumber: 19,
                commitSha: 'ghi789def456',
                qualityScore: 61,
                status: 'Completed',
                totalComments: 3,
                gitHubReviewId: 'gh-002',
                startedOn: new Date().toISOString(),
                completedOn: new Date().toISOString(),
                comments: [
                    {
                        id: 'c2',
                        codeReviewId: '3',
                        filePath: 'src/Services/DiffChunker.cs',
                        lineNumber: 18,
                        category: 'Bug',
                        severity: 'Critical',
                        comment: 'Append returns a new IEnumerable, original list is unchanged.',
                        suggestion: 'Use Add() instead of Append().',
                        createdOn: new Date().toISOString(),
                    },
                    {
                        id: 'c3',
                        codeReviewId: '3',
                        filePath: 'src/Services/GeminiService.cs',
                        lineNumber: 55,
                        category: 'Security',
                        severity: 'Critical',
                        comment: 'API key exposed in query parameter — visible in logs.',
                        suggestion: 'Pass via Authorization header instead.',
                        createdOn: new Date().toISOString(),
                    },
                    {
                        id: 'c4',
                        codeReviewId: '3',
                        filePath: 'src/Services/GeminiService.cs',
                        lineNumber: 72,
                        category: 'Performance',
                        severity: 'Warning',
                        comment: 'HttpClient is being instantiated per request, causing socket exhaustion.',
                        suggestion: 'Inject IHttpClientFactory and use CreateClient() instead.',
                        createdOn: new Date().toISOString(),
                    },
                ],
            },
        ];

    constructor(
        private route: ActivatedRoute,
    ) { }

    ngOnInit(): void {
        this.reviewId = this.route.snapshot.paramMap.get('id') ?? '';
        const found = this.stubReviews.find(r => r.id === this.reviewId);
        this.review = found ?? this.stubReviews[0];
    }
}
