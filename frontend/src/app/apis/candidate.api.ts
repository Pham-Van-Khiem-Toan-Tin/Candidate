import { environment } from '../../environments/environment';

export const CANDIDATE_API_ENDPOINT = {
    ALL_CANDIDATE: `${environment.apiUrl}candidate/all`,
    CREATE_CANDIDATE: `${environment.apiUrl}candidate/create`
}