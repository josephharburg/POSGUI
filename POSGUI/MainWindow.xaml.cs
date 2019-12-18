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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace POSGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            showlist.ItemsSource = Product.TextToList();
        }

        private
        class Product
        {
            public static List<Product> Products = TextToList();
            public string Name { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }

            public double Subtotal { get; set; }
            public double Quantity { get; set; }
            public double Invetory { get; set; }
            public double GrandTotal { get; set; }
            public double Tax { get; set; }
            public static List<Product> SelectedItems = new List<Product>();

            public Product()
            {

            }
            public Product(string name, string category, string description, double price, double invetory)
            {
                Name = name;
                Category = category;
                Description = description;
                Price = price;
                Invetory = invetory;
            }


            public static void ListToText()
            {
                StreamWriter sw = new StreamWriter(@"C:\Users\josep\Desktop\coding\C#\POSGUI\POSGUI\ProductList.txt");

                foreach (Product p in Products)
                {
                    sw.WriteLine($"{p.Name},{p.Category},{p.Description},{p.Price},{p.Invetory}");

                }
                sw.Close();
            }
            public static List<Product> TextToList()
            {
                List<String> items = new List<string>();

                List<Product> ProductList = new List<Product>();

                StreamReader sr = new StreamReader(@"C:\Users\josep\Desktop\coding\C#\POSGUI\POSGUI\ProductList.txt");

                var i = sr.ReadLine();

                while (i != null)
                {
                    items.Add(i);
                    i = sr.ReadLine();
                }

                foreach (string item in items)
                {
                    string[] par = item.Split(',');

                    double price = double.Parse(par[3]);
                    double inv = double.Parse(par[4]);

                    ProductList.Add(new Product(par[0], par[1], par[2], price, inv));

                }
                sr.Close();
                return ProductList;

            }

            public void GetSubtotal()
            {
                this.Subtotal = this.Quantity * this.Price;
            }



            public static List<Product> GetGrandTotalAndTax()
            {
                Product tempProduct = new Product();
                foreach (Product p in SelectedItems)
                {
                    tempProduct.GrandTotal += p.Subtotal;

                }

                tempProduct.Tax = tempProduct.GrandTotal * 0.06;

                return new List<Product> { tempProduct };

            }

        }


        private void showlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult addItem = MessageBox.Show("Would you like to add this item?", "Add Item?", MessageBoxButton.YesNo);
            if (addItem == MessageBoxResult.Yes)
            {
                Product hello = Product.Products[showlist.SelectedIndex];
                if (hello.Invetory > 1)
                {
                    if (!Product.SelectedItems.Contains(hello))
                    { Product.SelectedItems.Add(hello); }
                    hello.Quantity += 1;
                    hello.Invetory -= 1;
                    hello.GetSubtotal();
                    cart.ItemsSource = Product.SelectedItems;
                    CheckingOut.ItemsSource = Product.GetGrandTotalAndTax();
                }
                else
                {
                    MessageBox.Show("Sorry that item is out of stock!");
                }
                Product.ListToText();
                cart.Items.Refresh();
            }

        }


        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Checkout(object sender, RoutedEventArgs e)
        {
            



        }

        private void cart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult removeItem = MessageBox.Show("Would you like to remove this item from cart?", "Remove Item?", MessageBoxButton.YesNo);
            if (removeItem == MessageBoxResult.Yes)
            {
                Product hello = Product.SelectedItems[cart.SelectedIndex];
                Product.SelectedItems.Remove(hello);
                hello.Quantity = 0;
                hello.Invetory += 1;
                cart.ItemsSource = Product.SelectedItems;
                CheckingOut.ItemsSource = Product.GetGrandTotalAndTax();
                Product.ListToText();
                cart.Items.Refresh();
                CheckingOut.Items.Refresh();
            }
        }
    }
}
