using NewProjectSolution.Models.Const;
using NewProjectSolution.Infrastructure.Singleton;
using NewProjectSolution.Models.Main;
using NewProjectSolution.Models.ViewModels;
using NewProjectSolution.UnitOfWork.Main;
using RxWeb.Core.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewProjectSolution.Infrastructure.Security
{
     public class ApplicationTokenProvider : IApplicationTokenProvider
    {
        private ILoginUow LoginUow { get; set; }
        private UserAccessConfigInfo UserAccessConfig { get; set; }
        private IJwtTokenProvider TokenProvider { get; set; }

        private IUserClaim UserClaim { get; set; }

        public ApplicationTokenProvider(IJwtTokenProvider tokenProvider, UserAccessConfigInfo userAccessConfig, ILoginUow loginUow,IUserClaim userClaim)
        {
            TokenProvider = tokenProvider;
            UserAccessConfig = userAccessConfig;
            LoginUow = loginUow;
            UserClaim = userClaim;
        }
        public async Task<KeyValuePair<string, string>> GetTokenAsync(vUser user)
        {
            var token = TokenProvider.WriteToken(new[]{
                new Claim(
                    ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Locality,user.LanguageCode),
                    new Claim(CustomClaimTypes.TimeZone,user.ApplicationTimeZoneName)
                    }, "Web", "User", DateTime.Now.AddDays(2));
            await UserAccessConfig.SaveTokenAsync(user.UserId, "web", token, LoginUow);
            return token;
        }

        public async Task<KeyValuePair<string, string>> RefereshTokenAsync(vUser user,UserConfig userConfig)
        {
            if (!string.IsNullOrEmpty(userConfig.LanguageCode)) {
                var userRecord = await LoginUow.Repository<User>().SingleAsync(t => t.UserId == user.UserId);
                userRecord.LanguageCode = userConfig.LanguageCode;
                await LoginUow.RegisterDirtyAsync<User>(userRecord);
                await LoginUow.CommitAsync();
            }
            await UserAccessConfig.RemoveTokenAsync(user.UserId, userConfig.AudienceType, LoginUow);
            return await this.GetTokenAsync(user);
        }

        public async Task RemoveTokenAsync(UserConfig userConfig) =>
            await UserAccessConfig.RemoveTokenAsync(UserClaim.UserId,userConfig.AudienceType , LoginUow);

    }

    public interface IApplicationTokenProvider
    {
        Task<KeyValuePair<string, string>> GetTokenAsync(vUser user);

        Task<KeyValuePair<string, string>> RefereshTokenAsync(vUser user, UserConfig userConfig);

        Task RemoveTokenAsync(UserConfig userConfig);
    }
}



