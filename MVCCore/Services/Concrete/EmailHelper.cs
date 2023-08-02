using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MVCCore.Models;
using MVCCore.Options;
using MVCCore.Services.Abstract;
using System.Net;
using System.Net.Mail;

namespace MVCCore.Services.Concrete
{
    public class EmailHelper : IEmailHelper
    {
        private readonly EmailOptions _options;

        public EmailHelper(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendAsync(ContactModel model)
        {
            var client = new SmtpClient(_options.Server, _options.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_options.FromAddress, _options.Password)
            };

            await client.SendMailAsync(
                new MailMessage(from: _options.FromAddress,
                to: "alexandrumarica42@gmail.com", 
                model.Email,
                model.Message));
        }
    }
}
