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

public class Item
{


    private string name;
    private List<string> lotNumbers;
    private double priceSold;
    private List<double> pricesBought;
    private List<int> qtys;

    public Item()
    {
        this.lotNumbers = new List<string>();
        this.pricesBought = new List<double>();
        this.qtys = new List<int>();
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public List<string> LotNumbers
    {
        get { return lotNumbers; }
        set { }
    }

    public double PricesSold
    {
        get { return priceSold; }
        set { priceSold = value; }
    }

    public List<double> PricesBought
    {
        get { return pricesBought; }
        set { }
    }

    public List<int> Qtys
    {
        get { return qtys; }
        set { }
    }


    public int TotalQty(List<int> qtys)
    {
        int totalQty = 0;

        foreach (int qty in qtys)
        {
            Console.WriteLine(qty);
            totalQty += qty;
        }
        return totalQty;
    }

    public double TotalPricesBought(List<double> pricesBought)
    {
        double total = 0;

        foreach (double boughtPrices in pricesBought)
        {
            total += boughtPrices;
        }
        return total;
    }


    public override string ToString()
    {
        int totalQty = TotalQty(qtys);
        double totalBoughtPrice = TotalPricesBought(pricesBought);
        string output = string.Format("name: {0} | qty: {1} | priceSold: ${2:0.00} | pricesBought ${3:0.00}", name, totalQty, priceSold, totalBoughtPrice);
        return output;
    }


}

