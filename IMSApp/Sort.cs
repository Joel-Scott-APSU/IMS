using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;

namespace IMSApp
{
    internal class Sort
    {
        Item item = new Item();
        MainWindow mw = new();
        private int counter;
        public object[,] tempArray;
        private string[] nameArray = new string[2];
        private string Text;


        public Sort(CategorySorting cs)
        {
            counter = 0;
            tempArray = new object[cs.Items.GetLength(0), cs.Items.GetLength(1)];
            nameArray = new string[cs.Items.GetLength(1)];

            for (int i = 0; i < cs.Items.GetLength(1); i++)
            {

                nameArray[i] = (string)cs.Items[0, i];
            }


        }




        public object[,] SortByName(object[,] array)
        {
            try
            {
                if (array.GetLength(1) == 0)
                {
                    MessageBox.Show("No items in the category");
                    counter = 0;
                    return array;
                }
                Array.Sort(nameArray);
                var isEmpty = true;

                for (int i = 0; i < nameArray.Length; i++)
                {
                    if (nameArray[i] != "zzz")
                    {
                        isEmpty = false;
                    }
                }

                if (isEmpty == true)
                {
                    counter = 0;
                    return tempArray;

                }

                int j = 0;
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    if (nameArray[j] == "")
                    {
                        j++;
                    }
                    if (nameArray[j] == (string)array[0, i] && nameArray[j] != "")
                    {
                        string placeholder = nameArray[j];
                        for (int k = 0; k < array.GetLength(0); k++)
                        {
                            tempArray[k, counter] = array[k, i];
                        }
                        nameArray[j] = "zzz";
                        tempArray[0, counter] = placeholder;
                        counter++;
                        SortByName(array);
                    }
                }
            }
            catch (Exception)
            {  }
            counter = 0;
            return array;
        }

