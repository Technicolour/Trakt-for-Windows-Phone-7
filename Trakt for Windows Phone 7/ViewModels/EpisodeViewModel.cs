﻿using System;
using Caliburn.Micro;
using TraktAPI;
using TraktAPI.TraktModels;
using Microsoft.Phone.Reactive;
using System.Collections.Generic;
using System.Windows;

namespace Trakt_for_Windows_Phone_7.ViewModels
{
    public class EpisodeViewModel : BaseViewModel
    {
        readonly INavigationService navigationService;

        public EpisodeViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        private string _TVDBID;
        public string TVDBID { get { return _TVDBID; } set { _TVDBID = value; System.Diagnostics.Debug.WriteLine(TVDBID); } }

        private string _SeasonNumber;
        public string SeasonNumber { get { return _SeasonNumber; } set { _SeasonNumber = value; System.Diagnostics.Debug.WriteLine(SeasonNumber); } }

        private string _EpisodeNumber;
        public string EpisodeNumber { get { return _EpisodeNumber; } set { _EpisodeNumber = value; System.Diagnostics.Debug.WriteLine(EpisodeNumber); TraktAPI.TraktAPI.getEpisodeSummary(TVDBID, SeasonNumber, EpisodeNumber).Subscribe(response => Episode = response); } }

        private TraktEpisodeSummary _Episode;
        public TraktEpisodeSummary Episode { get { return _Episode; } set { _Episode = value; updateDisplay(); } }

        private TraktRatings ratings { set { Episode.Episode.Ratings = value; newRatings(); } }

        private void updateDisplay()
        {
            NotifyOfPropertyChange("Episode");
            NotifyOfPropertyChange("EpisodeTitle");
            NotifyOfPropertyChange("AirTime");
            NotifyOfPropertyChange("Country");
            NotifyOfPropertyChange("Certification");
            NotifyOfPropertyChange("CombinedSeasonAndEpisodeText");
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

        public string EpisodeTitle 
        {
            get
            {
                if (Episode == null)
                    return "";
                return Episode.Episode.Title;
            }
        }

        public string AirTime
        {
            get
            {
                if (Episode == null)
                    return "";
                return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Episode.Episode.FirstAired).ToLongDateString();
            }
        }

        public string Country
        {
            get
            {
                if (Episode == null)
                    return "";
                return Episode.Show.Country;
            }
        }

        public string Certification
        {
            get
            {
                if (Episode == null)
                    return "";
                return Episode.Show.Certification;
            }
        }

        public string CombinedSeasonAndEpisodeText
        {
            get
            {
                if (Episode == null)
                    return "";
                return Episode.Episode.CombinedSeasonAndEpisodeText;
            }
        }

        public string RatingImage
        {
            get
            {
                if (Episode == null || Episode.Episode.Ratings == null || Episode.Episode.Ratings.Percentage == 0)
                    return "";
                if (Episode.Episode.Ratings.Percentage >= 50)
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/iconLove.png";
                else
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/iconHate.png";
            }
        }
        public string RatingPercentage
        {
            get
            {
                if (Episode == null)
                    return "";
                return Episode.Episode.Ratings.Percentage + "%";
            }
        }

        public string RatingCount
        {
            get
            {
                if (Episode == null)
                    return "";
                return string.Format("{0} votes", Episode.Episode.Ratings.Votes);
            }

        }

        public string LoggedIn
        {
            get
            {
                if (Episode != null || TraktSettings.LoggedIn)
                    return "Visible";
                else
                    return "Collapsed";
            }
        }

        public string UserRatingText
        {
            get
            {
                if (Episode == null)
                    return "";
                if (Episode.Episode.Rating.CompareTo("love") == 0)
                    return "Love it!";
                if (Episode.Episode.Rating.CompareTo("hate") == 0)
                    return "Lame";
                return "";
            }
        }

        public string LoveImage
        {
            get
            {
                if (Episode == null)
                    return "";

                if (Episode.Episode.Rating.CompareTo("hate") == 0)
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/love_f.png";
                else
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/love.png";
            }
        }

        public string HateImage
        {
            get
            {
                if (Episode == null)
                    return "";
                if (Episode.Episode.Rating.CompareTo("love") == 0)
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/hate_f.png";
                else
                    return "/Trakt%20for%20Windows%20Phone%207;component/Artwork/hate.png";
            }
        }

        public void Love()
        {
            if (Episode.Episode.Rating.CompareTo("love") == 0)
            {
                TraktAPI.TraktAPI.rateEpisode(TVDBID, Episode.Show.IMDBID, EpisodeTitle, Episode.Show.Year, SeasonNumber, EpisodeNumber, TraktRateTypes.unrate.ToString()).Subscribe(response => ratings = response.Ratings);
                Episode.Episode.Rating = "";
            }
            else
            {
                TraktAPI.TraktAPI.rateEpisode(TVDBID, Episode.Show.IMDBID, EpisodeTitle, Episode.Show.Year, SeasonNumber, EpisodeNumber, TraktRateTypes.love.ToString()).Subscribe(response => ratings = response.Ratings);
                Episode.Episode.Rating = "love";
            }
            NotifyOfPropertyChange("LoveImage");
            NotifyOfPropertyChange("HateImage");
            NotifyOfPropertyChange("UserRatingText");
        }

        public void Hate()
        {
            if (Episode.Episode.Rating.CompareTo("hate") == 0)
            {
                TraktAPI.TraktAPI.rateEpisode(TVDBID, Episode.Show.IMDBID, EpisodeTitle, Episode.Show.Year, SeasonNumber, EpisodeNumber, TraktRateTypes.unrate.ToString()).Subscribe(response => ratings = response.Ratings);
                Episode.Episode.Rating = "";
            }
            else
            {
                TraktAPI.TraktAPI.rateEpisode(TVDBID, Episode.Show.IMDBID, EpisodeTitle, Episode.Show.Year, SeasonNumber, EpisodeNumber, TraktRateTypes.hate.ToString()).Subscribe(response => ratings = response.Ratings);
                Episode.Episode.Rating = "hate";
            }
            NotifyOfPropertyChange("LoveImage");
            NotifyOfPropertyChange("HateImage");
            NotifyOfPropertyChange("UserRatingText");
        }

    }
}
