using BankAuth.Models;
using MailKit.Net.Smtp;
using MimeKit;


public class Attachment
{
    public string FilePath { get; set; }
    public string FileName { get; set; }

    public Attachment(string filePath, string fileName)
    {
        FilePath = filePath;
        FileName = fileName;
    }
}

namespace BankAuth.Services
{
  
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig) => _emailConfig = emailConfig;
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }



        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Alpha Bank", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            var bodyBuilder = new BodyBuilder();

            var htmlBody = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };
            bodyBuilder.Attachments.Add(htmlBody);

           // bodyBuilder.TextBody = message.Content;

            foreach (var attachment in message.Attachments)
            {
                var attachmentPart = new MimePart()
                {
                    Content = new MimeContent(File.OpenRead(attachment.FilePath), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName
                };

                bodyBuilder.Attachments.Add(attachmentPart);
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();



            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

}
