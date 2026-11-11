
using System;
using System.Net.Http;

public class NetworkClient
{
    private int attempts = 0;

    public string GetApiResponse(string endpoint)
    {
        attempts++;

        if (attempts <= 4)
        {
            throw new HttpRequestException(
                "тимчасова мережева помилка"
            );
        }

        return "успішна відповідь API: " + endpoint;
    }
}
