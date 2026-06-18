import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html'
})
export class DashboardHomeComponent {
  recentSessions = [
    { id: 1, taskTitle: 'Sum of Unique Elements', date: '2 days ago', score: 95 },
    { id: 2, taskTitle: 'Binary Tree Depth', date: '1 week ago', score: 78 },
    { id: 3, taskTitle: 'Two Sum Algorithm', date: '2 weeks ago', score: 100 },
    { id: 4, taskTitle: 'LRU Cache Implementation', date: '1 month ago', score: 65 }
  ];
}