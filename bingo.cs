using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mono.Options;

namespace Bingo
{
    struct Options {
        internal string title;
        internal string center;
        internal uint size;

        internal Options(uint size) {
            this.size = size;
            title = null;
            center = null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options(4);
            string file = "bingo.txt";
            string dir = ".";
            uint count = 30;
            bool rtl = false;

            var opts = new Mono.Options.OptionSet {
                { "title=", "title of the bingo cards", t => options.title = t },
                { "file=", "text file containing the values to put in the bingo cells (an answer to a line)", f => file = f},
                { "center=", "value to be entered in center tile of all cards", c => options.center = c},
                { "size=", "dimensions of the bingo cards", s => options.size = UInt32.Parse(s) },
                { "count=", "number of bingo cards to create", c => count = UInt32.Parse(c)},
                { "rtl", "create Right To Left cards", x => rtl = true },
            };

            try
            {
                opts.Parse(args);
            }
            catch(OptionException e) {
                Console.WriteLine(e.Message);
                return;
            }

            if (options.center != null) {
                if ((options.size & 1) != 1) {
                    Console.WriteLine("Center should only be specified with odd sized cards");
                    return;
                }
            }

            var words = new List<string>();
            using (var r = new System.IO.StreamReader(Path.Combine(dir, file)))
            {
                while (!r.EndOfStream)
                    words.Add(r.ReadLine());
            }
            Console.WriteLine("Read {0} lines from {1}", words.Count, file);

            using (var w = new System.IO.StreamWriter(Path.Combine(dir, "output.html"), false, Encoding.UTF8))
            {
                writeCards(w, options, words, dir, count, rtl);
            }

            Console.WriteLine("done");
        }

        static void writeCards(System.IO.StreamWriter w, Options options, IEnumerable<string> words, string dir, uint count, bool rtl) {
            w.WriteLine("<html {0}><body>", rtl? "dir=\"rtl\"": "");

            for (uint i = 0; i < count; ++i)
            {
                writeCard(w, options, words, dir);
            }
            w.WriteLine("</body></html>");
        }

        static void writeCard(System.IO.StreamWriter w, Options options, IEnumerable<string> words, string dir) {
            var shuffled = words.OrderBy(a => Guid.NewGuid()).ToArray();
            w.WriteLine("<h1>"+ options.title +"</h1><table border=\"1\">");
            for (int row = 0; row < options.size; ++row)
            {
                w.WriteLine("<tr>");
                for (int col = 0; col < options.size; ++col)
                {
                    var x = shuffled[col + row * options.size];
                    var middle = options.size / 2;
                    if (col == middle && row == middle && options.center != null)
                        x = options.center;
                    if (System.IO.File.Exists(Path.Combine(dir, x)))
                        x = "<img width=\"100\" height=\"100\" src=\"" + x + "\"/>";
                    w.WriteLine("<td align=\"center\" style=\"width:100px;height:100px \" >" + x + "</td>");
                }
                w.WriteLine("</tr>");
            }
            w.WriteLine("</table><p style=\"page-break-after: always;\">&nbsp;</p>");
        }
    }
}