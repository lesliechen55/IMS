using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.Web.Models.Api.Request;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.APIController
{
    /// <summary>
    /// API身份认证控制器
    /// </summary>
    public class AuthController : BaseApiController
    {
        private readonly IHomeService _homeService;

        public AuthController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("getToken")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetToken([FromBody]TokenRequest request)
        {
            var (userId, account, nickName) = await _homeService.LoginAsync(request.Account, request.Password, HttpContext.Connection.RemoteIpAddress.ToString(), Request.Headers["User-Agent"].ToString());
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppConfig.IssuerSigningKey);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtClaimTypes.Audience,AppConfig.Audience),
                    new Claim(JwtClaimTypes.Issuer,AppConfig.Issuer),
                    new Claim(JwtClaimTypes.Id, userId.ToString()),
                    new Claim(JwtClaimTypes.Name, account),
                    new Claim(JwtClaimTypes.NickName, nickName)
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    sid = userId,
                    name = account,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }

        [HttpGet]
        [Route("values")]
        [Produces("application/json")]
        public IActionResult Values()
        {
            return Ok(new string[] { "value1" });
        }
    }
}