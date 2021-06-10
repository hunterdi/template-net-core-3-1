using System;
using System.Collections.Generic;
using System.Text;

namespace Architecture
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }
}
