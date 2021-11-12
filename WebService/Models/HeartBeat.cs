using System;

namespace WebService.Models
{
    // TODO Potentially rename IsJobeDone
    public enum MessageType
    {
        Working,
        Available,
        Done,
        IsJobDone,
        Error
    }

    public class HeartBeat
    {
        private MessageType _heartBeat;

        public HeartBeat(string input)
        {
            if (SetMessageType(input) == false)
            {
                _heartBeat = MessageType.Error;
            }
        }

        public HeartBeat()
        {
        }


        public MessageType GetMessageType()
        {
            return _heartBeat;
        }

        public bool SetMessageType(string input)
        {
            bool canParse = Enum.TryParse(StringCapitalization(input), out MessageType output);
            if (canParse)
            {
                _heartBeat = output;
            }

            return canParse;
        }

        private string StringCapitalization(string input)
        {
            char[] charRepresentation = input.ToCharArray();
            for (int i = 0; i < charRepresentation.Length; i++)
            {
                charRepresentation[i] = char.ToLower(charRepresentation[i]);
            }

            charRepresentation[0] = char.ToUpper(charRepresentation[0]);
            return new string(charRepresentation);
        }
    }
}