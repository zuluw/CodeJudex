import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

interface TaskEntry {
  id: number;
  title: string;
  difficulty: 'Easy' | 'Medium' | 'Hard';
  status: 'Published' | 'Draft';
}

@Component({
  selector: 'app-task-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-management.component.html'
})
export class TaskManagementComponent {
  private readonly fb = inject(FormBuilder);

  public isCreateMode: boolean = false;
  
  public tasks: TaskEntry[] = [
    { id: 1, title: 'Two Sum Algorithm', difficulty: 'Easy', status: 'Published' },
    { id: 2, title: 'Binary Tree Depth', difficulty: 'Medium', status: 'Published' },
    { id: 3, title: 'LRU Cache Design', difficulty: 'Hard', status: 'Draft' }
  ];

  public taskForm: FormGroup = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(5)]],
    difficulty: ['Easy', Validators.required],
    description: ['', [Validators.required, Validators.minLength(20)]],
    inputSpecs: ['', Validators.required],
    outputSpecs: ['', Validators.required],
    memoryLimit: [256, [Validators.required, Validators.min(16)]],
    cpuLimit: [1000, [Validators.required, Validators.min(100)]]
  });

  public toggleCreateMode(): void {
    this.isCreateMode = !this.isCreateMode;
    if (!this.isCreateMode) this.taskForm.reset({ difficulty: 'Easy', memoryLimit: 256, cpuLimit: 1000 });
  }

  public onSubmit(): void {
    if (this.taskForm.valid) {
      console.log('Task Data:', this.taskForm.value);
      this.toggleCreateMode();
    }
  }
}