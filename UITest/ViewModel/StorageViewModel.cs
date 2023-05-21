using System.Collections.ObjectModel;
using System.Linq;
using UITest.Core;
using UITest.Model;

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

        private StoragePlaceProduct _currentStoragePlaceProduct;
        public StoragePlaceProduct CurrentStoragePlaceProduct
        {
            get { return _currentStoragePlaceProduct; }
            set
            {
                _currentStoragePlaceProduct = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand DeleteStorageCommand { get; }
        public RelayCommand SelectCommand { get; }
        #endregion

        #region Lists
        public ObservableCollection<Storage> Storages { get; private set; } = new ObservableCollection<Storage>();
        public ObservableCollection<StoragePlace> CurrentStoragePlaces { get; private set; } = new ObservableCollection<StoragePlace>();
        public ObservableCollection<StoragePlaceProduct> CurrentStoragePlaceProducts { get; private set; } = new ObservableCollection<StoragePlaceProduct>();
        #endregion

        public StorageViewModel()
        {
            DeleteStorageCommand = new RelayCommand(o =>
            {
                Storages.Remove(o as Storage);
            });
            SelectCommand = new RelayCommand(o =>
            {
                //Checks if Storage is selected
                if (o is Storage)
                {
                    if(_currentStorage != o)
                    {
                        CurrentStoragePlaces.Clear();
                        CurrentStoragePlaceProducts.Clear();
                        //Added in the foreach so the property raises change notifications -> needed (e.g. converter)
                        (o as Storage).StoragePlaces.ForEach(x => CurrentStoragePlaces.Add(x));
                    }
                }

                //Checks if StoragePlace is selected
                if (o is StoragePlace)
                {
                    if (_currentStoragePlace != o)
                    {
                        CurrentStoragePlaceProducts.Clear();
                        //Added in the foreach so the property raises change notifications -> needed (e.g. converter)
                        (o as StoragePlace).StoragePlaceProducts.ForEach(x => CurrentStoragePlaceProducts.Add(x));
                    }
                }
            });

            Storage mainStorage = new Storage("Hauptlager", "Germany", 1000.0f);

            StoragePlace shelf = new StoragePlace("Shelf", "A1B3");
            StoragePlace chest = new StoragePlace("Chest", "A2B2");

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
    }
}
