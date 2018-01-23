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

namespace Dictionary
{
    public partial class DictionarySettings : UserControl
    {
        private readonly Settings settings;

        public DictionarySettings(Settings settings)
        {
            InitializeComponent();
            this.settings = settings;
        }

        private void View_Loaded(object sender, RoutedEventArgs re)
        {
            BighugelabsToken.Text = settings.BighugelabsToken;
            BighugelabsToken.TextChanged += (o, e) =>
            {
                settings.BighugelabsToken = BighugelabsToken.Text;
                settings.Save();
            };

            ICIBAToken.Text = settings.ICIBAToken;
            ICIBAToken.TextChanged += (o, e) =>
            {
                settings.ICIBAToken = ICIBAToken.Text;
                settings.Save();
            };

            MaxEditDistance.Text = settings.MaxEditDistance.ToString();
            MaxEditDistance.TextChanged += (o, e) =>
            {
                try
                {
                    settings.MaxEditDistance = Convert.ToInt32(MaxEditDistance.Text);
                }
                catch (Exception) { }
                settings.Save();
            };
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
