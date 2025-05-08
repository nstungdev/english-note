import { AbstractControl } from '@angular/forms';

/**
 * Returns appropriate error message based on validation errors for a given control and label.
 */
export function getErrorMessage(
  control: AbstractControl | null,
  label: string
): string | null {
  if (!control || !control.errors) {
    return null;
  }
  if (control.errors['required']) {
    return `${label} is required`;
  }
  if (control.errors['minlength']) {
    return `${label} must be at least ${control.errors['minlength'].requiredLength} characters long`;
  }
  if (control.errors['maxlength']) {
    return `${label} cannot be more than ${control.errors['maxlength'].requiredLength} characters long`;
  }
  if (control.errors['pattern']) {
    return `${label} is invalid`;
  }
  if (control.errors['email']) {
    return `Invalid email format`;
  }
  // Add more error handling as needed
  return 'Invalid value';
}
