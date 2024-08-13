import { environment } from '../../environments/environment';
export const AUTH_API_ENDPOINT = {
    LOGIN: `${environment.apiUrl}user/login`,
    FORGOT: `${environment.apiUrl}user/forgot`,
}