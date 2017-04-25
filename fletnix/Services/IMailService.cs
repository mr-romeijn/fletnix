using System;
namespace fletnix.Services
{
    public interface IMailService
    {
        void sendMail(string to, string from, string subject, string body);
    }
}
