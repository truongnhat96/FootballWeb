using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FootballWeb.Mail;
public static class MailExtension
{
    public static IServiceCollection AddMailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSetting>(configuration.GetSection("MailSettings"));
        services.AddTransient<IEmailSender, MailSender>();
        return services;
    }
}