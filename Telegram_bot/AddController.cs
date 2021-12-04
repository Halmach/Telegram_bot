using System;
using System.Collections.Generic;

namespace Telegram_bot
{
    public class AddController
    {
        Dictionary <long, AddState> controllerState;
        public AddController()
        {
            controllerState = new Dictionary<long, AddState>();
        }

        public void SetFirstState(Conversation chat)
        {
            controllerState.Add(chat.GetId(), AddState.Russian);
        }

        public AddState GetState(Conversation chat)
        {
            return controllerState[chat.GetId()];
        }

        public void NextState(Conversation chat)
        {
            var curState = controllerState[chat.GetId()];
            controllerState[chat.GetId()] = curState + 1;

            if(controllerState[chat.GetId()] == AddState.Finish)
            {
                controllerState.Remove(chat.GetId());
                chat.IsAddInProgress = false;
            }
        }
    }
}