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
		public Broadcast[] Broadcasts { get { return Config.Broadcasts; } }
		public int[] Intervals = new int[0];
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
				Config = ABConfig.Read(ConfigPath);
				Intervals = new int[Broadcasts.Length];
				for (int i = 0; i < Broadcasts.Length; i++)
				{
					Intervals[i] = Broadcasts[i].StartDelay;
				}
			}
			catch (Exception ex)
			{
				this.Config = new ABConfig();
				Log.ConsoleError("[AutoBroadcast] An exception occurred while parsing the AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
			}
		}

		public void OnChat(ServerChatEventArgs args)
		{
			foreach (Broadcast broadcast in Broadcasts)
			{
				if (broadcast == null || !broadcast.Enabled)
				{
					continue;
				}

				if (broadcast.Groups.Length > 0)
				{
					if (TShock.Players[args.Who] != null && !broadcast.Groups.Contains(TShock.Players[args.Who].Group.Name))
					{
						continue;
					}
				}

				foreach (string Word in broadcast.TriggerWords)
				{
					if (args.Text.Contains(Word))
					{
						if (broadcast.TriggerToWholeGroup && broadcast.Groups.Length > 0)
						{
							BroadcastToGroups(broadcast.Groups, broadcast.Messages, broadcast.ColorRGB);
						}
						else
						{
							BroadcastToPlayer(args.Who, broadcast.Messages, broadcast.ColorRGB);
						}
						break;
					}
				}
			}
		}
		public void OnUpdate(EventArgs args)
		{
			if ((DateTime.UtcNow - LastCheck).TotalSeconds >= 1)
			{
				LastCheck = DateTime.UtcNow;
				for (int i = 0; i < Broadcasts.Length; i++)
				{
					if (Broadcasts[i] == null || !Broadcasts[i].Enabled || Broadcasts[i].Interval < 1)
					{
						continue;
					}
					if (Intervals[i] > 0)
					{
						Intervals[i]--;
						continue;
					}
					if (Broadcasts[i].Groups.Length > 0)
					{
						BroadcastToGroups(Broadcasts[i].Groups, Broadcasts[i].Messages, Broadcasts[i].ColorRGB);
					}
					else
					{
						BroadcastToAll(Broadcasts[i].Messages, Broadcasts[i].ColorRGB);
					}
					Intervals[i] = Broadcasts[i].Interval;
				}
			}
		}

		public static void BroadcastToGroups(string[] Groups, string[] Messages, float[] Colour)
		{
			foreach (string Line in Messages)
			{
				if (Line.StartsWith("/"))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else
				{
					for (int plr = 0; plr < TShock.Players.Length; plr++)
					{
						if (TShock.Players[plr] != null && Groups.Contains(TShock.Players[plr].Group.Name))
						{
							TShock.Players[plr].SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
						}
					}
				}
			}
		}
		public static void BroadcastToAll(string[] Messages, float[] Colour)
		{
			foreach (string Line in Messages)
			{
				if (Line.StartsWith("/"))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else
				{
					TSPlayer.All.SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
				}
			}
		}
		public static void BroadcastToPlayer(int plr, string[] Messages, float[] Colour)
		{
			foreach (string Line in Messages)
			{
				if (Line.StartsWith("/"))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else if (TShock.Players[plr] != null)
				{
					TShock.Players[plr].SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
				}
			}
		}

		public void autobc(CommandArgs args)
		{
			string SubCommand = args.Parameters.Count > 0 ? args.Parameters[0].ToLower() : string.Empty;
			switch (SubCommand)
			{
				case "reload":
					{
						try
						{
							Config = ABConfig.Read(ConfigPath);
							Intervals = new int[Broadcasts.Length];
							for (int i = 0; i < Broadcasts.Length; i++)
							{
								Intervals[i] = Broadcasts[i].StartDelay;
							}
							args.Player.SendSuccessMessage("Successfully reloaded AutoBroadcast config!");
						}
						catch (Exception ex)
						{
							Config = new ABConfig();
							args.Player.SendWarningMessage("An exception occurred while parsing tbe AutoBroadcast config! check logs for more details!");
							Log.Error("[AutoBroadcast] An exception occurred while parsing tbe AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
						}
					}
					break;
				default:
					{
						args.Player.SendWarningMessage("Usage: /autobc reload - Reloads settings from config file");
					}
					break;
			}
		}
	}
}
