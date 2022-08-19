import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AttendeesListItem } from 'src/app/models/api/models';

@Injectable()
export class TrainingService {
  url: string = 'attendees';

  constructor(private http: HttpClient) {}

  getAttendees() {
    return this.http.get<AttendeesListItem[]>(this.url);
  }
}
