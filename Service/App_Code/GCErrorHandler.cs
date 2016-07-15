using System;
using System.Configuration;
using System.ServiceModel.Web;
using System.Net.Mail;
using User;
namespace ErrorHandler
{
    public class GCErrorHandler
    {
        public static void ReportError(Exception ex)
        {
            bool enableEmail = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ENABLE_ERROR_LOG_MAIL"]) && (Convert.ToBoolean(ConfigurationManager.AppSettings["ENABLE_ERROR_LOG_MAIL"]));
            if (enableEmail)
            {
                string emailFrom = ConfigurationManager.AppSettings["ERROR_EMAIL_FROM"];
                string emailTo = ConfigurationManager.AppSettings["EMAIL_DEV_TEAM"];
                string subject = ConfigurationManager.AppSettings["APP_NAME"] + " Error Report";
                string dateTime = DateTime.Now.ToLongDateString() + ", at " + DateTime.Now.ToShortTimeString();
                string SMTP_HOST = ConfigurationManager.AppSettings["SMTP_HOST"];
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Append("Exception generated on " + dateTime + "<br><br>" + Environment.NewLine);
                str.Append("Query String: " + WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.PathAndQuery + "<br><br>" + Environment.NewLine);
                str.Append("User Id: " + GCUser.GetUserLogOnName() + "<br><br>" + Environment.NewLine);
                str.Append("Message: " + ex.Message + "<br><br>" + Environment.NewLine);
                str.Append("Source: " + ex.Source + "<br><br>" + Environment.NewLine);
                str.Append("Stack Trace: " + ex.StackTrace + "<br><br>" + Environment.NewLine);

                SmtpClient mailClient = new SmtpClient(SMTP_HOST, 25);
                //Release resource
                mailClient.SendCompleted += new SendCompletedEventHandler(mailClient_SendCompleted);

                //Create email
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(emailFrom);
                mailMsg.Subject = subject;
                mailMsg.Body = str.ToString();
                mailMsg.IsBodyHtml = true;
                if (!String.IsNullOrEmpty(emailTo))
                {
                    string[] mailCollection = emailTo.Split(';');
                    foreach (string address in mailCollection)
                    {
                        if (!String.IsNullOrEmpty(address.Trim()))
                            mailMsg.To.Add(address.Trim());
                    }
                }
                try
                {
                    mailClient.Send(mailMsg);
                }
                finally
                {
                    mailMsg.Dispose();
                }
            }
        }

        private static void mailClient_SendCompleted(object sender,
       System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailMessage msg = (MailMessage)e.UserState;
            if (msg != null)
                msg.Dispose();
        }
    }
}