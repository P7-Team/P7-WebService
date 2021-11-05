namespace WebService.Models
{
    public enum MessageType
    {
        Working,
        ReadyToWork,
        Done
    }
    public class HeartBeat
    {
        private MessageType _messageType;
        public HeartBeat(MessageType messageType)
        {
            _messageType = messageType;
        }
    }
}