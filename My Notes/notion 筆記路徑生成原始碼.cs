using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Build_MarkDown_Path
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string mdContent = $"## Github筆記路徑自動生成\n生成時間{DateTime.Now}\n";
            List<Note> notes = new List<Note>();
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] fileList = dir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in fileList)
            {
                Note note = new Note();
                note.Name = file.Name;
                note.Path = file.Directory.ToString().Split("\\").SkipWhile(str => str != "My Notes").Skip(1).ToList();
                note.buildMDLink();
                if (note.subNameCheck())
                    notes.Add(note);
            }

            FileStream fileStream = new FileStream($"{dir}\\github_Path.md", FileMode.Create);

            fileStream.Close();

            List<IGrouping<List<string>, Note>> groupList = notes.GroupBy(note => note.Path).ToList();
            Dictionary<string, List<Note>> titleNotePairs = new Dictionary<string, List<Note>>();
            foreach (Note note in notes)
            {
                Console.WriteLine($"put info {note.Name}");
                string title = "\n---\n## ";
                foreach (string str in note.Path)
                    title += $"{str} ";
                title += "\n";
                if (!titleNotePairs.ContainsKey(title))
                    titleNotePairs.Add(title, new List<Note>() { note });
                else
                    titleNotePairs[title].Add(note);
            }
            foreach (var pair in titleNotePairs)
            {
                mdContent += pair.Key;
                foreach (Note note in pair.Value)
                    mdContent += $"* {note.MDLink}\n";
            }

            using (StreamWriter sw = new StreamWriter($"{dir}\\github_Path.md"))
            {
                sw.Write(mdContent);
            }
        }

        private class Note
        {
            public List<string> Path { get; set; }
            public string Name { get; set; }

            /// <summary>
            /// 連結到github筆記位置的完整字串。
            /// </summary>
            public string MDLink { get; set; }

            private List<string> subNameConsent = new List<string>(new string[] { "md", "cs", "pdf", "txt" });

            public void buildMDLink()
            {
                string name = Name.Split('.').First();
                MDLink = "https://github.com/STRockefeller/SAMPLES/tree/master/My%20Notes";
                foreach (var p in Path)
                    MDLink += $"/{p}";
                //網址的#要改成%23，不然會連錯位置。 空白會變成%20不過不改好像也不會出錯。
                MDLink = MDLink.Replace("#", "%23");
                MDLink += $"/{Name}";
                MDLink = $"[{name}]({MDLink})";
            }

            public bool subNameCheck() => subNameConsent.Contains(Name.Split('.').Last());
        }
    }
}