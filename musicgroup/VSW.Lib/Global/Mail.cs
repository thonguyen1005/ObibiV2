using System;
using System.Net;
using System.Net.Mail;
using VSW.Core.Global;
using Convert = VSW.Core.Global.Convert;

namespace VSW.Lib.Global
{
    public static class Mail
    {
        public static void SendMail(string to, string from, string name, string subject, string body)
        {
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = Convert.ToInt(Config.GetValue("Mod.SmtpPort"))
            };

            //setup Smtp authentication
            var credentials = new NetworkCredential(Config.GetValue("Mod.SmtpUser").ToString(), Config.GetValue("Mod.SmtpPass").ToString());

            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            var msg = new MailMessage { From = new MailAddress(@from, name) };
            //msg.To.Add(new MailAddress(to));

            msg.To.Add(to);

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
            }
        }

        public static void SendMail(string to, string from, string name, string subject, AlternateView body, AlternateView plain)
        {
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = Convert.ToInt(Config.GetValue("Mod.SmtpPort"))
            };

            //setup Smtp authentication
            var credentials =
                new NetworkCredential(Config.GetValue("Mod.SmtpUser").ToString(), Config.GetValue("Mod.SmtpPass").ToString());

            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            var msg = new MailMessage { From = new MailAddress(from, name) };
            //msg.To.Add(new MailAddress(to));

            msg.To.Add(to);

            msg.Subject = subject;
            msg.IsBodyHtml = true;

            if (body != null) msg.AlternateViews.Add(body);
            if (plain != null) msg.AlternateViews.Add(plain);

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
            }
        }
    }
}