import { environment } from '../../environments/environment';
export const EVENT_API_ENDPOINT = {
    ALL_EVENT: `${environment.apiUrl}event/all`,
    ALL_EVENT_INPROGESS: `${environment.apiUrl}event/in-progress`,
    CREATE_EVENT: `${environment.apiUrl}event/create`,
    EXPORT_EVENT: `${environment.apiUrl}event/export`,
    DETAIL_EVENT: `${environment.apiUrl}event/detail/`,
    DELETE_EVENT: `${environment.apiUrl}event/delete/`
}