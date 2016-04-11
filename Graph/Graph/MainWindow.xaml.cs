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
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, DataVertex> vertices = new Dictionary<string, DataVertex>();
            List<DataEdge> edges = new List<DataEdge>();
            FileParser.parse(App.FilePath, ref vertices, ref edges);
            Debug.WriteLine(vertices);

            var graph = new DataGraph(vertices.Values, edges);
            graphArea.GenerateGraph(graph);
        }
    }
}
