using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Data.Repositories;
using UsersLibrary.Services;

namespace PasswordManager.MainWindow.ViewModels
{
    public class AuthenticationDataVM : BaseModel
    {
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
        public AuthenticationDataVM(ServiceRepository serviceRepository)
        {  
            AuthenticationDataCollectionView = CollectionViewSource.GetDefaultView(serviceRepository.GetAll());
            AuthenticationDataCollectionView.Filter = FilterData;
            AuthenticationDataCollectionView.SortDescriptions.Add(
                new SortDescription(nameof(Service.Name), ListSortDirection.Ascending));
        }

        private bool FilterData(object obj)
        {
            if (obj is Service authenticationData)
                return authenticationData.Name.Contains(DataFilter.ToLower());

            return false;
        }
    }
}
