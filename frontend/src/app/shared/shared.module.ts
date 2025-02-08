import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RequiredLabelComponent } from './required-label/required-label.component';

@NgModule({
  declarations: [RequiredLabelComponent],
  imports: [CommonModule],
  exports: [RequiredLabelComponent]
})
export class SharedModule { }
