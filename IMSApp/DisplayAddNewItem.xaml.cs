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
using System.IO;

namespace IMSApp
{
    /// <summary>
    /// Interaction logic for DisplayAddNewItem.xaml
    /// </summary>
    public partial class DisplayAddNewItem : Window
    {
        private string _selectedCategoryName { get; set; }
        public DisplayAddNewItem(string PassedName)
        {
            InitializeComponent();
            _selectedCategoryName = PassedName;
            if(_selectedCategoryName == null)
            {
                    MessageBox.Show("Select a category before adding items");
                    this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
                string filename = filepath + "\\" + _selectedCategoryName + ".json";
                if (new FileInfo(filename).Length != 0)
                {
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
                    }

                    Item item = new Item();
                    item.Name = itemNameTextBox.Text;
                    item.Qtys.Add(int.Parse(itemQuantityTextBox.Text));
                    item.PricesSold = float.Parse(itemSellPriceTextBox.Text);
                    item.PricesBought.Add(float.Parse(itemBuyPriceTextBox.Text));
                    item.LotNumbers.Add(itemLotNumberTextBox.Text);
                    c1.Item.Add(item);

                FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                    // Serialize the Item object and append it to the JSON file
                    string json = JsonConvert.SerializeObject(c1);
                    streamWriter.WriteLine(json);

                    // Close the StreamWriter and FileStream
                    streamWriter.Close();
                    fileStream.Close();

                }
                else
                {
                    Item item = new Item();
                    item.Name = itemNameTextBox.Text;
                    item.Qtys.Add(int.Parse(itemQuantityTextBox.Text));
                    item.PricesSold = float.Parse(itemSellPriceTextBox.Text);
                    item.PricesBought.Add(float.Parse(itemBuyPriceTextBox.Text));
                    item.LotNumbers.Add(itemLotNumberTextBox.Text);
                    Category c1 = new Category(_selectedCategoryName);
                    c1.Item.Add(item);

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
                    itemNameTextBox.Text = "";
                    itemQuantityTextBox.Text = "";
                    itemBuyPriceTextBox.Text = "";
                    itemSellPriceTextBox.Text = "";
                    itemLotNumberTextBox.Text = "";
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
                MessageBox.Show("Select a category before adding items");
                this.Close();
            }
        }
    }
}
