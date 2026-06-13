import { CodeReview } from './code-review.model';

export interface PrFeedItem {
  codeReview: CodeReview;
  repositoryOwner: string;
  repositoryName: string;
  prAuthorLogin: string;
  pullRequestTitle: string;
}
