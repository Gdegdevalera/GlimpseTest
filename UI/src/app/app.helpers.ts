import { DatePipe } from "@angular/common";

const datePipe = new DatePipe('en-US');

export function dateToBarItemName(value: string | Date) {
  return datePipe.transform(value, 'h a');
}