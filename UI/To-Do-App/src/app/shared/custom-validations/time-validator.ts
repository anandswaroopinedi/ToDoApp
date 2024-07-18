import { AbstractControl } from '@angular/forms';

export function TimeValidator(control: AbstractControl) {
  if (control.value) {
    let [todayDate,time]:string=control.value.split('T')
    let t:string[]=time.split(':');
    let h=new Date().getHours()
    let m=new Date().getMinutes()
    let [year, month, date] = todayDate.split("-");
    let inputDate = new Date(Number(year), Number(month)-1, Number(date));
    let currDate=new Date();
    let current_date= new Date(currDate.getFullYear(),currDate.getMonth(),currDate.getDay());
    if(inputDate.getTime() > current_date.getTime() ||(inputDate.getTime() === current_date.getTime() && (h<Number(t[0]) ||(h==Number(t[0]) && m<=Number(t[1])))))
    {
        return null;
    }
    return { invalidTime: true };;
  }
  return null;
}