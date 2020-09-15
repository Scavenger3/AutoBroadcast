using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace AutoBroadcast
{
	[ApiVersion(2, 1)]
	public class AutoBroadcast : TerrariaPlugin
	{
		public override string Name { get { return "AutoBroadcast"; } }
		public override string Author { get { return "Scavenger & Simon311"; } }
		public override string Description { get { return "Automatically Broadcast a Message or Command every x seconds"; } }
		public override Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

		public string ConfigPath { get { return Path.Combine(TShock.SavePath, "AutoBroadcastConfig.json"); } }
		public ABConfig Config = new ABConfig();

		public AutoBroadcast(Main Game) : base(Game) { }

		static readonly Timer Update = new Timer(1000);

		public override void Initialize()
		{
			ServerApi.Hooks.GameInitialize.Register(this, OnInitialize, -5);
			ServerApi.Hooks.ServerChat.Register(this, OnChat);
		}

		protected override void Dispose(bool Disposing)
		{
			if (Disposing)
			{
				ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
				ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
				Update.Elapsed -= OnUpdate;
				Update.Stop();
			}
			base.Dispose(Disposing);
		}

		public void OnInitialize(EventArgs args)
		{
			Commands.ChatCommands.Add(new Command("abroadcast", AutoBC, "autobc"));

			try
			{
				Config = ABConfig.Read(ConfigPath).Write(ConfigPath);
			}
			catch (Exception ex)
			{
				Config = new ABConfig();
				TShock.Log.ConsoleError("[AutoBroadcast] An exception occurred while parsing the AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
			}
			Update.Elapsed += OnUpdate;
			Update.Start();
		}

		public void AutoBC(CommandArgs args)
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
				TShock.Log.Error("[AutoBroadcast] An exception occurred while parsing tbe AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
			}
		}

		#region Chat
		public void OnChat(ServerChatEventArgs args)
		{
			var PlayerGroup = TShock.Players[args.Who]?.Group?.Name;

			if (string.IsNullOrWhiteSpace(PlayerGroup))
			{
				return;
			}

			foreach (var broadcast in Config.Broadcasts)
			{
				if (broadcast == null || !broadcast.Enabled || !broadcast.Groups.Contains(PlayerGroup)) { continue; }

				foreach (string Word in broadcast.TriggerWords)
				{
					if (args.Text.Contains(Word))
					{
						if (broadcast.TriggerToWholeGroup && broadcast.Groups.Length > 0)
						{
							BroadcastToGroups(broadcast.Groups, broadcast);
						}
						else BroadcastToPlayer(args.Who, broadcast);
						break;
					}
				}
			}
		}
		#endregion

		#region Update
		public void OnUpdate(object Sender, EventArgs e)
		{
			if (Main.worldID == 0) { return; }

			foreach (Broadcast broadcast in Config.Broadcasts)
			{
				if (broadcast == null || !broadcast.Enabled || broadcast.Interval < 1)
				{
					continue;
				}

				if (broadcast.StartDelay > 0)
				{
					broadcast.StartDelay--;
					continue;
				}

				broadcast.StartDelay = broadcast.Interval; // Start Delay used as Interval Countdown

				if (broadcast.Groups.Length > 0)
				{
					BroadcastToGroups(broadcast.Groups, broadcast);
				}
				else
				{
					BroadcastToAll(broadcast);
				}
			}
		}
		#endregion

		public static void BroadcastToGroups(string[] Groups, Broadcast broadcast)
		{
			foreach (string Line in broadcast.Messages)
			{
				if (Line.StartsWith(TShock.Config.CommandSpecifier) || Line.StartsWith(TShock.Config.CommandSilentSpecifier))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else
				{
					foreach (var player in TShock.Players)
					{
						if (player?.Group != null && Groups.Contains(player.Group.Name))
						{
							player.SendMessage(Line, (byte)broadcast.ColorRGB[0], (byte)broadcast.ColorRGB[1], (byte)broadcast.ColorRGB[2]);
						}
					}
				}
			}
		}

		public static void BroadcastToAll(Broadcast broadcast)
		{
			foreach (string Line in broadcast.Messages)
			{
                if (Line.StartsWith(TShock.Config.CommandSpecifier) || Line.StartsWith(TShock.Config.CommandSilentSpecifier))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else
				{
					TSPlayer.All.SendMessage(Line, (byte)broadcast.ColorRGB[0], (byte)broadcast.ColorRGB[1], (byte)broadcast.ColorRGB[2]);
				}
			}
		}
		public static void BroadcastToPlayer(int Who, Broadcast broadcast)
		{
			foreach (string Line in broadcast.Messages)
			{
				if (Line.StartsWith(TShock.Config.CommandSpecifier) || Line.StartsWith(TShock.Config.CommandSilentSpecifier))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else
				{
					TShock.Players[Who]?.SendMessage(Line, (byte)broadcast.ColorRGB[0], (byte)broadcast.ColorRGB[1], (byte)broadcast.ColorRGB[2]);
				}
			}
		}
	}
}
