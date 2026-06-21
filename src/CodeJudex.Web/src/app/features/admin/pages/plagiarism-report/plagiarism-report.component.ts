import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

interface PlagiarismIncident {
  id: string;
  taskTitle: string;
  similarity: number;
  studentA: { id: string, code: string };
  studentB: { id: string, code: string };
  detectedAt: string;
}

@Component({
  selector: 'app-plagiarism-report',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './plagiarism-report.component.html'
})
export class PlagiarismReportComponent {
  public readonly incidents: PlagiarismIncident[] = [
    {
      id: 'PL-902',
      taskTitle: 'Two Sum Algorithm',
      similarity: 94.5,
      studentA: { id: 'USR-042', code: 'public int[] TwoSum(int[] nums, int target) {\n  for(int i=0; i<nums.Length; i++) {\n    // ... logic\n  }\n}' },
      studentB: { id: 'USR-891', code: 'public int[] Solution(int[] a, int t) {\n  for(int x=0; x<a.Length; x++) {\n    // Different names, same AST structure\n  }\n}' },
      detectedAt: '2026-06-17 14:20'
    },
    {
      id: 'PL-905',
      taskTitle: 'Binary Tree Depth',
      similarity: 82.1,
      studentA: { id: 'USR-112', code: '// Original recursion' },
      studentB: { id: 'USR-334', code: '// Copied recursion' },
      detectedAt: '2026-06-17 16:45'
    }
  ];

  public selectedIncident = signal<PlagiarismIncident | null>(this.incidents[0]);

  public selectIncident(incident: PlagiarismIncident): void {
    this.selectedIncident.set(incident);
  }
}