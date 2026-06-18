import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './tasks.component.html'
})
export class TasksComponent {
  public auth = inject(AuthService);
  private router = inject(Router);

  categories = ['Algorithms', 'Data Structures', 'System Design', 'Clean Code', 'Security'];

  tasks = [
    { id: 1, title: 'Two Sum Algorithm', difficulty: 'Easy', tags: ['Array', 'Hash'], score: 98 },
    { id: 2, title: 'Longest Substring', difficulty: 'Medium', tags: ['String', 'Sliding Window'], score: null },
    { id: 3, title: 'Memory-Efficient LRU Cache', difficulty: 'Hard', tags: ['System', 'Memory'], score: null },
    { id: 4, title: 'Naming Convention Audit', difficulty: 'Easy', tags: ['C#', 'PascalCase'], score: 100 },
    { id: 5, title: 'Cyclomatic Complexity Test', difficulty: 'Hard', tags: ['Logic', 'Refactoring'], score: null },
  ];

  getDifficultyClass(diff: string) {
    return {
      'text-green-500 bg-green-500/10': diff === 'Easy',
      'text-yellow-500 bg-yellow-500/10': diff === 'Medium',
      'text-red-500 bg-red-500/10': diff === 'Hard'
    };
  }

  showLoginPrompt() {
    this.router.navigate(['/auth/login']);
  }
}