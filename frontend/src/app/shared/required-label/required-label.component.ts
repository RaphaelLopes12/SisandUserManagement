import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-required-label',
  templateUrl: './required-label.component.html',
  styleUrls: ['./required-label.component.scss'],
})
export class RequiredLabelComponent {
  @Input() label!: string;
  @Input() isRequired: boolean = false;
  @Input() forId!: string;
}
