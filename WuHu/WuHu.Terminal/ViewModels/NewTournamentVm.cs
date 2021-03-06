﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class NewTournamentVm : BaseVm
    {
        public IList<int> AmountVirtualization { get; } = new List<int>(Enumerable.Range(1, 100));
        private readonly Tournament _tournament;
        private int _amountMatches;
        
        public ICommand CancelCommand { get; }
        public ICommand SubmitCommand { get; }

        public NewTournamentVm(Action showMatchList, Action reloadParent, Action<string> queueMessage)
        {
            _tournament = new Tournament("", DateTime.Now);
            
            var locked = IsAuthenticated &&
                TournamentManager.LockTournament();
            if (!locked)
            {
                queueMessage?.Invoke("Spielplan wird zur Zeit bearbeitet. Bitte warten.");
                showMatchList?.Invoke();
            }

            var captureAction = showMatchList;
            CancelCommand = new RelayCommand(_ => captureAction?.Invoke());

            SubmitCommand = new RelayCommand(async _ =>
            {
                var players = Players.Where(p => p.IsChecked)
                    .Select(p => p.PlayerItem).ToList();
                _tournament.Datetime = DateTime.Now;
                showMatchList?.Invoke();
                var success = await Task.Run(() =>
                IsAuthenticated &&
                    TournamentManager.CreateTournament(
                        _tournament, players, AmountMatches));
                reloadParent?.Invoke();
                queueMessage?.Invoke(success
                    ? "Neuer Spielplan erstellt."
                    : "Fehler: Spielplan konnte nicht erstellt werden.");
            });

            LoadPlayersAsync();
        }

        public string Name
        {
            get { return _tournament.Name; }
            set
            {
                if (Equals(_tournament.Name, value)) return;
                _tournament.Name = value;
                OnPropertyChanged(this);
            }
        }

        public int AmountMatches
        {
            get { return _amountMatches; }
            set
            {
                if (_amountMatches == value) return;
                _amountMatches = value;
                OnPropertyChanged(this);
            }
        }
    }
}
