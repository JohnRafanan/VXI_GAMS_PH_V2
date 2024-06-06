using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VXI_GAMS_US.HELPER
{
    public static class EmailHelper
    {
        public static async Task SendEmail(string emailTo, string emailCc = "", string emailBcc = "", string emailSubject = "", string emailBody = "")
        {
            var displayName = ConfigurationManager.AppSettings["EMAIL_DISPLAY_NAME"].ToString();
            try
            {
                using (var client = new SmtpClient(ConfigurationManager.AppSettings["EMAIL_SMTP_HOST"]))
                {
                    if (ConfigurationManager.AppSettings["EMAIL_SMTP_HOST"] == "TESTING")
                    {
                        
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = false;
                        client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EMAIL_USERNAME"], ConfigurationManager.AppSettings["EMAIL_PASSWORD"], ConfigurationManager.AppSettings["EMAIL_DOMAIN"]);
                        using (var mail = new MailMessage())
                        {
                            mail.From = new MailAddress(ConfigurationManager.AppSettings["EMAIL_FAKE_EMAIL"], displayName, Encoding.UTF8);
                            mail.To.Add(emailTo);
                            if (!string.IsNullOrEmpty(emailCc))
                                mail.CC.Add(emailCc);
                            if (!string.IsNullOrEmpty(emailBcc?.Trim()))
                                if (!emailBcc.Contains(","))
                                {
                                    mail.Bcc.Add(emailBcc.Trim());
                                }
                                else
                                {
                                    var bcc = emailBcc.Split(',');
                                    foreach (var email in bcc)
                                        if (!string.IsNullOrEmpty(email.Trim()))
                                            mail.Bcc.Add(email.Trim());
                                }

                            mail.Headers.Add("Content-Type", "text/plain");
                            mail.HeadersEncoding = Encoding.UTF8;
                            mail.From = new MailAddress("no-reply@vxi.com.ph", displayName, Encoding.UTF8);
                            mail.Subject = emailSubject;
                            mail.SubjectEncoding = Encoding.UTF8;
                            mail.IsBodyHtml = true;
                            mail.Priority = MailPriority.High;
                            mail.Body = emailBody;
                            await client.SendMailAsync(mail);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
