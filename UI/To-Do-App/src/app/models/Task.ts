export class Task {
  id: number;
  name: string;
  description: string;
  userid: number;
  statusid: number;
  createdon: Date;
  completedon: Date;
  statusName: string;
  constructor(response: any) {
    this.id = response.id;
    this.name = response.name;
    this.description = response.description;
    this.userid = response.userid;
    this.completedon = response.completedon;
    this.createdon = response.createdon;
    this.statusid = response.statusid;
    this.statusName = response.statusName;
  }
}
