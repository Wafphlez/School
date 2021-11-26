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

namespace Kurochkin_ProductSchool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Product> list;
        bool isactive;

        public MainWindow()
        {
            InitializeComponent();

            filterManufacturer.Items.Add("Все");

            foreach (var item in Helper.entities.Manufacturers.ToList())
            {
                filterManufacturer.Items.Add(item.Name);
            }

            filterManufacturer.SelectedIndex = 0;
            list = Helper.entities.Products.ToList();
            LVProducts.ItemsSource = list;
        }

        public void Filter()
        {
            var data
                = Helper.entities.Products
                .Where(i => i.Title.Contains(filterTitle.Text))
                .Where(i => i.Description.Contains(filterDesc.Text))
                .Where(i => i.IsActive == isactive).ToList()
                .ToList();


            if (filterManufacturer.SelectedIndex != 0)
                data = data.Where(i => i.ManufacturerID == filterManufacturer.SelectedIndex).ToList();

            if (sortByComboBoxs.SelectedIndex == 0)
                LVProducts.ItemsSource = data.OrderByDescending(i => i.Cost).ToList();
            else
                LVProducts.ItemsSource = data.OrderBy(i => i.Cost).ToList();
        }

        private void filterManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void filterTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();

        }

        private void filterDesc_TextChanged(object sender, TextChangedEventArgs e)
        {

            Filter();
        }

        private void clearFilters_Click(object sender, RoutedEventArgs e)
        {
            filterTitle.Text = "";
            filterDesc.Text = "";
            filterManufacturer.SelectedIndex = 0;
            filterIsActive.IsChecked = false;

            Filter();
        }

        private void viewSellHistory_Click(object sender, RoutedEventArgs e)
        {
            if (LVProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ничего не выбрано!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void deleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (LVProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ничего не выбрано!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы уверены?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (Product item in LVProducts.SelectedItems)
                {
                    Helper.entities.Products.Remove(item);
                }

                MessageBox.Show("Выбранные данные были успешно удалены!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);

                Helper.entities.SaveChanges();
            }
            Filter();
        }

        private void editProduct_Click(object sender, RoutedEventArgs e)
        {
            if (LVProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выделение пустое!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new AddEditProductWindow(((Product)LVProducts.SelectedItem).ID).ShowDialog();
            Filter();
        }

        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            new AddEditProductWindow().ShowDialog();
            Helper.entities = new Entities();
            Filter();
        }

        private void filterIsActive_Checked(object sender, RoutedEventArgs e)
        {
            isactive = true;
            Filter();

        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            isactive = false;
            Filter();
        }
    }
}
