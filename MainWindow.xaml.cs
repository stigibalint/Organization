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
            File.ReadAllLines("organizations-100000.csv").Skip(1).ToList().ForEach(x => lista.Add(new Organization(x.Split(";"))));
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

        private void FilterBy(object sender, SelectionChangedEventArgs e)
        {

            string selectedO, selectedE;

            if (cbOrszag.SelectedItem == null)
                selectedO = "Összes";
            else
                selectedO = cbOrszag.SelectedItem.ToString();


            if (cbEv.SelectedItem == null)
                selectedE = "Összes";
            else
                selectedE = cbEv.SelectedItem.ToString();


            dgAdatok.ItemsSource = organizations
                .Where(x => selectedE == "Összes" || x.Founded.ToString() == selectedE)
                .Where(x => selectedO == "Összes" || x.Country == selectedO).ToList();
        }
    }
}

