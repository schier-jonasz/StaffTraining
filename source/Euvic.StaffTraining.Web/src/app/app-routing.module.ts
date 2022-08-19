import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttendeesListComponent } from './features/attendees/components/attendees-list/attendees-list.component';
import { TrainingsListComponent } from './features/trainings/components/trainings-list/trainings-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/trainings', pathMatch: 'full' },
  { path: 'trainings', component: TrainingsListComponent },
  { path: 'attendees', component: AttendeesListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
