﻿using DeleeRefreshMonkey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeleeRefreshMonkey.Services;

namespace DeleeRefreshMonkey.ViewModels
{
    public class MonkeyViewModel:ViewModelBase
    {
        private ObservableCollection<Monkey> monkeys;
        public ObservableCollection<Monkey> Monkeys
        {
            get
            {
                return this.monkeys;
            }
            set
            {
                this.monkeys = value;
                OnPropertyChanged();
            }
        }
        public MonkeyViewModel()
        {
            monkeys = new ObservableCollection<Monkey>();
            IsRefreshing = false;
            ReadMonkeys();
        }

        private async void ReadMonkeys()
        {
            MonkeyService service = new MonkeyService();
            List<Monkey> list = await service.GetMonkeys();
            this.Monkeys = new ObservableCollection<Monkey>(list);
        }


        private Object selectedMonkey;
        public Object SelectedMonkey
        {
            get
            {
                return this.selectedMonkey;
            }
            set
            {
                this.selectedMonkey = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteCommand => new Command<Monkey>(RemoveMonkey);
        void RemoveMonkey(Monkey monkey)
        {
            if (Monkeys.Contains(monkey))
            {
                Monkeys.Remove(monkey);
            }
        }
        private ObservableCollection<Location> locations;
        public ObservableCollection<Location> Locations
        {
            get
            {
                return this.locations;
            }
            set
            {
                this.locations = value;
                OnPropertyChanged();
            }
        }

        private Location selectedLocation;
        public Location SelectedLocation
        {
            get
            {
                return this.selectedLocation;
            }
            set
            {
                this.selectedLocation = value;
                OnPickerChanged();
                OnPropertyChanged();
            }
        }


        private void OnPickerChanged()
        {
            ReadMonkeys();
            if (SelectedLocation != null)
            {
                List<Monkey> tobeRemoved = Monkeys.Where(s => s.Location != SelectedLocation.LocationName).ToList();
                foreach (Monkey monkey in tobeRemoved)
                {
                    Monkeys.Remove(monkey);
                }
            }


        }

        private void FillMonths()
        {
            
        }


        #region Refresh View
        public ICommand RefreshCommand => new Command(Refresh);
        private async void Refresh()
        {

            ReadMonkeys();

            IsRefreshing = false;
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }
            set
            {
                this.isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand SingleSelectCommand => new Command(OnSingleSelectMonkey);
        async void OnSingleSelectMonkey()
        {
            if (SelectedMonkey != null)
            {
                var navParam = new Dictionary<string, object>()
            {
                { "selectedMonkey",SelectedMonkey}
            };
                await Shell.Current.GoToAsync($"monkeyDetails", navParam);

                SelectedMonkey = null;
            }
        }
        #endregion
    }
}
