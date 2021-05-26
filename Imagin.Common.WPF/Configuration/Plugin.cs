using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// A wrapper for an independent class library used to extend functionality of an application.
    /// </summary>
    [Serializable]
    public abstract class Plugin : Base, IPlugin
    {
        #region Properties

        public event EventHandler<EventArgs> Disabled;

        public event EventHandler<EventArgs> Enabled;

        protected IApp App
        {
            get
            {
                return Application.Current as IApp;
            }
        }

        /// <summary>
        /// Gets reference to the assembly context in which the plugin corresponds.
        /// </summary>
        internal AssemblyContext AssemblyContext
        {
            get; set;
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");

                if (value)
                {
                    OnEnabled();
                }
                else OnDisabled();
            }
        }

        //...............................................

        string author;
        public string Author
        {
            get
            {
                return author;
            }
            private set
            {
                author = value;
                OnPropertyChanged("Author");
            }
        }

        string description;
        public string Description
        {
            get
            {
                return description;
            }
            private set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        string icon;
        public string Icon
        {
            get
            {
                return icon;
            }
            private set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        string name;
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public ResourceDictionary Resources
        {
            get; private set;
        }

        string uri;
        public string Uri
        {
            get
            {
                return uri;
            }
            private set
            {
                uri = value;
                OnPropertyChanged("Uri");
            }
        }

        Version version;
        public Version Version
        {
            get
            {
                return version;
            }
            private set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        #endregion

        #region Plugin

        public Plugin() : base()
        {
            Author = GetAuthor();
            Description = GetDescription();
            Icon = GetIcon();
            Name = GetName();
            Resources = GetResources();
            Uri = GetUri();
            Version = GetVersion();
        }

        #endregion

        #region Methods

        protected abstract string GetAuthor();

        protected abstract string GetDescription();

        protected abstract string GetIcon();

        protected abstract string GetName();

        protected abstract ResourceDictionary GetResources();

        protected abstract string GetUri();

        protected abstract Version GetVersion();

        /// <summary>
        /// Merge all facilities used by plugin with application
        /// </summary>
        protected virtual void Merge()
        {
            App.Resources.As<ResourceDictionary>().MergedDictionaries.Add(this.Resources);
        }

        /// <summary>
        /// Release all facilities used by plugin from application.
        /// </summary>
        protected virtual void Release()
        {
            App.Resources.As<ResourceDictionary>().MergedDictionaries.Remove(Resources);
        }

        public virtual void OnDisabled()
        {
            Release();
            Disabled?.Invoke(this, new EventArgs());
        }

        public virtual void OnEnabled()
        {
            Merge();
            Enabled?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
