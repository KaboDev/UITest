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
        public ObservableCollection<StoragePlace> CurrentStoragePlaces { get; private set; } = new ObservableCollection<StoragePlace>();
        public ObservableCollection<StoragePlaceProduct> CurrentStoragePlaceProducts { get; private set; } = new ObservableCollection<StoragePlaceProduct>();

        public ObservableCollection<PopupBaseViewModel> StoragePopups { get; private set; } = new ObservableCollection<PopupBaseViewModel>();
        public ObservableCollection<PopupBaseViewModel> StoragePlacePopups { get; private set; } = new ObservableCollection<PopupBaseViewModel>();
        public ObservableCollection<PopupBaseViewModel> StoragePlaceProductPopups { get; private set; } = new ObservableCollection<PopupBaseViewModel>();
        #endregion

        public StorageViewModel()
        {
            DeleteCommand = new RelayCommand(o =>
            {
                if (o is Storage storage)
                {
                    if (CurrentStorage == storage)
                    {
                        CurrentStoragePlaceProducts.Clear();
                        CurrentStoragePlaces.Clear();
                    }
                    Storages.Remove(storage);
                }

                if (o is StoragePlace storagePlace)
                {
                    if (CurrentStoragePlace == storagePlace)
                    {
                        CurrentStoragePlaceProducts.Clear();
                    }
                    CurrentStoragePlaces.Remove(storagePlace);
                }

                if (o is StoragePlaceProduct storagePlaceProduct)
                {
                    CurrentStoragePlaceProducts.Remove(storagePlaceProduct);
                }
            });
            SelectCommand = new RelayCommand(o =>
            {
                //Checks if Storage is selected
                if (o is Storage storage)
                {
                    if (_currentStorage != storage)
                    {
                        CurrentStorage = storage;
                        CurrentStoragePlaces.Clear();
                        CurrentStoragePlaceProducts.Clear();
                        //Added through the foreach (instead of setting directly) so the property raises change notifications -> needed (e.g. converter)
                        (o as Storage).StoragePlaces.ForEach(x => CurrentStoragePlaces.Add(x));
                    }
                }

                //Checks if StoragePlace is selected
                if (o is StoragePlace storagePlace)
                {
                    if (_currentStoragePlace != storagePlace)
                    {
                        CurrentStoragePlace = storagePlace;
                        CurrentStoragePlaceProducts.Clear();
                        //Added through the foreach (instead of setting directly) so the property raises change notifications -> needed (e.g. converter)
                        (o as StoragePlace).StoragePlaceProducts.ForEach(x => CurrentStoragePlaceProducts.Add(x));
                    }
                }
            });
            AddCommand = new RelayCommand(o =>
            {
                Type classType = o as Type;

                if (classType != null)
                {
                    CheckClassType(classType, typeof(Storage), () =>
                    {
                        popupList = new ObservableCollection(StoragePopups;
                    });

                    CheckClassType(classType, typeof(StoragePlace), () =>
                    {
                        popupList = StoragePlacePopups;
                    });

                    CheckClassType(classType, typeof(StoragePlaceProduct), () =>
                    {
                        popupList = StoragePlaceProductPopups;
                    });
                }
                CreatePopup(classType, ref popupList);
            });

            Storage mainStorage = new Storage("Hauptlager", "Germany", 1000.0f);

            StoragePlace shelf = new StoragePlace("Shelf", "A1B3", 10000, 3, @"D:\Leon\source\repos\UITest\UITest\Images\Icons\Shelf.png");
            StoragePlace chest = new StoragePlace("Drawer", "A2B2", 20, 1, @"D:\Leon\source\repos\UITest\UITest\Images\Icons\Drawer.png");

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
            Storages.Add(new Storage("Nebenlager 1", "USA", 2000f));
            Storages.Add(new Storage("Nebenlager 2", "USA", 2000f));
            Storages.Add(new Storage("Nebenlager 3", "USA", 2000f));
            Storages.Add(new Storage("Nebenlager 4", "USA", 2000f));
            Storages.Add(new Storage("Nebenlager 5", "USA", 2000f));
        }

        private void CreatePopup(Type classType, ref ObservableCollection<object> popupList)
        {
            CustomPopup popup = CreatePopupData(classType);
            PopupBaseViewModel popupVM = new PopupBaseViewModel(popup);

            Action<List<Element>> PopupAction = null;
            PopupAction = (list) =>
            {
                AddNewItem(list, classType, popupVM);
                popupVM.PopupClosed -= PopupAction;
            };
            popupVM.PopupClosed += PopupAction;

            popupList.Add(popupVM);
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
                    new Element("Label", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Location", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Size", new InputElement(typeof(float),@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX m²")),
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
                    new Element("Label", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Location", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Size", new InputElement(typeof(float),@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX m²")),
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
                    new Element("Label", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Location", new InputElement(typeof(string),@"^[a-zA-Z]+$", "Input can only contain letters")),
                    new Element("Size", new InputElement(typeof(float),@"^(?!.*[.,].*[.,])[^\n]*$", "X.XX m²")),
                    }
                };
            });

            return popup;
        }

        private void AddNewItem(List<Element> items, Type classType, PopupBaseViewModel popupVM)
        {
            CheckClassType(classType, typeof(Storage), () =>
            {
                Storage storage = new Storage
                (
                    CustomExtensions.SearchElement<string>(items, "Label", Elements.Input, "Empty"),
                    CustomExtensions.SearchElement<string>(items, "Location", Elements.Input, "Unknown"),
                    CustomExtensions.SearchElement<float>(items, "Size")
                );

                Storages.Add(storage);
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

                CurrentStoragePlaces.Add(storagePlace);
            });

            CheckClassType(classType, typeof(StoragePlace), () =>
            {
                StoragePlaceProduct storagePlaceProduct = new StoragePlaceProduct
                (
                    CustomExtensions.SearchElement<Product>(items, "Product", Elements.Dropdown),
                    CustomExtensions.SearchElement<int>(items, "Amount")
                );

                CurrentStoragePlaceProducts.Add(storagePlaceProduct);
            });

            if (popupVM != null)
            {
                StoragePopups.Remove(popupVM);
            }
        }

        private void CheckClassType<T>(T classType, Type targetType, Action code) where T : Type
        {
            if (classType == targetType)
            {
                code();
            }
        }
    }
}
