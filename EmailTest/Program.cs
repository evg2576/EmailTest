using System.Net;
using System.Net.Mail;

internal class Program
{
    private static void Main(string[] args)
    {
        Send();
    }

    public static void Send()
    {
        //MailMessage mail = new MailMessage();
        //SmtpClient SmtpServer = new SmtpClient("smtp.ethereal.email");
        //mail.From = new MailAddress("24evgeniy00@gmail.com");
        //mail.To.Add("24evgeniy03@gmail.com");
        //mail.Subject = "Test Mail - 1";
        //mail.Body = "mail with attachment";

        //Attachment attachment;
        //attachment = new Attachment("c:/textfile.txt");
        //mail.Attachments.Add(attachment);

        //SmtpServer.Port = 587;
        //SmtpServer.Credentials = new NetworkCredential("24evgeniy00@gmail.com", "22evgeniy22");
        //SmtpServer.EnableSsl = true;
        //SmtpServer.UseDefaultCredentials = false;

        //SmtpServer.Send(mail);


        String SendMailFrom = "24evgeniy00@gmail.com";
        String SendMailTo = "24evgeniy03@gmail.com";
        String SendMailSubject = "Email Subject";
        String SendMailBody = "Email Body";

        try
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage email = new MailMessage();
            // START
            email.From = new MailAddress(SendMailFrom);
            email.To.Add(SendMailTo);
            email.CC.Add(SendMailFrom);
            email.Subject = SendMailSubject;
            email.Body = SendMailBody;
            //END
            SmtpServer.Timeout = 5000;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "qvopvrxbosdagrsd");
            SmtpServer.Send(email);

            Console.WriteLine("Email Successfully Sent");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.ReadKey();
        }
    }
}