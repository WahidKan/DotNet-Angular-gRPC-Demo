import { Injectable } from '@angular/core';
import { UserServiceClient } from './generated/UserServiceClientPb';
import { UserRequest, UserResponse } from './generated/user_pb';

@Injectable({
  providedIn: 'root',
})
export class GrpcService {
  private client: UserServiceClient;

  constructor() {
    this.client = new UserServiceClient('https://localhost:5001'); // Adjust port if necessary
  }

  getUser(userId: number): Promise<UserResponse> {
    const request = new UserRequest();
    request.setUserId(userId);

    return new Promise((resolve, reject) => {
      this.client.getUser(request, {}, (err, response) => {
        if (err) {
          reject(err);
        } else {
          resolve(response);
          debugger
        }
      });
    });
  }
}
