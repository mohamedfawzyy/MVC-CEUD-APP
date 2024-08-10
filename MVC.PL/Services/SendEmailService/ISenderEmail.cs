using System.Threading.Tasks;

namespace MVC.PL.Services.SendEmailService
{
	public interface ISenderEmail
	{
		Task SendAsync(string From , string Recipients , string Body , string Subject);
	}
}
