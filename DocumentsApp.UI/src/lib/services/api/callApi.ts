import type {HttpMethod} from "@sveltejs/kit";
import {HttpResponseError} from "$lib/models/classes/httpResponseError";
import type {HandleResponseErrorFunction} from "$lib/models/types/handleResponseErrorFunction";
import type {HandleResponseSuccessFunction} from "$lib/models/types/handleResponseSuccessFunction";
import {HandleDefaultResponseError} from "$lib/services/handlers/handleDefaultResponseError";
import {HandleDefaultResponseSuccess} from "$lib/services/handlers/handleDefaultResponseSuccess";

export async function callApi<T>(
    url: string,
    method: HttpMethod = 'GET',
    data: object | string | null = null,
    handleError: HandleResponseErrorFunction = HandleDefaultResponseError,
    handleSuccess: HandleResponseSuccessFunction<T> = HandleDefaultResponseSuccess
){

    const requestOptions: RequestInit = {
        method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer'
        },
        body: data ? JSON.stringify(data) : undefined,
    };

    try {
        const res: Response = await fetch(url, requestOptions);
        if (!res.ok) throw new HttpResponseError('Network response was not ok', res);
        return handleSuccess(res);
    } catch (err) {
        handleError(err as Error);
    }
}