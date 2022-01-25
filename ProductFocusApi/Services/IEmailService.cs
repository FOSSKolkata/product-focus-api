namespace ProductFocus.Services
{
    public interface IEmailService
    {
        void Send(string emailBody, string email);
    }
}
