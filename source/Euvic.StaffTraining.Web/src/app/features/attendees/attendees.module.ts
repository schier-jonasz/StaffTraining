import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttendeesListComponent } from './components/attendees-list/attendees-list.component';
import { TrainingService } from './services/attendees.service';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [CommonModule, SharedModule],
  declarations: [AttendeesListComponent],
  providers: [TrainingService],
})
export class AttendeesModule {}
