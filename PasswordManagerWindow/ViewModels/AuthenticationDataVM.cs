using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PasswordManagerWindow.Models;

namespace PasswordManagerWindow.ViewModels
{
    public class AuthenticationDataVM: BaseModel
    {
        public ObservableCollection<AuthenticationData> AuthenticationDataViewModels;

        public ICollectionView AuthenticationDataCollectionView { get; }
        private string dataFilter = string.Empty;

        public string DataFilter
        {
            get
            {
                return dataFilter;
            }

            set
            {
              
                    dataFilter = value;
                    
                    AuthenticationDataCollectionView.Refresh();

                
            }
        }
      

       

        public AuthenticationDataVM() {

            AuthenticationDataViewModels = new ObservableCollection<AuthenticationData>
            {
                new AuthenticationData("specialshit", "ggd","vfvfg"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                //new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                //new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
                //new AuthenticationData("test 23", "asf@gnauk.com", "1234567899"),
                //new AuthenticationData("tenuhbcud", "dfvfdvasf@gnaukfvdf.com", "1234567899"),
                //new AuthenticationData("vudvudvbduvb", "asf@dfvfdvfdvdfvgnauk.com", "1234567899"),
            };
            AuthenticationDataCollectionView = CollectionViewSource.GetDefaultView(AuthenticationDataViewModels);
            AuthenticationDataCollectionView.Filter = FilterData;
            AuthenticationDataCollectionView.SortDescriptions.Add(new SortDescription(nameof(AuthenticationData.Resource), ListSortDirection.Ascending));
           

        }

        private bool FilterData(object obj)
        {
            if(obj is AuthenticationData authenticationData)
            {
                return authenticationData.Resource.Contains(DataFilter.ToLower());
            }
            return false;
        }
    }
}
