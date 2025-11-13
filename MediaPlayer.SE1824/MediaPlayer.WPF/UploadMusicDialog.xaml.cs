using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
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
    /// Interaction logic for UploadMusicDialog.xaml
    /// </summary>
    public partial class UploadMusicDialog : Window
    {
        private SongService _songService = new();

        public UploadMusicDialog()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text;
            string album = txtAlbum.Text;
            string artist = txtArtist.Text;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Adio Files|*.mp3;*.wav;*.wma;*.m4a|All Files|*.*",
                Multiselect = true,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    SaveSongToDatabase(title, album, title, filePath);
                }
            }
        }

        private void SaveSongToDatabase(string title, string artist, string album, string filePath)
        {
            try
            {
                var createSong = new Song()
                {
                    Title = title,
                    //Artist = artist,
                    //AlbumId = album,
                    Duration = 0,
                    FilePath = filePath
                };

                _songService.CreateSong(createSong);

                MessageBox.Show("Song uploaded successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving song: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
