using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITest.Core;
using UITest.View;

namespace UITest.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private ProductViewModel _productVM;
        private StorageViewModel _storageVM;

        public RelayCommand ProductViewCommand { get; }
        public RelayCommand StorageViewCommand { get; }

        public MainWindowViewModel()
        {
            _productVM = new ProductViewModel();
            _storageVM = new StorageViewModel();

            ProductViewCommand = new RelayCommand(o => {
                CurrentView = _productVM;
            });
            StorageViewCommand = new RelayCommand(o =>
            {
                CurrentView = _storageVM;
            });

            CurrentView = _productVM;
        }
    }
}
