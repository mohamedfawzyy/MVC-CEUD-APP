using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MVC.PL.Services.SendEmailService
{
	public class SendEmail : ISenderEmail
	{
		private readonly IConfiguration configuration;

		public SendEmail(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        public async Task SendAsync(string From, string Recipients, string Body, string Subject)
		{
			var EmailMessage = new MailMessage();
			EmailMessage.From = new MailAddress(From);
			EmailMessage.To.Add(Recipients); 
			EmailMessage.Subject = Subject;
			EmailMessage.Body =$"<html><body><a href=\"{Body}\">Reset Your Password</a></body></html>";
			EmailMessage.IsBodyHtml = true; 

			 var smtpClient=new SmtpClient(this.configuration["EmailSetting:Host"],int.Parse(this.configuration["EmailSetting:Port"])) {
				Credentials =new NetworkCredential(this.configuration["EmailSetting:EmailUser"], this.configuration["EmailSetting:Password"]),
				EnableSsl = true
			
			};
			await smtpClient.SendMailAsync(EmailMessage);

		}
	}
}
