import { Component, OnInit, inject, signal, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { ContentService } from '../../../../core/services/content.service';
import { AuditApiService } from '../../services/audit.service';
import { ProblemResponseDto, Difficulty } from '../../../../shared/models/problem.model';
import { AuditResponse } from '../../../../shared/models/audit.models';

@Component({
  selector: 'app-audit-workspace',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './audit-workspace.component.html'
})
export class AuditWorkspaceComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly contentService = inject(ContentService);
  private readonly auditApi = inject(AuditApiService);
  private readonly destroy$ = new Subject<void>();

  public problem = signal<ProblemResponseDto | null>(null);
  public isLoadingProblem = signal<boolean>(true);
  public errorMessage = signal<string | null>(null);

  public activeTab = signal<'desc' | 'ai' | 'telemetry'>('desc');
  public sourceCode = '';
  public isAuditing = signal<boolean>(false);
  public auditResult = signal<AuditResponse | null>(null);

  public ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        const slug = params.get('taskId');
        if (slug) {
          this.loadProblem(slug);
        }
      });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadProblem(slug: string): void {
    this.isLoadingProblem.set(true);
    this.errorMessage.set(null);

    this.contentService.getProblemBySlug(slug).subscribe({
      next: (data) => {
        this.problem.set(data);
        this.isLoadingProblem.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Challenge not found in repository.');
        this.isLoadingProblem.set(false);
        this.problem.set(null);
      }
    });
  }

  public formatDifficulty(difficulty: any): string {
    const map: Record<number, string> = { 0: 'Easy', 1: 'Medium', 2: 'Hard' };
    return typeof difficulty === 'number' ? map[difficulty] : difficulty;
  }

  public onAuditSubmit(): void {
    if (!this.sourceCode.trim()) return;

    this.isAuditing.set(true);
    this.auditResult.set(null);

    this.auditApi.runAudit(this.sourceCode).subscribe({
      next: (response) => {
        this.auditResult.set(response);
        this.isAuditing.set(false);
      },
      error: () => {
        this.errorMessage.set('Audit failed. Service unreachable.');
        this.isAuditing.set(false);
      }
    });
  }
}