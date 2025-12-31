import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'leftETA'
})
export class LeftETAPipe implements PipeTransform {

  transform(totalETA: number,usedETA:number): string {
    let leftETA:number = totalETA- usedETA;
    if(leftETA <= 0){
      leftETA = 0;
    }
    return `${leftETA}h`;
  }

}
