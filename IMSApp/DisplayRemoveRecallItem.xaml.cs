using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IMSApp
{
    /// <summary>
    /// Interaction logic for DisplayRemoveRecallItem.xaml
    /// </summary>
    public partial class DisplayRemoveRecallItem : Window
    {

        private string _selectedCategoryName { get; set; }
        public DisplayRemoveRecallItem(string PassedName)
        {
            try {
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
                    RemoveRecallItemCombobox.Items.Add(oldItem.Name);
                }
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

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                MainWindow mw = new();
                mw.DisplayBlock.Text = String.Empty;
                if((string)RemoveRecallItemCombobox.SelectedValue == string.Empty || RemoveRecallItemTextBox.Text == string.Empty)
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
                }
                foreach (Item aItems in c1.Item)
                {
                    //if the name of the current item selected matches the item selected in
                    //the comboBox then add the values to the ArrayList else do nothing.
                    if (aItems.Name == RemoveRecallItemCombobox.Text)
                    {
                        if (aItems.LotNumbers.Contains(RemoveRecallItemTextBox.Text))
                        {
                            int index = aItems.LotNumbers.IndexOf(RemoveRecallItemTextBox.Text);
                            aItems.LotNumbers.RemoveAt(index);
                            aItems.Qtys.RemoveAt(index);
                            aItems.PricesBought.RemoveAt(index);
                            if (aItems.LotNumbers.Count == 0)
                            {
                                c1.Item.Remove(aItems);
                            }
                            break;
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
    }
}
