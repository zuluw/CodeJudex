import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ContentService } from '../../../../core/services/content.service';
import { AuthService } from '../../../../core/services/auth.service';
import { ProblemListDto } from '../../../../shared/models/problem.model';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './tasks.component.html'
})
export class TasksComponent implements OnInit {
  private readonly contentService = inject(ContentService);
  public readonly auth = inject(AuthService);

  public problems = signal<ProblemListDto[]>([]);
  public isLoading = signal<boolean>(true);
  public errorMessage = signal<string | null>(null);

  public categories = ['Algorithms', 'Data Structures', 'Strings', 'Dynamic Programming'];

  public ngOnInit(): void {
    this.loadProblems();
  }

  private loadProblems(): void {
    this.isLoading.set(true);
    this.contentService.getProblems().subscribe({
      next: (data) => {
        this.problems.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load challenges. Please try again later.');
        this.isLoading.set(false);
        console.error('[TasksComponent] Loading failed:', err);
      }
    });
  }

  public getDifficultyLabel(difficulty: any): string {
    const map: Record<number, string> = { 0: 'Easy', 1: 'Medium', 2: 'Hard' };
    return typeof difficulty === 'number' ? map[difficulty] : difficulty;
  }

  public getDifficultyClass(difficulty: string): string {
    switch (difficulty) {
      case 'Easy': return 'text-green-500 bg-green-500/10';
      case 'Medium': return 'text-yellow-500 bg-yellow-500/10';
      case 'Hard': return 'text-red-500 bg-red-500/10';
      default: return 'text-slate-400 bg-slate-400/10';
    }
  }

  public showLoginPrompt(): void {
    alert('Please sign in to access the audit workspace.');
  }
}