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
    /// Interaction logic for DisplayEditItem.xaml
    /// </summary>
    public partial class DisplayEditItem : Window
    {

        private string _selectedCategoryName { get; set; }
        public DisplayEditItem(string PassedName)
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
                    EditItemComboBox.Items.Add(oldItem.Name);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter all values before hitting confirm");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before editing items");
                this.Close();
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try {
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
                    //match name to name in comboBox
                    if (aItems.Name == EditItemComboBox.Text)
                    {
                        string newName = EditNameTextBox.Text;
                        newName = newName.Trim();
                        if (newName.Length > 0)
                        {
                            aItems.Name = newName;
                        }
                        string newPrice = EditPriceTextBox.Text;
                        newPrice = newPrice.Trim();
                        if (newPrice.Length > 0)
                        {
                            try
                            {
                                aItems.PricesSold = Double.Parse(newPrice);
                            }
                            catch
                            {
                                string message = "Error: Price must be a numerical value.";

                                MessageBox.Show(message);
                            }
                        }

                    }
                }
                FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                // Serialize the Item object and append it to the JSON file
                string json = JsonConvert.SerializeObject(c1);
                streamWriter.WriteLine(json);

                // Close the StreamWriter and FileStream
                streamWriter.Close();
                fileStream.Close();


                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter all values before hitting confirm");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before removing items");
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
