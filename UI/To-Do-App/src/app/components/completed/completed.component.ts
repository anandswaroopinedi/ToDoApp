import { Component, OnDestroy, OnInit } from '@angular/core';
import { TaskService } from '../../services/task/task.service';
import { TaskMenuComponent } from '../../shared/components/task-menu/task-menu.component';
import { TaskHeaderComponent } from '../../shared/components/task-header/task-header.component';
import { ApiResponse } from '../../models/ApiResponse';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Task } from '../../models/Task';

@Component({
  selector: 'app-completed',
  standalone: true,
  imports: [TaskMenuComponent, TaskHeaderComponent],
  templateUrl: './completed.component.html',
  styleUrl: './completed.component.scss',
})
export class CompletedComponent implements OnInit, OnDestroy {
  pageManiulatedSubscribtion!: Subscription;
  taskServiceSubscription!: Subscription;
  completedTasks!:Task[];
  constructor(
    private taskService: TaskService,
    private toaster: ToastrService
  ) {}
  ngOnInit() {
    this.checkChangesMade();
    this.getCompletedTasksData();
  }
  checkChangesMade() {
    this.pageManiulatedSubscribtion =
      this.taskService.pageManiulated$.subscribe((response) => {
        if (response == 'completed') {
          this.sendUpdatedData();
        }
      });
  }
  sendUpdatedData() {
    this.getCompletedTasksData();
  }

  getCompletedTasksData() {
    this.taskService.isLoading$.next(true);
    this.taskServiceSubscription = this.taskService
      .getCompletedTasks<ApiResponse>()
      .subscribe({
        next: (response) => {
          this.taskService.isLoading$.next(false);
          this.completedTasks=response.result;
        },
        error: (error) => {
          this.taskService.isLoading$.next(false);
          this.toaster.error('Something went wrong. Please try again.');
        },
      });
  }
  ngOnDestroy() {
    this.taskServiceSubscription.unsubscribe();
    this.pageManiulatedSubscribtion.unsubscribe();
  }
}
