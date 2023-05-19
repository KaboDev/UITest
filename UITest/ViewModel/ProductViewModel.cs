using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using UITest.Controls;
using UITest.Core;
using UITest.Model;

namespace UITest.ViewModel
{
    class ProductViewModel
    {
        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<PopupBaseViewModel> Popups { get; }

        public RelayCommand AddPopupCommand { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand ValidationCommand { get; }

        public ProductViewModel()
        {
            AddPopupCommand = new RelayCommand(o =>
            {
                CreatePopup();
            },
            o =>
            {
                return Popups.Count < 4;
            });

            DeleteProductCommand = new RelayCommand(o =>
            {
                Products.Remove(o as Product);
            });

            ValidationCommand = new RelayCommand(o =>
            {
            });

            SolidColorBrush previewCategory = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D8AA"));

            Products = new ObservableCollection<Product>()
            {
                new Product("Banana", 0.1f, 0.4f,5, @"D:\Leon\source\repos\UITest\UITest\Images\Icons\Banana.png", "South Africa", previewCategory),
                new Product("Apple", 0.2f, 0.2f,6, @"D:\Leon\source\repos\UITest\UITest\Images\Icons\Apple.png", "Germany", previewCategory),
                new Product("Pear", 0.25f, 0.5f,3, @"D:\Leon\source\repos\UITest\UITest\Images\Icons\Pear.png", "Russia", previewCategory),
                new Product("Strawberry", 0.25f, 0.5f,3, @"D:\Leon\source\repos\UITest\UITest\Images\Icons\Strawberry.png", "Germany", previewCategory),
            };

            Popups = new ObservableCollection<PopupBaseViewModel>();
        }

        private void CreatePopup()
        {
            CustomPopup popup = new CustomPopup()
            {
                Title = "Product",
                Elements = new List<Element>()
                {
                    new Element("Label", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Price", new InputElement(typeof(float),@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX €")),
                    new Element("Weight", new InputElement(typeof(float), @"^(?!.*[.,].*[.,])[^\n]*$", "X.XX kg")),
                    new Element("Size", new InputElement(typeof(float),@"^(?!.*[.,].*[.,])[^\n]*$","X.XX m³")),
                    new Element("Category", new SelectionElement(new List<object>()
                        {
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6969")),
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D8AA")),
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3DB2FF")),
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7D060")),
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F94892")),
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#717277"))
                        })
                    ),
                    new Element("Icon", new ImageElement()),
                    new Element("Location", new InputElement(typeof(string),@"^[a-zA-Z]+$","Input can only contain letters")),
                }
            };
            PopupBaseViewModel popupVM = new PopupBaseViewModel(popup);

            Action<List<Element>> PopupAction = null;
            PopupAction = (list) =>
            {
                AddNewProduct(list, Popups.IndexOf(popupVM));

                popupVM.PopupClosed -= PopupAction;
            };
            popupVM.PopupClosed += PopupAction;

            Popups.Add(popupVM);
        }

        private void AddNewProduct(List<Element> items, int index)
        {
            //Product gets created with the correct Elements in the items list with error-proof dynamic parsing
            Product newProduct = new Product(
                SearchElement<string>(items, "Label", Elements.Input, "Empty"),
                SearchElement<float>(items, "Price"),
                SearchElement<float>(items, "Weight"),
                SearchElement<float>(items, "Size"),
                SearchElement<string>(items, "Icon", Elements.Image),
                SearchElement<string>(items, "Location", Elements.Input, "Unknown"),
                SearchElement<SolidColorBrush>(items, "Category", Elements.Selection)
                );

            Products.Add(newProduct);
            ClosePopup(index);
        }

        private void ClosePopup(int index)
        {
            Popups.RemoveAt(index);
        }

        //Returns path to correct variable of any element
        private T SearchElement<T>(List<Element> list, string name, Elements elementType = Elements.Input, T defaultValue = default(T))
        {
            object element = list.FirstOrDefault(x => x.Label == name).CurrentElement;

            T elementReturn = default(T);
            switch (elementType)
            {
                case Elements.Input:
                    string input = (element as InputElement).Input;
                    elementReturn = GetParsedValue<T>(input, defaultValue);
                    break;
                case Elements.Selection:
                    elementReturn = (T)(object)(element as SelectionElement).SelectedCategory;
                    break;
                case Elements.Image:
                    elementReturn = (T)(object)(element as ImageElement).ImagePath;
                    break;
            }

            return elementReturn;
        }

        //Returns parsed value => when parsed value is default value and custom defaultValue is passed as param => use given defaultValue
        private T GetParsedValue<T>(string input, T defaultValue = default(T))
        {
            T value = TypeParseExtension.TryParseValue<T>(input);
            //string needs extra condition, because default value is null
            return value.Equals(default(T)) ? defaultValue.Equals(default(T)) ? value : defaultValue : (value.GetType() == typeof(string) && value.Equals("") ? defaultValue : value);
        }
    }
}
