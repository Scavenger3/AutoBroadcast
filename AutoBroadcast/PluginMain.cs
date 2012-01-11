using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using Terraria;
using Hooks;
using TShockAPI;
using TShockAPI.DB;
using System.ComponentModel;
using Config;
using System.IO;
using Newtonsoft.Json;

namespace AutoBroadcast
{
    [APIVersion(1, 10)]
    public class AutoBroadcast : TerrariaPlugin
    {
        public static abcConfig getConfig { get; set; }
        internal static string getConfigPath { get { return Path.Combine(TShock.SavePath, "AutoBroadcastConfig.json"); } }
        public static DateTime CountDown1 = DateTime.UtcNow;
        public static DateTime CountDown2 = DateTime.UtcNow;
        public static DateTime CountDown3 = DateTime.UtcNow;
        public static List<bcplayer> bcplayers = new List<bcplayer>();

        public override string Name
        {
            get { return "AutoBroadcast"; }
        }

        public override string Author
        {
            get { return "by Scavenger"; }
        }

        public override string Description
        {
            get { return "Automatically Broadcast a Message every x seconds"; }
        }
         
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override void Initialize()
        {
            GameHooks.Update += OnUpdate;
            GameHooks.Initialize += OnInitialize;
            NetHooks.GreetPlayer += OnGreetPlayer;
            ServerHooks.Leave += OnLeave;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GameHooks.Update -= OnUpdate;
                GameHooks.Initialize -= OnInitialize;
                NetHooks.GreetPlayer -= OnGreetPlayer;
                ServerHooks.Leave -= OnLeave;
            }
            base.Dispose(disposing);
        }

        public AutoBroadcast(Main game)
            : base(game)
        {
            getConfig = new abcConfig(); 
        }

        #region Config

        public static void SetupConfig()
        {
            try
            {
                if (File.Exists(getConfigPath))
                {
                    getConfig = abcConfig.Read(getConfigPath);
                }
                getConfig.Write(getConfigPath);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error in Auto Broadcast config file");
                Console.ForegroundColor = ConsoleColor.Gray;
                Log.Error("Config Exception in Auto Broadcast Config file");
                Log.Error(ex.ToString());
            }
        }
        #endregion Config

        public void OnInitialize()
        {
            bool abroadcast = false;
            SetupConfig();
            foreach (Group group in TShock.Groups.groups)
            {
                if (group.Name != "superadmin")
                {
                    if (group.HasPermission("abroadcast"))
                        abroadcast = true;
                }
            }

            List<string> permlist = new List<string>();
            if (!abroadcast)
                permlist.Add("abroadcast");
            TShock.Groups.AddPermissions("trustedadmin", permlist);

            Commands.ChatCommands.Add(new Command("abroadcast", autobc, "autobc"));
        }

