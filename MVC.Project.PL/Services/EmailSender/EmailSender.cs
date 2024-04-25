using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MVC.Project.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration) 
        {
			_configuration = configuration;
		}

        public async Task SendAsync(string from, string recipients, string subject, string body)
		{
			var senderEmail = _configuration["EmailSetting:SenderEmail"];
			var senderPassword = _configuration["EmailSetting:SenderPassword"];


			var emailMessage = new MailMessage();
			emailMessage.From = new MailAddress(from);
			emailMessage.To.Add(recipients);
			emailMessage = new MailMessage();
			emailMessage.Subject = subject;
			emailMessage.Body = $"<html><body>{body}</body></html>";
			emailMessage.IsBodyHtml = true;


			var smtpClient = new SmtpClient(_configuration["EmailSetting:SmtpClientServer"], int.Parse(_configuration["EmailSetting:SmtpCLientPort"]) )
			{
				Credentials = new NetworkCredential(senderEmail, senderPassword),
				EnableSsl = true
			};

			await smtpClient.SendMailAsync(emailMessage);

		}
	}
}
