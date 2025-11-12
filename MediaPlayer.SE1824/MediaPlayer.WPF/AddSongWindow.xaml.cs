using Microsoft.Win32;
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

namespace MediaPlayer.WPF
{
    /// <summary>
    /// Interaction logic for AddSongWindow.xaml
    /// </summary>
    public partial class AddSongWindow : Window
    {
        private string selectedSongPath;
        private string selectedCoverPath;

        public AddSongWindow()
        {
            InitializeComponent();
        }

        private void SelectSong_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 Files|*.mp3;*.wav;*.flac";
            if (ofd.ShowDialog() == true)
            {
                selectedCoverPath = ;
                selectedSongPath. = ofd.FileName;
            }
        }

        private void SelectCoverImage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
