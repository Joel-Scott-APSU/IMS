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
using System.Windows.Shapes;

namespace IMSApp
{
    /// <summary>
    /// Interaction logic for DisplayRemoveSaleItem.xaml
    /// </summary>
    public partial class DisplayRemoveSaleItem : Window
    {

        private string _selectedCategoryName { get; set; }
        public DisplayRemoveSaleItem(string PassedName)
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
                    RemoveItemNameComboBox.Items.Add(oldItem.Name);
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
                MessageBox.Show("Select a category before removing items");
                this.Close();
                return;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)RemoveItemNameComboBox.SelectedValue == string.Empty || RemoveSaleQtyTextBox.Text == string.Empty)
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
                
                int inputFromUser = int.Parse(RemoveSaleQtyTextBox.Text);
                int indexOfName = 0;
                string name = "";
                bool shouldRemove = false;
                ArrayList indexList = new ArrayList();
                foreach (Item aItem in c1.Item)
                {
                    if (aItem.Name == RemoveItemNameComboBox.Text)
                    {
                        name = aItem.Name;
                        indexOfName = c1.Item.IndexOf(aItem);
                        Debug.WriteLine("IndexOfName: " + indexOfName);
                        int indexOfCurrentQty = 0;
                        foreach (int qty in aItem.Qtys)
                        {

                            Debug.WriteLine($"test1");

                            int indexValue = aItem.Qtys.IndexOf(qty);

                            int current = qty;
                            int totalQtyLeft = current - inputFromUser;

                            Debug.WriteLine("inputFromUser: " + totalQtyLeft);
                            if (totalQtyLeft <= 0)
                            {
                                Debug.WriteLine("test2");

                                inputFromUser = +inputFromUser - current;
                                Debug.WriteLine("new inputFromUser total: " + inputFromUser);
                                indexList.Add(current);
                                shouldRemove = true;
                            }
                            else
                            {
                                Debug.WriteLine("Test3");
                                aItem.Qtys.Remove(qty);
                                aItem.Qtys.Insert(indexOfCurrentQty, totalQtyLeft);
                                break;
                            }
                            indexOfCurrentQty++;
                        }

                    }
                }
                Debug.WriteLine("IndexOfName: " + indexOfName);
                Debug.WriteLine(c1);

                foreach (int indexs in indexList)
                {
                    Debug.WriteLine(indexs);
                }

                if (shouldRemove == true)
                {
                    Debug.WriteLine("Test4");

                    foreach (int indexs in indexList)
                    {
                        if (name == RemoveItemNameComboBox.Text)
                        {
                            Debug.WriteLine("Test5");
                            Debug.WriteLine(indexs);
                            if (c1.Item[indexOfName].Qtys.Contains(indexs))
                            {
                                int indexValue = c1.Item[indexOfName].Qtys.IndexOf(indexs);
                                c1.Item[indexOfName].Qtys.Remove(indexs);
                                c1.Item[indexOfName].PricesBought.RemoveAt(indexValue);
                                c1.Item[indexOfName].LotNumbers.RemoveAt(indexValue);
                                if (c1.Item[indexOfName].Qtys.Count == 0)
                                {
                                    c1.Item.RemoveAt(indexOfName);
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
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter all values before hitting confirm");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before removing items");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
