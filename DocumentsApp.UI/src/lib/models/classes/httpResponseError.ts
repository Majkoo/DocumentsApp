export class HttpResponseError extends Error {
    response: Response;
    constructor(message: string, res: Response) {
        super(message)
        this.response = res
    }
}
