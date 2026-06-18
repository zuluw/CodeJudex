import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface TaskPreview {
  id: number;
  title: string;
  difficulty: string;
  description: string;
}

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './landing.component.html'
})
export class LandingComponent {
  sampleTasks: TaskPreview[] = [
    { id: 1, title: 'Two Sum Algorithm', difficulty: 'Easy', description: 'Find two numbers such that they add up to a specific target.' },
    { id: 2, title: 'Binary Tree Depth', difficulty: 'Medium', description: 'Calculate the maximum depth of a provided binary tree structure.' },
    { id: 3, title: 'Memory-Efficient LRU Cache', difficulty: 'Hard', description: 'Design a data structure that follows the constraints of a Least Recently Used (LRU) cache.' }
  ];
}