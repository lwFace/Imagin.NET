﻿using System.ComponentModel;
using System;
using System.Diagnostics;

namespace Imagin.Gadgets.Search
{
    public enum SearchEngineType
    {
        None,
        Google,
        Yahoo, 
        Bing
    }

    public class SearchEngine : INotifyPropertyChanged
    {
        public event EventHandler<EventArgs> Queried; 

        string name = string.Empty;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        SearchEngineType type = SearchEngineType.None;
        public SearchEngineType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                OnPropertyChanged("Type");
            }
        }

        public SearchEngine(SearchEngineType Type)
        {
            this.Type = Type;
            this.Name = Type.ToString();
        }

        public void Query(string Query)
        {
            string BaseQuery = string.Empty;
            switch (this.Type)
            {
                case SearchEngineType.Google:
                    BaseQuery = @"https://www.google.com/#q=";
                    break;
                case SearchEngineType.Yahoo:
                    BaseQuery = @"https://search.yahoo.com/search;?p=";
                    break;
                case SearchEngineType.Bing:
                    BaseQuery = @"http://www.bing.com/search?q=";
                    break;
            }
            if (!string.IsNullOrEmpty(BaseQuery))
                Process.Start(string.Concat(BaseQuery, Query));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
