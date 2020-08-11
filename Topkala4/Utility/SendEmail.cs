using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TopKala3.Utility
{
    public static class SendEmail
    {
        public static void Send(string To, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
          
            mail.From = new MailAddress("topkalacore@topkalacore.ir", "تاپ  کالا");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
          
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient("win2016-740ir.hostnegar.com", 25))
            {
                
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("topkalacore@topkalacore.ir", "rampageyuri78");
                smtp.EnableSsl = false;
                
                smtp.Send(mail);

            }

          

        }
    }
}
