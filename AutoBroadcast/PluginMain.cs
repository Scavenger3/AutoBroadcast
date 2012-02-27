using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using Terraria;
using TShockAPI;
using Hooks;
using System.ComponentModel;
using AutoBroadcastConfig;
using System.Net;

namespace AutoBroadcast
{
    [APIVersion(1, 11)]
    public class AutoBroadcast : TerrariaPlugin
    {
        public static aBList aBroadcasts;
        public static String savepath = "";
        public static List<int> TTNext = new List<int>();

        public static Timer Broadcast = new Timer(1000);

        public static List<abcPlayer> abcPlayers = new List<abcPlayer>();

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
            get { return new Version("1.4.1"); }
        }

        public override void Initialize()
        {
            GameHooks.Initialize += OnInitialize;
            NetHooks.GreetPlayer += OnGreetPlayer;
            ServerHooks.Leave += OnLeave;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GameHooks.Initialize -= OnInitialize;
                NetHooks.GreetPlayer -= OnGreetPlayer;
                ServerHooks.Leave -= OnLeave;
            }
            base.Dispose(disposing);
        }

        public AutoBroadcast(Main game)
            : base(game)
        {
            Order = 4;
            savepath = Path.Combine(Path.Combine(TShockAPI.TShock.SavePath, "PluginConfigs/AutoBroadcastConfig.json"));

            ConfR reader = new ConfR();
            if (File.Exists(savepath))
            {
                aBroadcasts = reader.readFile(Path.Combine(TShockAPI.TShock.SavePath, "PluginConfigs/AutoBroadcastConfig.json"));
                Console.WriteLine(aBroadcasts.AutoBroadcast.Count + " Broadcasts have been loaded.");
            }
            else
            {
                aBroadcasts = reader.writeFile(Path.Combine(TShockAPI.TShock.SavePath, "PluginConfigs/AutoBroadcastConfig.json"));
                Console.WriteLine("Empty Broadcast Config file being created.");
            }
        }

        #region Hooks
        public void OnInitialize()
        {
            Broadcast.Elapsed += new ElapsedEventHandler(Broadcast_Elapsed);
            foreach (aBc bc in aBroadcasts.AutoBroadcast)
            {
                TTNext.Add(bc.Interval);
            }
            Broadcast.Start();

            bool abroadcast = false;
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

        public void OnGreetPlayer(int who, HandledEventArgs e)
        {
            lock (abcPlayers)
                abcPlayers.Add(new abcPlayer(who));
        }

        public void OnLeave(int ply)
        {
            lock (abcPlayers)
            {
                for (int i = 0; i < abcPlayers.Count; i++)
                {
                    if (abcPlayers[i].Index == ply)
                    {
                        abcPlayers.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Timer
        static void Broadcast_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < TTNext.Count; i++)
            {
                TTNext[i]--;
            }

            int v = 0;
            foreach (aBc bc in aBroadcasts.AutoBroadcast)
            {
                if (bc.Enabled)
                {
                    if (TTNext[v] < 1)
                    {
                        if (bc.Groups.Count < 1)
                            bctoAll(bc.Messages, (byte)bc.ColorR, (byte)bc.ColorG, (byte)bc.ColorB);
                        else
                            bctoGroup(bc.Groups, bc.Messages, (byte)bc.ColorR, (byte)bc.ColorG, (byte)bc.ColorB);

                        TTNext[v] = bc.Interval;
                    }
                }
                v++;
            }
        }
        #endregion

        #region Methods
        public static void bctoGroup(List<string> bcgroup, List<string> messages, byte colorr, byte colorg, byte colorb)
        {
            foreach (string msg in messages)
            {
                if (msg != "")
                {
                    foreach (abcPlayer player in abcPlayers)
                    {
                        foreach (string grp in bcgroup)
                        {
                            if (player.TSPlayer.Group.Name == grp)
                                player.SendMessage(msg, colorr, colorg, colorb);
                        }
                    }
                }
            }
        }

        public static void bctoAll(List<string> messages, byte colorr, byte colorg, byte colorb)
        {
            foreach (string msg in messages)
            {
                if (msg != "")
                {
                    foreach (abcPlayer player in abcPlayers)
                        player.SendMessage(msg, colorr, colorg, colorb);
                }
            }
        }
        #endregion

        #region Commands
        public static void autobc(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                return;
            }

            string subcmd = args.Parameters[0].ToLower();

            if (subcmd == "reload")
            {
                try
                {
                    ConfR reader = new ConfR();
                    aBroadcasts = reader.readFile(Path.Combine(TShockAPI.TShock.SavePath, "PluginConfigs/AutoBroadcastConfig.json"));
   
                    int v = 0;
                    foreach (aBc bc in aBroadcasts.AutoBroadcast)
                    {
                        TTNext[v] = bc.Interval;
                        v++;
                    }
                    args.Player.SendMessage("Settings reloaded from config file!", Color.MediumSeaGreen);
                }
                catch (Exception ex)
                {
                    args.Player.SendMessage("Error: Could not reload config file!, Check Logs!", Color.Red);
                    Log.Error("Config Exception in Auto Broadcast Config file");
                    Log.Error(ex.ToString());
                }
            }
            else
            {
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
            }
        }
        #endregion Commands
    }

    #region abcPlayerClass
    public class abcPlayer
    {
        public int Index { get; set; }
        public TSPlayer TSPlayer { get { return TShock.Players[Index]; } }

        public abcPlayer(int index)
        {
            Index = index;
        }

        public void SendMessage(string message, int colorR, int colorG, int colorB)
        {
            NetMessage.SendData((int)PacketTypes.ChatText, Index, -1, message, 255, colorR, colorG, colorB);
        }
    }
    #endregion
}