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
            //Commands.ChatCommands.Add(new Command("abroadcast", msgset, "setm"));
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
                //args.Player.SendMessage("Usage: /autobc set - set broadcast configs!", Color.Red);
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                //args.Player.SendMessage("Usage: /autobc sync all - syncronise broadcasts to your current time", Color.Red);
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
            #region Old Sub Commands
            /*else if (subcmd == "set")
            {
                if (args.Parameters.Count == 1)
                {
                    args.Player.SendMessage("Usage: /autobc set <Message Number> <Setting> <Value>", Color.Red);
                    args.Player.SendMessage("Settings: Enabled/Message/Colour/Interval/Group", Color.Red);
                    return;
                }
                else if (args.Parameters.Count == 2)
                {
                    args.Player.SendMessage("Settings: Enabled/Message/Colour/Interval/Group", Color.Red);
                    return;
                }
                else if (args.Parameters.Count == 3)
                {
                    string sett = args.Parameters[2].ToLower();
                    if (sett == "enabled")
                        args.Player.SendMessage("values for Enabled: True/False", Color.Red);
                    else if (sett == "message")
                        args.Player.SendMessage("Please Use: /setm <Message Number> <Message Line> <Message Text>", Color.Red);
                    else if (sett == "color" || sett == "colour")
                        args.Player.SendMessage("values for Colour: R,G,B", Color.Red);
                    else if (sett == "interval")
                        args.Player.SendMessage("value for Interval: <time in seconds>", Color.Red);
                    else if (sett == "groups")
                        args.Player.SendMessage("value for groups: <group name>", Color.Red);
                    else
                        args.Player.SendMessage("Settings: Enabled/Message/Colour/Interval/Group", Color.Red);
                    
                    return;
                }

                #region set values
                int msgnumber = 0;
                if (!int.TryParse(args.Parameters[1], out msgnumber) || msgnumber > 3)//SET Message Number
                {
                    args.Player.SendMessage("Message number must be 1-3!", Color.IndianRed);
                    return;
                }
                string setting = args.Parameters[2].ToLower(); //SET Selected Setting
                string value = "";
                for (int i = 3; i < args.Parameters.Count; i++)
                    value = value + args.Parameters[i] + " ";
                value = value.Remove(value.Length - 1, 1); //SET Setting Value
                #endregion

                #region Set Config 1
                if (msgnumber == 1)
                {
                    if (setting == "enabled")
                    {
                        if (value == "true")
                        {
                            getConfig.Message1_Enabled = true;
                            getConfig.Write(getConfigPath);
                            if (!Broadcast.Enabled)
                                Broadcast.Start();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else if (value == "false")
                        {
                            getConfig.Message1_Enabled = false;
                            getConfig.Write(getConfigPath);
                            if (!getConfig.Message2_Enabled && !getConfig.Message3_Enabled && Broadcast.Enabled)
                                Broadcast.Stop();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else
                            args.Player.SendMessage("Invalid value for enabled, Please use: True/False", Color.IndianRed);
                    }
                    else if (setting == "message")
                    {
                        args.Player.SendMessage("Please Use: /setm <Message Number> <Message Line> <Message Text>", Color.Red);
                        return;
                    }
                    else if (setting == "color" || setting == "colour")
                    {
                        List<byte> colors = new List<byte>();
                        try
                        {
                            string[] clist = value.Split(',');
                            if (clist.Length != 3)
                            {
                                args.Player.SendMessage("Error with format, Correct format: R,G,B", Color.IndianRed);
                                return;
                            }
                            colors.Add(byte.Parse(clist[0]));
                            colors.Add(byte.Parse(clist[1]));
                            colors.Add(byte.Parse(clist[2]));
                        }
                        catch (Exception)
                        {
                            args.Player.SendMessage("Error with format, Correct format: R,G,B", Color.IndianRed);
                            return;
                        }

                        getConfig.Message1_ColorR = colors[0];
                        getConfig.Message1_ColorG = colors[1];
                        getConfig.Message1_ColorB = colors[2];
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }
                    else if (setting == "interval")
                    {
                        int val = 0;
                        if (!int.TryParse(value, out val))
                        {
                            args.Player.SendMessage("Could not parse interval!", Color.IndianRed);
                            return;
                        }
                        getConfig.Message1_Interval = val;
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }

                    else if (setting == "group")
                    {
                        getConfig.Message1_Group = value;
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }
                    else
                        args.Player.SendMessage("Invalid Setting, Valid Settings: Enabled/Message/Colour/Interval/Group", Color.IndianRed);
                }
                #endregion

                #region Set Config 2
                if (msgnumber == 2)
                {
                    if (setting == "enabled")
                    {
                        if (value == "true")
                        {
                            getConfig.Message2_Enabled = true;
                            getConfig.Write(getConfigPath);
                            if (!Broadcast.Enabled)
                                Broadcast.Start();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else if (value == "false")
                        {
                            getConfig.Message2_Enabled = false;
                            getConfig.Write(getConfigPath);
                            if (!getConfig.Message1_Enabled && !getConfig.Message3_Enabled && Broadcast.Enabled)
                                Broadcast.Stop();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else
                            args.Player.SendMessage("Invalid value for enabled, Please use: True/False", Color.IndianRed);
                    }
                    else if (setting == "message")
                    {
                        args.Player.SendMessage("Changing Message is currently not implimented!", Color.IndianRed);
                    }
                    else if (setting == "color" || setting == "colour")
                    {
                        List<byte> colors = new List<byte>();
                        try
                        {
                            string[] clist = value.Split(',');
                            if (clist.Length != 3)
                            {
                                args.Player.SendMessage("Error with format, Correct format: R,G,B", Color.IndianRed);
                                return;
                            }
                            colors.Add(byte.Parse(clist[0]));
                            colors.Add(byte.Parse(clist[1]));
                            colors.Add(byte.Parse(clist[2]));
                        }
                        catch (Exception)
                        {
                            args.Player.SendMessage("Error with format, Correct format: R,G,B", Color.IndianRed);
                            return;
                        }

                        getConfig.Message2_ColorR = colors[0];
                        getConfig.Message2_ColorG = colors[1];
                        getConfig.Message2_ColorB = colors[2];
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }
                    else if (setting == "interval")
                    {
                        int val = 0;
                        if (!int.TryParse(value, out val))
                        {
                            args.Player.SendMessage("Could not parse interval!", Color.IndianRed);
                            return;
                        }
                        getConfig.Message2_Interval = val;
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }

                    else if (setting == "group")
                    {
                        getConfig.Message2_Group = value;
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }
                    else
                        args.Player.SendMessage("Invalid Setting, Valid Settings: Enabled/Message/Colour/Interval/Group", Color.IndianRed);
                }
                #endregion

                #region Set Config 3
                if (msgnumber == 3)
                {
                    if (setting == "enabled")
                    {
                        if (value == "true")
                        {
                            getConfig.Message3_Enabled = true;
                            getConfig.Write(getConfigPath);
                            if (!Broadcast.Enabled)
                                Broadcast.Start();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else if (value == "false")
                        {
                            getConfig.Message3_Enabled = false;
                            getConfig.Write(getConfigPath);
                            if (!getConfig.Message1_Enabled && !getConfig.Message2_Enabled && Broadcast.Enabled)
                                Broadcast.Stop();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else
                            args.Player.SendMessage("Invalid value for enabled, Please use: True/False", Color.IndianRed);
                    }
                    else if (setting == "message")
                    {
                        args.Player.SendMessage("Changing Message is currently not implimented!", Color.IndianRed);
                    }
                    else if (setting == "color" || setting == "colour")
                    {
                        List<byte> colors = new List<byte>();
                        try
                        {
                            string[] clist = value.Split(',');
                            if (clist.Length != 3)
                            {
                                args.Player.SendMessage("Error with format, Correct format: R,G,B", Color.IndianRed);
                                return;
                            }
                            colors.Add(byte.Parse(clist[0]));
                            colors.Add(byte.Parse(clist[1]));
                            colors.Add(byte.Parse(clist[2]));
                        }
                        catch (Exception)
                        {
                            args.Player.SendMessage("Error with format, Correct format: R,G,B", Color.IndianRed);
                            return;
                        }

                        getConfig.Message3_ColorR = colors[0];
                        getConfig.Message3_ColorG = colors[1];
                        getConfig.Message3_ColorB = colors[2];
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }
                    else if (setting == "interval")
                    {
                        int val = 0;
                        if (!int.TryParse(value, out val))
                        {
                            args.Player.SendMessage("Could not parse interval!", Color.IndianRed);
                            return;
                        }
                        getConfig.Message3_Interval = val;
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }

                    else if (setting == "group")
                    {
                        getConfig.Message3_Group = value;
                        getConfig.Write(getConfigPath);
                        args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                    }
                    else
                        args.Player.SendMessage("Invalid Setting, Valid Settings: Enabled/Message/Colour/Interval/Group", Color.IndianRed);
                }
                #endregion
            }*/
            //SYNC:::::
            /*else if (subcmd == "sync" && args.Parameters.Count == 2)
            {
                if (args.Parameters[1] == "all")
                {
                    int v = 0;
                    foreach (aBc bc in aBroadcasts.AutoBroadcast)
                    {
                        TTNext[v] = bc.Interval;
                        v++;
                    }
                    args.Player.SendMessage("All broadcasts syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "1")
                {
                    Broadcast1 = getConfig.Message1_Interval;
                    args.Player.SendMessage("First broadcast syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "2")
                {
                    Broadcast2 = getConfig.Message2_Interval;
                    args.Player.SendMessage("Second broadcast syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "3")
                {
                    Broadcast3 = getConfig.Message3_Interval;
                    args.Player.SendMessage("Third broadcast syncronised to the current time", Color.Red);
                }*/
            #endregion
            else
            {
                //args.Player.SendMessage("Usage: /autobc set - set broadcast configs!", Color.Red);
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                //args.Player.SendMessage("Usage: /autobc sync all - syncronise all broadcasts to your current time", Color.Red);
            }
        }

        #region old msg set
        /*public static void msgset(CommandArgs args)
        {
            #region Set Values
            if (args.Parameters.Count < 2)
            {
                args.Player.SendMessage("Usage: /setm <Message Number> <Message Line> <Message Text/None>", Color.Red);
                return;
            }

            int msgnumber = 0;
            if (!int.TryParse(args.Parameters[0], out msgnumber) || msgnumber > 3)//SET Message Number
            {
                args.Player.SendMessage("Message number must be 1-3!", Color.IndianRed);
                return;
            }

            int lnenumber = 0;
            if (!int.TryParse(args.Parameters[0], out lnenumber) || lnenumber > 7)//SET Message Line Number
            {
                args.Player.SendMessage("Message Line Number must be 1-7!", Color.IndianRed);
                return;
            }

            string MText = "";
            for (int i = 2; i < args.Parameters.Count; i++)
                MText = MText + args.Parameters[i] + " ";
            MText = MText.Remove(MText.Length - 1, 1); //SET Setting Value
            string MTCheck = MText.ToLower();
            #endregion

            #region Broadcast 1
            if (msgnumber == 1)
            {
                if (lnenumber == 1)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line1 = "";
                    else
                        getConfig.Message1_Line1 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 2)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line2 = "";
                    else
                        getConfig.Message1_Line2 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 3)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line3 = "";
                    else
                        getConfig.Message1_Line3 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 4)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line4 = "";
                    else
                        getConfig.Message1_Line4 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 5)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line5 = "";
                    else
                        getConfig.Message1_Line5 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 6)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line6 = "";
                    else
                        getConfig.Message1_Line6 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 7)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Line7 = "";
                    else
                        getConfig.Message1_Line7 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
            }
            #endregion

            #region Broadcast 2
            if (msgnumber == 2)
            {
                if (lnenumber == 1)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line1 = "";
                    else
                        getConfig.Message2_Line1 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 2)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line2 = "";
                    else
                        getConfig.Message2_Line2 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 3)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line3 = "";
                    else
                        getConfig.Message2_Line3 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 4)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line4 = "";
                    else
                        getConfig.Message2_Line4 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 5)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line5 = "";
                    else
                        getConfig.Message2_Line5 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 6)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line6 = "";
                    else
                        getConfig.Message2_Line6 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 7)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Line7 = "";
                    else
                        getConfig.Message2_Line7 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
            }
            #endregion

            #region Broadcast 3
            if (msgnumber == 3)
            {
                if (lnenumber == 1)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line1 = "";
                    else
                        getConfig.Message3_Line1 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 2)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line2 = "";
                    else
                        getConfig.Message3_Line2 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 3)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line3 = "";
                    else
                        getConfig.Message3_Line3 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 4)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line4 = "";
                    else
                        getConfig.Message3_Line4 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 5)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line5 = "";
                    else
                        getConfig.Message3_Line5 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 6)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line6 = "";
                    else
                        getConfig.Message3_Line6 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 7)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Line7 = "";
                    else
                        getConfig.Message3_Line7 = MText;

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
            }
            #endregion
        }*/
        #endregion

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