import { AuthenticationService } from '@app/_services/authentication.service';

export function appInitializer(authService: AuthenticationService) {
    return () => new Promise(resolve => {
        authService.refreshToken()
            .subscribe()
            .add(resolve);
    });
}