import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { TaskService } from './services/task/task.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [RouterOutlet, SpinnerComponent, CommonModule],
})
export class AppComponent implements OnInit {
  title = 'To-Do-App';
  isLoading: boolean = false;
  constructor(
    private taskService: TaskService,
    private cdr: ChangeDetectorRef
  ) {}
  ngOnInit() {
    this.taskService.isLoading$.subscribe((value) => {
      this.isLoading = value;
      this.cdr.detectChanges();
    });
  }
}
