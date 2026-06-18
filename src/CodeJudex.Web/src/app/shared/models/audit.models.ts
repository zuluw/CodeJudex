export interface AuditIssue {
  ruleId: string;
  message: string;
  severity: 'Info' | 'Warning' | 'Error';
  lineNumber: number;
}

export interface AuditResponse {
  qualityScore: number;
  issues: AuditIssue[];
  analyzedAt: string;
}