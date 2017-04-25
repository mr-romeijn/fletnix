using System;
using System.Diagnostics;
namespace fletnix.Services
{
    public class DebugMailService : IMailService
    {
        public DebugMailService()
        {
        }

        public void sendMail(string to, string from, string subject, string body)
        {
            Console.WriteLine($"Sending Mail: To:{to} From: {from} Subject: {subject}");
        }
    }
}
