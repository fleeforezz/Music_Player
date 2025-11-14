using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace MediaPlayer.WPF
{
    /// <summary>
    /// Interaction logic for UploadMusicDialog.xaml
    /// </summary>
    public partial class UploadMusicDialog : Window
    {
        private SongService _songService = new();
        private string _selectedFilePath;

        public UploadMusicDialog()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a song title.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTitle.Focus();
                return;
            }

            // Open file dialog to select song file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a;*.flac;*.aac|All Files|*.*",
                Multiselect = false, // Changed to single file selection for clearer UX
                Title = "Select Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string title = txtTitle.Text.Trim();
                string artist = txtArtist.Text.Trim(); 
                string genre = txtGenre.Text.Trim();

                // Save to database
                if (SaveSongToDatabase(title, artist, genre, filePath))
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private bool SaveSongToDatabase(string title, string artist, string genre, string filePath)
        {
            int? artistId = null;
            if (!string.IsNullOrWhiteSpace(artist))
                artistId = _songService.GetOrCreateArtist(artist);

            try
            {
                // Validate file exists
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Selected file does not exist.", "File Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                // Get actual duration if possible (you may need to implement this)
                var duration = GetAudioDuration(filePath);

                var createSong = new Song()
                {
                    Title = title,
                    ArtistId = artistId,
                    Duration = duration,
                    FilePath = filePath,
                    Genre = genre,
                    CoverImagePath = ""
                };

                _songService.CreateSong(createSong);

                MessageBox.Show($"Song '{title}' uploaded successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving song: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show($"Error saving song: {ex.Message}");
                return false;
            }
        }

        private int GetAudioDuration(string filePath)
        {
            try
            {
                // You can use TagLib or NAudio to get actual duration
                // For now, returning 0 as placeholder
                // Example with TagLib:
                // var file = TagLib.File.Create(filePath);
                // return (int)file.Properties.Duration.TotalSeconds;

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a;*.flac;*.aac|All Files|*.*",
                Multiselect = false,
                Title = "Select Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                txtFileName.Text = $"Selected: {System.IO.Path.GetFileName(_selectedFilePath)}";

                // Auto-fill title if empty
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    txtTitle.Text = System.IO.Path.GetFileNameWithoutExtension(_selectedFilePath);
                }
            }
        }
    }
}
