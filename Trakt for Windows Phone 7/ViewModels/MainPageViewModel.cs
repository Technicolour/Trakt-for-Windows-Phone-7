﻿using System;
using Caliburn.Micro;
using TraktAPI;
using TraktAPI.TraktModels;
using Microsoft.Phone.Reactive;
using System.Collections.Generic;
using System.Windows;

namespace Trakt_for_Windows_Phone_7.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        readonly INavigationService navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            TraktAPI.TraktAPI.getTrendingMovies().Subscribe(onNext: res => Movies = res, onError: error => handleError(error));
            TraktAPI.TraktAPI.getTrendingShows().Subscribe(onNext: res => Shows = res, onError: error => handleError(error));
            NotifyOfPropertyChange("UserAccount");
        }

        private TraktMovie[] _movies;
        public TraktMovie[] Movies { get { return _movies; } set { _movies = value; NotifyOfPropertyChange("Movies"); } }

        private TraktShow[] _shows;
        public TraktShow[] Shows { get { return _shows; } set { _shows = value; NotifyOfPropertyChange("Shows"); } }

        public string UserAccount
        {
            get
            {
                return "Account";
            }
        }

        public void Account()
        {
            navigationService.Navigate(new Uri("/Views/LogIn.xaml", UriKind.Relative));
            
            NotifyOfPropertyChange("UserAccount");
        }

        public void Movie0()
        {
            if(_movies != null && _movies[0] != null)
                navigationService.Navigate(new Uri("/Views/Movie.xaml?MovieTitle=" + _movies[0].IMDBID, UriKind.Relative));
        }

        public void Movie1()
        {
            if (_movies != null && _movies[1] != null)
                navigationService.Navigate(new Uri("/Views/Movie.xaml?MovieTitle=" + _movies[1].IMDBID, UriKind.Relative));
        }

        public void Movie2()
        {
            if (_movies != null && _movies[2] != null)
                navigationService.Navigate(new Uri("/Views/Movie.xaml?MovieTitle=" + _movies[2].IMDBID, UriKind.Relative));
        }

        public void Show0()
        {
            if (_shows != null && _shows[0] != null)
                navigationService.Navigate(new Uri("/Views/Show.xaml?TVDBID=" + _shows[0].TVDBID, UriKind.Relative));
        }

        public void Show1()
        {
            if (_shows != null && _shows[1] != null)
                navigationService.Navigate(new Uri("/Views/Show.xaml?TVDBID=" + _shows[1].TVDBID, UriKind.Relative));
        }

        public void Show2()
        {
            if (_shows != null && _shows[2] != null)
                navigationService.Navigate(new Uri("/Views/Show.xaml?TVDBID=" + _shows[2].TVDBID, UriKind.Relative));
        }
    }
}