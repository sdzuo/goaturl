import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { z } from 'zod';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private readonly APIUrl = 'http://localhost:7203/api/UrlMapping/';  
  private readonly urlValidationSchema = z.string().url();

  // httpOptions was used for debugging and testing
  // private httpOptions : Object = {
  //   headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  //   responseType: 'json',
  //   observe: 'response' as const
  // };

  constructor(private http: HttpClient) { }

  validateUrl(url: string) {
    try {
      this.urlValidationSchema.parse(url);
      return { success: true };
    } catch (error) {
      return { success: false, error };
    }
  }
  shortenUrl(longUrl: string): Observable<any> {
    console.log('Sending request to:', `${this.APIUrl}CreateShortUrl`);
    console.log('Payload:', { longUrl });

    return this.http.post<any>(`${this.APIUrl}CreateShortUrl`, { longUrl });  //, this.httpOptions// );
  }
}
