import { Component, EventEmitter, Output } from '@angular/core';
import { TaskService } from '../../../services/task/task.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-header.component.html',
  styleUrl: './task-header.component.scss',
})
export class TaskHeaderComponent {
  pageName: string = '';
  today: Date;
  constructor(private taskService: TaskService, private router: Router) {
    this.today = new Date();
  }
  ngOnInit(): void {
    this.setPageName();
  }
  setPageName() {
    let name: string | undefined = this.router.url.split('/').pop();
    if (name) this.pageName = name[0].toUpperCase() + name.slice(1);
  }
  deleteAll() {
    this.taskService.deleteConfirm$.next(0);
  }
}
