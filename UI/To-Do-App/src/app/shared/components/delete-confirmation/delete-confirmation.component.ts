import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ApiResponse } from '../../../models/ApiResponse';
import { ToastrService } from 'ngx-toastr';
import { TaskService } from '../../../services/task/task.service';
import { Router } from '@angular/router';
import { ErrorDisplay } from '../../exception-handling/exception-handle';

@Component({
  selector: 'app-delete-confirmation',
  standalone: true,
  imports: [],
  templateUrl: './delete-confirmation.component.html',
  styleUrl: './delete-confirmation.component.scss',
})
export class DeleteConfirmationComponent {
  @Input() id!: number;
  @Output() close: EventEmitter<null> = new EventEmitter<null>();
  constructor(
    private taskService: TaskService,
    private toaster: ToastrService,
    private router: Router,
    private errorDisplay:ErrorDisplay
  ) {}
  onCancel() {
    this.close.emit();
  }
  onDelete() {
    if (this.id == 0) {
      this.DeleteAll();
    } else {
      this.DeleteSingleTask();
    }
  }
  DeleteAll() {
    this.taskService.isLoading$.next(true);
    this.taskService.deleteAllTasks<ApiResponse>().subscribe({
      next: (response) => {
        this.operationSucceded(response);
      },
      error: (error) => {
        this.errorDisplay.errorOcurred(error);
      },
    });
  }
  DeleteSingleTask() {
    this.taskService.isLoading$.next(true);
    this.taskService.deleteTask<ApiResponse>(this.id).subscribe({
      next: (response) => {
        this.operationSucceded(response);
      },
      error: (error) => {
        this.errorDisplay.errorOcurred(error);
        this.onCancel();
      },
    });
  }
  operationSucceded(response: ApiResponse) {
    this.taskService.isLoading$.next(false);
    const url = this.router.url.split('/').pop();
    if (url) {
      this.taskService.pageManiulated$.next(url);
    }
    this.toaster.success(response.message);
    this.onCancel();
  }
}
