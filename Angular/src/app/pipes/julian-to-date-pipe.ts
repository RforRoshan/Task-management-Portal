import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'julianToDate'
})
export class JulianToDatePipe implements PipeTransform {

  transform(dateInJulian:number|null): string {
    if(dateInJulian == null){
      return `${dateInJulian}`;
    }

    const dateInJulianStr = dateInJulian.toString();

    if (dateInJulianStr.length !== 7) return dateInJulianStr;
    const year = parseInt(dateInJulianStr.slice(0, 4), 10);
    const dayOfYear = parseInt(dateInJulianStr.slice(4), 10);

    // Create date from year + day
    const date = new Date(year, 0); // January 1st
    date.setDate(dayOfYear);

    // Format to dd/MM/yyyy
    const dd = String(date.getDate()).padStart(2, '0');
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const yyyy = date.getFullYear();

    return `${dd}/${mm}/${yyyy}`;
  }

}
