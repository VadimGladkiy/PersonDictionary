using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace PersonDictionary.SendingToEmail
{
    public static class SendPasswordToEmail
    {
        private static String adminEmail = "VadimHellsing@gmail.com";
        private static String adminMailKeyAccess = "swhqtltsmdwedeou";
        public static bool SendPassword(String UserEmail, String Password)
        {
            MailMessage message = new MailMessage(adminEmail,UserEmail);
            message.Subject = "Here is Your password to PersonDictionary";
            message.Sender = new MailAddress(adminEmail);
            message.Body = Password;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential()
            {
                UserName = adminEmail,           
                // this is special key to access the prev mail 
                Password = adminMailKeyAccess
            };

            client.EnableSsl = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                return false;
                // some handler also
            }
            return true;
        }
        public static void ChangeAdminEmail(String newAdminEmail)
        {
            adminEmail = newAdminEmail;
        }
        public static void ChangeAdminMailKeyAccess(String newAdminMailKeyAccess)
        {
            adminMailKeyAccess = newAdminMailKeyAccess;
        }
    }
}