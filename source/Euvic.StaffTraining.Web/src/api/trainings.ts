export interface Training {
    id: string;
    title: string;
    description?: string;
    duration: number;
    createDate: string;
    trainingDate: string;
    lecturerId: number;
    lecturerFullName: string;
    technologyId: number;
    technologyName: string;
    confirmedAttendances: number;
    totalAttendances: number;
    attendanceStatusId: number;
}