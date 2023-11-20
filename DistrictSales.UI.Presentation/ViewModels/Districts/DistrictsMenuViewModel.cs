using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictsMenuViewModel : Conductor<Screen>.Collection.OneActive
{
    private readonly IWindowManager _windowManager;
    private readonly IDistrictSalesApi _districtSalesApi;

    public DistrictsMenuViewModel(IWindowManager windowManager)
    {
        _windowManager = windowManager;

        _districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();
        _districts =
            new ObservableCollection<District>(
                _districtSalesApi
                    .GetDistrictsAsync()
                    .Result.Content?
                    .Select(district => district.MapToDistrict())
                    .ToList() ??
                new List<District>()
            );
    }

    private ObservableCollection<District> _districts;

    public ObservableCollection<District> Districts
    {
        get { return _districts; }
        set
        {
            _districts = value;
            NotifyOfPropertyChange(() => Districts);
        }
    }

    private District? _selectedDistrict;

    public District? SelectedDistrict
    {
        get => _selectedDistrict;
        set
        {
            _selectedDistrict = value;
            NotifyOfPropertyChange(() => SelectedDistrict);
        }
    }

    public void ReadDetailsCommand()
    {
        if (SelectedDistrict is null)
            return;

        var detailsViewModel = new DistrictDetailsViewModel(SelectedDistrict.Id);
        _windowManager.ShowWindow(detailsViewModel);
    }

    public void CreateCommand()
    {
        var createViewModel = new DistrictCreateViewModel();
        _windowManager.ShowWindow(createViewModel);
    }

    public void UpdateCommand()
    {
        if (SelectedDistrict is null)
            return;

        var updateViewModel = new DistrictUpdateViewModel(SelectedDistrict.Id);
        _windowManager.ShowWindow(updateViewModel);
    }

    public void DeleteCommand()
    {
        if (SelectedDistrict is null)
            return;

        MessageBoxResult result = MessageBox
            .Show(
                $"Do you want to delete district {SelectedDistrict.Name}?",
                "Delete District",
                MessageBoxButton.YesNoCancel
            );

        if (result is not MessageBoxResult.Yes)
            return;

        IApiResponse apiResponse = _districtSalesApi.DeleteDistrictAsync(SelectedDistrict.Id).Result;

        if (!apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Failed to delete district. Did you delete all secondary salespeople for this district?",
                "Delete District",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }

        Districts.Remove(SelectedDistrict);
    }

    public void AddSecondarySalespersonCommand()
    {
        if (SelectedDistrict == null)
            return;

        var addSecondarySalespersonViewModel = new DistrictAddSecondarySalespersonViewModel(SelectedDistrict.Id);
        _windowManager.ShowWindow(addSecondarySalespersonViewModel);
    }

    public void RemoveSecondarySalespersonCommand()
    {
        if (SelectedDistrict == null)
            return;

        var removeSecondarySalespersonViewModel = new DistrictRemoveSecondarySalespersonViewModel(SelectedDistrict.Id);
        _windowManager.ShowWindow(removeSecondarySalespersonViewModel);
    }
}
