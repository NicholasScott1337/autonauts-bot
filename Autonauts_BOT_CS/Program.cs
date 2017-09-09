using Discord;
using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Rest;
using System.Collections;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;

namespace Autonauts_BOT_CS
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler CommandSys;
        private VoteHandler VoteHandler;
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            VoteHandler = new VoteHandler();

            CommandSys = new CommandHandler(new CommandObject[] {
                new CommandObject("Help", "Shows the help menu", "", async(MSG, ARGS, CMDS) =>
                {
                    // Embed Main
                    EmbedBuilder HelpEmbed = new EmbedBuilder()
                        .WithColor(new Color(0, 255, 0))
                        .WithCurrentTimestamp()
                        .WithFooter("GeT_ReKt", "https://cdn.discordapp.com/avatars/160901181692968971/6721b2aaba4ec2e0eebe94eb4bad371b.png");
                    foreach (CommandObject CmdObj in CMDS)
                    {
                        HelpEmbed.AddField("!" + CmdObj.CmdName + " " + CmdObj.CmdArgs, CmdObj.CmdDesc);
                    }
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("PLEASE SEND ALL COMMANDS IN DIRECT MESSAGE WITH THE BOT IF YOU CAN\nYou can replace `!` with `?`.", false, HelpEmbed.Build());
                    } else
                    {
                        await MSG.Author.SendMessageAsync("PLEASE SEND ALL COMMANDS IN DIRECT MESSAGE WITH THE BOT IF YOU CAN\nYou can replace `!` with `?`.", false, HelpEmbed.Build());
                    }
                }),
                new CommandObject("Download", "Get the download link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://denki.itch.io/autonauts");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://denki.itch.io/autonauts");
                    }
                }),
                new CommandObject("Changelog", "Get the changelog link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://docs.google.com/spreadsheets/d/1s2nIsLg-suLVX_O6MgDGtxZDLgmVXvwIzwauNDpUoiQ");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://docs.google.com/spreadsheets/d/1s2nIsLg-suLVX_O6MgDGtxZDLgmVXvwIzwauNDpUoiQ");
                    }
                }),
                new CommandObject("Editor", "Get JohnGames Level Editor link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://john-games.itch.io/unofficial-autonauts-level-editor");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://john-games.itch.io/unofficial-autonauts-level-editor");
                    }
                }),
                new CommandObject("Reddit", "Get the Autonauts reddit link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://www.reddit.com/r/Autonauts/");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://www.reddit.com/r/Autonauts/");
                    }
                }),
                new CommandObject("Twitter", "Get the Autonauts twitter link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://twitter.com/Automationauts");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://twitter.com/Automationauts");
                    }
                }),
                new CommandObject("Instagram", "Get the Autonauts instagram link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://www.instagram.com/automationauts/");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://www.instagram.com/automationauts/");
                    }
                }),
                new CommandObject("Denki", "Get the Denki website link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://www.denki.co.uk/");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://www.denki.co.uk/");
                    }
                }),
                new CommandObject("CommunityHub", "Get the Unofficial Community Hub link.", "", async(MSG, ARGS, CMDS) =>
                {
                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("https://gamewiki-autonauts.jimdo.com/");
                    } else
                    {
                        await MSG.Author.SendMessageAsync("https://gamewiki-autonauts.jimdo.com/");
                    }
                }),
                new CommandObject("OldSaves", "Where are my old saves located?", "", async(MSG, ARGS, CMDS) =>
                {
                    EmbedBuilder OldSavesEmbed = new EmbedBuilder()
                        .WithColor(new Color(0, 255, 0))
                        .WithFooter("GeT_ReKt", "https://cdn.discordapp.com/avatars/160901181692968971/6721b2aaba4ec2e0eebe94eb4bad371b.png")
                        .WithCurrentTimestamp()
                        .AddField("Windows", @"`%userprofile%\Appdata\LocalLow\Denki\Autonauts`")
                        .AddField("Mac", @"`Library/ApplicationSupport/unity.Denki.Autonauts`")
                        .AddField("Linux", @"`~/.config/unity3d/Denki/Autonauts`");

                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("", false, OldSavesEmbed.Build());
                    } else
                    {
                        await MSG.Author.SendMessageAsync("", false, OldSavesEmbed.Build());
                    }
                }),
                new CommandObject("SaveLocation", "Where are my old saves located?", "", async(MSG, ARGS, CMDS) =>
                {
                    EmbedBuilder OldSavesEmbed = new EmbedBuilder()
                        .WithColor(new Color(0, 255, 0))
                        .WithFooter("GeT_ReKt", "https://cdn.discordapp.com/avatars/160901181692968971/6721b2aaba4ec2e0eebe94eb4bad371b.png")
                        .WithCurrentTimestamp()
                        .AddField("Windows", @"`%userprofile%\Appdata\LocalLow\Denki\Autonauts`")
                        .AddField("Mac", @"`Library/ApplicationSupport/unity.Denki.Autonauts`")
                        .AddField("Linux", @"`~/.config/unity3d/Denki/Autonauts`");

                    if (MSG.MentionedUsers.Count > 0)
                    {
                        await MSG.Channel.SendMessageAsync("", false, OldSavesEmbed.Build());
                    } else
                    {
                        await MSG.Author.SendMessageAsync("", false, OldSavesEmbed.Build());
                    }
                }),
                new CommandObject("Feedback", "Send feedback or information directly to the developer of me!", "<Your Message>", async(SocketMessage MSG, string[] ARGS, CommandObject[] CMDS) =>
                {
                    EmbedBuilder FeedbackEmbed = new EmbedBuilder()
                        .WithColor(new Color(255, 0, 0))
                        .WithAuthor(MSG.Author.Username, MSG.Author.GetAvatarUrl())
                        .WithCurrentTimestamp()
                        .WithDescription(MSG.Content.Substring(10))
                        .AddInlineField("In Channel", "<#" + MSG.Channel.Id + ">")
                        .AddInlineField("From User", "<@" + MSG.Author.Id + ">");
                    await _client.GetGuild(345372586378526722).GetTextChannel(354804021300822018).SendMessageAsync("Feedback!", false, FeedbackEmbed.Build());
                    await MSG.Author.SendMessageAsync("Sent feedback to <@160901181692968971> !");
                }),
                new CommandObject("Suggest", "Make a suggestion to improve Autonauts!", "", async(SocketMessage MSG, string[] ARGS, CommandObject[] CMDS) =>
                {
                    if (MSG.Content.Substring(8).Length < 25)
                    {
                        await MSG.Author.SendMessageAsync("Your suggestion was rejected due to a lack of content, please make sure your suggestion is well explained and greater than 25 characters :D");
                        return;
                    }
                    EmbedBuilder NewSuggestEmbed = new EmbedBuilder()
                        .WithAuthor(MSG.Author.Username, MSG.Author.GetAvatarUrl())
                        .WithColor(new Color(0, 0, 255))
                        .WithCurrentTimestamp()
                        .WithDescription(MSG.Content.Substring(8))
                        .WithFooter(_client.GetUser(160901181692968971).Username, _client.GetUser(160901181692968971).GetAvatarUrl())
                        .AddInlineField("Rate!", "Once a suggestion recieves a net score of " + new VoteMessage().Threshold + " upvotes it will be added to the suggestions command!");
                    RestUserMessage SentMessage = await _client.GetGuild(331886561044201473).GetTextChannel(332835394620096512).SendMessageAsync("", false, NewSuggestEmbed.Build());
                    
                    await SentMessage.AddReactionAsync(new UpArrow(), null);
                    await SentMessage.AddReactionAsync(new DownArrow(), null);
                    
                    VoteHandler.CreateNewVoteMessage(SentMessage.Id, MSG.Author, MSG.Content.Substring(8));
                }),
                new CommandObject("Suggestions", "Get the suggestions made!", "[page]", async(SocketMessage MSG, string[] ARGS, CommandObject[] CMDS) =>
                {
                    if (VoteHandler.Config.SuccesfullMessages.Count == 0)
                    {
                        await MSG.Author.SendMessageAsync("There isn't any successful suggestions to display!");
                    } else
                    {
                        int CurrentPage;
                        if (!Int32.TryParse(ARGS[0], out CurrentPage))
                        {
                            CurrentPage = 1;
                        }
                        float AmountOfPages = VoteHandler.Config.SuccesfullMessages.Count / 10;
                        int PagesRounded = (int)Math.Ceiling(AmountOfPages);

                        if (CurrentPage > PagesRounded) CurrentPage = PagesRounded;
                        EmbedBuilder SuggestionsPageEmbed = new EmbedBuilder()
                            .WithColor(new Color(0, 255, 0))
                            .WithCurrentTimestamp()
                            .WithDescription("Current Page: " + ARGS[0] + "/" + PagesRounded)
                            .WithFooter(_client.GetUser(160901181692968971).Username, _client.GetUser(160901181692968971).GetAvatarUrl());

                        for (int i = CurrentPage*10-10; i == CurrentPage*10; i++)
                        {
                            VoteMessage temp_MSG = VoteHandler.Config.SuccesfullMessages[i];
                            if (temp_MSG != null)
                            {
                                SuggestionsPageEmbed.AddInlineField(temp_MSG.Author.Username, temp_MSG.Suggestion);
                            }
                        }
                        await MSG.Author.SendMessageAsync("", false, SuggestionsPageEmbed.Build());
                    }
                })
            });
            
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.ReactionAdded += ReactionAdded;

            //string token = "MjkxNjc3MTU2Njc1NjgyMzA0.DJN23A.dTSMgGNCWZ0XzxmOpSw-y8Xcj5Y"; //BenderBot
            string token = "MzQ1MzY5NjM3OTYzNDMxOTM3.DIvfgw.Mh5kIKgnPx84gXvNolSbSvjJXrA"; //Autonauts Utility
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            ulong MsgID = arg1.Id;
            foreach(VoteMessage CurMessage in VoteHandler.Config.Messages)
            {
                if (CurMessage.MessageID == MsgID)
                {
                    if (arg3.Emote.Name == "⬆")
                    {
                        if (!CurMessage.Upvotes.Contains(arg3.UserId))
                        {
                            CurMessage.Upvotes.Add(arg3.UserId);
                            VoteHandler.SaveToFile();
                        }
                    } else if (arg3.Emote.Name == "⬇")
                    {
                        if (!CurMessage.Downvotes.Contains(arg3.UserId))
                        {
                            CurMessage.Downvotes.Add(arg3.UserId);
                            VoteHandler.SaveToFile();
                        }
                    }
                    break;
                }
            }
            return Task.CompletedTask;
        }
        public Task Log(LogMessage msg)
        {
            if (msg.Severity == LogSeverity.Critical) { Console.ForegroundColor = ConsoleColor.DarkRed; }
            if (msg.Severity == LogSeverity.Debug) { Console.ForegroundColor = ConsoleColor.Cyan; }
            if (msg.Severity == LogSeverity.Error) { Console.ForegroundColor = ConsoleColor.Red; }
            if (msg.Severity == LogSeverity.Info) { Console.ForegroundColor = ConsoleColor.White; }
            if (msg.Severity == LogSeverity.Warning) { Console.ForegroundColor = ConsoleColor.Yellow; }
            Console.WriteLine(msg.ToString());
            Console.ResetColor();
            return Task.CompletedTask;
        }
        private async Task MessageReceived(SocketMessage message)
        {
            await Task.Run(() => CommandSys.EvaluateCommandAsync(message, _client));
        }
    }
}
