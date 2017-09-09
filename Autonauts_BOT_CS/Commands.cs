using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using Discord.Rest;
using Discord;

namespace Autonauts_BOT_CS
{
    class CommandHandler
    {
        private CommandObject[] Commands;
        private RegularExpressions RegExpression = new RegularExpressions();
        public CommandHandler(CommandObject[] CommandList)
        {
            Commands = CommandList;
        }
        public CommandHandler()
        {
            Commands = new CommandObject[] { };
        }
        public async void EvaluateCommandAsync(SocketMessage message, DiscordSocketClient client)
        {
            Match ExpRet = RegExpression.SplitCommandsFromArgs.Match(message.Content);
            if (ExpRet.Success)
            {
                String Command = ExpRet.Groups[1].Value;
                String Args = ExpRet.Groups[2].Value;
                String[] ArgsSplit = RegExpression.SplitArgs.Split(Args);
                int IndexOfCommand = Array.FindIndex(Commands, (CMD) => { return (CMD.CmdName.ToLower() == Command.ToLower()); }); //IsValidCommand(CMD, Command)
                if (IndexOfCommand > -1)
                {
                    try
                    {
                        await Log(new LogMessage(LogSeverity.Info, "Evaluation", "Recieved command: " + Command + " | From: " + message.Author.Username + " | Args: " + Args));
                        await Task.Run(() => Commands[IndexOfCommand].CmdExec(message, ArgsSplit, Commands));
                        
                    }
                    catch (Exception err)
                    {
                        await message.Author.SendMessageAsync("I've encountered an error and your command was not completely processed, please ignore any replies you got to your issued command.");
                        LogMessage MsgToLog = new LogMessage(LogSeverity.Error, "Evaluation", "Command Errored: " + err.ToString());
                        await Log(MsgToLog);
                    }
                }
            }
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
    }
    class CommandObject
    {
        public String CmdName;
        public String CmdDesc;
        public String CmdArgs;
        public CmdObjectFuncExec CmdExec;
        public CommandObject(String CommandName, String CommandDescription, String CommandArgs, CmdObjectFuncExec FunctionToExecute)
        {
            CmdName = CommandName;
            CmdDesc = CommandDescription;
            CmdArgs = CommandArgs;
            CmdExec = FunctionToExecute;
        }
    }
    class RegularExpressions
    {
        public Regex SplitCommandsFromArgs = new Regex(@"^(?:[!?]|<@345369637963431937>\s)(\w+)\s*(.*)", RegexOptions.IgnoreCase);
        public Regex SplitArgs = new Regex(@"\s", RegexOptions.IgnoreCase);
    }
    delegate void CmdObjectFuncExec(SocketMessage Message, String[] Params, CommandObject[] CommandsAvailable);
}
