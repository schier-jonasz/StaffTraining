import axios from 'axios';
import { BASE_URL } from '../utils/config';

export interface Attendee {
    id: number;
    firstname: string;
    lastname: string;
    allowedHours: number;
    totalHours: number;
    totalConfirmedHours: number;
}

export const getAttendees = async () => {
    const response = await axios.get<Attendee[]>(`${BASE_URL}/attendees`);
    console.log(response);
}
