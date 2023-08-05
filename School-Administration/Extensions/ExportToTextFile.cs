using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace School_Administration.Extensions
{
    public static class ExportToTextFile
    {
        public static int LogNumber = 0; //you can make it Guid or reset it after specific time but for simplicity I did that

        public static void ExportToText<T>(this IEnumerable<T> data, string FileName, char ColumnSeperator)
        {
            LogNumber += 1;

            FileName = FileName + "-" + LogNumber;

            using (var sw = File.CreateText(FileName))
            {
                var plist = typeof(T).GetProperties().Where(p => p.CanRead && (p.PropertyType.IsValueType || p.PropertyType == typeof(string)) && p.GetIndexParameters().Length == 0).ToList();
                if (plist.Count > 0)
                {
                    var seperator = ColumnSeperator.ToString();
                    sw.WriteLine(string.Join(seperator, plist.Select(p => p.Name)));
                    foreach (var item in data)
                    {
                        var values = new List<object>();
                        foreach (var p in plist) values.Add(p.GetValue(item, null));
                        sw.WriteLine(string.Join(seperator, values));
                    }
                }
            }
        }
    }
}
