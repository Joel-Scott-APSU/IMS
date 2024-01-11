using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using Path = System.IO.Path;
using Newtonsoft.Json;

namespace IMSApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            

            string filepath = System.IO.Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
               "Categories");
            string filename = filepath + "\\About.txt";

            if (!System.IO.Directory.Exists(filepath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }
                catch (IOException)
                {
                    Console.WriteLine("IO Error: ");
                }
                catch (Exception)
                {
                    Console.WriteLine("General Error: ");
                }
            }

            if (!System.IO.File.Exists(filename)){
                try
                {
                    using (FileStream fs = File.Create(filename)) { }
                }
                catch (IOException)
                {
                    Console.WriteLine("IO Error: ");
                }
                catch (Exception)
                {
                    Console.WriteLine("General Error: ");
                }
            }

            try
            {
                string path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
                string[] files = Directory.EnumerateFiles(path).Select(x => Path.GetFileName(x)).ToArray();
                Array.Sort(files);
                foreach (string s in files)
                {
                    if(s == "About.txt")
                    {
                        continue;
                    }
                    int pos = s.IndexOf(".json");
                    string newString = s.Remove(pos);
                    SelectCategory.Items.Add(newString);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Folder not found");
            }

            Sort.Items.Add("Sort by Name");
            Sort.Items.Add("Sort by Quantity");
            Sort.Items.Add("Sort by Bought Price");
            Sort.Items.Add("Sort by Overall Price");

            buildAboutPage();
            
        }


        private void addCategory(object sender, RoutedEventArgs e)
        {
            CategorySorting cs = new();
            cs.AddCategory(CategoryName.Text);
            CategoryName.Text = "Enter Category Name";
        }
        private void RemoveCategory(object sender, RoutedEventArgs e)
        {
            CategorySorting cs = new();
            cs.RemoveCategory(SelectCategory.Text);
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            CategorySorting cs = new();
            cs.EditCategory(SelectCategory.Text, CategoryName.Text);
            CategoryName.Text = "Enter Category Name";
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            DisplayAddNewItem newWindow = new DisplayAddNewItem(SelectCategory.Text);
            newWindow.Show();
            DisplayBlock.Text = string.Empty;
        }

        private void AddExistingItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddExistingWindow newWindow = new AddExistingWindow(SelectCategory.Text);
                newWindow.Show();
            }
            catch (Exception) { }
        }

        private void RemoveSaleItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DisplayRemoveSaleItem newWindow = new DisplayRemoveSaleItem(SelectCategory.Text);
                newWindow.Show();
            }
            catch (Exception) { }
        }

        private void RemoveWriteoffItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string item = SelectCategory.Text;
                DisplayRemoveWriteOffItem newWindow = new DisplayRemoveWriteOffItem(item);
                newWindow.Show();
            }
            catch (Exception) { }
        }

        private void RemoveRecallItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string item = SelectCategory.Text;
                DisplayRemoveRecallItem newWindow = new(item);
                newWindow.Show();
            }
            catch (Exception) { }
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DisplayEditItem newWindow = new DisplayEditItem(SelectCategory.Text);
                newWindow.Show();
            }
            catch (Exception) { }
        }

        private void CategoryName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CategoryName.Clear();
        }

        private void FIFO_Click(object sender, RoutedEventArgs e)
        {
            FIFOWindow newWindow = new FIFOWindow();
            newWindow.Show();

        }

        private void ViewStore(object sender, RoutedEventArgs e)
        {
            string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
            string filename = filepath + "\\storetotals.json";
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, "");
                Store store = new Store(0, 0, true);
            }
            else
            {
                string jsonRead = File.ReadAllText(filename);
                Store store = JsonConvert.DeserializeObject<Store>(jsonRead);

                //Edit store values here
                //string display = "Total Sales: " + store.Sales + "\nCost of Goods Sold: " + store.Cogs +
                //    "Gross Profit: " + gross_profit;
                //DisplayBlock.Text = display;
            }

        }

        private void ViewCategory(object sender, RoutedEventArgs e)
        {
            string category = (string)SelectCategory.SelectedValue;
            CategorySorting cs = new();
            if ((string)Sort.SelectedValue == "Sort by Name")
            {
                DisplayBlock.Text = string.Empty;
                AboutBlock.Text = string.Empty;
                AboutBlock.Visibility = Visibility.Collapsed;
                cs.BuildArray(category);
                Sort so = new(cs);
                so.SortByName(cs.Items);
                for (int i = 0; i < so.tempArray.GetLength(1); i++)
                {
                    DisplayBlock.Text += so.displayName(so.tempArray);
                    so.Counter();
                }
            }
            else if ((string)Sort.SelectedValue == "Sort by Quantity")
            {
                DisplayBlock.Text = string.Empty;
                AboutBlock.Text = string.Empty;
                AboutBlock.Visibility = Visibility.Collapsed;
                cs.BuildArray(category);
                Sort so = new(cs);
                so.SortByQuantity(cs.Items);
                for (int i = 0; i < so.tempArray.GetLength(1); i++)
                {
                    DisplayBlock.Text += so.displayQuantity(so.tempArray);
                    so.Counter();
                }
            }
            else if ((string)Sort.SelectedValue == "Sort by Bought Price")
            {
                DisplayBlock.Text = string.Empty;
                AboutBlock.Text = string.Empty;
                AboutBlock.Visibility = Visibility.Collapsed;
                cs.BuildArray(category);
                Sort so = new(cs);
                so.SortByBoughtPrice(cs.Items);
                for (int i = 0; i < so.tempArray.GetLength(1); i++)
                {
                    DisplayBlock.Text += so.displayBoughtPrice(so.tempArray);
                    so.Counter();
                }
            }
            else if ((string)Sort.SelectedValue == "Sort by Overall Price")
            {
                DisplayBlock.Text = string.Empty;
                AboutBlock.Text = string.Empty;
                AboutBlock.Visibility = Visibility.Collapsed;
                cs.BuildArray(category);
                Sort so = new(cs);
                so.SortByPrice(cs.Items);
                for (int i = 0; i < so.tempArray.GetLength(1); i++)
                {
                    DisplayBlock.Text += so.displayOverallPrice(so.tempArray);
                    so.Counter();
                }
            }
            else
            {
                MessageBox.Show("Please select a valid sorting method.");
            }
        }

        public static bool isFileEmpty(string categoryName)
        {
            bool isEmpty = false;
            string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
            string filename = filepath + "\\" + categoryName + ".json";
            if (new FileInfo(filename).Length != 0)
            {
                MessageBox.Show("This file does not contain items");
                return isEmpty = true;
            }
            string jsonContent = File.ReadAllText(filename);
            JsonCategory? CategoryItems = JsonConvert.DeserializeObject<JsonCategory>(jsonContent);
            JsonItem[] items = CategoryItems.Item;
            if (CategoryItems.Item.Length == 0)
            {
                return isEmpty = true;
            }
            return isEmpty;
        }

        private void terminate(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            DisplayBlock.Text = string.Empty;
            AboutBlock.Text = string.Empty;
            AboutBlock.Visibility = Visibility.Visible;
            string path = System.IO.Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
            string filename = path + "\\About.txt";
            string text = File.ReadAllText(filename);
            AboutBlock.Text += text;
        }

        private void buildAboutPage()
        {
            string path = System.IO.Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
            string filename = path + "\\About.txt";
            using (StreamWriter writer = new(filename))
            {
                writer.WriteLine("Welcome to the Inventory Managment System, better known as the IMS. In this program you will be able to manage your store inventory. " +
                    "I am testing adding more info into the file to see if everything works the way that it should.\n");

                writer.WriteLine("Add Category: use the textbox with enter category Name to add that name to the list" +
                    " of catagories.\n");

                writer.WriteLine("Remove Cactegory: uses the combobox at the top of the page, select the category" +
                    " and hit Remove Category.\n");

                writer.WriteLine("Edit Category: Uses both the combobox mentioned and the textbox mentioned " +
                    "select the category enter a name into the text box select edit category and it changes the category name.\n");

                writer.WriteLine("Sorting: Sort by name, qty, bought price, sold price for category items.\n");

                writer.WriteLine("Item manipulation: The following is all under the combobox of item manipulation:\n");

                writer.WriteLine("- Add New Item: enables you to add new item to the system for information storage.\n"); 
                
                writer.WriteLine("- Add Existing Item: Enables you to add new batch values to the current selected item in the specified category.\n"); 
                
                writer.WriteLine("- Edit Item: Enables you to edit the name and price sold of the selected item.\n");

                writer.WriteLine("- Remove Sale Item: removes a specified qty of items sold for a selected item.\n");

                writer.WriteLine("- Remove Write-off Item: Removes a secified qty of items stolen for a sleeted Item.\n");

                writer.WriteLine("- Remove Recall Item: removes a specified items batch by entering the lot number.\n");
                
            }
        }


    }
    public class JsonCategory
    {
        public string CategoryName { get; set; }
        //public double TCOGSCategory { get; set; }
        public JsonItem[] Item { get; set; }
    }

    public class JsonItem
    {
        public string Name { get; set; }
        public string[] LotNumbers { get; set; }
        public double PricesSold { get; set; }
        public double[] PricesBought { get; set; }
        public int[] Qtys { get; set; }
    }
}

