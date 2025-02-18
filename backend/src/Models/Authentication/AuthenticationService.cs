using RestSharp;

namespace src.Models.Authentication
{
    public class AuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _authServerUrl;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authServerUrl = _configuration["AuthenticationServer:BaseUrl"];
        }

        public async Task<RestResponse> RegisterUserAsync(RegisterViewModel model)
        {
            // Create the RestClient with the base URL of the authentication server.
            var client = new RestClient($"{_authServerUrl}/api/authentication/register");

            // Create a RestRequest and set the method to POST explicitly.
            var request = new RestRequest { Method = Method.Post };

            // Add the JSON body to the request.
            request.AddJsonBody(model);

            // Execute the request asynchronously and return the response.
            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> LoginUserAsync(LoginViewModel model)
        {
            var client = new RestClient($"{_authServerUrl}/api/Authentication/login");

            // Create a RestRequest and set the method to POST explicitly.
            var request = new RestRequest { Method = Method.Post };

            // Add the JSON body to the request.
            request.AddJsonBody(new { Username = model.UserName, Password = model.Password });
            //request.AddJsonBody(model);

            // Execute the request asynchronously and return the response.
            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> ValidateSSOTokenAsync(string ssoToken)
        {
            var client = new RestClient($"{_authServerUrl}/api/authentication/validate-sso-token");

            // Create a RestRequest and set the method to POST explicitly.
            var request = new RestRequest { Method = Method.Post };

            // Add the JSON body to the request.
            request.AddJsonBody(new { SSOToken = ssoToken });

            // Execute the request asynchronously and return the response.
            return await client.ExecuteAsync(request);
        }
    }
}