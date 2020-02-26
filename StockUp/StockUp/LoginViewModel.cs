using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StockUp
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        string storeID = string.Empty;
        public string StoreID
        {
            get => storeID;
            set
            {
                if (storeID == value)
                {
                    return;
                }
                else
                {
                    storeID = value;
                    OnPropertyChanged(nameof(StoreID));
                    OnPropertyChanged(nameof(DisplayStoreID));
                }
            }
        }

        // updates value automatically
        public string DisplayStoreID => $"StoreID Entered: {StoreID}";

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string storeID)
        {
            // used whenever we want to notify the User Interface that one of the properties has changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(storeID));
        }
    }
}
