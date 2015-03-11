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
	[ApiVersion(1, 17)]
	public class AutoBroadcast : TerrariaPlugin
	{
		public override string Name { get { return "AutoBroadcast"; } }
		public override string Author { get { return "Scavenger"; } }
		public override string Description { get { return "Automatically Broadcast a Message or Command every x seconds"; } }
		public override Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

		public string ConfigPath { get { return Path.Combine(TShock.SavePath, "AutoBroadcastConfig.json"); } }
		public ABConfig Config = new ABConfig();

		public AutoBroadcast(Main Game) : base(Game) { }

		static readonly Timer Update = new System.Timers.Timer(1000);
		public static bool ULock = false;
		public const int UpdateTimeout = 501;

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
				Log.ConsoleError("[AutoBroadcast] An exception occurred while parsing the AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
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
				Log.Error("[AutoBroadcast] An exception occurred while parsing tbe AutoBroadcast config!\n{0}".SFormat(ex.ToString()));
			}
		}

		#region Chat
		public void OnChat(ServerChatEventArgs args)
		{
			var Start = DateTime.Now;
			string[] Groups = new string[0];
			string[] Messages = new string[0];
			float[] Colour = new float[0];
			var PlayerGroup = TShock.Players[args.Who].Group.Name;

			lock (Config.Broadcasts)
				foreach (var broadcast in Config.Broadcasts)
				{
					if (Timeout(Start)) return;
					if (broadcast == null || !broadcast.Enabled || !broadcast.Groups.Contains(PlayerGroup)) continue;

					foreach (string Word in broadcast.TriggerWords)
					{
						if (Timeout(Start)) return;
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
		public void OnUpdate(object Sender, EventArgs e)
		{
			if (Main.worldID == 0) return;
			if (ULock) return;
			ULock = true;
			var Start = DateTime.Now;

			int NumBroadcasts = 0;
			lock (Config.Broadcasts)
			NumBroadcasts = Config.Broadcasts.Length;
			for (int i = 0; i < NumBroadcasts; i++)
			{
				if (Timeout(Start, UpdateTimeout)) return;
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
					Config.Broadcasts[i].StartDelay = Config.Broadcasts[i].Interval; // Start Delay used as Interval Countdown
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
			ULock = false;
		}
		#endregion

		public static void BroadcastToGroups(string[] Groups, string[] Messages, float[] Colour)
		{
			foreach (string Line in Messages)
			{
				if (Line.StartsWith(TShock.Config.CommandSpecifier) || Line.StartsWith(TShock.Config.CommandSilentSpecifier))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else
				{
					lock (TShock.Players)
						foreach (var player in TShock.Players)
						{
							if (player != null && Groups.Contains(player.Group.Name))
							{
								player.SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
							}
						}
				}
			}
		}
		public static void BroadcastToAll(string[] Messages, float[] Colour)
		{
			foreach (string Line in Messages)
			{
                if (Line.StartsWith(TShock.Config.CommandSpecifier) || Line.StartsWith(TShock.Config.CommandSilentSpecifier))
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
                if (Line.StartsWith(TShock.Config.CommandSpecifier) || Line.StartsWith(TShock.Config.CommandSilentSpecifier))
				{
					Commands.HandleCommand(TSPlayer.Server, Line);
				}
				else lock(TShock.Players)
				{
					TShock.Players[plr].SendMessage(Line, (byte)Colour[0], (byte)Colour[1], (byte)Colour[2]);
				}
			}
		}

		public static bool Timeout(DateTime Start, int ms = 500, bool warn = true)
		{
			bool ret = (DateTime.Now - Start).TotalMilliseconds >= ms;
			if (ms == UpdateTimeout && ret) ULock = false;
			if (warn && ret)
			{
				Console.WriteLine("Hook timeout detected in AutoBroadcast. You might want to report this.");
				Log.Error("Hook timeout detected in AutoBroadcast. You might want to report this.");
			}
			return ret;
		}
	}
}
