namespace ProductFocus.Services
{
    public interface IEmailService
    {
        void send(string emailBody, string email);
    }
}
