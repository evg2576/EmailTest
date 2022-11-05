using MailKit.Net.Smtp;
using MimeKit;

internal class Program
{
    static async Task Main(string[] args)
    {
        MailService mailService = new MailService(new MailOptions { DisplayName = "Администрация сайта", Mail = "", Port = 465, Host = "smtp.gmail.com", SSL = true, Password = "" });
        FileStream fileStream = new FileStream("c:/textfile.txt", FileMode.Open);
        string[] mailsTo = { "" };
        var res = await mailService.Send(mailsTo, "Subject123", "<h1>Body123</h1><p>123321</p>123", fileStream, "Test123");
    }
}

// readme для заказчика по добавлению почты, с которой будут отправляться сообщения
// зайти в управление аккаунтом Google 
// на вкладке "Безопасность" в разделе Вход в аккаунт Google включить Двухэтапную аутентификацию
// в этом же разделе зайти в "Пароли приложений"
// в выпадающем списке "Приложение" выбрать "Другое", ввести название и нажать "Создать, после чего сгенерируется пароль для приложения

public class MailOptions
{
    public string Host { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
    public int Port { get; set; }
    public bool SSL { get; set; }
}

public interface IMailService
{
    Task<bool> Send(string[] to, string subject, string body, Stream file = null, string file_name = null);
}

public class MailService : IMailService
{
    MailOptions mailOptions;

    public MailService(MailOptions mailOptions)
    {
        this.mailOptions = mailOptions;
    }

    public async Task<bool> Send(string[] to, string subject, string body, Stream file = null, string file_name = null)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(mailOptions.DisplayName, mailOptions.Mail));
            emailMessage.Subject = subject;

            var recipients = new List<MailboxAddress>();
            foreach (var item in to)
                recipients.Add(new MailboxAddress("", item));
            emailMessage.To.AddRange(recipients);

            var builder = new BodyBuilder();
            builder.HtmlBody = body;

            if (file != null && file_name != null)
            {
                FileStream fs = file as FileStream;
                builder.Attachments.Add(file_name + Path.GetExtension(fs.Name), file);
            }
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailOptions.Host, mailOptions.Port, mailOptions.SSL);
                await client.AuthenticateAsync(mailOptions.Mail, mailOptions.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}