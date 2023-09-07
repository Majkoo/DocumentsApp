import type {JwtClaimsDto} from "$lib/models/dtos/auth/JwtClaimsDto";

export class JwtDataDto {
    authToken: string = null!;
    refreshToken: string = null!;
    claims: JwtClaimsDto = null!;
}