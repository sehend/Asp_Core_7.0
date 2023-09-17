using IdentityUyelik.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IdentityUyelik.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host = _emailSettings.Host;

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.UseDefaultCredentials = false;

            smtpClient.Port = 587;

            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

            smtpClient.EnableSsl = true;

            var mailMassege = new MailMessage();

            mailMassege.From = new MailAddress(_emailSettings.Email);

            mailMassege.To.Add(ToEmail);

            mailMassege.Subject = "Local Host Şifre Sıfırlama Linki....";

            mailMassege.Body = $"<h4>Şifrenizi Yenilemek İçin Aşadaki Linke Tıklayınız.<p><a href='{resetPasswordEmailLink}'>Şifre Yenileme Link</a></p></h4>";

            mailMassege.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMassege);
        }
    }
}
