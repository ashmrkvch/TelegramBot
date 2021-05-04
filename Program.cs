using System;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System.Net;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using News;
using Newtonsoft.Json;

namespace ConsoleApp5
{
    class Program
    {
        private static string Token { get; set; } = "1625761442:AAEgw4-nuNKyf22dYHQo8jvaRJmhbE7-Ih8";
        private static TelegramBotClient client;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(Token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();       
        }
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text == "/start")
            {
                await client.SendTextMessageAsync(msg.Chat.Id, $"Give me a topic for news!");
            }
            if (msg.Text != null && msg.Text != "/start")
            {
                var q = msg.Text;
                var url = "https://newsapi.org/v2/everything?" +
              $"q={q}&" +
              "from=2021-05-04&" +
              "sortBy=popularity&" +
              "apiKey=630aabc7eaa044c18d4cccd39f888335";

                var json = new WebClient().DownloadString(url);
                Console.WriteLine(json);
                New FromJson(string json) => JsonConvert.DeserializeObject<New>(json, Converter.Settings);
                New jsonEvent = JsonConvert.DeserializeObject<New>(json, Converter.Settings);
                if(jsonEvent.TotalResults == 0)
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, $" Ooops! No news today");
                }
                foreach (var x in jsonEvent.Articles)
                {
                   
                        await client.SendTextMessageAsync(msg.Chat.Id, $" <b>{x.Title}</b>\n<i>{x.Source.Name}</i>\n{x.Description}\n{x.Url}", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    

                }
                await client.SendTextMessageAsync(msg.Chat.Id, $"Give me a topic for news!");

            }
        }

    }
}
