import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { MegaMenuItem } from 'primeng/api';
import { AttendeeProfile } from 'src/app/models/api/attendeeProfile';
import { AttendeeSelectItem } from 'src/app/models/api/attendeeSelectItem';
import { LookupsService } from '../../services/lookups.service';
import { AttendeeProfileService } from '../../services/profile.service';

@Component({
  selector: 'top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.scss'],
})
export class TopBarComponent implements OnInit {
  items: MegaMenuItem[] = [];
  attendeesLookup: AttendeeSelectItem[] = [];
  selectedAttendee: AttendeeSelectItem = {};
  profile: AttendeeProfile = {};

  constructor(
    private attendeeProfileService: AttendeeProfileService,
    private lookupsService: LookupsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.lookupsService.attendees.subscribe(
      (attendees) => (this.attendeesLookup = attendees)
    );

    this.attendeeProfileService.profile.subscribe((profile) => {
      this.profile = profile;
    });

    // ;
    this.items = [
      {
        label: 'Szkolenia',
        icon: 'pi pi-calendar',
        command: () => {
          this.router.navigateByUrl('');
        },
      },
      {
        label: 'Uczestnicy',
        icon: 'pi pi-users',
        command: () => {
          this.router.navigateByUrl('attendees');
        },
      },
      {
        label: 'Wykładowcy',
        icon: 'pi pi-user',
        command: () => {},
      },
      {
        label: 'Technologie',
        icon: 'pi pi-cog',
        command: () => {},
      },
    ];
  }
}
