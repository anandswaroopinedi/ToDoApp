import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { TaskMenuComponent } from '../../shared/components/task-menu/task-menu.component';
import { TaskStatusComponent } from '../../shared/components/task-status/task-status.component';
import { TaskHeaderComponent } from '../../shared/components/task-header/task-header.component';
import { TaskService } from '../../services/task/task.service';
import { Task } from '../../models/Task';
import { ApiResponse } from '../../models/ApiResponse';
import { Subscription } from 'rxjs';
import { ErrorDisplay } from '../../shared/exception-handling/exception-handle';
import { storeNotifyTimes } from '../../shared/notifications/store-notifyTimes';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  imports: [
    CommonModule,
    TaskMenuComponent,
    TaskStatusComponent,
    TaskHeaderComponent,
  ],
})
export class DashboardComponent implements OnInit, OnDestroy {
  name: string = 'dashboard';
  tasks: Task[] = [];
  @ViewChild('taskStatus') taskStatus!: TaskStatusComponent;
  pageManiulatedSubscribtion!: Subscription;
  taskServiceSubscription!: Subscription;
  isButtonDisplay:boolean=true;
  constructor(
    private taskService: TaskService,
    private errorDisplay: ErrorDisplay,
  ) {}
  ngOnInit() {
    this.checkDashboardManipulated();
    this.getAllTasksData();
  }
  checkDashboardManipulated() {
    this.pageManiulatedSubscribtion =
      this.taskService.pageManiulated$.subscribe((value) => {
        if (value == 'dashboard') {
          this.sendUpdatedData();
        }
      });
  }
  sendUpdatedData() {
    this.getAllTasksData();
    this.taskStatus.getCompletionpercentage();
  }
  getAllTasksData() {
    this.taskService.isLoading$.next(true);
    this.taskServiceSubscription = this.taskService
      .getAllTasks<ApiResponse>()
      .subscribe({
        next: (response) => {
          this.taskService.isLoading$.next(false);
          this.tasks = response.result;
          this.isButtonDisplay=this.tasks.length>0
        },
        error: (error) => {
          this.errorDisplay.errorOcurred(error);
        },
      });
  }
  ngOnDestroy() {
    this.taskServiceSubscription.unsubscribe();
    this.pageManiulatedSubscribtion.unsubscribe();
  }
}
