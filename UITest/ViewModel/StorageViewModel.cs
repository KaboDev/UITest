using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using UITest.Core;
using UITest.Model;
using System.Windows.Media;

namespace UITest.ViewModel
{
    class StorageViewModel : ObservableObject
    {
        #region FullProps
        private Storage _currentStorage;
        public Storage CurrentStorage
        {
            get { return _currentStorage; }
            set
            {
                _currentStorage = value;
                OnPropertyChanged();
            }
        }

        private StoragePlace _currentStoragePlace;
        public StoragePlace CurrentStoragePlace
        {
            get { return _currentStoragePlace; }
            set
            {
                _currentStoragePlace = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand DeleteCommand { get; }
        public RelayCommand SelectCommand { get; }
        public RelayCommand AddCommand { get; }
        #endregion

        #region Lists
        public ObservableCollection<Storage> Storages { get; private set; } = new ObservableCollection<Storage>();

        public ObservableCollection<PopupBaseViewModel> StoragePopups { get; private set; } = new ObservableCollection<PopupBaseViewModel>();
        public ObservableCollection<PopupBaseViewModel> StoragePlacePopups { get; private set; } = new ObservableCollection<PopupBaseViewModel>();
        public ObservableCollection<PopupBaseViewModel> StoragePlaceProductPopups { get; private set; } = new ObservableCollection<PopupBaseViewModel>();
        #endregion

        public StorageViewModel()
        {
            DeleteCommand = new RelayCommand(o =>
            {
                Type classType = o.GetType();

                CheckClassType(classType, typeof(Storage), () =>
                {
                    ClearSelection();

                    CurrentStorage = null;
                    Storages.Remove(o as Storage);
                });
                CheckClassType(classType, typeof(StoragePlace), () =>
                {
                    ClearSelection();

                    CurrentStorage.StoragePlaces.Remove(o as StoragePlace);
                });
                CheckClassType(classType, typeof(StoragePlaceProduct), () =>
                {
                    CurrentStoragePlace.StoragePlaceProducts.Remove(o as StoragePlaceProduct);
                });
            });
            SelectCommand = new RelayCommand(o =>
            {
                Type classType = o.GetType();

                CheckClassType(classType, typeof(Storage), () =>
                {
                    if (o != CurrentStorage)
                    {
                        ClearSelection();
                        CurrentStorage = o as Storage;
                    }
                });
                CheckClassType(classType, typeof(StoragePlace), () =>
                {
                    if (o != CurrentStoragePlace)
                    {
                        ClearSelection();
                        CurrentStoragePlace = o as StoragePlace;
                    }
                });
            });
            AddCommand = new RelayCommand(o =>
            {
                Type classType = o as Type;
                ObservableCollection<object> popupList = new ObservableCollection<object>();

                if (classType != null)
                {
                    CheckClassType(classType, typeof(Storage), () =>
                    {
                        StoragePopups.Add(CreatePopup(classType, ref popupList));
                    });

                    CheckClassType(classType, typeof(StoragePlace), () =>
                    {
                        StoragePlacePopups.Add(CreatePopup(classType, ref popupList));
                    });

                    CheckClassType(classType, typeof(StoragePlaceProduct), () =>
                    {
                        StoragePlaceProductPopups.Add(CreatePopup(classType, ref popupList));
                    });
                }
            });

            Storage mainStorage = new Storage("Hauptlager", "Germany", 200);

            StoragePlace shelf = new StoragePlace("Shelf", "A1B3", 50, 3, @"C:\Users\Leon\source\repos\KaboDev\UITest\UITest\Images\Icons\Shelf.png");
            StoragePlace chest = new StoragePlace("Drawer", "A2B2", 20, 1, @"C:\Users\Leon\source\repos\KaboDev\UITest\UITest\Images\Icons\Drawer.png");

            StoragePlaceProduct apple = new StoragePlaceProduct(ProductViewModel.Products.FirstOrDefault(x => x.Name == "Apple"), 2);
            StoragePlaceProduct pear = new StoragePlaceProduct(ProductViewModel.Products.FirstOrDefault(x => x.Name == "Pear"), 4);
            StoragePlaceProduct strawberry = new StoragePlaceProduct(ProductViewModel.Products.FirstOrDefault(x => x.Name == "Strawberry"), 7);

            shelf.AddStoragePlaceProduct(apple);
            shelf.AddStoragePlaceProduct(pear);

            chest.AddStoragePlaceProduct(strawberry);
            chest.AddStoragePlaceProduct(apple);

            mainStorage.AddStoragePlace(shelf);
            mainStorage.AddStoragePlace(chest);

            Storages.Add(mainStorage);
        }

        private PopupBaseViewModel CreatePopup(Type classType, ref ObservableCollection<object> popupList)
        {
            CustomPopup popup = CreatePopupData(classType);
            PopupBaseViewModel popupVM = new PopupBaseViewModel(popup);

            Action<List<Element>> PopupAction = null;
            PopupAction = (list) =>
            {
                if (AddNewItem(list, classType, popupVM))
                {
                    popupVM.PopupClosed -= PopupAction;
                }
            };
            popupVM.PopupClosed += PopupAction;

            return popupVM;
        }

        private CustomPopup CreatePopupData(Type classType)
        {
            CustomPopup popup = null;

            CheckClassType(classType, typeof(Storage), () =>
            {
                popup = new CustomPopup()
                {
                    Title = "Storage",
                    Elements = new List<Element>()
                    {
                    new Element("Label", new InputElement(typeof(string), "Name...",@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Location", new InputElement(typeof(string),"Location...",@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Size", new InputElement(typeof(float), "Size in m²",@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX m²")),
                    }
                };
            });
            CheckClassType(classType, typeof(StoragePlace), () =>
            {
                popup = new CustomPopup()
                {
                    Title = "Storage Place",
                    Elements = new List<Element>()
                    {
                    new Element("Label", new InputElement(typeof(string), "Name...",@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Location", new InputElement(typeof(string),"Location...")),
                    new Element("Size", new InputElement(typeof(float),"Size in m³",@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX m³")),
                    new Element("Weight", new InputElement(typeof(float),"Weight in kg",@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX kg")),
                    new Element("Image", new ImageElement()),
                    }
                };
            });
            CheckClassType(classType, typeof(StoragePlaceProduct), () =>
            {
                popup = new CustomPopup()
                {
                    Title = "Product",
                    Elements = new List<Element>()
                    {
                    new Element("Product", new DropdownElement()),
                    new Element("Amount", new InputElement(typeof(int), "1...",@"^[1-9]\d*$", "Input has to be a number greater than 0")),
                    }
                };
            });

            return popup;
        }

        private bool AddNewItem(List<Element> items, Type classType, PopupBaseViewModel popupVM)
        {
            bool validDeletion = true;

            CheckClassType(classType, typeof(Storage), () =>
            {
                Storage storage = new Storage
                (
                    CustomExtensions.SearchElement<string>(items, "Label", Elements.Input, "Empty"),
                    CustomExtensions.SearchElement<string>(items, "Location", Elements.Input, "Unknown"),
                    CustomExtensions.SearchElement<float>(items, "Size")
                );

                Storages.Add(storage);
                StoragePopups.Remove(popupVM);
            });

            CheckClassType(classType, typeof(StoragePlace), () =>
            {
                StoragePlace storagePlace = new StoragePlace
                (
                    CustomExtensions.SearchElement<string>(items, "Label", Elements.Input, "Empty"),
                    CustomExtensions.SearchElement<string>(items, "Location", Elements.Input, "Unknown"),
                    CustomExtensions.SearchElement<float>(items, "Weight"),
                    CustomExtensions.SearchElement<float>(items, "Size"),
                    CustomExtensions.SearchElement<string>(items, "Image", Elements.Image)
                );

                CurrentStorage.AddStoragePlace(storagePlace);
                StoragePlacePopups.Remove(popupVM);
            });

            CheckClassType(classType, typeof(StoragePlaceProduct), () =>
            {
                StoragePlaceProduct storagePlaceProduct = new StoragePlaceProduct
                (
                    CustomExtensions.SearchElement<Product>(items, "Product", Elements.Dropdown),
                    CustomExtensions.SearchElement<int>(items, "Amount")
                );

                if (storagePlaceProduct.Product != null && CurrentStoragePlace.AddStoragePlaceProduct(storagePlaceProduct))
                {
                    StoragePlaceProductPopups.Remove(popupVM);
                }
                else
                {
                    validDeletion = false;
                }
            });

            return validDeletion;
        }

        private void CheckClassType<T>(T classType, Type targetType, Action code) where T : Type
        {
            if (classType == targetType)
            {
                code();
            }
        }

        private void ClearSelection()
        {
            CurrentStoragePlace = null;

            StoragePopups.Clear();
            StoragePlacePopups.Clear();
            StoragePlaceProductPopups.Clear();
        }
    }
}
