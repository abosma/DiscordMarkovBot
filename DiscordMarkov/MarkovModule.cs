using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordMarkov
{
    public class MarkovModule : ModuleBase
    {
        [Command("markov"), Summary("Sends a sentence based on a markov chain of previous chat logs")]
        public async Task Markov([Remainder, Summary("Markov Sentence")] string markov = null)
        {
            if (markov == null)
            {
                string ReturnMessage = MarkovHandler.ReturnMessage();

                if (ReturnMessage.Length < 2000)
                {
                    await Context.Channel.SendMessageAsync(ReturnMessage);
                }
                else
                {
                    string NewMessage = ReturnMessage.Substring(0, 2000);
                    await Context.Channel.SendMessageAsync(NewMessage);
                }
                
            }
            else
            {
                MarkovHandler.AddString(markov);
                await Context.Channel.SendMessageAsync("Added message to markov chain.");
            }
        }

    }
}
