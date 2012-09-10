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
	[APIVersion(1, 12)]
	public class AutoBroadcast : TerrariaPlugin
	{
		public static aBList aBroadcasts;
		public static String savepath = "";
		public static List<int> TTNext = new List<int>();

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
			get { return new Version("1.4.3"); }
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
			foreach (aBc bc in aBroadcasts.AutoBroadcast)
			{
				TTNext.Add(bc.Interval);
			}
			Broadcast = DateTime.UtcNow;

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
		#endregion

		#region Timer
		public static void OnUpdate()
		{
			if ((DateTime.UtcNow - Broadcast).TotalMilliseconds >= 1000)
			{
				Broadcast = DateTime.UtcNow;
				try
				{
					for (int i = 0; i < TTNext.Count; i++)
					{
						TTNext[i]--;
					}
					int v = 0;
					foreach (aBc bc in aBroadcasts.AutoBroadcast)
					{
						if (bc.Enabled && TTNext[v] < 1)
						{
							if (bc.Groups.Count < 1)
								bctoAll(bc.Messages, (byte)bc.ColorR, (byte)bc.ColorG, (byte)bc.ColorB);
							else
								bctoGroup(bc.Groups, bc.Messages, (byte)bc.ColorR, (byte)bc.ColorG, (byte)bc.ColorB);

							TTNext[v] = bc.Interval;
						}
						v++;
					}
				}
				catch { }
			}
		}
		#endregion

		#region Methods
		public static void bctoGroup(List<string> bcgroup, List<string> messages, byte colorr, byte colorg, byte colorb)
		{
			foreach (string msg in messages)
			{
				if (msg.StartsWith("/"))
				{
					Commands.HandleCommand(TSPlayer.Server, msg);
				}
				else if (msg != null)
				{
					foreach (TSPlayer player in TShock.Players)
					{
						if (player == null) continue;
						if (bcgroup.Contains(player.Group.Name))
						{
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
				if (msg.StartsWith("/"))
				{
					Commands.HandleCommand(TSPlayer.Server, msg);
				}
				else if (msg != null)
				{
					TSPlayer.All.SendMessage(msg, colorr, colorg, colorb);
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
}