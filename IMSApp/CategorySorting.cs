using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMSApp
{
    internal class CategorySorting
    {
        private string categoryName;

        public object[,] Items;

        public CategorySorting()
        {
            try
            {
                Items = new object[5, 1];
            }
            catch (NullReferenceException)
            {
            }
        }

        public object Item
        {
            get { return Items; }
        }

        public CategorySorting(string categoryName)
        {
            this.categoryName = categoryName;

        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public void AddCategory(string AddCategory)
        {
            MainWindow mw = (MainWindow)App.Current.MainWindow;
            try
            {
                string categoryName = string.Join("", AddCategory.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                categoryName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(categoryName.ToLower());
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath
            (Environment.SpecialFolder.Desktop), "Categories");
                string filename = filepath + "\\" + categoryName + ".json";
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    MessageBox.Show("Please enter a category name into the box");
                }
                else if (categoryName == "Entercategoryname")
                {
                    MessageBox.Show("Please enter a category name into the box");
                    return;
                }

                else if (File.Exists(filename))
                {

                    MessageBox.Show("Category already exists");
                }
                else
                {
                    using (FileStream fs = File.Create(filename))
                    {
                        string path = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
                        mw.SelectCategory.Items.Clear();
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
                            mw.SelectCategory.Items.Add(newString);
                        }
                        mw.CategoryName.Clear();
                        MessageBox.Show("Category successfully created");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter a valid value");
            }
        }

        public void RemoveCategory(string CategoryName)
        {
            MainWindow mw = (MainWindow)App.Current.MainWindow;

                string FileName = string.Join("", CategoryName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                FileName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FileName.ToLower());
                string path = System.IO.Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.Desktop), "Categories");

                string filename = path + "\\" + FileName + ".json";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                    mw.SelectCategory.Items.Clear();
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
                        mw.SelectCategory.Items.Add(newString);
                    }
                    mw.CategoryName.Clear();
                    MessageBox.Show("Category successfully removed");

                }
                else
                {
                    MessageBox.Show("Category not found");
                }
            

        }

        public void EditCategory(string CategoryName, string newCategoryName)
        {
            MainWindow mw = (MainWindow)App.Current.MainWindow;
            string path = System.IO.Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
               "Categories");
            string FileName = string.Join("", newCategoryName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            FileName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FileName.ToLower());

            try
            {
                if (FileName == CategoryName)
                {
                    MessageBox.Show("Cannot rename the file to the same name");
                    return;
                }
                else if(FileName == "Entercategoryname")
                {
                    MessageBox.Show("Enter the new name of the category into the box");
                    return;
                }
                File.Move(path + "\\" + CategoryName + ".json", path + "\\" + FileName + ".json");
                mw.SelectCategory.Items.Clear();
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
                    mw.SelectCategory.Items.Add(newString);
                }
                mw.CategoryName.Clear();
                MessageBox.Show("Category name successfully changed");

            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Category not found");
            }
            catch (IOException)
            {
                MessageBox.Show("Category " + FileName + " already exists");
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter a valid value");
            }
        }

        public void BuildArray(string categoryName)
        {

            try
            {
                //create new item of type json
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Categories");
                string FileName = string.Join("", categoryName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                FileName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FileName.ToLower());
                string filename = filepath + "\\" + FileName + ".json";
                Debug.WriteLine(filename);
                string jsonContent = File.ReadAllText(filename);
                JsonCategory? CategoryItems = JsonConvert.DeserializeObject<JsonCategory>(jsonContent);
                JsonItem[] items = CategoryItems.Item;

                Items = new object[5, CategoryItems.Item.Length];
                for (int i = 0; i < CategoryItems.Item.Length; i++)
                {
                    Item thing = new();
                    for (int j = 0; j < 5; j++)
                    {
                        Items[j, i] = CategoryItems.Item[i].Name;
                        j++;
                        foreach (int qtys in CategoryItems.Item[i].Qtys)
                        {
                            thing.Qtys.Add(qtys);
                        }
                        Items[j, i] = thing.Qtys;

                        j++;
                        Items[j, i] = CategoryItems.Item[i].PricesSold;
                        j++;
                        foreach (double boughtPrice in CategoryItems.Item[i].PricesBought)
                        {
                            thing.PricesBought.Add(boughtPrice);
                        }
                        Items[j, i] = thing.PricesBought;
                        j++;
                        foreach (string lotNumber in CategoryItems.Item[i].LotNumbers)
                        {
                            thing.LotNumbers.Add(lotNumber);
                        }
                        Items[j, i] = thing.LotNumbers;
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("IOORE");
            }
            catch (Exception) {
                MessageBox.Show("Error");
            }

        }
    }
}
