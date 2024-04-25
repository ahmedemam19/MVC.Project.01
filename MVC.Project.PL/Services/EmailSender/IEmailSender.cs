using System.Threading.Tasks;

namespace MVC.Project.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendAsync(string from, string recipients, string subject, string bode);
	}
}
