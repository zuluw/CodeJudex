import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuditApiService } from '../../services/audit.service';
import { AuditResponse } from '../../../../shared/models/audit.models';

@Component({
  selector: 'app-audit-workspace',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './audit-workspace.component.html'
})
export class AuditWorkspaceComponent {
  private readonly auditApi = inject(AuditApiService);

  public activeTab: 'desc' | 'ai' | 'telemetry' = 'desc';
  public sourceCode: string = '';
  public isLoading: boolean = false;
  public auditResult: AuditResponse | null = null;
  public errorMessage: string | null = null;

  public onAuditSubmit(): void {
    if (!this.sourceCode.trim()) {
      return;
    }

    this.prepareForAnalysis();

    this.auditApi.runAudit(this.sourceCode).subscribe({
      next: (response) => {
        this.auditResult = response;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.isLoading = false;
        console.error('[AuditWorkspace] Analysis failed:', error);
      }
    });
  }

  private prepareForAnalysis(): void {
    this.isLoading = true;
    this.auditResult = null;
    this.errorMessage = null;
  }
}