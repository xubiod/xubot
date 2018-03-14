using System;
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

namespace xubot_setup_xubiod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Men(object sender, RoutedEventArgs e)
        {
            Start.Visibility = Visibility.Collapsed;
            Review.Visibility = Visibility.Collapsed;

            Options.Visibility = Visibility.Visible;
        }

        private void Button_Click_Opt(object sender, RoutedEventArgs e)
        {
            Options.Visibility = Visibility.Collapsed;

            Discord.Visibility = Visibility.Visible;
        }

        private void Button_Click_Discord(object sender, RoutedEventArgs e)
        {
            Discord.Visibility = Visibility.Collapsed;

            if ((bool)reddit.IsChecked)
            {
                Reddit_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)twitter.IsChecked)
            {
                Twitter_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)googleShorten.IsChecked)
            {
                Google_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)steam.IsChecked)
            {
                Steam_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)github.IsChecked)
            {
                GitHub_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Review.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Reddit(object sender, RoutedEventArgs e)
        {
            Reddit_Grid.Visibility = Visibility.Collapsed;

            if ((bool)twitter.IsChecked)
            {
                Twitter_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)googleShorten.IsChecked)
            {
                Google_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)steam.IsChecked)
            {
                Steam_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)github.IsChecked)
            {
                GitHub_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Review.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Twitter(object sender, RoutedEventArgs e)
        {
            Twitter_Grid.Visibility = Visibility.Collapsed;

            if ((bool)googleShorten.IsChecked)
            {
                Google_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)steam.IsChecked)
            {
                Steam_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)github.IsChecked)
            {
                GitHub_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Review.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Google(object sender, RoutedEventArgs e)
        {
            Google_Grid.Visibility = Visibility.Collapsed;

            if ((bool)cat.IsChecked)
            {
                Cat_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)steam.IsChecked)
            {
                Steam_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)github.IsChecked)
            {
                GitHub_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Review.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Cat(object sender, RoutedEventArgs e)
        {
            Cat_Grid.Visibility = Visibility.Collapsed;

            if ((bool)steam.IsChecked)
            {
                Steam_Grid.Visibility = Visibility.Visible;
            }
            else if ((bool)github.IsChecked)
            {
                GitHub_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Review.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Steam(object sender, RoutedEventArgs e)
        {
            Steam_Grid.Visibility = Visibility.Collapsed;

            if ((bool)github.IsChecked)
            {
                GitHub_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Review.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Github(object sender, RoutedEventArgs e)
        {
            GitHub_Grid.Visibility = Visibility.Collapsed;

            Review.Visibility = Visibility.Visible;
            
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            string json = @"{
    ""discord"": """ + discordToken.Text + @""",
    ""discord_dev"": """ + discordToken.Text + @""",

    ""reddit"": {
        ""user"": """ + reddit_username.Text + @""",
        ""pass"": """ + reddit_pass.Text + @""",
        ""key1"": """ + reddit_id.Text + @""",
        ""key2"": """ + reddit_secret.Text + @""",
    },

    ""twitter"": {
        ""key1"": """ + twitter_consumerkey.Text + @""",
        ""key2"": """ + twitter_consumersec.Text + @""",
        ""key3"": """ + twitter_uat.Text + @""",
        ""key4"": """ + twitter_uak.Text + @""",
    },

    ""googleLinkShort"": """ + googlKey.Text + @""",
    ""cat"": """ + catKey.Text + @""",
    ""steam"": """ + steamKey.Text + @""",
    ""github:"": """ + ghpat.Text + @"""
}";
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Keys"; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "JSON files (.json)|*.json"; // Filter files by extension
            //dlg.CheckFileExists = true;

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                File.WriteAllText(filename, json);

                Review.Visibility = Visibility.Collapsed;
                Complete.Visibility = Visibility.Visible;
            }
        }
    }
}
