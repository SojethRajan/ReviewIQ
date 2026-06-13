import { Component } from '@angular/core';
import { PrFeedItem } from '../../core/models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard.component',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  summaryCards = [
    {
      label: 'Reviews today',
      value: '12',
      sub: '↑ 4 from yesterday',
      subClass: 'positive',
    },
    {
      label: 'Avg quality score',
      value: '84',
      sub: '↓ 3 this week',
      subClass: 'negative',
    },
    {
      label: 'Critical issues',
      value: '7',
      sub: 'across 5 PRs',
      subClass: 'neutral',
    },
  ];

  prFeed: PrFeedItem[] = [
    {
      codeReview: {
        id: '1',
        repositoryId: 'r1',
        incomingEventId: 'e1',
        pullRequestNumber: 21,
        commitSha: 'abc123',
        qualityScore: 0,
        status: 'InProgress',
        totalComments: 0,
        gitHubReviewId: null,
        startedOn: new Date().toISOString(),
        completedOn: null,
        comments: [],
      },
      repositoryOwner: 'SojethRajan',
      repositoryName: 'ReviewIQ',
      prAuthorLogin: 'SojethRajan',
      pullRequestTitle: 'feat: add JWT refresh token rotation',
    },
    {
      codeReview: {
        id: '2',
        repositoryId: 'r1',
        incomingEventId: 'e2',
        pullRequestNumber: 20,
        commitSha: 'def456',
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
      repositoryOwner: 'SojethRajan',
      repositoryName: 'ReviewIQ',
      prAuthorLogin: 'SojethRajan',
      pullRequestTitle: 'fix: retry on transient SQL error 40613',
    },
    {
      codeReview: {
        id: '3',
        repositoryId: 'r1',
        incomingEventId: 'e3',
        pullRequestNumber: 19,
        commitSha: 'ghi789',
        qualityScore: 61,
        status: 'Completed',
        totalComments: 5,
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
            comment:
              'Append returns a new IEnumerable, original list is unchanged.',
            suggestion: 'Use Add() instead of Append().',
            createdOn: new Date().toISOString(),
          },
          {
            id: 'c3',
            codeReviewId: '3',
            filePath: 'src/Services/GeminiService.cs',
            lineNumber: 55,
            category: 'Security',
            severity: 'Suggestion',
            comment: 'API key exposed in query parameter — visible in logs.',
            suggestion: 'Pass via Authorization header instead.',
            createdOn: new Date().toISOString(),
          },
          {
            id: 'c4',
            codeReviewId: '3',
            filePath: 'src/Services/GeminiService.cs',
            lineNumber: 55,
            category: 'Security',
            severity: 'Critical',
            comment: 'API key exposed in query parameter — visible in logs.',
            suggestion: 'Pass via Authorization header instead.',
            createdOn: new Date().toISOString(),
          },
        ],
      },
      repositoryOwner: 'SojethRajan',
      repositoryName: 'ReviewIQ',
      prAuthorLogin: 'SojethRajan',
      pullRequestTitle: 'refactor: extract DiffChunker service',
    },
  ];

  getScoreClass(item: PrFeedItem): string {
    if (item.codeReview.status === 'InProgress') {
      return 'riq-score riq-score--analysing';
    }

    const score = item.codeReview.qualityScore;

    if (score >= 85) {
      return 'riq-score riq-score--high';
    }
    if (score >= 65) {
      return 'riq-score riq-score--mid';
    }
    return 'riq-score riq-score--low';
  }

  getScoreLabel(item: PrFeedItem): string {
    if (item.codeReview.status === 'InProgress') {
      return '';
    }
    return item.codeReview.qualityScore.toString();
  }

  getCriticalCount(item: PrFeedItem): number {
    return item.codeReview.comments.filter((c) => c.severity === 'Critical')
      .length;
  }

  getWarningCount(item: PrFeedItem): number {
    return item.codeReview.comments.filter((c) => c.severity === 'Warning')
      .length;
  }

  getSuggestionCount(item: PrFeedItem): number {
    return item.codeReview.comments.filter((c) => c.severity === 'Suggestion')
      .length;
  }
}
