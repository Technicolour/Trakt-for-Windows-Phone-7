﻿using System;
using Caliburn.Micro;
using TraktAPI;
using TraktAPI.TraktModels;
using Microsoft.Phone.Reactive;
using System.Collections.Generic;
using System.Windows;

namespace Trakt_for_Windows_Phone_7.ViewModels
{
    public class ShowViewModel : BaseViewModel
    {
        readonly INavigationService navigationService;

        public ShowViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        private string _TVDBID;
        public string TVDBID { get { return _TVDBID; } set { _TVDBID = value; NotifyOfPropertyChange("TVDBID"); TraktAPI.TraktAPI.getShow(TVDBID).Subscribe(response => Show = response); TraktAPI.TraktAPI.getSeasonInfo(TVDBID).Subscribe(response => Seasons = new List<TraktSeasonInfo>(response)); } }

        private TraktShow _show;
        public TraktShow Show { get { return _show; } set { _show = value; updateDisplay(); } }

        public string ShowTitle 
        { 
            get 
            {
                if (Show == null)
                    return "";

                return Show.Title;
            }
        }

        public string RunTime
        {
            get
            {
                if (Show == null)
                    return "";
                return String.Format("{0} min", Show.RunTime);
            }
        }

        public string AirTime
        {
            get
            {
                if (Show == null)
                    return "";
                return String.Format("{0} at {1}", Show.AirDay, Show.AirTime);
            }
        }

        public string Certification
        {
            get
            {
                if (Show == null)
                    return "";

                return Show.Certification;
            }
        }

        public string Country
        {
            get
            {
                if (Show == null)
                    return "";

                return Show.Country;
            }
        }

        public string RatingImage
        {
            get
            {
                if (Show == null || Show.Ratings == null || Show.Ratings.Percentage == 0)
                    return "";
                if (Show.Ratings.Percentage >= 50)
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/iconLove.png";
                else
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/iconHate.png";
            }
        }

        public string RatingPercentage
        {
            get
            {
                if (Show == null)
                    return "";
                return Show.Ratings.Percentage + "%";
            }
        }

        public string RatingCount
        {
            get
            {
                if (Show == null)
                    return "";
                return string.Format("{0} votes", Show.Ratings.Votes);
            }

        }

        public string LoggedIn
        {
            get
            {
                if (Show != null || TraktSettings.LoggedIn)
                    return "Visible";
                else
                    return "Collapsed";
            }
        }

        public string UserRatingText
        {
            get
            {
                if (Show == null)
                    return "";
                if (Show.Rating.CompareTo("love") == 0)
                    return "Love it!";
                if (Show.Rating.CompareTo("hate") == 0)
                    return "Lame";
                return "";
            }
        }

        public string LoveImage
        {
            get
            {
                if (Show == null)
                    return "";

                if (Show.Rating.CompareTo("hate") == 0)
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/love_f.png";
                else
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/love.png";
            }
        }

        public string HateImage
        {
            get
            {
                if (Show == null)
                    return "";
                if (Show.Rating.CompareTo("love") == 0)
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/hate_f.png";
                else
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/hate.png";
            }
        }

        private TraktRatings ratings { set { Show.Ratings = value; newRatings(); } }

        public void Love()
        {
            if (Show.Rating.CompareTo("love") == 0)
            {
                TraktAPI.TraktAPI.rateShow(Show.TVDBID, Show.IMDBID, Show.Title, Show.Year, TraktRateTypes.unrate.ToString()).Subscribe(response => ratings = response.Ratings);
                Show.Rating = "";
            }
            else
            {
                TraktAPI.TraktAPI.rateShow(Show.TVDBID, Show.IMDBID, Show.Title, Show.Year, TraktRateTypes.love.ToString()).Subscribe(response => ratings = response.Ratings);
                Show.Rating = "love";
            }
            NotifyOfPropertyChange("LoveImage");
            NotifyOfPropertyChange("HateImage");
            NotifyOfPropertyChange("UserRatingText");
        }

        public void Hate()
        {
            if (Show.Rating.CompareTo("hate") == 0)
            {
                TraktAPI.TraktAPI.rateShow(Show.TVDBID, Show.IMDBID, Show.Title, Show.Year, TraktRateTypes.unrate.ToString()).Subscribe(response => ratings = response.Ratings);
                Show.Rating = "";
            }
            else
            {
                TraktAPI.TraktAPI.rateShow(Show.TVDBID, Show.IMDBID, Show.Title, Show.Year, TraktRateTypes.hate.ToString()).Subscribe(response => ratings = response.Ratings);
                Show.Rating = "hate";
            }
            NotifyOfPropertyChange("LoveImage");
            NotifyOfPropertyChange("HateImage");
            NotifyOfPropertyChange("UserRatingText");
        }

        private List<TraktSeasonInfo> _seasons;
        public List<TraktSeasonInfo> Seasons { get { return _seasons; } set { _seasons = value; NotifyOfPropertyChange("Seasons"); } }

        public TraktSeasonInfo SelectedSeason { get; set; }

        public void ViewSeason()
        {
            if(SelectedSeason != null)
                MessageBox.Show(SelectedSeason.AsString);
        }

        public void updateDisplay()
        {
            NotifyOfPropertyChange("Show");
            NotifyOfPropertyChange("ShowTitle");
            NotifyOfPropertyChange("RunTime");
            NotifyOfPropertyChange("AirTime");
            NotifyOfPropertyChange("Certification");
            NotifyOfPropertyChange("Country");
            NotifyOfPropertyChange("RatingImage");
            NotifyOfPropertyChange("RatingPercentage");
            NotifyOfPropertyChange("RatingCount");
            NotifyOfPropertyChange("LoggedIn");
            NotifyOfPropertyChange("UserRatingText");
            NotifyOfPropertyChange("LoveImage");
            NotifyOfPropertyChange("HateImage");
        }

        private void newRatings()
        {
            NotifyOfPropertyChange("RatingImage");
            NotifyOfPropertyChange("RatingPercentage");
            NotifyOfPropertyChange("RatingCount");
        }
    }
}
