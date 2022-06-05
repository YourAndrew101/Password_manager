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
    public class ServiceVM : BaseModel
    {
        public ICollectionView ServiceCollectionView { get; }
        private string dataFilter = string.Empty;

        public string DataFilter
        {
            get => dataFilter;
            set
            {
                dataFilter = value;
                OnPropertyChanged(nameof(DataFilter));
                ServiceCollectionView.Refresh();
            }
        }
        public ServiceVM(ServiceRepository serviceRepository)
        {  
            ServiceCollectionView = CollectionViewSource.GetDefaultView(serviceRepository.GetAll());
            ServiceCollectionView.Filter = FilterData;
            ServiceCollectionView.SortDescriptions.Add(new SortDescription(nameof(Service.Name), ListSortDirection.Ascending));
        }

        private bool FilterData(object obj)
        {
            if (obj is Service service)
                return service.Name.Contains(DataFilter.ToLower());

            return false;
        }
    }
}
