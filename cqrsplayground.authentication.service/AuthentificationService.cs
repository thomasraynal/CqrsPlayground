using cqrsplayground.shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.authentication.service
{
    public abstract class AuthentificationService : ControllerBase, IAuthenticatedService
    {
        private IServiceInstance _instance;
        private IAuthenticationService _authenticationService;

        public AuthentificationService(IAuthenticationService auth, IServiceInstance instance)
        {
            _authenticationService = auth;
            _instance = instance;
        }

        [HttpGet("auth/{consumerServiceKey}")]
        [AllowAnonymous]
        public async Task<ServiceToken> GetServiceToken(String consumerServiceKey)
        {
            var isValid = await _authenticationService.IsValid(consumerServiceKey);
            if (isValid)
            {
        
                var creds = new SigningCredentials(_instance.ServiceKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _instance.Issuer,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);


                return new ServiceToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo.Ticks,
                    InstanceId = _instance.Id,
                    Service = _instance.Key,
                };
            }

            return null;
        }

    }
}
