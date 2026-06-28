import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ContentService } from '../../../../core/services/content.service';
import { ProblemListDto, ProblemRequest } from '../../../../shared/models/problem.model';

@Component({
  selector: 'app-task-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-management.component.html'
})
export class TaskManagementComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly contentService = inject(ContentService);

  public tasks = signal<ProblemListDto[]>([]);
  public isCreateMode = false;
  public isEditMode = signal(false);
  public isSubmitting = signal(false);
  public currentEditId = signal<string | null>(null);

  public taskForm: FormGroup = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(5)]],
    difficulty: ['Easy', Validators.required],
    description: ['', [Validators.required, Validators.minLength(20)]],
    input: [''], 
    expectedOutput: [''],
    memoryLimit: [256, [Validators.required, Validators.min(16)]],
    cpuLimit: [1000, [Validators.required, Validators.min(100)]]
  });

  public ngOnInit(): void {
    this.loadTasks();
  }

  private loadTasks(): void {
    this.contentService.getProblems().subscribe(data => this.tasks.set(data));
  }

  public onEdit(task: ProblemListDto): void {
    this.isSubmitting.set(true);
    
    this.contentService.getProblemBySlug(task.slug).subscribe({
      next: (fullTask) => {
        this.isEditMode.set(true);
        this.isCreateMode = true; 
        this.currentEditId.set(fullTask.id);

        this.taskForm.patchValue({
          title: fullTask.title,
          difficulty: this.getDifficultyLabel(fullTask.difficulty),
          description: fullTask.description,
          memoryLimit: fullTask.memoryLimitMb,
          cpuLimit: fullTask.cpuLimitMs,
          input: fullTask.testCases[0]?.input || '',
          expectedOutput: fullTask.testCases[0]?.expectedOutput || ''
        });
        
        this.isSubmitting.set(false);
      },
      error: (err) => {
        console.error('Failed to load task details:', err);
        this.isSubmitting.set(false);
      }
    });
  }

  public toggleCreateMode(): void {
    this.isCreateMode = !this.isCreateMode;
    if (!this.isCreateMode) {
      this.isEditMode.set(false);
      this.currentEditId.set(null);
      this.taskForm.reset({ difficulty: 'Easy', memoryLimit: 256, cpuLimit: 1000 });
    }
  }

  public onSubmit(): void {
    if (this.taskForm.invalid) return;

    this.isSubmitting.set(true);
    const val = this.taskForm.value;
    const difficultyMap: Record<string, number> = { 'Easy': 0, 'Medium': 1, 'Hard': 2 };

    const request: ProblemRequest = {
      title: val.title,
      description: val.description,
      difficulty: difficultyMap[val.difficulty],
      memoryLimitMb: val.memoryLimit,
      cpuLimitMs: val.cpuLimit,
      testCases: [
        { input: val.input, expectedOutput: val.expectedOutput, isHidden: false }
      ]
    };

    const operation: import('rxjs').Observable<any> = this.isEditMode() 
      ? this.contentService.updateProblem(this.currentEditId()!, request)
      : this.contentService.createProblem(request);

    operation.subscribe({
      next: () => {
        alert(this.isEditMode() ? 'Challenge updated!' : 'Challenge committed successfully!');
        this.isSubmitting.set(false);
        this.toggleCreateMode();
        this.loadTasks();
      },
      error: (err: any) => {
        console.error('Operation failed:', err);
        alert(err.error?.message || err.error?.Message || 'Error processing request');
        this.isSubmitting.set(false);
      }
    });
  }

  public getDifficultyLabel(difficulty: any): string {
    const map: Record<number, string> = { 0: 'Easy', 1: 'Medium', 2: 'Hard' };
    return typeof difficulty === 'number' ? map[difficulty] : difficulty;
  }

  public onDrop(id: string): void {
    if (confirm('Are you sure you want to permanently delete this challenge?')) {
      this.contentService.deleteProblem(id).subscribe({
        next: () => {
          this.tasks.update(prev => prev.filter(t => t.id !== id));
        },
        error: (err) => alert('Delete failed: ' + err.message)
      });
    }
  }
}