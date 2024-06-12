import { SafeUrl } from "@angular/platform-browser";

export interface User{
    id: string;
    username: string;
    email: string;
    token: string;
    roles: string[];
    avatarUrl?: SafeUrl;
}