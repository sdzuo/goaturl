import { Component } from '@angular/core';
import { UrlShortenerService } from '../url-shortener.service';
import { ChangeDetectorRef } from '@angular/core';
import { TooltipPosition } from '@angular/material/tooltip';

@Component({
  selector: 'app-input-form',
  templateUrl: './input-form.component.html',
  styleUrls: ['./input-form.component.css']
})
export class InputFormComponent {
  longUrl = '';
  shortenedUrl: string | null = null;
  showShortenedUrl = false;
  errormessage: string | null = null;
  tooltipPosition: TooltipPosition = 'after'; // Use TooltipPosition enum
  buttonState: string = "shorten";

  constructor(private urlShortenerService: UrlShortenerService, private cdr: ChangeDetectorRef) { }

  // Shorten the URL
  onSubmit() {
    // Validate the input URL using the service
    const validationResult = this.urlShortenerService.validateUrl(this.longUrl);

    if (validationResult.success) {
      this.errormessage = null;
      // Call the URL shortening service and subscribe to the response
      this.urlShortenerService.shortenUrl(this.longUrl).subscribe({
        next: (response) => {
          if (response.newurl !== true) {
            this.shortenedUrl = response.shortenedUrl;
            console.log("URL exists!");
          } else {
            this.shortenedUrl = response.shortenedUrl;
            console.log("New URL created");
          }
          // Display the shortened URL and update the button state
          this.showShortenedUrl = true;
          this.cdr.detectChanges(); // Trigger change detection
          this.buttonState = "shortenanother";
        },
        error: (error) => {
          console.error(error);
        }
      });
    } else {
      // Handle URL validation error and display the error message
      this.errormessage = validationResult.error as string;
      console.log(validationResult.error);
    }
  }

  // Change the tooltip position
  changeTooltipPosition(newPosition: TooltipPosition) {
    this.tooltipPosition = newPosition;
  }
}
