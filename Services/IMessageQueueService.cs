public interface IMessageQueueService
{
    Task PublishEmailNotificationAsync(string userEmail, string message);
}
