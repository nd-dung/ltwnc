using System.Net;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;

namespace Web_TuyenDung.Utils
{
    public class MailUtils
    {
        public static async Task<int> GuiThongBao(string _to, string _subject, string _body)
        {
			_body = _body.Replace("\r\n", "<br/>");
			string _from = "vngtuyendungg21@gmail.com";
            string _password = "uhafjshtxyhbmkur";
			MailMessage mailMessage = new MailMessage(_from, _to, _subject, _body);
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            mailMessage.ReplyToList.Add(new MailAddress(_from));
            mailMessage.Sender = new MailAddress(_from);

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential(_from, _password);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("-------lỗi: " + ex.ToString());
                return 0;
            }

        }

    }
}
