import { Component, OnInit } from '@angular/core';
import { GrpcService } from '../grpc.service';
import { NgIf } from '@angular/common'; // Import NgIf directive

interface User {
  id: number;
  name: string;
  email: string;
}

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css'],
  standalone: true, // Mark as standalone
  imports: [NgIf] // Add NgIf here
})
export class UserDetailsComponent implements OnInit {
  user: User | null = null; // Using the local User model
  loading: boolean = false;
  error: string | null = null;

  constructor(private grpcService: GrpcService) {}

  ngOnInit(): void {
    this.getUser(1); // Fetch user with ID 1
  }

  getUser(userId: number): void {
    this.loading = true;
    this.grpcService.getUser(userId)
      .then(response => {
        this.user = {
          id: response.getUserId(),
          name: response.getName(),
          email: response.getEmail(),
        };
        this.loading = false;
      })
      .catch(err => {
        this.error = 'Error fetching user data';
        console.error(err);
        this.loading = false;
      });
  }
}