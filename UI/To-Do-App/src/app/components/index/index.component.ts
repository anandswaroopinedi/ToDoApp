import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { routePaths } from '../../shared/route-paths/route-paths';
@Component({
  selector: 'app-index',
  standalone: true,
  imports: [],
  templateUrl: './index.component.html',
  styleUrl: './index.component.scss',
})
export class IndexComponent implements OnInit {
  constructor(private router: Router) {}
  ngOnInit() {
    setTimeout(() => {
      this.router.navigate(routePaths.login);
    }, 2000);
  }
}
