using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace VersionDecoder
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
        
        private void EncodeVersionString(object sender, RoutedEventArgs e)
        {
            Status.Content = string.Empty;
            if (!Version.TryParse(Output.Text, out var version))
            {
                Status.Content = "Could not parse text in output";
                return;
            }

            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, version);
                var bytes = memoryStream.ToArray();
                Input.Text = Convert.ToBase64String(bytes);
            }
        }

        private void DecodeVersionString(object sender, RoutedEventArgs e)
        {
            Status.Content = string.Empty;
            if (string.IsNullOrEmpty(Input.Text))
            {
                Status.Content = "Cannot decode empty string";
                return;
            }

            var rawBinaryVersionString = Convert.FromBase64String(Input.Text);
            using (var serializationStream = new MemoryStream(rawBinaryVersionString))
            {
                var formatter = new BinaryFormatter();
                var deserializedVersion = (Version)formatter.Deserialize(serializationStream);
                Output.Text = deserializedVersion.ToString();
            }
        }
    }
}
