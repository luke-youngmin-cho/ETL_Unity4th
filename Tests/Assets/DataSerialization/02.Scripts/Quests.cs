using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Test.DataSerialization
{
    [Serializable]
    public struct Quest
    {
        public int id;
        public string title;
        public string description;
    }


    public class Quests
    {
        public static List<Quest> list;

        static string splitWordPatterns = ",";
        static string splitLinePatterns = "\r\n|\n|\r";

        public static void Load()
        {
            TextAsset asset = Resources.Load<TextAsset>("Quests");

            string[] lines = Regex.Split(asset.text, splitLinePatterns);

            string[] header = Regex.Split(lines[0], splitWordPatterns);

            list = new List<Quest>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = Regex.Split(lines[i], splitWordPatterns);
                Quest quest = new Quest();

                if (int.TryParse(values[0], out quest.id) == false)
                    throw new Exception($"[Quests] : Failed to parse data. Format is wrong");
                quest.title = values[1];
                quest.description = values[2];
                list.Add(quest);
            }
        }
    }
}