using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace Autonauts_BOT_CS
{
    public class VoteHandler
    {
        public VoteConfig Config = new VoteConfig();
        string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutonautsBot");
        string DataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutonautsBot\\Vote_Data.json");
        public VoteHandler()
        {
            List<VoteMessage> Messages = new List<VoteMessage>();
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            if (!File.Exists(DataFile))
            {
                File.Create(DataFile);
            }
            LoadFromFile();
        }
        public VoteMessage CreateNewVoteMessage(ulong MsgID, SocketUser Author, string Content)
        {
            VoteMessage NewVoteMessage = new VoteMessage()
            {
                MessageID = MsgID,
                Suggestion = Content,
                Author = new VoteAuthor()
                {
                    Username = Author.Username,
                    AvatarURL = Author.GetAvatarUrl()
                }
            };
            List<VoteMessage> Temp_List = new List<VoteMessage>();
            Config.Messages.Add(NewVoteMessage);
            SaveToFile();
            return NewVoteMessage;
        }
        public void SaveToFile()
        {
            Config.SuccesfullMessages.Clear();
            foreach(VoteMessage temp_MSG in Config.Messages)
            {
                if (temp_MSG.CheckForThreshold())
                {
                    Config.SuccesfullMessages.Add(temp_MSG);
                }
            }
            File.WriteAllText(DataFile, JsonConvert.SerializeObject(Config));
        }
        public void LoadFromFile()
        {
            string FileContents = File.ReadAllText(DataFile);
            VoteConfig LoadedConfig = JsonConvert.DeserializeObject<VoteConfig>(FileContents);
            Config = LoadedConfig ?? new VoteConfig();
        }
    }
    public class VoteMessage
    {
        public ulong MessageID;
        public VoteAuthor Author;
        public string Suggestion;

        public List<ulong> Upvotes = new List<ulong>();
        public List<ulong> Downvotes = new List<ulong>();
        public int Threshold = 5;
        public bool CheckForThreshold()
        {
            if (Upvotes.Count - Downvotes.Count >= Threshold)
            {
                return true;
            }
            return false;
        }
    }
    public class VoteConfig
    {
        public List<VoteMessage> Messages = new List<VoteMessage>();
        public List<VoteMessage> SuccesfullMessages = new List<VoteMessage>();
    }
    public class VoteAuthor
    {
        public string Username;
        public string AvatarURL;
    }
}
