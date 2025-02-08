import { Injectable } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable({
  providedIn: 'root',
})
export class CustomDateParserFormatter extends NgbDateParserFormatter {
  
  parse(value: string): NgbDateStruct | null {
    if (!value) return null;
    const parts = value.split('/');
    return parts.length === 3
      ? { day: +parts[0], month: +parts[1], year: +parts[2] }
      : null;
  }

  format(date: NgbDateStruct | null): string {
    return date ? `${this.padNumber(date.day)}/${this.padNumber(date.month)}/${date.year}` : '';
  }

  private padNumber(value: number): string {
    return value < 10 ? `0${value}` : `${value}`;
  }
}
