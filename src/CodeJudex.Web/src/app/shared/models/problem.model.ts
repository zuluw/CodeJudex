export type Difficulty = 'Easy' | 'Medium' | 'Hard';

export interface TestCaseRequest {
  input: string;
  expectedOutput: string;
  isHidden: boolean;
}

export interface ProblemRequest {
  title: string;
  description: string;
  difficulty: number;
  memoryLimitMb: number;
  cpuLimitMs: number;
  testCases: TestCaseRequest[];
}

export interface ProblemListDto {
  id: string;
  title: string;
  slug: string;
  difficulty: Difficulty;
}

export interface TestCaseResponse {
  input: string;
  expectedOutput: string;
}

export interface ProblemResponseDto {
  id: string;
  title: string;
  slug: string;
  description: string;
  difficulty: Difficulty;
  memoryLimitMb: number;
  cpuLimitMs: number;
  testCases: TestCaseResponse[];
}