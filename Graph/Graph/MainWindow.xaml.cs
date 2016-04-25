using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            Debug.Listeners.Add(new TextboxTraceListener(logTexbox));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vertices = new Dictionary<string, Vertex>();
            var edges = new List<Edge>();

            FileParser.parse(App.FilePath, ref vertices, ref edges);
            Debug.WriteLine(vertices);

            var graph = new Grapher(false, vertices, edges);
            graphArea.GenerateGraph(graph); 

            Aufgabe2.A();
            zoomControl.ZoomToFill();
        }
    }

    class TextboxTraceListener : TraceListener
    {
        private TextBox textbox;

        public TextboxTraceListener(TextBox tb)
        {
            textbox = tb;
        }

        public override void Write(string message)
        {
            textbox?.AppendText(message);
        }

        public override void WriteLine(string message)
        {
            textbox?.AppendText(message);
            textbox?.AppendText(Environment.NewLine);
        }
    }
}