        internal void OnGreetPlayer(int who, HandledEventArgs e)
        {
            lock (bcplayers)
                bcplayers.Add(new bcplayer(who));
        }
        internal void OnLeave(int ply)
        {
            lock (bcplayers)
            {
                for (int i = 0; i < bcplayers.Count; i++)
                {
                    if (bcplayers[i].Index.ToString() == ply.ToString())
                    {
                        bcplayers.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public static void bctoGroup(string bcgroup, string message, byte colorr, byte colorg, byte colorb)
        {
            try
            {
                foreach (bcplayer player in bcplayers)
                    if (player.TSPlayer.Group.Name == bcgroup)
                        player.SendMessage(message, colorr, colorg, colorb);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error With AutoBroadcast - bctogroup");
                Console.WriteLine("Please Post Error in Server Log to the thread!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Log.Error("Origin - bctogroup");
                Log.Error(ex.ToString());
            }
        }
        public static void bctoAll(string message, byte colorr, byte colorg, byte colorb)
        {
            try
            {
                foreach (bcplayer player in bcplayers)
                    player.SendMessage(message, colorr, colorg, colorb);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error With AutoBroadcast - bctoall");
                Console.WriteLine("Please Post Error in Server Log to the thread!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Log.Error("Origin - bctoall");
                Log.Error(ex.ToString());
            }
        }

        public void OnUpdate()
        {
            #region Message 1
            double tick1 = (DateTime.UtcNow - CountDown1).TotalMilliseconds;
            if (tick1 > (getConfig.Message1_Interval * 1000) && getConfig.Message1_Enabled == true)
            {
                if (getConfig.Message1_Group == "")
                {
                    if (getConfig.Message1_Line1 != "")
                        bctoAll(getConfig.Message1_Line1, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line2 != "")
                        bctoAll(getConfig.Message1_Line2, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line3 != "")
                        bctoAll(getConfig.Message1_Line3, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line4 != "")
                        bctoAll(getConfig.Message1_Line4, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line5 != "")
                        bctoAll(getConfig.Message1_Line5, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line6 != "")
                        bctoAll(getConfig.Message1_Line6, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line7 != "")
                        bctoAll(getConfig.Message1_Line7, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                }
                else
                {
                    if (getConfig.Message1_Line1 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line1, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line2 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line2, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line3 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line3, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line4 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line4, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line5 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line5, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line6 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line6, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                    if (getConfig.Message1_Line7 != "")
                        bctoGroup(getConfig.Message1_Group, getConfig.Message1_Line7, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);

                }
                CountDown1 = DateTime.UtcNow;
            }
            #endregion Message 1

            #region Message 2
            double tick2 = (DateTime.UtcNow - CountDown2).TotalMilliseconds;
            if (tick2 > (getConfig.Message2_Interval * 1000) && getConfig.Message2_Enabled == true)
            {
                if (getConfig.Message2_Group == "")
                {
                    if (getConfig.Message2_Line1 != "")
                        bctoAll(getConfig.Message2_Line1, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line2 != "")
                        bctoAll(getConfig.Message2_Line2, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line3 != "")
                        bctoAll(getConfig.Message2_Line3, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line4 != "")
                        bctoAll(getConfig.Message2_Line4, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line5 != "")
                        bctoAll(getConfig.Message2_Line5, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line6 != "")
                        bctoAll(getConfig.Message2_Line6, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line7 != "")
                        bctoAll(getConfig.Message2_Line7, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);
               
                }
                else
                {
                    if (getConfig.Message2_Line1 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line1, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line2 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line2, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line3 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line3, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line4 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line4, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line5 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line5, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line6 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line6, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                    if (getConfig.Message2_Line7 != "")
                        bctoGroup(getConfig.Message2_Group, getConfig.Message2_Line7, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);

                }
                CountDown2 = DateTime.UtcNow;
            }
            #endregion Message 2

            #region Message 3
            double tick3 = (DateTime.UtcNow - CountDown3).TotalMilliseconds;
            if (tick3 > (getConfig.Message3_Interval * 1000) && getConfig.Message3_Enabled == true)
            {
                if (getConfig.Message3_Group == "")
                {
                    if (getConfig.Message3_Line1 != "")
                        bctoAll(getConfig.Message3_Line1, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line2 != "")
                        bctoAll(getConfig.Message3_Line2, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line3 != "")
                        bctoAll(getConfig.Message3_Line3, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line4 != "")
                        bctoAll(getConfig.Message3_Line4, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line5 != "")
                        bctoAll(getConfig.Message3_Line5, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line6 != "")
                        bctoAll(getConfig.Message3_Line6, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line7 != "")
                        bctoAll(getConfig.Message3_Line7, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);
                
                }
                else
                {
                    if (getConfig.Message3_Line1 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line1, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line2 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line2, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line3 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line3, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line4 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line4, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line5 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line5, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line6 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line6, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                    if (getConfig.Message3_Line7 != "")
                        bctoGroup(getConfig.Message3_Group, getConfig.Message3_Line7, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);

                }
                CountDown3 = DateTime.UtcNow;
            }
            #endregion Message 3
        }

        public static void autobc(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Usage: /autobc disable - Temporarily disable all broadcasts", Color.Red);
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                args.Player.SendMessage("Usage: /autobc sync <all/1/2/3> - syncronise broadcasts to your current time", Color.Red);
                return;
            }

            string subcmd = args.Parameters[0];
            
            if (subcmd == "disable")
            {
                getConfig.Message1_Enabled = false;
                getConfig.Message2_Enabled = false;
                getConfig.Message3_Enabled = false;
                args.Player.SendMessage("All Broadcasts temporarily disaled. use \"/autobc reload\" to reload from config file", Color.Red);
            }
            else if (subcmd == "reload")
            {
                SetupConfig();
                args.Player.SendMessage("Settings reloaded from config file", Color.Red);
            }
            else if (subcmd == "sync" && args.Parameters.Count == 2)
            {
                if (args.Parameters[1] == "all")
                {
                    CountDown1 = DateTime.UtcNow;
                    CountDown2 = DateTime.UtcNow;
                    CountDown3 = DateTime.UtcNow;
                    args.Player.SendMessage("All broadcasts syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "1")
                {
                    CountDown1 = DateTime.UtcNow;
                    args.Player.SendMessage("First broadcast syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "2")
                {
                    CountDown2 = DateTime.UtcNow;
                    args.Player.SendMessage("Second broadcast syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "3")
                {
                    CountDown3 = DateTime.UtcNow;
                    args.Player.SendMessage("Third broadcast syncronised to the current time", Color.Red);
                }
            }
            else
            {
                args.Player.SendMessage("Usage: /autobc disable - Temporarily disable all broadcasts", Color.Red);
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                args.Player.SendMessage("Usage: /autobc sync <all/1/2/3> - syncronise broadcasts to the current time", Color.Red);
            }
        }

        public static bcplayer GetbcPlayerByIndex(int index)
        {
            foreach (bcplayer player in bcplayers)
            {
                if (player.Index == index)
                    return player;
            }
            return new bcplayer(-1);
        }
    }

    public class bcplayer
    {
        public int Index { get; set; }
        public TSPlayer TSPlayer { get { return TShock.Players[Index]; } }
        //public string PlayerGrp { get { return TShock.Players[Index].Group.Name; } }
        //public string PlayerName { get { return Main.player[Index].name; } }


        public bcplayer(int index)
        {
            Index = index;
        }

        public void SendMessage(string message, byte colorr, byte colorg, byte colorb)
        {
            NetMessage.SendData((int)PacketTypes.ChatText, Index, -1, message, 255, colorr, colorg, colorb);
        }
    }
}

namespace Config
{
    public class abcConfig
    {
        //Message 1
        public bool Message1_Enabled = false;
        public string Message1_Line1 = "";
        public string Message1_Line2 = "";
        public string Message1_Line3 = "";
        public string Message1_Line4 = "";
        public string Message1_Line5 = "";
        public string Message1_Line6 = "";
        public string Message1_Line7 = "";
        public byte Message1_ColorR = 255;
        public byte Message1_ColorG = 255;
        public byte Message1_ColorB = 255;
        public int Message1_Interval = 300;
        public string Message1_Group = "";
        //Message 2
        public bool Message2_Enabled = false;
        public string Message2_Line1 = "";
        public string Message2_Line2 = "";
        public string Message2_Line3 = "";
        public string Message2_Line4 = "";
        public string Message2_Line5 = "";
        public string Message2_Line6 = "";
        public string Message2_Line7 = "";
        public byte Message2_ColorR = 255;
        public byte Message2_ColorG = 255;
        public byte Message2_ColorB = 255;
        public int Message2_Interval = 300;
        public string Message2_Group = "";
        //Message 3
        public bool Message3_Enabled = false;
        public string Message3_Line1 = "";
        public string Message3_Line2 = "";
        public string Message3_Line3 = "";
        public string Message3_Line4 = "";
        public string Message3_Line5 = "";
        public string Message3_Line6 = "";
        public string Message3_Line7 = "";
        public byte Message3_ColorR = 255;
        public byte Message3_ColorG = 255;
        public byte Message3_ColorB = 255;
        public int Message3_Interval = 300;
        public string Message3_Group = "";

        public static abcConfig Read(string path)
        {
            if (!File.Exists(path))
                return new abcConfig();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Read(fs);
            }
        }

        public static abcConfig Read(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var cf = JsonConvert.DeserializeObject<abcConfig>(sr.ReadToEnd());
                if (ConfigRead != null)
                    ConfigRead(cf);
                return cf;
            }
        }
        public void Write(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                Write(fs);
            }
        }

        public void Write(Stream stream)
        {
            var str = JsonConvert.SerializeObject(this, Formatting.Indented);
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(str);
            }
        }

        public static Action<abcConfig> ConfigRead;
    }
}