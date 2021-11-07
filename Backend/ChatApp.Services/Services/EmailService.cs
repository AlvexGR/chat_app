using System;
using System.Threading.Tasks;
using ChatApp.Dtos.Models.Emails;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace ChatApp.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> Send(EmailDto emailDto)
        {
            try
            {
                if (emailDto == null) return false;
                var emailSetting = GetEmailSetting();

                var builder = new BodyBuilder
                {
                    HtmlBody = emailDto.Content
                };


                var message = new MimeMessage
                {
                    Subject = emailDto.Title,
                    Body = builder.ToMessageBody(),
                    Sender = new MailboxAddress(emailSetting.DisplayName, emailSetting.Username),
                };

                message.To.Add(MailboxAddress.Parse(emailDto.Address));

                using var smtpClient = new SmtpClient
                {
                    CheckCertificateRevocation = false,
                };

                await smtpClient.ConnectAsync(emailSetting.Host, emailSetting.Port);
                await smtpClient.AuthenticateAsync(emailSetting.Username, emailSetting.Password);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Send email error: {ex}");
                return false;
            }
        }

        private EmailSettingDto GetEmailSetting()
        {
            var emailSetting = new EmailSettingDto();
            _configuration
                .GetSection(AppSettingKeys.EmailSettingSection)
                .Bind(emailSetting);
            return emailSetting;
        }
    }
}
