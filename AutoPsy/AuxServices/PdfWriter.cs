using System;
using System.Collections.Generic;
using System.IO;

namespace AutoPsy.AuxServices
{

    public static class PdfWriter       // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ДОДЕЛАТЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        public static void CreateDocument(Database.Entities.User user, List<Database.Entities.DiaryPage> diaryPages)
        {
            if (diaryPages == null || diaryPages.Count == 0) throw new Exception();
            var title = string.Format("{0}, {1} records from {2}.txt", string.Concat(user.PersonSurname, ' ', user.PersonName, '.'), diaryPages.Count, DateTime.Now.ToShortDateString());
            var directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);
            var path = Path.Combine(directory, title);

            using (StreamWriter stream = File.CreateText(path))
            {
                foreach (Database.Entities.DiaryPage page in diaryPages)
                {
                    stream.WriteLine(string.Join(", ", page.DateOfRecord, page.Topic));
                    stream.WriteLine(page.MainText);
                    stream.WriteLine(string.Concat("Tags: ", string.Join(", ", page.AttachedSymptoms.Split('\\'))));
                    stream.WriteLine(new string('-', 50));
                }
            }
        }

    }
}
