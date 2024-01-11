using IMSApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

public class Store
{
	//cogs == cost of goods sold
	private double COGS;
	private double sales;
	private bool fifo;
	public Store(double cogs, double _sales, bool Fifo = true  )
	{
		COGS = cogs;
		sales = _sales;
		Fifo = fifo;
	}
	
	public double Cogs
    {
        get { return COGS; }
        set => COGS = value;
    }
	public double Sales
	{ get { return sales; }
		set { sales = value; }
	}

	public bool FIFO
	{
		get { return fifo; }
		set { fifo = value; }
	}

    //JSON for Store Totals
    public void load_store()
    {
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

            //Edit store values here

            string updatedJsonData = JsonConvert.SerializeObject(store);
            File.WriteAllText(filename, updatedJsonData);
        }

    }

}
