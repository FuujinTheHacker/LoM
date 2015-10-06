using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace AdministrationProgram
{
    class EmailSender
    {
        public static void SendEmail(string aSenderAdress, string aSenderPassword, string aReciever, string aTopic, string aText)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(aReciever);
            mail.From = new MailAddress(aSenderAdress);
            mail.Subject = aTopic;
            mail.Body = aText;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(aSenderAdress, aSenderPassword);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        //private static void SendEmail(string aSender, string aSenderPassword, string aReciever, string aTopic, string aText)
        //{
        //    MailMessage mail = new MailMessage();

        //    mail.To.Add(aReciever);
        //    mail.From = new MailAddress(aSender);
        //    mail.Subject = aTopic;
        //    mail.Body = aText;
        //    mail.IsBodyHtml = true;

        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.Credentials = new System.Net.NetworkCredential(aSender, aSenderPassword);
        //    smtp.Port = 587;

        //    //Or your Smtp Email ID and Password
        //    smtp.EnableSsl = true;
        //    smtp.Send(mail);

        //}
    }
}
