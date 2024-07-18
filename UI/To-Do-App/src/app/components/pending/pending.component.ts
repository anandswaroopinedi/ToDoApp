import { Component, ElementRef, ViewChild } from '@angular/core';
import { TaskHeaderComponent } from '../../shared/components/task-header/task-header.component';
import { Task } from '../../models/Task';
import { TaskService } from '../../services/task/task.service';
import { TaskMenuComponent } from '../../shared/components/task-menu/task-menu.component';
import { ApiResponse } from '../../models/ApiResponse';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ErrorDisplay } from '../../shared/exception-handling/exception-handle';

@Component({
  selector: 'app-pending',
  standalone: true,
  templateUrl: './pending.component.html',
  styleUrl: './pending.component.scss',
  imports: [TaskHeaderComponent, TaskMenuComponent, CommonModule],
})
export class PendingComponent {
  pendingTasks!: Task[];
  pageManiulatedSubscribtion!: Subscription;
  @ViewChild('selectMenu') selectMenuRef!:ElementRef<HTMLInputElement>;
  constructor(
    private taskService: TaskService,
    private errorDisplay:ErrorDisplay
  ) {}
  ngOnInit() {
    this.checkPendingDataManipulated();
  }
  ngAfterViewInit()
  {
    var a=this.selectMenuRef.nativeElement.value.split(' ')
    this.sendUpdatedData(a[0], a[1]);
  }
  checkPendingDataManipulated() {
    this.pageManiulatedSubscribtion =
      this.taskService.pageManiulated$.subscribe((value) => {
        if (value.toLowerCase() == 'pending') {
          var a=this.selectMenuRef.nativeElement.value.split(' ')
          this.sendUpdatedData(a[0], a[1]);
        }
      });
  }
  sendUpdatedData(property: string, order: string) {
    this.taskService.isLoading$.next(true);
    this.taskService.getPendingTasks<ApiResponse>(property, order).subscribe({
      next: (response: ApiResponse) => {
        this.taskService.isLoading$.next(false);
        this.pendingTasks = response.result;
      },
      error: (error) => {
        this.errorDisplay.errorOcurred(error);
      },
    });
  }
  sortTable(event: Event) {
    const value = (event.target as HTMLSelectElement).value; 
    if (value)
    {
      const [property, order] = value.split(' ');
      this.sendUpdatedData(property,order);
    }
  }
}
