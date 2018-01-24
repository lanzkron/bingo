using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;

namespace Bingo
{
    class Program
    {
        static void Main(string[] args)
        {
            string title = "";
            var opts = new Mono.Options.OptionSet {
                { "t|title=", "title of the bingo cards", t => title = t },
            };

            try
            {
                opts.Parse(args);
            }
            catch(OptionException e) {
                Console.WriteLine(e.Message);
                return;
            }

            const string dir = @".\";
            const string center = "center.jpg";
            const int size = 4;
            const int count = 30;
            var words = new List<string>();
            using (var r = new System.IO.StreamReader(dir + "bingo.txt"))
            {
                while (!r.EndOfStream)
                    words.Add(r.ReadLine());
            }

            using (var w = new System.IO.StreamWriter(dir + "output.html", false, Encoding.UTF8))
            {
                w.WriteLine("<html dir=\"rtl\"><body>");
                for (int i = 0; i < count; ++i)
                {
                    var shuffled = words.OrderBy(a => Guid.NewGuid()).ToArray();
                    w.WriteLine("<h1>"+ title +"</h1><table border=\"1\">");
                    for (int row = 0; row < size; ++row)
                    {
                        w.WriteLine("<tr>");
                        for (int col = 0; col < size; ++col)
                        {
                            var x = shuffled[col + row * size];
                            int middle = size / 2;
                            if (col == middle && row == middle && System.IO.File.Exists(center))
                                x = center;
                            if (System.IO.File.Exists(dir + x))
                                x = "<img width=\"100\" height=\"100\" src=\"" + x + "\"/>";
                            w.WriteLine("<td align=\"center\" style=\"width:100px;height:100px \" >" + x + "</td>");
                        }
                        w.WriteLine("</tr>");
                    }
                    w.WriteLine("</table><p style=\"page-break-after: always;\">&nbsp;</p>");
                }
                w.WriteLine("</body></html>");
            }

            Console.WriteLine("done");
        }

    }
}