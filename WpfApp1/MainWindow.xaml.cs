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
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Organization> organizations;
        private void dgAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAdatok.SelectedItem is Organization)
            {
                Organization valasztottObj = dgAdatok.SelectedItem as Organization;
                labID.Content = valasztottObj.Id.ToString();
                labWEB.Content = valasztottObj.Website;
                labISM.Content = valasztottObj.Description;
            }
            
        }
        public MainWindow()
        {
            InitializeComponent();
            organizations = LoadData();

            dgAdatok.ItemsSource = organizations;
            cbOrszag.ItemsSource = GetDistinctCountries();

            cbEv.ItemsSource = GetDistinctYears();
          

            cbOrszag.SelectionChanged += FilterBy;
            cbEv.SelectionChanged += FilterBy;
        }

        private List<Organization> LoadData()
        {
            List<Organization> lista = new List<Organization>();
            File.ReadAllLines("organizations-100.csv").Skip(1).ToList().ForEach(x => lista.Add(new Organization(x.Split(";"))));
            return lista;
        }

        private List<string> GetDistinctCountries()
        {
            return organizations.Select(x => x.Country).OrderBy(x => x).Distinct().ToList();
        }

        private List<int> GetDistinctYears()
        {
            return organizations.Select(x => x.Founded).OrderBy(x => x).Distinct().ToList();
        }

        private bool asd(Organization x, string selectedO, string selectedE)
        {
            return x.Country == selectedO && x.Founded == int.Parse(selectedE);
        }

        private void FilterBy(object sender, SelectionChangedEventArgs e)
        {
            dgAdatok.ItemsSource = null;
            string selectedO = cbOrszag.SelectedItem as string;
            object ev = cbEv.SelectedItem.ToString();
            string selectedE = ev as string;

            if (selectedO != null && selectedE != null)
            {
                dgAdatok.ItemsSource = organizations.Where(x => asd(x, selectedO, selectedE)).ToList();
              }
            else if (selectedE != null)
            {
                dgAdatok.ItemsSource = organizations.Where(x => x.Founded == int.Parse(selectedE)).ToList();
            }
            else if (selectedO != null)
            {
                dgAdatok.ItemsSource = organizations.Where(x => x.Country == selectedO).ToList();
            }

        }




    }
}

