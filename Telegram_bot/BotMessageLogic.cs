
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    class BotMessageLogic
    {
        private Messenger messanger;
        ITelegramBotClient botClient;
        private Dictionary<long, Conversation> chatList;

        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            if (!chatList.ContainsKey(id))
            {
                var newchat = new Conversation(e.Message.Chat);
                chatList.Add(id,newchat);
            }

            var chat = chatList[id];
            chat.AddMessage(e.Message);

            await SendTextMessage(chat);


        }

        private async Task SendTextMessage(Conversation chat)
        {
            var text = messanger.CreateTextMessage(chat);

            
            int numButtonCommand = messanger.isThisButton(chat);
            if(numButtonCommand == 1) 
                await SendTextWithKeyBoard(chat, text, ReturnKeyBoard());
            else await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

       

        private async Task SendTextWithKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(
                chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

       

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "Пушкин",
                    CallbackData = "pushkin"
                },

                new InlineKeyboardButton
                {
                    Text = "Есенин",
                    CallbackData = "esenin"
                }

            };
            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            chatList = new Dictionary<long, Conversation>();
            messanger = new Messenger();

        }


    }
}