﻿using System.Threading.Tasks;

namespace MojBus.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
