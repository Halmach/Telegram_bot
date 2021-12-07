using System;
using System.Collections.Generic;

namespace Telegram_bot
{
    public class AddController
    {
        private Dictionary <long, AddState> controllerState;

        public AddController()
        {
            this.controllerState = new Dictionary<long, AddState>();
        }

        public void SetFirstState(Conversation chat)
        {
            this.controllerState.Add(chat.GetId(), AddState.Russian);
        }

        public AddState GetState(Conversation chat)
        {
            return this.controllerState[chat.GetId()];
        }

        public void NextState(Conversation chat)
        {
            var curState = this.controllerState[chat.GetId()];
            this.controllerState[chat.GetId()] = curState + 1;

            if (this.controllerState[chat.GetId()] == AddState.Finish)
            {
                this.controllerState.Remove(chat.GetId());
                chat.IsAddInProgress = false;
            }
        }
    }
}