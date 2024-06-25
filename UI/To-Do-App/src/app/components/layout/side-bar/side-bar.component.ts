import { Component, EventEmitter, Output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.scss',
  imports: [RouterLink, RouterLinkActive],
})
export class SideBarComponent {
  @Output() name: EventEmitter<string> = new EventEmitter<string>();
  @Output() flag: EventEmitter<null> = new EventEmitter<null>();
  sendAddTaskRequest() {
    this.flag.emit();
  }
  select(pageName: string) {
    this.name.emit(pageName);
  }
}
