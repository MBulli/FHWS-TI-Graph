using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Graph
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string BlattAufgabe { get; set; }
        public static string UnterAufgabe { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string[] args = e.Args;
            if(args.Length > 0)
            {
                BlattAufgabe = args[0];
                //UnterAufgabe = args[1];
            }
        }
    }
}
