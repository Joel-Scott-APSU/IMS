using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IMSApp
{
    /// <summary>
    /// Interaction logic for AddExistingWindow.xaml
    /// </summary>
    public partial class AddExistingWindow : Window
    {

        public string _selectedCategoryName { get; set; }
        public AddExistingWindow(string PassedName)
        {
            try
            {
                InitializeComponent();
                _selectedCategoryName = PassedName;
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
                string filename = filepath + "\\" + _selectedCategoryName + ".json";
                string jsonContent = File.ReadAllText(filename);

                JsonCategory? CategoryItems = JsonConvert.DeserializeObject<JsonCategory>(jsonContent);
                JsonItem[] items = CategoryItems.Item;

                //create new Category instance
                Category c1 = new Category(_selectedCategoryName);

                for (int i = 0; i < items.Length; i++)
                {
                    Item oldItem = new Item();
                    oldItem.Name = items[i].Name;
                    foreach (int qtys in items[i].Qtys)
                    {
                        int j = 1;
                        oldItem.Qtys.Add(qtys);
                        j++;
                    }
                    foreach (string lotNumbers in items[i].LotNumbers)
                    {
                        int j = 1;
                        oldItem.LotNumbers.Add(lotNumbers);
                        j++;
                    }
                    foreach (double pricesBought in items[i].PricesBought)
                    {
                        int j = 1;
                        oldItem.PricesBought.Add(pricesBought);
                        j++;
                    }
                    oldItem.PricesSold = items[i].PricesSold;
                    c1.Item.Add(oldItem);
                    AddExistingComboBox.Items.Add(oldItem.Name);
                }

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No items in the category");
                this.Close();
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before editing items");
                this.Close();
                return;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(AddExistingComboBox.Text == String.Empty || AddExistingCostTextBox.Text == String.Empty || AddExistingLotNumberTextBox.Text == String.Empty
                    || AddExistingQtyTextBox.Text == string.Empty)
                {
                    MessageBox.Show("Enter all values before hitting confirm");
                    return;
                }
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
                string filename = filepath + "\\" + _selectedCategoryName + ".json";
                string jsonContent = File.ReadAllText(filename);

                JsonCategory? CategoryItems = JsonConvert.DeserializeObject<JsonCategory>(jsonContent);
                JsonItem[] items = CategoryItems.Item;

                //create new Category instance
                Category c1 = new Category(_selectedCategoryName);

                //iterate through list items and add new items to the category
                for (int i = 0; i < items.Length; i++)
                {
                    Item oldItem = new Item(); //this is item class
                    oldItem.Name = items[i].Name;
                    foreach (int qtys in items[i].Qtys)
                    {
                        oldItem.Qtys.Add(qtys);
                    }
                    foreach (string lotNumbers in items[i].LotNumbers)
                    {
                        oldItem.LotNumbers.Add(lotNumbers);
                    }
                    foreach (double pricesBought in items[i].PricesBought)
                    {
                        oldItem.PricesBought.Add(pricesBought);
                    }
                    oldItem.PricesSold = items[i].PricesSold;
                    c1.Item.Add(oldItem);
                }
                foreach (Item aItems in c1.Item)
                {
                    //if the name of the current item selected matches the item selected in
                    //the comboBox then add the values to the ArrayList else do nothing.
                    if (aItems.Name == AddExistingComboBox.Text)
                    {
                        aItems.Qtys.Add(int.Parse(AddExistingQtyTextBox.Text));
                        aItems.PricesBought.Add(double.Parse(AddExistingCostTextBox.Text));
                        aItems.LotNumbers.Add(AddExistingLotNumberTextBox.Text);
                    }

                }

                foreach (Item aItems in c1.Item)
                {
                    Debug.WriteLine($" {aItems.Name}");
                    Debug.WriteLine($"-----------------");
                    foreach (int qty in aItems.Qtys)
                    {
                        int i = 1;
                        Debug.WriteLine($"qty - {i}: {qty}");
                        i++;
                    }
                    foreach (double priceBought in aItems.PricesBought)
                    {
                        int i = 1;
                        Debug.WriteLine($"qty - {i}: {priceBought}");
                        i++;
                    }
                    foreach (string lotNumbers in aItems.LotNumbers)
                    {
                        int i = 1;
                        Debug.WriteLine($"qty - {i}: {lotNumbers}");
                        i++;
                    }

                    FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(fileStream);

                    // Serialize the Item object and append it to the JSON file
                    string json = JsonConvert.SerializeObject(c1);
                    streamWriter.WriteLine(json);

                    // Close the StreamWriter and FileStream
                    streamWriter.Close();
                    fileStream.Close();

                }

                //ask the user if they'd like to enter another item
                    MessageBoxResult result = MessageBox.Show("Would you like to enter another item?", "ChoiceWindow", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //reset Text Boxes to empty
                        AddExistingQtyTextBox.Text = "";
                        AddExistingCostTextBox.Text = "";
                        AddExistingLotNumberTextBox.Text = "";
                    }
                    else
                    {
                        this.Close();
                    }
                

            }
            catch (FormatException)
            {
                MessageBox.Show("Enter all values before hitting confirm");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before editing items");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
