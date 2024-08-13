import { environment } from '../../environments/environment';
export const USER_API_ENDPOINT = {
    ALL_USER: `${environment.apiUrl}user/all`,
    CHANGE_ACTIVATED: `${environment.apiUrl}user/activate`
}