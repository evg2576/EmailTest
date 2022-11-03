using MailKit.Net.Smtp;
using MimeKit;

internal class Program
{
    static async Task Main(string[] args)
    {
        MailService mailService = new MailService();
        FileStream fileStream = new FileStream("c:/textfile.txt", FileMode.Open);
        string[] mailsTo = { "24evgeniy03@gmail.com" };
        var res = await mailService.Send(mailsTo, "Subject123", "Body123", fileStream, "Test123");
    }
}

public class MailService : IMailService
{
    public async Task<bool> Send(string[] to, string subject, string body, Stream file = null, string file_name = null)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "24evgeniy00@gmail.com"));
            emailMessage.Subject = subject;

            var recipients = new List<MailboxAddress>();
            foreach (var item in to)
                recipients.Add(new MailboxAddress("", item));
            emailMessage.To.AddRange(recipients);

            var builder = new BodyBuilder();
            builder.TextBody = body;
            if (file != null && file_name != null)
            {
                FileStream fs = file as FileStream;
                builder.Attachments.Add(file_name + Path.GetExtension(fs.Name), file);
            }
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("24evgeniy00@gmail.com", "password");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception)
        {
            throw;
            return false;
        }
    }
}

public interface IMailService
{
    Task<bool> Send(string[] to, string subject, string body, Stream file = null, string file_name = null);
}