using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

namespace AutoBroadcastConfig
{
    class ConfR
    {
        public aBList writeFile(String file)
        {
            TextWriter tw = new StreamWriter(file);

            aBList autoBcs = new aBList();

            List<string> defMessages = new List<string> { "", "", "", "", "", "", "" };
            List<string> defGroups = new List<string> { };
            aBc new1 = new aBc("Broadcast 1", false, defMessages, 255, 255,255, 300, defGroups);

            autoBcs.AutoBroadcast.Add(new1);

            tw.Write(JsonConvert.SerializeObject(autoBcs, Formatting.Indented));
            tw.Close();

            return autoBcs;
        }

        public aBList readFile(String file)
        {
            TextReader tr = new StreamReader(file);
            String raw = tr.ReadToEnd();
            tr.Close();

            aBList autoBcs = JsonConvert.DeserializeObject<aBList>(raw);
            return autoBcs;
        }

        public void saveFile(String file, aBList lst)
        {
            TextWriter tw = new StreamWriter(file);

            tw.Write(JsonConvert.SerializeObject(lst, Formatting.Indented));
            tw.Close();
        }
    }

    public class aBList
    {
        public List<aBc> AutoBroadcast;

        public aBList()
        {
            AutoBroadcast = new List<aBc>();
        }
    }

    public class aBc
    {
        public string Name;
        public bool Enabled;
        public List<string> Messages;
        public int ColorR;
        public int ColorG;
        public int ColorB;
        public int Interval;
        public List<string> Groups;

        public aBc(string Name, bool Enabled, List<string> Messages, int ColorR, int ColorG, int ColorB, int Interval, List<string> Groups)
        {
            this.Name = Name;
            this.Enabled = Enabled;
            this.Messages = Messages;
            this.ColorR = ColorR;
            this.ColorG = ColorG;
            this.ColorB = ColorG;
            this.Interval = Interval;
            this.Groups = Groups;
        }
    }
}
