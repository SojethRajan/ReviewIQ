export type ReviewStatus = 'Pending' | 'InProgress' | 'Completed' | 'Failed';
export type Severity = 'Critical' | 'Warning' | 'Suggestion';
export type Category = 'Security' | 'Bug' | 'Performance' | 'Style';

export interface ReviewComment {
  id: string;
  codeReviewId: string;
  filePath: string;
  lineNumber: number;
  category: Category;
  severity: Severity;
  comment: string;
  suggestion: string;
  createdOn: string;
}

export interface CodeReview {
  id: string;
  repositoryId: string;
  incomingEventId: string;
  pullRequestNumber: number;
  commitSha: string;
  qualityScore: number;
  status: ReviewStatus;
  totalComments: number;
  gitHubReviewId: string | null;
  startedOn: string;
  completedOn: string | null;
  comments: ReviewComment[];
}
