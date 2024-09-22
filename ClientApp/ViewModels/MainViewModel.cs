using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using ClientApp.Commands;
using ClientApp.Contracts;
using ClientApp.Models;
using ClientApp.ServiceClient;

namespace ClientApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Medicine _selectedItem = new Medicine { Id = -1 };
        private RelayCommand _addCommand;
        private RelayCommand _updateCommand;
        private RelayCommand _deleteCommand;
        private readonly MedicineClient _medicineClient;

        public ObservableCollection<Medicine> Medicines { get; set; } = new ObservableCollection<Medicine>();

        public MainViewModel(MedicineClient medicineClient)
        {
            _medicineClient = medicineClient;
        }

        public async Task OnLoad()
        {
            try
            {
                var loadedMedicines = await _medicineClient.GetAllMedicinesAsync();
                foreach (var loadedMedicine in loadedMedicines)
                {
                    Medicines.Add(new Medicine
                    {
                        Id = loadedMedicine.Id,
                        Description = loadedMedicine.Description,
                        Name = loadedMedicine.Name,
                        Price = loadedMedicine.Price
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }
        }

        public Medicine SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null)
                {
                    _selectedItem = Medicines.First();
                    OnPropertyChanged();
                    return;
                }
                
                _selectedItem = new Medicine
                {
                    Description = value.Description,
                    Name = value.Name,
                    Price = value.Price,
                    Id = value.Id
                };
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RelayCommand(async obj =>
                {
                    long id = 0;
                    try
                    {
                        var newMedicine = await _medicineClient.AddMedicineAsync(new MedicineRequest
                        {
                            Description = _selectedItem.Description,
                            Name = _selectedItem.Name,
                            Price = _selectedItem.Price
                        });
                        id = newMedicine.Id;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                        return;
                    }
                    
                    var medicine = new Medicine
                    {
                        Id = id,
                        Description = _selectedItem.Description,
                        Name = _selectedItem.Name,
                        Price = _selectedItem.Price
                    };
                    Medicines.Add(medicine);
                    _selectedItem = medicine;
                });
            }
        }

        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand ??= new RelayCommand(async obj =>
                {
                    var item = Medicines.FirstOrDefault(x => x.Id == _selectedItem.Id);
                    
                    if (item == null)
                        return;
                    
                    try
                    {
                        await _medicineClient.UpdateMedicineAsync(item.Id, new MedicineRequest
                        {
                            Description = _selectedItem.Description,
                            Name = _selectedItem.Name,
                            Price = _selectedItem.Price
                        });
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                        return;
                    }
                    
                    item.Price = _selectedItem.Price;
                    item.Description = _selectedItem.Description;
                    item.Name = _selectedItem.Name;
                },
                (obj) => _selectedItem?.Id != null && _selectedItem.Id > 0);
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(async obj =>
                    {
                        if (!(obj is Medicine medicine)) return;
                            
                        var item = Medicines.FirstOrDefault(x => x.Id == medicine.Id);
                            
                        if (item == null)
                            return;
                        
                        try
                        {
                            await _medicineClient.DeleteMedicineAsync(item.Id);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                            return;
                        }

                        Medicines.Remove(item);
                    },
                    (obj) => Medicines.Count > 0);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}