﻿using System;
using Caliburn.Micro;
using TraktAPI;
using TraktAPI.TraktModels;
using Microsoft.Phone.Reactive;
using System.Collections.Generic;
using System.Windows;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace Trakt_for_Windows_Phone_7.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        readonly INavigationService navigationService;
        private IsolatedStorageSettings userSettings;

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            userSettings = IsolatedStorageSettings.ApplicationSettings;
            if (userSettings.Contains("TraktUsername"))
            {
                //Try log in
                TraktAPI.TraktAPI.testAccount((string)userSettings["TraktUsername"], (string)userSettings["TraktPassword"]).Subscribe(onNext: res => loginSuccess(), onError: error => getTrending(true));
            }
            else
            {
                getTrending(true);
            }

        }

        public string TraktLogo
        {
            get
            {
                PhoneApplicationPage x = new PhoneApplicationPage();
                var backColor = x.Resources["PhoneBackgroundColor"];
                if (backColor.ToString() == "#FF000000")
                    return "/Trakt for Windows Phone 7;component/Artwork/traktLogoInverted.png";
                else
                    return "/Trakt for Windows Phone 7;component/Artwork/traktLogo.png";
            }
        }

        private bool ready = false;

        private void getTrending(bool clearIsolatedStorage)
        {
            if (clearIsolatedStorage)
                userSettings.Clear();
            TraktAPI.TraktAPI.getTrendingMovies().Subscribe(onNext: res => Movies = res, onError: error => handleError(error));
            TraktAPI.TraktAPI.getTrendingShows().Subscribe(onNext: res => Shows = res, onError: error => handleError(error));
            NotifyOfPropertyChange("UserAccount");
            ready = true;
        }

        private void loginSuccess()
        {
            TraktSettings.Username = (string) userSettings["TraktUsername"];
            TraktSettings.Password = (string) userSettings["TraktPassword"];
            TraktSettings.LoggedIn = true;
            getTrending(false);
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
            if(ready)
                navigationService.Navigate(new Uri("/Views/LogIn.xaml", UriKind.Relative));
            
            NotifyOfPropertyChange("UserAccount");
        }

        public void Movie0()
        {
            if(_movies != null && _movies[0] != null)
                navigationService.Navigate(new Uri("/Views/Movie.xaml?IMDBID=" + _movies[0].IMDBID, UriKind.Relative));
        }

        public void Movie1()
        {
            if (_movies != null && _movies[1] != null)
                navigationService.Navigate(new Uri("/Views/Movie.xaml?IMDBID=" + _movies[1].IMDBID, UriKind.Relative));
        }

        public void Movie2()
        {
            if (_movies != null && _movies[2] != null)
                navigationService.Navigate(new Uri("/Views/Movie.xaml?IMDBID=" + _movies[2].IMDBID, UriKind.Relative));
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

        public void DoSearch()
        {
            navigationService.Navigate(new Uri("/Views/Search.xaml", UriKind.Relative));
        }

        public void GetRecommendations()
        {
            navigationService.Navigate(new Uri("/Views/Recommendations.xaml", UriKind.Relative));
        }

        public void GetWatchList()
        {
            navigationService.Navigate(new Uri("/Views/WatchList.xaml", UriKind.Relative));
        }
    }
}