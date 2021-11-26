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

namespace Kurochkin_ProductSchool
{
    /// <summary>
    /// Interaction logic for AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        bool editMode = false;
        string photoPath = "";
        byte[] photo;
        Product product;

        public AddEditProductWindow()
        {
            InitializeComponent();
            foreach (var item in Helper.entities.Manufacturers.ToList())
            {
                manufacturer.Items.Add(item.Name);
            }
        }

        public AddEditProductWindow(int id) : this()
        {
            editMode = true;

            product = Helper.entities.Products.Where(i => i.ID == id).FirstOrDefault();

            title.Text = product.Title;
            desc.Text = product.Description;
            manufacturer.SelectedIndex = product.ManufacturerID.Value;
            isActive.IsChecked = product.IsActive;
            price.Text = product.Cost.ToString();

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                product.Title = title.Text;
                product.Description = desc.Text;
                product.ManufacturerID = manufacturer.SelectedIndex;
                product.IsActive = isActive.IsChecked.Value;
                product.Cost = Convert.ToDecimal(price.Text);


                Helper.entities.SaveChanges();
                Close();

                MessageBox.Show("Запись о товаре успешно изменена!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            else
            {
                product = new Product()
                {
                    Title = title.Text,
                    Description = desc.Text,
                    ManufacturerID = manufacturer.SelectedIndex,
                    IsActive = IsActive,
                    Cost = Convert.ToDecimal(price.Text),
                    MainImagePath = ""

                };

                Helper.entities.Products.Add(product);
                Helper.entities.SaveChanges();
                Close();

                MessageBox.Show("Запись о товаре успешно добавлена!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void isActive_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void isActive_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
