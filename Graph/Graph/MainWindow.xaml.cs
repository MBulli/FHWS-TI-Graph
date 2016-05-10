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

namespace Graph {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            Debug.Listeners.Add(new TextboxTraceListener(logTexbox));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            //var graph = FileParser.Parse(App.BlattAufgabe);

            ////Blatt1Aufgabe2.A();
            //Blatt2Aufgabe1.B();

            Grapher<VertexBase> graph = null;

            if (App.BlattAufgabe == "Blatt1Aufgabe2") {
                graph = Blatt1Aufgabe2.A();
            } else if (App.BlattAufgabe == "Blatt2Aufgabe1") {
                graph = Blatt2Aufgabe1.B();
            }

            graphArea.GenerateGraph(graph);
            zoomControl.ZoomToFill();
        }
    }

    class TextboxTraceListener : TraceListener {
        private TextBox textbox;

        public TextboxTraceListener(TextBox tb) {
            textbox = tb;
        }

        public override void Write(string message) {
            textbox?.AppendText(message);
            textbox?.ScrollToEnd();
        }

        public override void WriteLine(string message) {
            textbox?.AppendText(message);
            textbox?.AppendText(Environment.NewLine);
            textbox?.ScrollToEnd();
        }
    }
}
