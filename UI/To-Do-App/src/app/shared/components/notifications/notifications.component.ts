import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { TaskService } from '../../../services/task/task.service';
import { ApiResponse } from '../../../models/ApiResponse';
import { Subscription, interval } from 'rxjs';
import { Router,Event as NavigationEvent, NavigationStart, NavigationEnd, RouterLink } from '@angular/router';
import { ErrorDisplay } from '../../exception-handling/exception-handle';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.scss'
})
export class NotificationsComponent implements OnInit {
  deletedNotificationIds:number[]=[];
  displayMenu:boolean=false;
  routerSubscription!:Subscription;
  notificationIds:number[]=[];
  notificationMessages:string[]=[];

  constructor(private taskService:TaskService,private router:Router,private errorDisplay:ErrorDisplay)
  {
  }
  ngOnInit() {
    this.routerSubscription = this.router.events.subscribe(
      (event: NavigationEvent) => {
        if (event instanceof NavigationStart) {
          if(this.deletedNotificationIds.length)
            {
              this.taskService.isLoading$.next(true);
              this.taskService.cancelNotifications(this.deletedNotificationIds).subscribe({
                next:(response) => {
                  this.deletedNotificationIds = [];
                  this.taskService.isLoading$.next(false);
                },
                error:(error) => {
                  this.errorDisplay.errorOcurred(error);
                  this.taskService.isLoading$.next(false);
                }
              }
              );
            }
        }
      }
    );
  }
ngAfterContentInit()
{
  this.getTodaysPendingTask();
  this.addNewNotifications();
}
getTodaysPendingTask()
{
  this.taskService.getNotifyTasks<ApiResponse>().subscribe((value:ApiResponse)=>{
    for(var i=0;i<value.result.length;i++)
      {
        this.notificationIds.push(value.result[i].id)
        this.notificationMessages.push(value.result[i].name);
      }
  });
}
  displayNotifications()
  {
    if(this.notificationIds.length!=0)
    this.displayMenu=this.displayMenu?false:true;
  }
  addNewNotifications()
  {
    this.taskService.notificationMessage$.subscribe((value:string[])=>{
      if(value[2]=='add' && !this.notificationIds.includes(Number(value[1])))
        {
          this.notificationIds.push(Number(value[1]))
          this.notificationMessages.push(value[0]);
        }
    else if(value[2]=='del' && this.notificationIds.includes(Number(value[1]))){
      this.notificationIds=this.notificationIds.filter(k=>k!=Number(value[1]))
      this.notificationMessages=this.notificationMessages.filter(k=>k!=value[0]);
    }
    })
  }
  onCancelMenu()
  {
    this.displayMenu=false
    this.deletedNotificationIds.push(...this.notificationIds);
    this.notificationIds=[]
  }
  onCancelTask(index:number)
  {
    this.deletedNotificationIds.push(this.notificationIds[index]);
    this.notificationIds=this.notificationIds.filter(k=>k!=this.notificationIds[index])
    this.notificationMessages=this.notificationMessages.filter(k=>k!=this.notificationMessages[index]);
    if(this.notificationIds.length==0)
    {
      this.displayMenu=false;
    }
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedInside = (event.target as HTMLElement).closest(
      '.notifation-container,.notification-menu'
    );
    if (!clickedInside) {
      this.displayMenu=false;
    }
  }
  
  ngOnDestroy()
  {
    this.routerSubscription.unsubscribe();
  }
}
