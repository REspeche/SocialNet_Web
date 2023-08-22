using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Hosting;
using Web_BusinessLayer.Classes;
using Web_Resource;

namespace Web.Helpers
{
    public static class MailHelper
    {
        public static ResponseMessage SendMail(string template, string[] parse)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                String sMessage_HTML = String.Empty;
                //Read template file from the App_Data folder
                string pathTem = HostingEnvironment.MapPath("/App_Data/Templates/");
                using (var sr = new StreamReader(String.Concat(pathTem, template)))
                {
                    sMessage_HTML = String.Format(sr.ReadToEnd().ToString(), parse);
                }

                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(parse[2], String.Concat(parse[0], " ", parse[1])));

                // From
                mailMsg.From = new MailAddress(Web_Resource.Mail_Template.ResetPass.fromMail, Web_Resource.Mail_Template.ResetPass.fromName);

                // Subject and multipart/alternative Body
                mailMsg.Subject = String.Format(Web_Resource.Mail_Template.ResetPass.subject, parse);
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(sMessage_HTML, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMsg);

                response.code = 0;
                response.message = Message.loginConfirm;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }

            return response;
        }
    }
}