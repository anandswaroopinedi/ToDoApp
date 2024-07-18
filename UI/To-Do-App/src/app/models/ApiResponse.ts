export class ApiResponse {
  status: number;
  message: string;
  result: any;
  constructor(response: ApiResponse) {
    this.status = response.status;
    this.message = response.message;
    this.result = response.result;
  }
}
