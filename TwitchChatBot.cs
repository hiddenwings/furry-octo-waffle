using System;
using System.Speech.Synthesis;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace TwitchChatBot_WPF_
{
    internal class TwitchChatBot
    {
        //Образ класса логгера
        Logger logger = new Logger();
        TwitchClient client;
        //криденшеналы для подключения
        readonly ConnectionCredentials credentials = new ConnectionCredentials(TwitchBotInfo.BotUsername, TwitchBotInfo.BotToken);
        SpeechSynthesizer synth = new SpeechSynthesizer();
        //Коннектит бота к чату после нажатия на кнопку
        internal void Connect()
        {
            Console.WriteLine("Connecting...");

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, TwitchBotInfo.ChannelName);
            //Функции бота логи тоже здесь
            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnConnectionError += Client_Error;
            client.OnMessageReceived += Client_OnMessageReceived;

            client.Connect();
        }
        //Вызывается  как только начнет подключаться и начнет выводить строки подключения
        public void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);//для наглядности че должно быть в логе
            logger.Set(e.Data);
        }
        //Все что нмже тебя неинтересует так как выполняются только в чате во время написания
        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Hey guys! I am a bot connected via TwitchLib!");
            client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("badword"))
                client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");
            else if (e.ChatMessage.Message.Contains("Привет"))
                client.SendMessage(e.ChatMessage.Channel, $"Дарова клоун {e.ChatMessage.DisplayName}");
            else if (e.ChatMessage.IsHighlighted)
                client.SendMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} купил сообщение PogChamp");
        }

        private void Client_Error(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!!! + {e.Error}");
        }

        internal void Disconnect()
        {
            client.SendMessage(TwitchBotInfo.ChannelName, "Sorry guys, but I'm leaving :(");
            client.Disconnect();
        }
    }
}