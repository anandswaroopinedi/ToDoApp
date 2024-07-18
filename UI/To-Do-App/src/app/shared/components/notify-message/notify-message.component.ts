import { Component, Input } from '@angular/core';
import { TaskService } from '../../../services/task/task.service';

@Component({
  selector: 'app-notify-message',
  standalone: true,
  imports: [],
  templateUrl: './notify-message.component.html',
  styleUrl: './notify-message.component.scss'
})
export class NotifyMessageComponent {
  @Input()message='';
}
