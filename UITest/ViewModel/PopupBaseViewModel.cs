using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITest.Core;

namespace UITest.ViewModel
{
    class PopupBaseViewModel : ObservableObject
    {
        #region FullProps
        private double _popupWidth;
        public double PopupWidth
        {
            get { return _popupWidth; }
            set
            {
                _popupWidth = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region UIProperties
        public ObservableCollection<Element> FirstItems { get; }
        public ObservableCollection<Element> SecondItems { get; }
        public string Title { get; }
        #endregion

        public RelayCommand AddCommand { get; }

        public event Action<List<Element>> PopupClosed;

        private List<Element> Elements => new List<Element>().Concat(FirstItems).Concat(SecondItems).ToList(); //Total amount of Items

        public PopupBaseViewModel(CustomPopup popupInfo)
        {
            AddCommand = new RelayCommand(o =>
            {
                int errorAmount = 0;
                foreach (var element in Elements)
                {
                    if (element.CurrentElement.GetType() == typeof(InputElement))
                    {
                        if ((element.CurrentElement as InputElement).Error != null)
                        {
                            errorAmount++;
                        }
                    }
                }

                if (errorAmount <= 0)
                {
                    PopupClosed?.Invoke(Elements);
                }
            });

            PopupWidth = 272.5f;

            Title = popupInfo.Title;

            int mid = (popupInfo.Elements.Count + 1) / 2;
            FirstItems = new ObservableCollection<Element>(popupInfo.Elements.Take(mid));
            SecondItems = new ObservableCollection<Element>(popupInfo.Elements.Skip(mid));
        }
    }
}
