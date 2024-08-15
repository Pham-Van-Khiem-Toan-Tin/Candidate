import { environment } from '../../environments/environment';
export const USER_API_ENDPOINT = {
    ALL_USER: `${environment.apiUrl}user/all`,
    CHANGE_ACTIVATED: `${environment.apiUrl}user/activate`,
    CREATE_USER: `${environment.apiUrl}user/create`,
    USER_DETAIL: `${environment.apiUrl}user/detail/`,
    UPDATE_USER: `${environment.apiUrl}user/edit/`,
    DELETE_USER: `${environment.apiUrl}user/delete/`,
    ALL_ROLE: `${environment.apiUrl}user/roles`,
    EXPORT_USER: `${environment.apiUrl}user/exports`
}