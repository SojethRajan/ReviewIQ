import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ShellComponent } from './layout/shell/shell';

@Component({
  selector: 'app-root',
  imports: [ShellComponent, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('reviewiq-dashboard');
}
