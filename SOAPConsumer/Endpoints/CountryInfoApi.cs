namespace SOAPConsumer.Endpoints;

public static class CountryInfoApi
{
    public static void ConfigureCountryInfoApi(this WebApplication app)
    {
        app.MapGet("getcapitalcity", GetCapitalCity);
    }

    private static async Task<IResult> GetCapitalCity(string countryCode)
    {
        try
        {
            var client = new CountryInfoServiceSoapTypeClient(
                CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

            var response = await client.CapitalCityAsync(countryCode);

            var capital = response.Body.CapitalCityResult?.Trim();

            if (string.IsNullOrWhiteSpace(capital) || 
                capital.Equals("Country not found in the database", StringComparison.OrdinalIgnoreCase))
            {
                return Results.NotFound($"Country code '{countryCode}' not found in the database.");
            }

            return Results.Ok(capital);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error fetching capital: {ex.Message}");
        }
    }
}