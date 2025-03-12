import { RouterOutlet } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'employees-FE';

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.router.navigate(['/employees']);
  }
}
