import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { TaskService } from '../../services/task/task.service';
import { SideBarComponent } from '../layout/side-bar/side-bar.component';
import { HeaderComponent } from '../layout/header/header.component';
import { SideBarMobileComponent } from '../layout/side-bar-mobile/side-bar-mobile.component';
import { RouterModule } from '@angular/router';
import { AddTaskComponent } from '../../shared/components/add-task/add-task.component';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { DeleteConfirmationComponent } from '../../shared/components/delete-confirmation/delete-confirmation.component';
import { Subscription, interval } from 'rxjs';
import { storeNotifyTimes } from '../../shared/notifications/store-notifyTimes';
import { ApiResponse } from '../../models/ApiResponse';
import { NotifyMessageComponent } from '../../shared/components/notify-message/notify-message.component';
import { ErrorDisplay } from '../../shared/exception-handling/exception-handle';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [
    SideBarComponent,
    HeaderComponent,
    DashboardComponent,
    SideBarMobileComponent,
    RouterModule,
    AddTaskComponent,
    DeleteConfirmationComponent,
    NotifyMessageComponent,
    CommonModule,
  ],
})
export class HomeComponent implements OnInit, OnDestroy {
  pageName: string = '';
  @ViewChild('addTask') addTaskRef!: ElementRef<HTMLInputElement>;
  @ViewChild('deleteConfirmation')
  deleteConfirmationRef!: ElementRef<HTMLInputElement>;
  @ViewChild('home') homeDivRef!: ElementRef<HTMLInputElement>;
  deleteItemId!: number;
  editTaskSubscribtion!: Subscription;
  deleteTaskSubscription!: Subscription;
  isNotify: boolean = false;
  message: string = '';
  sub!: Subscription;
  isNotificationsRefreshed: boolean = false;
  constructor(
    private taskService: TaskService,
    private errorDisplay: ErrorDisplay
  ) {}
  ngOnInit() {
    this.checkEditButtonClicked();
    this.checkDeleteButtonClicked();
    this.getFurtherNotifyTasks();
  }
  checkEditButtonClicked() {
    this.editTaskSubscribtion = this.taskService.editTask$.subscribe(
      (value) => {
        if (value != null) {
          this.openAddTaskContainer();
        }
      }
    );
  }
  checkDeleteButtonClicked() {
    this.deleteTaskSubscription = this.taskService.deleteConfirm$.subscribe(
      (value) => {
        this.openDeleteConfirmContainer(value);
      }
    );
  }
  getFurtherNotifyTasks() {
    this.taskService
      .getFurtherNotifyTasks<ApiResponse>()
      .subscribe((value: ApiResponse) => {
        storeNotifyTimes(value.result);
      });
  }
  openDeleteConfirmContainer(id: number) {
    this.deleteItemId = id;
    this.deleteConfirmationRef.nativeElement.style.display = 'block';
    this.homeDivRef.nativeElement.classList.add('blur');
    this.homeDivRef.nativeElement.style.pointerEvents = 'none';
  }
  openAddTaskContainer() {
    this.addTaskRef.nativeElement.style.display = 'block';
    this.homeDivRef.nativeElement.classList.add('blur');
    this.homeDivRef.nativeElement.style.pointerEvents = 'none';
  }
  closeDeleteConfirmContainer() {
    this.deleteConfirmationRef.nativeElement.style.display = 'none';
    this.homeDivRef.nativeElement.classList.remove('blur');
    this.homeDivRef.nativeElement.style.pointerEvents = 'auto';
  }
  closeAddTaskContainer() {
    this.addTaskRef.nativeElement.style.display = 'none';
    this.homeDivRef.nativeElement.classList.remove('blur');
    this.homeDivRef.nativeElement.style.pointerEvents = 'auto';
  }
  ngAfterViewInit() {
    this.notifyMessages();
  }
  notifyMessages() {
    this.sub = interval(60000).subscribe((t) => {
      var d = new Date();
      if (d.getHours() == 0 && d.getMinutes() == 1) {
        this.taskService.modifyNotificationStatus().subscribe({
          next: () => {
            this.isNotificationsRefreshed = true;
            this.isNotificationsRefreshed = false;
          },
          error: (err) => {
            this.errorDisplay.errorOcurred(err);
          },
        });
      }
      var keys = Object.keys(localStorage);
      var i = keys.length;
      while (i--) {
        var arr: string[] = localStorage.getItem(keys[i])!.split(',');
        const parsedDateTime = new Date(arr[1]);
        const currentDateTime = new Date();
        if (parsedDateTime <= currentDateTime) {
          setTimeout(() => {
            this.isNotify = false;
          }, 5000);
          this.isNotify = true;
          this.message = arr[0];
          localStorage.removeItem(keys[i]);
          this.taskService.notificationMessage$.next([arr[0], keys[i], 'add']);
        }
      }
    });
  }
  ngOnDestroy() {
    this.editTaskSubscribtion.unsubscribe();
    this.deleteTaskSubscription.unsubscribe();
  }
}
