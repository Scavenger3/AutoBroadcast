using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using Terraria;
using TShockAPI;
using Hooks;
using Config;

namespace AutoBroadcast
{
    [APIVersion(1, 11)]
    public class PluginMain : TerrariaPlugin
    {
        public static abcConfig getConfig { get; set; }
        internal static string getConfigPath { get { return Path.Combine(TShock.SavePath, "AutoBroadcastConfig.json"); } }

        public static Timer Broadcast1 = new Timer();
        public static Timer Broadcast2 = new Timer();
        public static Timer Broadcast3 = new Timer();

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
            get { return new Version("1.4"); }
        }

        public override void Initialize()
        {
            GameHooks.Initialize += OnInitialize;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GameHooks.Initialize -= OnInitialize;
            }
            base.Dispose(disposing);
        }

        public PluginMain(Main game)
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
                Broadcast1.Interval = (getConfig.Message1_Interval * 1000);
                Broadcast2.Interval = (getConfig.Message2_Interval * 1000);
                Broadcast3.Interval = (getConfig.Message3_Interval * 1000);
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

        public static void ReloadConfig(CommandArgs args)
        {
            try
            {
                if (File.Exists(getConfigPath))
                {
                    getConfig = abcConfig.Read(getConfigPath);
                }
                getConfig.Write(getConfigPath);
                Broadcast1.Interval = (getConfig.Message1_Interval * 1000);
                Broadcast2.Interval = (getConfig.Message2_Interval * 1000);
                Broadcast3.Interval = (getConfig.Message3_Interval * 1000);
                args.Player.SendMessage("Settings reloaded from config file!", Color.MediumSeaGreen);
            }
            catch (Exception ex)
            {
                args.Player.SendMessage("Error: Could not reload config file!, Check Logs!", Color.IndianRed);
                Log.Error("Config Exception in Auto Broadcast Config file");
                Log.Error(ex.ToString());
            }
        }
        #endregion

        public void OnInitialize()
        {
            SetupConfig();
            Broadcast1.Elapsed += new ElapsedEventHandler(Broadcast1_Elapsed);
            Broadcast2.Elapsed += new ElapsedEventHandler(Broadcast2_Elapsed);
            Broadcast2.Elapsed += new ElapsedEventHandler(Broadcast3_Elapsed);
            Broadcast1.Interval = (getConfig.Message1_Interval * 1000);
            Broadcast2.Interval = (getConfig.Message2_Interval * 1000);
            Broadcast3.Interval = (getConfig.Message3_Interval * 1000);
            if (getConfig.Message1_Enabled)
                Broadcast1.Start();
            if (getConfig.Message2_Enabled)
                Broadcast2.Start();
            if (getConfig.Message3_Enabled)
                Broadcast3.Start();

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
            Commands.ChatCommands.Add(new Command("abroadcast", msgset, "mset", "sm"));
        }

        #region Broadcast 1
        static void Broadcast1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!getConfig.Message1_Enabled)
            {
                Broadcast1.Stop();
            }
            else
            {
                if (getConfig.Message1_Group == "")
                {
                    foreach (string msg in getConfig.Message1_Messages)
                    {
                        if (msg != null && msg != " " && msg != "")
                            bctoAll(msg, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);
                    }
                }
                else
                {
                    foreach (string msg in getConfig.Message1_Messages)
                    {
                        if (msg != null && msg != " " && msg != "")
                            bctoGroup(getConfig.Message1_Group, msg, getConfig.Message1_ColorR, getConfig.Message1_ColorG, getConfig.Message1_ColorB);
                    }
                }
            }
        }
        #endregion

        #region Broadcast 2
        static void Broadcast2_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!getConfig.Message2_Enabled)
            {
                Broadcast2.Stop();
            }
            else
            {
                if (getConfig.Message2_Group == "")
                {
                    foreach (string msg in getConfig.Message2_Messages)
                    {
                        if (msg != null && msg != " " && msg != "")
                            bctoAll(msg, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);
                    }
                }
                else
                {
                    foreach (string msg in getConfig.Message2_Messages)
                    {
                        if (msg != null && msg != " " && msg != "")
                            bctoGroup(getConfig.Message2_Group, msg, getConfig.Message2_ColorR, getConfig.Message2_ColorG, getConfig.Message2_ColorB);
                    }
                }
            }
        }
        #endregion

        #region Broadcast 3
        static void Broadcast3_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!getConfig.Message3_Enabled)
            {
                Broadcast3.Stop();
            }
            else
            {
                if (getConfig.Message3_Group == "")
                {
                    foreach (string msg in getConfig.Message3_Messages)
                    {
                        if (msg != null && msg != " " && msg != "")
                            bctoAll(msg, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);
                    }
                }
                else
                {
                    foreach (string msg in getConfig.Message3_Messages)
                    {
                        if (msg != null && msg != " " && msg != "")
                            bctoGroup(getConfig.Message3_Group, msg, getConfig.Message3_ColorR, getConfig.Message3_ColorG, getConfig.Message3_ColorB);
                    }
                }
            }
        }
        #endregion

        #region Broadcast Methods
        public static void bctoGroup(string bcgroup, string message, byte colorr, byte colorg, byte colorb)
        {
            foreach (TSPlayer player in TShock.Players)
            {
                if (player.Group.Name == bcgroup)
                player.SendMessage(message, colorr, colorg, colorb);
            }
        }

        public static void bctoAll(string message, byte colorr, byte colorg, byte colorb)
        {
            foreach (TSPlayer player in TShock.Players) 
                player.SendMessage(message, colorr, colorg, colorb);
        }
        #endregion

        #region Commands
        public static void autobc(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Usage: /autobc help - shows help for the /autobc set command", Color.Red);
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                args.Player.SendMessage("Usage: /autobc sync <all/1/2/3> - syncronise broadcasts to your current time", Color.Red);
                return;
            }

            string subcmd = args.Parameters[0].ToLower();

            if (subcmd == "help")
            {
                args.Player.SendMessage("/autobc set <Message Number> <Setting> <Value>", Color.IndianRed);
                args.Player.SendMessage("Settings: Enabled/Message/Colour/Interval/Group", Color.IndianRed);
            }
            else if (subcmd == "set")
            {
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
                            if (!Broadcast1.Enabled)
                                Broadcast1.Start();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else if (value == "false")
                        {
                            getConfig.Message1_Enabled = false;
                            getConfig.Write(getConfigPath);
                            if (Broadcast1.Enabled)
                                Broadcast1.Stop();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else
                            args.Player.SendMessage("Invalid value for enabled, Please use: True/False", Color.IndianRed);
                    }
                    else if (setting == "message")
                    {
                        args.Player.SendMessage("Please Use: /mset <Message Number> <Message Line> <Message Text>", Color.Red);
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
                        Broadcast1.Interval = (getConfig.Message1_Interval * 1000);
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
                            if (!Broadcast2.Enabled)
                                Broadcast2.Start();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else if (value == "false")
                        {
                            getConfig.Message2_Enabled = false;
                            getConfig.Write(getConfigPath);
                            if (Broadcast2.Enabled)
                                Broadcast2.Stop();
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
                        Broadcast2.Interval = (getConfig.Message2_Interval * 1000);
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
                            if (!Broadcast3.Enabled)
                                Broadcast3.Start();
                            args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                        }
                        else if (value == "false")
                        {
                            getConfig.Message3_Enabled = false;
                            getConfig.Write(getConfigPath);
                            if (Broadcast3.Enabled)
                                Broadcast3.Stop();
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
                        Broadcast3.Interval = (getConfig.Message3_Interval * 1000);
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
            }
            else if (subcmd == "reload")
            {
                ReloadConfig(args);
            }
            else if (subcmd == "sync" && args.Parameters.Count == 2)
            {
                if (args.Parameters[1] == "all")
                {
                    //1
                    Broadcast1.Stop();
                    Broadcast1.Start();
                    //2
                    Broadcast2.Stop();
                    Broadcast2.Start();
                    //3
                    Broadcast3.Stop();
                    Broadcast3.Start();
                    args.Player.SendMessage("All broadcasts syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "1")
                {
                    Broadcast1.Stop();
                    Broadcast1.Start();
                    args.Player.SendMessage("First broadcast syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "2")
                {
                    Broadcast2.Stop();
                    Broadcast2.Start();
                    args.Player.SendMessage("Second broadcast syncronised to the current time", Color.Red);
                }
                else if (args.Parameters[1] == "3")
                {
                    Broadcast3.Stop();
                    Broadcast3.Start();
                    args.Player.SendMessage("Third broadcast syncronised to the current time", Color.Red);
                }
            }
            else
            {
                args.Player.SendMessage("Usage: /autobc help - shows help for the /autobc set command", Color.Red);
                args.Player.SendMessage("Usage: /autobc reload - Reload settings from config file", Color.Red);
                args.Player.SendMessage("Usage: /autobc sync <all/1/2/3> - syncronise broadcasts to your current time", Color.Red);
            }
        }

        public static void msgset(CommandArgs args)
        {
            #region Set Values
            if (args.Parameters.Count < 2)
            {
                args.Player.SendMessage("Usage: /mset <Message Number> <Message Line> <Message Text/None>", Color.Red);
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
                        getConfig.Message1_Messages.SetValue("", 0);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 0);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 2)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Messages.SetValue("", 1);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 1);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 3)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Messages.SetValue("", 2);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 2);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 4)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Messages.SetValue("", 3);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 3);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 5)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Messages.SetValue("", 4);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 4);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 6)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Messages.SetValue("", 5);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 5);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 7)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message1_Messages.SetValue("", 6);
                    else
                        getConfig.Message1_Messages.SetValue(MText, 6);

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
                        getConfig.Message2_Messages.SetValue("", 0);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 0);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 2)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Messages.SetValue("", 1);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 1);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 3)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Messages.SetValue("", 2);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 2);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 4)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Messages.SetValue("", 3);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 3);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 5)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Messages.SetValue("", 4);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 4);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 6)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Messages.SetValue("", 5);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 5);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 7)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message2_Messages.SetValue("", 6);
                    else
                        getConfig.Message2_Messages.SetValue(MText, 6);

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
                        getConfig.Message3_Messages.SetValue("", 0);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 0);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 2)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Messages.SetValue("", 1);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 1);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 3)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Messages.SetValue("", 2);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 2);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 4)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Messages.SetValue("", 3);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 3);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 5)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Messages.SetValue("", 4);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 4);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 6)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Messages.SetValue("", 5);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 5);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
                else if (lnenumber == 7)
                {
                    if (MTCheck == "none" || MTCheck == " " || MTCheck == "-")
                        getConfig.Message3_Messages.SetValue("", 6);
                    else
                        getConfig.Message3_Messages.SetValue(MText, 6);

                    getConfig.Write(getConfigPath);
                    args.Player.SendMessage("Updated Succesfully!", Color.MediumSeaGreen);
                }
            }
            #endregion
        }
        #endregion Commands
    }
}