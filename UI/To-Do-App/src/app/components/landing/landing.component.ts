import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { routePaths } from '../../shared/route-paths/route-paths';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent  implements OnInit {
  constructor(private router: Router) {}
  ngOnInit() {
    setTimeout(() => {
      this.router.navigate(routePaths.login);
    }, 2000);
  }
}