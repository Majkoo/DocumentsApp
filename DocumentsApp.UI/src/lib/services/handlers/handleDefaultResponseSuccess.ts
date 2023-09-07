export const HandleDefaultResponseSuccess = async <T>(res: Response): Promise<T> => {
    return await res.json() as T;
};