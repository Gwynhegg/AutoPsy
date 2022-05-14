using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using AutoPsy.AuxServices;

namespace AutoPsy.AuxServices
{

    public static class PdfWriter       // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ДОДЕЛАТЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        public static void CreateDocument(Database.Entities.User user, List<Database.Entities.DiaryPage> diaryPages)
        {
            if (diaryPages == null || diaryPages.Count == 0) throw new Exception();
            var title = String.Format("{0}, {1} records from {2}.txt", String.Concat(user.PersonSurname, ' ', user.PersonName, '.'), diaryPages.Count, DateTime.Now.ToShortDateString());
            var directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);
            var path = Path.Combine(directory, title);
            
            using (var stream = File.CreateText(path))
                foreach (var page in diaryPages)
                {
                    stream.WriteLine(String.Join(", ", page.DateOfRecord, page.Topic));
                    stream.WriteLine(page.MainText);
                    stream.WriteLine(String.Concat("Tags: ", String.Join(", ", page.AttachedSymptoms.Split('\\'))));
                    stream.WriteLine(new String('-', 50));
                }
        }

    }
}
