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
using System.Reflection;

namespace AutoBroadcast
{
	[APIVersion(1, 12)]
	public class AutoBroadcast : TerrariaPlugin
	{
		public static aBList aBroadcasts;
		public static String savepath = String.Empty;
		public static List<int> IntervalCooldown = new List<int>();

		public static DateTime Broadcast = DateTime.UtcNow;

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
			get { return "Automatically Broadcast a Message / Command every X seconds"; }
		}

		public override Version Version
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version; }
		}

		public override void Initialize()
		{
			GameHooks.Initialize += OnInitialize;
			GameHooks.Update += OnUpdate;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				GameHooks.Initialize -= OnInitialize;
				GameHooks.Update -= OnUpdate;
			}
			base.Dispose(disposing);
		}

		public AutoBroadcast(Main game) : base(game)
		{
			Order = 4;
			savepath = Path.Combine(TShockAPI.TShock.SavePath, "PluginConfigs", "AutoBroadcastConfig.json");

			ConfR reader = new ConfR();
			if (File.Exists(savepath))
			{
				aBroadcasts = reader.readFile(savepath);
				Console.WriteLine(aBroadcasts.AutoBroadcast.Count + " Broadcasts have been loaded.");
			}
			else
			{
				aBroadcasts = reader.writeFile(savepath);
				Console.WriteLine("Empty Broadcast Config file being created.");
			}
		}

		#region Hooks
		public void OnInitialize()
		{
			foreach (aBc bc in aBroadcasts.AutoBroadcast)
			{
				if (bc == null) continue;
				IntervalCooldown.Add(bc.Interval);
			}
			Broadcast = DateTime.UtcNow;

			Commands.ChatCommands.Add(new Command("abroadcast", autobc, "autobc"));
		}
		#endregion

		#region Timer
		public static void OnUpdate()
		{
			if ((DateTime.UtcNow - Broadcast).TotalMilliseconds >= 1000)
			{
				Broadcast = DateTime.UtcNow;
				try
				{
					if (aBroadcasts.AutoBroadcast == null) return;
					for (int i = 0; i < IntervalCooldown.Count; i++)
					{
						IntervalCooldown[i]--;
					}
					int v = 0;
					foreach (aBc bc in aBroadcasts.AutoBroadcast)
					{
						if (bc == null) continue;
						if (bc.Enabled && IntervalCooldown[v] < 1)
						{
							if (bc.Groups.Count < 1)
								BroadcastToAll(bc.Messages, (byte)bc.ColorR, (byte)bc.ColorG, (byte)bc.ColorB);
							else
								BroadcastToGroup(bc.Groups, bc.Messages, (byte)bc.ColorR, (byte)bc.ColorG, (byte)bc.ColorB);

							IntervalCooldown[v] = bc.Interval;
						}
						v++;
					}
				}
				catch { }
			}
		}
		#endregion

		#region Methods
		public static void BroadcastToGroup(List<string> bcgroup, List<string> messages, byte colorr, byte colorg, byte colorb)
		{
			try
			{
				if (bcgroup == null || messages == null) return;
				foreach (string msg in messages)
				{
					if (msg == null) continue;
					if (msg.StartsWith("/"))
					{
						Commands.HandleCommand(TSPlayer.Server, msg);
					}
					else
					{
						foreach (TSPlayer player in TShock.Players)
						{
							if (player != null && bcgroup.Contains(player.Group.Name))
							{
								player.SendMessage(msg, colorr, colorg, colorb);
							}
						}
					}
				}
			}
			catch { }
		}

		public static void BroadcastToAll(List<string> messages, byte colorr, byte colorg, byte colorb)
		{
			try
			{
				if (messages == null) return;
				foreach (string msg in messages)
				{
					if (msg == null) continue;
					if (msg.StartsWith("/"))
					{
						Commands.HandleCommand(TSPlayer.Server, msg);
					}
					else
					{
						TSPlayer.All.SendMessage(msg, colorr, colorg, colorb);
					}
				}
			}
			catch { }
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

					foreach (aBc bc in aBroadcasts.AutoBroadcast)
					{
						if (bc == null) continue;
						IntervalCooldown.Add(bc.Interval);
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
}