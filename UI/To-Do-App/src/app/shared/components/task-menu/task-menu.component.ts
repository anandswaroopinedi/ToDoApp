import {
  Component,
  HostListener,
  Input,
} from '@angular/core';
import { Task } from '../../../models/Task';
import { TaskService } from '../../../services/task/task.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../models/ApiResponse';
import { message } from '../../enums/response';
import { ErrorDisplay } from '../../exception-handling/exception-handle';

@Component({
  selector: 'app-task-menu',
  standalone: true,
  templateUrl: './task-menu.component.html',
  styleUrl: './task-menu.component.scss',
  imports: [CommonModule],
})
export class TaskMenuComponent {
  pageName: string = '';
  @Input() tasks: Task[] = [];
  constructor(
    private taskService: TaskService,
    private router: Router,
    private toaster: ToastrService,
    private errorDisplay:ErrorDisplay
  ) {}
  ngOnInit() {
    this.getUpdatedTasks();
  }
  getUpdatedTasks() {
    let name: string | undefined = this.router.url.split('/').pop();
    if (name) this.pageName = name[0].toUpperCase() + name.slice(1);
  }
  openTaskInfo(index: number) {
    var taskDetail = document.getElementsByClassName(
      'task-item-info'
    ) as HTMLCollectionOf<HTMLInputElement>;
    taskDetail[index].style.display =
      taskDetail[index].style.display == 'none' ||
      taskDetail[index].style.display == ''
        ? 'flex'
        : 'none';
    this.HideTaskDescriptions(index);
  }
  HideTaskDescriptions(index: number) {
    var taskDetail = document.getElementsByClassName(
      'task-item-info'
    ) as HTMLCollectionOf<HTMLInputElement>;
    for (let i = 0; i < taskDetail.length; i++) {
      if (i != index) taskDetail[i].style.display = 'none';
    }
  }
  delete(task:Task) {
    this.taskService.deleteConfirm$.next(task.id);
    this.taskService.notificationMessage$.next([task.name,task.id.toString(),"del"])
  }
  ToggleActiveComplete(task:Task) {
    if (this.pageName.toLowerCase() == 'active' ||this.pageName.toLowerCase() =='pending') {
      this.makeTaskAsCompleted(task.id);
      this.taskService.notificationMessage$.next([task.name,task.id.toString(),"del"]);
    } else if (this.pageName.toLowerCase() == 'completed') {
      this.makeTaskAsActive(task.id);
    }
  }
  makeTaskAsCompleted(id: number) {
    this.taskService.isLoading$.next(true);
    this.taskService.makeTaskAsCompleted<ApiResponse>(id).subscribe({
      next: (response) => {
        this.operationSucceded(response);
      },
      error: (error) => {
        this.errorDisplay.errorOcurred(error);
      },
    });
  }
  operationSucceded(response:ApiResponse)
  {
    this.taskService.isLoading$.next(false);
    if (response.status == message.Success) {
      this.taskService.pageManiulated$.next(this.pageName.toLowerCase());
      this.toaster.success(response.message);
    } else {
      this.toaster.error(response.message);
    }
  }
  makeTaskAsActive(id: number) {
    this.taskService.isLoading$.next(true);
    this.taskService.makeTaskAsActive<ApiResponse>(id).subscribe({
      next: (response) => {
        this.operationSucceded(response);
      },
      error: (error) => {
        this.errorDisplay.errorOcurred(error);
      },
    });
  }
  openEditForm(task: Task) {
    this.taskService.editTask$.next(task);
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedInside = (event.target as HTMLElement).closest(
      '.task-item, .task-item-info'
    );
    if (!clickedInside) {
      this.HideTaskDescriptions(-1);
    }
  }
  calculateTime(dateTime: any) {
    const createdDate = new Date(dateTime);
    const currentDate = new Date();
    const timeDifference = currentDate.getTime() - createdDate.getTime();
    const Difference = Math.floor(timeDifference / (1000 * 60 * 60));
    if (Difference == 0) {
      const minDifference = Math.floor(timeDifference / (1000 * 60));
      if (minDifference == 0) {
        return Math.floor(timeDifference / 1000) + ' seconds';
      }
      return minDifference + ' minutes';
    }
    if(this.pageName.toLowerCase()=='pending')
    {
      const days=Math.ceil(Difference/24);
      if(days==1)
      {
        return days+' day'
      }
      return  days+' days';
    }
    return Difference + ' hours';
  }
}
