import { SafeUrl } from "@angular/platform-browser";
import { Subject } from "./subject.model";

export interface Student{
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    semester: number;
    wiek: number;
    editing: boolean;
    avatarUrl?: SafeUrl;
    enrolledSubjects: string[];

}