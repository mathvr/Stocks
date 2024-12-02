export class ConnectionUtility
{
    public static getServerBaseUrl(): string{
        const url : string = import.meta.env.VITE_APP_SERVER_NAME;

        return `http://${url}:8080`;
    }
}