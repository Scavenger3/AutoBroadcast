using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TShockAPI;

namespace AutoBroadcast
{
	public class ABConfig
	{
		public Broadcast[] Broadcasts = new Broadcast[0];

		public ABConfig Write(string file)
		{
			File.WriteAllText(file, JsonConvert.SerializeObject(this, Formatting.Indented));
			return this;
		}

		public static ABConfig Read(string file)
		{
			if (!File.Exists(file))
			{
				ABConfig.WriteExample(file);
			}
			return JsonConvert.DeserializeObject<ABConfig>(File.ReadAllText(file));
		}

		public static void WriteExample(string file)
		{
			var Ex = new Broadcast();
			Ex.Name = "Example Broadcast";
			Ex.Enabled = false;
			Ex.Messages = new string[]
			{
				"This is an example broadcast",
				"It will broadcast every 5 minutes",
				"Broadcasts can also execute commands",
				"/time noon"
			};
			Ex.ColorRGB = new float[] { 255f, 0f, 0f };
			Ex.Interval = 300;
			Ex.StartDelay = 60;

			var Conf = new ABConfig();
			Conf.Broadcasts = new Broadcast[] { Ex };

			Conf.Write(file);
		}
	}

	public class Broadcast
	{
		public string Name = string.Empty;
		public bool Enabled = false;
		public string[] Messages = new string[0];
		public float[] ColorRGB = new float[3];
		public int Interval = 0;
		public int StartDelay = 0;
		public string[] Groups = new string[0];
		public string[] TriggerWords = new string[0];
		public bool TriggerToWholeGroup = false;
	}
}
