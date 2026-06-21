import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface TeamMember {
  name: string;
  role: string;
  specialization: string;
  bio: string;
  github: string;
}

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './team.component.html'
})
export class TeamComponent {
  public readonly leadDeveloper: TeamMember = {
    name: 'Alexander Mikhalkov',
    role: 'Lead Architect & Software Engineer',
    specialization: 'Distributed Systems / Compilers',
    bio: 'Primary architect and developer of the CodeJudex platform. Specialized in building automated code auditing engines and secure sandbox environments.',
    github: 'https://github.com/zu1uw'
  };
}