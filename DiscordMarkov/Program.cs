using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace DiscordMarkov
{
    class Program
    {
        private CommandService Commands;
        private DiscordSocketClient Client;
        private IServiceProvider Services;

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            string FileLocation = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Info.json";

            JObject InfoJsonObject = JObject.Parse(File.ReadAllText(FileLocation));

            string Token = InfoJsonObject.GetValue("BotToken").ToString();

            MarkovHandler.InitializeChain();

            Client = new DiscordSocketClient();
            Commands = new CommandService();

            Services = new ServiceCollection().BuildServiceProvider();

            await InstallCommands();

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task InstallCommands()
        {
            Client.MessageReceived += HandleCommand;

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
        }

        public async Task HandleCommand(SocketMessage MessageParameter)
        {
            SocketUserMessage Message = MessageParameter as SocketUserMessage;

            int ArgumentPosition = 0;

            if (Message == null || 
                !Message.HasCharPrefix('!', ref ArgumentPosition))
            {
                return;
            }

            CommandContext Context = new CommandContext(Client, Message);
            IResult Result = await Commands.ExecuteAsync(Context, ArgumentPosition, Services);

            if (!Result.IsSuccess)
            {
                await Context.Channel.SendMessageAsync(Result.ErrorReason);
            }
        }
    }
}
