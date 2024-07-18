import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { TaskService } from '../../services/task/task.service';
import { ToastrService } from 'ngx-toastr';
import { TaskHeaderComponent } from '../../shared/components/task-header/task-header.component';
import { TaskMenuComponent } from '../../shared/components/task-menu/task-menu.component';
import { ApiResponse } from '../../models/ApiResponse';
import { Subscription } from 'rxjs';
import { Task } from '../../models/Task';
import { ErrorDisplay } from '../../shared/exception-handling/exception-handle';
import { storeNotifyTimes } from '../../shared/notifications/store-notifyTimes';
@Component({
  selector: 'app-active',
  standalone: true,
  templateUrl: './active.component.html',
  styleUrl: './active.component.scss',
  imports: [TaskHeaderComponent, TaskMenuComponent],
})
export class ActiveComponent implements OnInit, OnDestroy {
  activeTasks!: Task[];
  taskServiceSubscription!: Subscription;
  pageManipulatedSubscription!: Subscription;
  constructor(
    private taskService: TaskService,
    private errorDisplay: ErrorDisplay
  ) {}
  ngOnInit() {
    this.checkPageManipulated();
    this.getActiveTasksData();
  }

  checkPageManipulated() {
    this.pageManipulatedSubscription =
      this.taskService.pageManiulated$.subscribe((response) => {
        if (response == 'active') {
          this.sendUpdatedData();
        }
      });
  }
  sendUpdatedData() {
    this.getActiveTasksData();
  }
  getActiveTasksData() {
    this.taskService.isLoading$.next(true);
    this.taskServiceSubscription = this.taskService
      .getActiveTasks<ApiResponse>()
      .subscribe({
        next: (response) => {
          this.taskService.isLoading$.next(false);
          this.activeTasks = response.result;
        },
        error: (error) => {
          this.errorDisplay.errorOcurred(error);
        },
      });
  }
  ngOnDestroy(): void {
    this.taskServiceSubscription.unsubscribe();
    this.pageManipulatedSubscription.unsubscribe();
  }
}
