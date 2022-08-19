import { Component, OnInit } from '@angular/core';
import { AttendeesListItem } from 'src/app/models/api/models';
import { TrainingService } from '../../services/attendees.service';

@Component({
  selector: 'app-attendees-list',
  templateUrl: './attendees-list.component.html',
  styleUrls: ['./attendees-list.component.scss'],
})
export class AttendeesListComponent implements OnInit {
  public attendees: AttendeesListItem[] = [];
  constructor(private trainingService: TrainingService) {}

  ngOnInit(): void {
    this.trainingService.getAttendees().subscribe((attendees) => {
      this.attendees = attendees;
    });
  }
}
