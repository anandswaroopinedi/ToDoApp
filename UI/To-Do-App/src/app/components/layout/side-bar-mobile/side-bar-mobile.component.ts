import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  Output,
  ViewChild,
} from '@angular/core';
import {
  NavigationEnd,
  Router,
  RouterLink,
  Event as NavigationEvent,
} from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-side-bar-mobile',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './side-bar-mobile.component.html',
  styleUrl: './side-bar-mobile.component.scss',
})
export class SideBarMobileComponent implements AfterViewInit, OnDestroy {
  @Output() name: EventEmitter<string> = new EventEmitter<string>();
  @ViewChild('menu') selectMenu!: ElementRef<HTMLInputElement>;
  routerSubscription!: Subscription;
  constructor(private router: Router) {}
  ngAfterViewInit(): void {
    this.assignDefaultSelect();
  }
  assignDefaultSelect() {
    const url: any = this.router.url.split('/').pop();
    if (url) this.selectMenu.nativeElement.value = url;
    this.routerSubscription = this.router.events.subscribe(
      (event: NavigationEvent) => {
        if (event instanceof NavigationEnd) {
          const url: any = this.router.url.split('/').pop();
          if (url) this.selectMenu.nativeElement.value = url;
        }
      }
    );
  }
  select(pageName: string) {
    this.name.emit(pageName);
    if (pageName == 'dashboard') {
      this.router.navigateByUrl('/home/dashboard');
    } else if (pageName == 'active') {
      this.router.navigateByUrl('/home/active');
    } else {
      this.router.navigateByUrl('/home/completed');
    }
  }
  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }
}
