export class User {
  userName: string;
  password: string;
  constructor(response: any) {
    this.userName = response.userName;
    this.password = response.password;
  }
}
