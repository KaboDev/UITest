using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using UITest.Model;
using UITest.View;
using UITest.ViewModel;

namespace UITest.Core
{
    public enum Elements
    {
        Input,
        Selection,
        Image,
        Dropdown
    }

    class CustomPopup
    {
        public string Title { get; set; }
        public List<Element> Elements = new List<Element>();
    }

    class Element
    {
        public string Label { get; set; }
        public object CurrentElement { get; }

        public Element(string label, object element)
        {
            Label = label;
            CurrentElement = element;
        }
    }
    class InputElement : DefaultElementData, IDataErrorInfo
    {
        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
            }
        }

        private string _temporaryText;
        public string TemporaryText
        {
            get { return _temporaryText; }
            set
            {
                _temporaryText = value;
                OnPropertyChanged();
            }
        }

        public Type InputType { get; }

        public string InputPattern { get; }
        public string VisualRegexTooltip { get; }

        #region InputErrorHandling
        public string this[string input]
        {
            get
            {
                string result = null;
                string errorMessage = null;

                if (string.IsNullOrEmpty(Input))
                {
                    result = "Input can not be empty";
                }
                else
                {
                    switch (Type.GetTypeCode(InputType))
                    {
                        case TypeCode.Int32:
                            errorMessage = "Input has to be a number with no decimal places";
                            break;
                        case TypeCode.Single:
                            Input = Input.Replace('.', ',');
                            errorMessage = "Input has to be a number with decimal places";
                            break;
                    }

                    //Checks if input is correct type 
                    if (!CustomExtensions.IsParsePossible(Input, InputType))
                    {
                        result = errorMessage;
                    }

                    if (!string.IsNullOrEmpty(InputPattern))
                    {
                        Input = Regex.Replace(Input, @"\s", "");
                        if (!Regex.IsMatch(Input, InputPattern))
                        {
                            result = VisualRegexTooltip;
                        }
                    }
                }

                Error = result;
                OnPropertyChanged(nameof(Error));

                return result;
            }
        }

        public string Error { get; private set; }
        #endregion

        #region Constructors
        public InputElement(Type inputType, string tempTxt = "", string inputPattern = "", string visualRegexTooltip = "")
        {
            TemporaryText = tempTxt;
            InputType = inputType;
            InputPattern = inputPattern;
            VisualRegexTooltip = visualRegexTooltip;
        }

        public InputElement()
        {
            InputType = typeof(string);
        }
        #endregion
    }

    class SelectionElement : DefaultElementData
    {
        public RelayCommand SelectCommand { get; set; }

        public object SelectedCategory { get; private set; }

        public ObservableCollection<object> Categories { get; set; }

        public SelectionElement(List<object> categories)
        {
            Categories = new ObservableCollection<object>(categories);

            SelectCommand = new RelayCommand(o =>
            {
                //Tries to find category in list with given parameter
                SelectCategory(Categories.IndexOf(Categories.FirstOrDefault(x => x == o)));
            });

            SelectCategory(Categories.Count - 1);
        }

        public void SelectCategory(int index)
        {
            if (index >= 0 && Categories.Count > index)
            {
                SelectedCategory = Categories[index];
            }
        }
    }

    class ImageElement : DefaultElementData
    {
        private string _imagePath = "";
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SelectFileCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ImageElement()
        {
            ItemExtraSize = 1;

            SelectFileCommand = new RelayCommand(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.DefaultExt = ".png";
                openFileDialog.InitialDirectory = @"C:\Users\Leon\source\repos\KaboDev\UITest\UITest\Images\Icons\";
                openFileDialog.Filter = "PNG Files *.png|*.png|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";

                if (openFileDialog.ShowDialog() == true)
                {
                    ImagePath = openFileDialog.FileName;
                }
            });
            PasteCommand = new RelayCommand(async o =>
            {
                //Make sure client is connected to a network
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    await TryDownloadFromURL();
                }
            });
            ClearCommand = new RelayCommand(o =>
            {
                //The file should be deleted right here,
                //but somehow the deletion with File.Delete gives an Error about another process using this file
                //Probably Windows Defender scanning problem - ChatGPT/Google answer

                ImagePath = null;
            },
            o =>
            {
                return !string.IsNullOrEmpty(ImagePath);
            });
        }

        private async Task TryDownloadFromURL()
        {
            // Get the URL from the clipboard
            string url = Clipboard.GetText();

            // Check if the URL is a valid image URL
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                string fileName = url.Substring(url.LastIndexOf("/"));
                string downloadURL = url.Substring(0, url.LastIndexOf('/')) + "/download/png/512";

                // Download and save the image
                await DownloadImageAsync(downloadURL, fileName);
            }
        }

        private async Task DownloadImageAsync(string url, string fileName)
        {
            using (var client = new WebClient())
            {
                try
                {
                    byte[] imageData = await client.DownloadDataTaskAsync(url);
                    string savePath = $@"C:\Users\Leon\source\repos\KaboDev\UITest\UITest\Images\Icons\{fileName}.png"; // You can customize the file name and extension as needed

                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        using (Image image = Image.FromStream(memoryStream))
                        {
                            using (Bitmap bitmap = new Bitmap(image))
                            {
                                // Iterate through each pixel of the bitmap and set it to white (because black on dark colors = ugly)
                                for (int x = 0; x < bitmap.Width; x++)
                                {
                                    for (int y = 0; y < bitmap.Height; y++)
                                    {
                                        Color pixelColor = bitmap.GetPixel(x, y);
                                        Color newColor = pixelColor.ToArgb() == Color.Black.ToArgb() ? Color.White : pixelColor.ToArgb() == Color.White.ToArgb() ? Color.Black : pixelColor; //Inverts black and white colors of the image
                                        bitmap.SetPixel(x, y, newColor);
                                    }
                                }

                                // Save the modified bitmap to the specified file path
                                bitmap.Save(savePath, ImageFormat.Png);
                            }
                        }
                    }

                    ImagePath = savePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred while processing the image: " + ex, "Error");
                }
            }
        }
    }

    class DropdownElement : DefaultElementData
    {
        public Product CurrentProduct { get; set; }

        private ObservableCollection<Product> _availableProducts;
        public ObservableCollection<Product> AvailableProducts
        {
            get
            {
                return _availableProducts;
            }
            set
            {
                _availableProducts = value;
                OnPropertyChanged();
            }
        }

        public DropdownElement()
        {
            AvailableProducts = new ObservableCollection<Product>();
            AvailableProducts = ProductViewModel.Products;
        }
    }

    class DefaultElementData : ObservableObject
    {
        public int ItemExtraSize { get; protected set; } = 0;
    }
}
