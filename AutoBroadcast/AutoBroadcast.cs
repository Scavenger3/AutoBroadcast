using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace AutoBroadcast
{
	[ApiVersion(1, 14)]
	public class AutoBroadcast : TerrariaPlugin
	{
		public override string Name { get { return "AutoBroadcast"; } }
		public override string Author { get { return "Scavenger"; } }
		public override string Description { get { return "Automatically Broadcast a Message or Command every x seconds"; } }
		public override Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

		public string ConfigPath { get { return Path.Combine(TShock.SavePath, "AutoBroadcastConfig.json"); } }
		public ABConfig Config = new ABConfig();
		public DateTime LastCheck = DateTime.UtcNow;

		public AutoBroadcast(Main Game) : base(Game) { }

		public override void Initialize()
		{
			ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
			ServerApi.Hooks.GameUpdate.Register(this, OnUpdate);
			ServerApi.Hooks.ServerChat.Register(this, OnChat);
		}

		protected override void Dispose(bool Disposing)
		{
			if (Disposing)
			{
				ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
				ServerApi.Hooks.GameUpdate.Deregister(this, OnUpdate);
				ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
			}
			base.Dispose(Disposing);
		}

		public void OnInitialize(EventArgs args)
		{
			Commands.ChatCommands.Add(new Command("abroadcast", autobc, "autobc"));

			try
			{
				Config = ABConfig.Read(ConfigPath).Write(ConfigPath);
			}
			catch (Exception ex)
			{
				Config = new ABConfig();
				Log.ConsoleError("[AutoBroadcast] An exception occurred while parsing the AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
			}
		}

		public void autobc(CommandArgs args)
		{
			try
			{
				Config = ABConfig.Read(ConfigPath).Write(ConfigPath);
				args.Player.SendSuccessMessage("Successfully reloaded AutoBroadcast config!");
			}
			catch (Exception ex)
			{
				Config = new ABConfig();
				args.Player.SendWarningMessage("An exception occurred while parsing the AutoBroadcast config! check logs for more details!");
				Log.Error("[AutoBroadcast] An exception occurred while parsing tbe AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
			}
		}

		#region Chat
		public void OnChat(ServerChatEventArgs args)
		{
			string[] Groups = new string[0];
			string[] Messages = new string[0];
			float[] Colour = new float[0];
			var PlayerGroup = TShock.Players[args.Who].Group.Name;

			lock (Config.Broadcasts)
				foreach (var broadcast in Config.Broadcasts)
				{
					if (broadcast == null || !broadcast.Enabled ||
						(broadcast.TriggerToWholeGroup && !broadcast.Groups.Contains(PlayerGroup)))
					{
						continue;
					}

					foreach (string Word in broadcast.TriggerWords)
					{
						if (args.Text.Contains(Word))
						{
							if (broadcast.TriggerToWholeGroup && broadcast.Groups.Length > 0)
							{
								Groups = broadcast.Groups;
							}
							Messages = broadcast.Messages;
							Colour = broadcast.ColorRGB;
							break;
						}
					}
				}

			if (Groups.Length > 0)
			{
				BroadcastToGroups(Groups, Messages, Colour);
			}
			else
			{
				BroadcastToPlayer(args.Who, Messages, Colour);
			}
		}
		#endregion

		#region Update
		public void OnUpdate(EventArgs args)
		{
			if ((DateTime.UtcNow - LastCheck).TotalSeconds >= 1)
			{
				LastCheck = DateTime.UtcNow;
				int NumBroadcasts = 0;
				lock (Config.Broadcasts)
					NumBroadcasts = Config.Broadcasts.Length;
				for (int i = 0; i < NumBroadcasts; i++)
				{
					string[] Groups = new string[0];
					string[] Messages = new string[0];
					float[] Colour = new float[0];

					lock (Config.Broadcasts)
					{
						if (Config.Broadcasts[i] == null || !Config.Broadcasts[i].Enabled || Config.Broadcasts[i].Interval < 1)
						{
							continue;
						}
						if (Config.Broadcasts[i].StartDelay > 0)
						{
							Config.Broadcasts[i].StartDelay--;
							continue;
						}
						Config.Broadcasts[i].StartDelay = Config.Broadcasts[i].Interval;// Start Delay used as Interval Countdown
						Groups = Config.Broadcasts[i].Groups;
						Messages = Config.Broadcasts[i].Messages;
						Colour = Config.Broadcasts[i].ColorRGB;
					}

					if (Groups.Length > 0)
					{
						BroadcastToGroups(Groups, Messages, Colour);
					}
					else
					{
						BroadcastToAll(Messages, Colour);
					}
				}
			}
		}
		#endregion

		public static void BroadcastToGroups(string[] Groups, string[] Messages, float[] Colour)
		{
            for (int i = 0; i < Messages.Length; i++)
            {
                var Line = Messages[i];

                if (Line.Contains("%map%") || Line.Contains("%players%"))
                    Line = parseLine(Line);
                /* The line contains one or both of our two variables, so it gets parsed */

                if (Line.StartsWith("/"))
                    Commands.HandleCommand(TSPlayer.Server, Line);
                else
                    lock (TShock.Players)
                        foreach (var player in TShock.Players)
                            if (player != null && Groups.Contains(player.Group.Name))
                                player.SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
            }
		}
		public static void BroadcastToAll(string[] Messages, float[] Colour)
		{
            for (int i = 0; i < Messages.Length; i++)
            {
                var Line = Messages[i];
                if (Line.StartsWith("/"))
                    Commands.HandleCommand(TSPlayer.Server, Line);
                else
                    TSPlayer.All.SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
            }
		}
        public static void BroadcastToPlayer(int plr, string[] Messages, float[] Colour)
        {
            for (int i = 0; i < Messages.Length; i++)
            {
                var Line = Messages[i];
                if (Line.StartsWith("/"))
                    Commands.HandleCommand(TSPlayer.Server, Line);
                else
                    lock (TShock.Players)
                        TShock.Players[plr].SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
            }
        }


        public static string parseLine(string Line)
        {
            string worldName = TShock.Config.UseServerName ? TShock.Config.ServerName : Main.worldName;
            if (Line.Contains("%map%"))
            {
                var words = Line.Split(' ');
                /*If your string (the text in the broadcast) is
                 * "this is the map name: %map%"
                 * Then we split that at every space, returning a string array (string[]) like this:
                 * "this", "is", "the", "map", "name:", "%map%"
                 */
                for (int i = 0; i < words.Length; i++)
                    if (words[i] == "%map%")
                        words[i] = words[i].Replace("%map%", worldName);
                /* Then we iterate over it with a for loop, starting at the 0 index of the string[]
                 * and ending at the end of the string[].
                 If any word is "%map%", replace "%map%" with the world name*/

                Line = string.Join(" ", words);
                /* Then we join all the words together again with spaces */
            }

            if (Line.Contains("%players%"))
            {
                var words = Line.Split(' ');
                for (int i = 0; i < words.Length; i++)
                    if (words[i] == "%players%")
                        words[i] = words[i].Replace("%players%",
                            string.Join(", ",TShock.Utils.GetPlayers(false)));

                Line = string.Join(" ", words);
            }

            return Line;
            /* Return the Line that we've changed, replacing the old one */
        }
	}
}
