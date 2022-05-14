using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PasswordManager.MainWindow.Models;

namespace PasswordManager.MainWindow.ViewModels
{
    public class AuthenticationDataVM : BaseModel
    {
        public ObservableCollection<AuthenticationData> AuthenticationDataViewModels;

        public ICollectionView AuthenticationDataCollectionView { get; }
        private string dataFilter = string.Empty;

        public string DataFilter
        {
            get => dataFilter;

            set
            {
                dataFilter = value;
                OnPropertyChanged(nameof(DataFilter));
                AuthenticationDataCollectionView.Refresh();
            }
        }
        public AuthenticationDataVM()
        {

            AuthenticationDataViewModels = new ObservableCollection<AuthenticationData>
            {
                new AuthenticationData("discord.com", "ggd","vfvfg"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("steam.com", "asf@gnauk.com", "1234567899"),
               
                new AuthenticationData("robinhood.com", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("binance.com", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("facebook.com", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("twitter.com", "asf@gnauk.com", "1234567899"),

            };
            AuthenticationDataCollectionView = CollectionViewSource.GetDefaultView(AuthenticationDataViewModels);
            AuthenticationDataCollectionView.Filter = FilterData;
            AuthenticationDataCollectionView.SortDescriptions.Add(new SortDescription(nameof(AuthenticationData.Resource), ListSortDirection.Ascending));


        }

        private bool FilterData(object obj)
        {
            if (obj is AuthenticationData authenticationData)
            {
                return authenticationData.Resource.Contains(DataFilter.ToLower());
            }
            return false;
        }
    }
}