        public object[,] SortByQuantity(object[,] array)
        {
            try
            {
                if (array.Length == 0)
                {
                    MessageBox.Show("No items in the category");
                    counter = 0;
                    return array;
                }
                int maxValue = -1;
                var isZero = true;

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    List<int> qty = (List<int>)array[1, i];
                    if (qty.Count != 0)
                    {
                        isZero = false;
                    }
                }
                if (isZero == true)
                {
                    counter = 0;
                    return tempArray;
                }

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    List<int> newQty = (List<int>)array[1, i];
                    int qtytotal = 0;
                    foreach (int item in newQty)
                    {
                        qtytotal += item;
                    }
                    if (maxValue < qtytotal)
                    {
                        maxValue = qtytotal;
                    }
                }

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    List<int> qty3 = qty3 = (List<int>)array[1, i];
                    int qtytotal = 0;
                    foreach (int item in qty3)
                    {
                        qtytotal += item;
                    }
                    if (maxValue == qtytotal)
                    {
                        for (int j = 0; j < array.GetLength(0); j++)
                        {
                            tempArray[j, counter] = array[j, i];
                        }
                        List<int> qty4 = (List<int>)tempArray[1, counter];
                        int number = 0;
                        foreach (int item in qty4)
                        {
                            number += item;
                        }
                        qty3.Clear();
                        array[1, i] = qty3;
                        tempArray[1, counter] = number;
                        counter++;
                        SortByQuantity(array);
                    }
                }
            }
            catch (Exception) { }
            counter = 0;
            return array;
        }


        public object[,] SortByBoughtPrice(object[,] array)
        {
            try
            {
                if (array.Length == 0)
                {
                    MessageBox.Show("No items in the category");
                    counter = 0;
                    return array;
                }
                double maxValue = 0.0;
                var isZero = true;

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    List<double> qty = (List<double>)array[3, i];
                    if (qty.Count != 0)
                    {
                        isZero = false;
                    }
                }
                if (isZero == true)
                {
                    counter = 0;
                    return tempArray;
                }

                for (int i = 0; i < array.GetLength(1); i++)
                {

                    List<double> newQty = (List<double>)array[3, i];
                    double qtytotal = 0;
                    foreach (double item in newQty)
                    {
                        qtytotal += item;
                    }
                    if (maxValue < qtytotal)
                    {
                        maxValue = qtytotal;
                    }

                }

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    List<double> qty3 = (List<double>)array[3, i];
                    double qtytotal = 0;
                    foreach (double item in qty3)
                    {
                        qtytotal += item;
                    }
                    if (maxValue == qtytotal)
                    {
                        for (int j = 0; j < array.GetLength(0); j++)
                        {
                            tempArray[j, counter] = array[j, i];
                        }
                        List<double> qty4 = (List<double>)tempArray[3, counter];
                        double number = 0;
                        foreach (double item in qty4)
                        {
                            number += item;
                        }
                        qty3.Clear();
                        array[3, i] = qty3;

                        tempArray[3, counter] = number;
                        counter++;


                        SortByBoughtPrice(array);
                    }
                }
            }
            catch (Exception)
            { }            
            counter = 0;
            return array;
        }

        public object[,] SortByPrice(object[,] array)
        {
            try
            {
                //checks if the array is empty
                if (array.Length == 0)
                {
                    MessageBox.Show("No items in the category");
                    counter = 0;
                    return array;
                }
                double maxValue = 0.0;
                var isZero = true;
                //checks if all values of the array have been iterated through 
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    double price = (double)array[2, i];
                    if (price != 0.0)
                    {
                        isZero = false;
                    }
                }
                if (isZero == true)
                {
                    counter = 0;
                    return tempArray;
                }

                //finds the max value in the array
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    double priceTotal = (double)array[2, i];
                    if (maxValue < priceTotal)
                    {
                        maxValue = priceTotal;
                    }

                }
                //copies the array at index i to the new temporary array
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    double priceTotal = (double)array[2, i];
                    if (maxValue == priceTotal)
                    {
                        for (int j = 0; j < array.GetLength(0); j++)
                        {
                            tempArray[j, counter] = array[j, i];
                        }
                        array[2, i] = 0.0;

                        tempArray[2, counter] = priceTotal;
                        counter++;

                        //recursive function call
                        SortByPrice(array);
                    }
                }
            }
            catch (Exception)
            {}
            counter = 0;
            return array;
        }

        public string displayName(object[,] array)
        {
            try
            {
                List<int> quantity = (List<int>)tempArray[1,counter];
                int number = 0;
                foreach (int item in quantity)
                {
                    number += item;
                }
                List<double> boughtPrice = (List<double>)tempArray[3, counter];
                double bp = 0.0;
                foreach (double item in boughtPrice)
                {
                    bp += item;
                }
                Text = "Name: " + array[0, counter] + " | Quantity: " + number + " | Price: $" + string.Format("{0:0.00}",array[2, counter]) + " | Bought Price: $" + string.Format("{0:0.00}", bp) + "\n -------------------------------------------------------------------------- \n";
                return Text;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No items in the category");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before sorting");
            }
            return "";
        }

        public string displayQuantity(object[,] array)
        {
            try
            {
                List<double> boughtPrice = (List<double>)tempArray[3, counter];
                double bp = 0.0;
                foreach (double item in boughtPrice)
                {
                    bp += item;
                }
                Text = "Name: " + array[0, counter] + " | Quantity: " + array[1, counter] + " | Price: $" + string.Format("{0:0.00}",array[2, counter]) + " | Bought Price: $" + string.Format("{0:0.00}",bp) + "\n -------------------------------------------------------------------------- \n";
                return Text;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No items in the category");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before sorting");
            }
            return "";
        }

        public string displayBoughtPrice(object[,] array)
        {
            try
            {
                List<int> Quantity = (List<int>)tempArray[1, counter];
                int qty = 0;
                foreach (int item in Quantity)
                {
                    qty += item;
                }
                Text = "Name: " + array[0, counter] + " | Quantity: " + qty + " | Price: $" + string.Format("{0:0.00}",array[2, counter]) + " | Bought Price: $" + string.Format("{0:0.00}",array[3, counter]) + "\n -------------------------------------------------------------------------- \n";
                return Text;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No items in the category");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before sorting");
            }
            return "";
        }

        public string displayOverallPrice(object[,] array)
        {
            try
            {
                List<int> quantity = (List<int>)tempArray[1, counter];
                int number = 0;
                foreach (int item in quantity)
                {
                    number += item;
                }
                List<double> boughtPrice = (List<double>)tempArray[3, counter];
                double bp = 0.0;
                foreach (double item in boughtPrice)
                {
                    bp += item;
                }
                Text = "    Name: " + array[0, counter] + " | Quantity: " + number + " | Price: $" + string.Format("{0:0.00}", array[2, counter]) + " | Bought Price: $" + string.Format("{0:0.00}",bp) + "\n -------------------------------------------------------------------------- \n";
                return Text;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No items in the category");
            }
            catch (Exception)
            {
                MessageBox.Show("Select a category before sorting");
            }
            return "";
        }

        public void Counter()
        {
            counter++;
        }
    }
}
