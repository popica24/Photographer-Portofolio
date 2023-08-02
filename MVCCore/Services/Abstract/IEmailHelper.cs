using MVCCore.Models;

namespace MVCCore.Services.Abstract
{
    public interface IEmailHelper
    {
        Task SendAsync(ContactModel model);
    }
}
