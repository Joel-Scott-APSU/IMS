using IMSApp;
using System;
using System.Collections;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

public class Category
{
    private string categoryName;
    private List<Item> Items;
    public Category()
    {
        this.Items = new List<Item>();
    }


    public Category(string categoryName)
    {
        this.categoryName = categoryName;
        this.Items = new List<Item>();
    }

    public string CategoryName
    {
        get { return categoryName; }
        set { categoryName = value; }
    }

    public List<Item> Item
    {
        get { return Items; }
        set { }
    }


    public override string ToString()
    {
        string output = categoryName + ":\n";
        foreach (Item item in Items)
        {
            output += item.ToString() + "\n";
        }
        return output;
    }

}