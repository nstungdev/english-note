import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule],
})
export class HomeComponent {
  featuredItems = [
    { title: 'Lesson 1', description: 'Introduction to English Grammar' },
    {
      title: 'Tip of the Day',
      description: 'Practice speaking daily for fluency',
    },
    { title: 'Resource', description: 'Download our free vocabulary guide' },
    {
      title: 'Quiz of the Week',
      description: 'Test your knowledge with our weekly quiz',
    },
    {
      title: 'Upcoming Webinar',
      description: 'Join our live session on advanced English skills',
    },
    {
      title: 'Daily Challenge',
      description: 'Complete today’s challenge to improve your vocabulary',
    },
  ];
}
