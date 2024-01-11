using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Path = System.IO.Path;
using System.IO;

namespace IMSApp
{
    /// <summary>
    /// Interaction logic for FIFOWindow.xaml
    /// </summary>
    public partial class FIFOWindow : Window
    {
        public FIFOWindow()
        {
            InitializeComponent();
        }

        private void FIFOButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have selected to release your items on a first in first out (FIFO) basis.");
            string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
            string filename = filepath + "\\storetotals.json";
            if (!File.Exists(filepath))
            {
                File.WriteAllText(filepath, "");
                Store store = new Store(0, 0, true);
            }
            else
            {
                string jsonRead = File.ReadAllText(filename);
                Store store = JsonConvert.DeserializeObject<Store>(jsonRead);
                store.FIFO = true;
                string updatedJsonData = JsonConvert.SerializeObject(store);
                File.WriteAllText(filename, updatedJsonData);
            }
        }

        private void LIFOButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have selected to release your items on a last in first out (LIFO) basis.");
            string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
            string filename = filepath + "\\storetotals.json";
            if (!File.Exists(filepath))
            {
                File.WriteAllText(filepath, "");
                Store store = new Store(0, 0, false);
            }
            else
            {
                string jsonRead = File.ReadAllText(filename);
                Store store = JsonConvert.DeserializeObject<Store>(jsonRead);
                store.FIFO = false;
                string updatedJsonData = JsonConvert.SerializeObject(store);
                File.WriteAllText(filename, updatedJsonData);
            }
        }
    }
}
