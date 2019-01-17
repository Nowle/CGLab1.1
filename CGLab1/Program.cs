using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGLab1
{
    static class Program
    {
        public static void Main()
        {
            Application.Run(new MyForm() {ClientSize = new Size(862, 500)});
        }
    }
}
