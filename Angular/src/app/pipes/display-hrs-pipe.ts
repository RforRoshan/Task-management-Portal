import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'displayHrs'
})
export class DisplayHrsPipe implements PipeTransform {

  transform(hrs:number|null, isNARequired:boolean=false): string {
    if(hrs == null || hrs <= 0){
      if(isNARequired){
        return 'N.A.'
      }
      return ''
    }
    return `${hrs}h`;
  }

}
