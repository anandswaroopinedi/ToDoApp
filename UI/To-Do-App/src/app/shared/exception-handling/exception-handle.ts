import { Injectable} from '@angular/core';
import { TaskService } from '../../services/task/task.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ErrorDisplay {
  constructor(
    private taskService: TaskService,
    private toaster: ToastrService
  ) {}
  errorOcurred(response: HttpErrorResponse) {
    if(response.error.errors)
    {
      const validationErrors = response.error.errors;
          Object.keys(validationErrors).forEach(prop => {
            const messages = validationErrors[prop];
            messages.forEach((message: any) => {
              this.toaster.error(`${message}`);
            });
          });
    }
    else
    {
      this.toaster.error(response.error.Message);
    }
    this.taskService.isLoading$.next(false)
  }
}
