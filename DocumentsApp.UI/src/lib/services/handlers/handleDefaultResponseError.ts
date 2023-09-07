import {HttpResponseError} from "$lib/models/classes/httpResponseError";

export const HandleDefaultResponseError = async (err: Error): Promise<void> => {
    if (err instanceof HttpResponseError) {
        switch (err.response.status) {
            // todo: impl toast messages
            default:
                throw err;
        }
    }
};