using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault
{
    [Serializable]
    public class Options : Data<MainViewModel>
    {
        enum Category
        {
            Generate,
            Passwords,
            Tasks
        }

        #region Generate

        string generateCharacters = string.Empty;
        [Hidden]
        public string GenerateCharacters
        {
            get => generateCharacters;
            set => this.Change(ref generateCharacters, string.Concat(value.Distinct()));
        }

        string generateCustomCharacters = string.Empty;
        [Category(Category.Generate)]
        [DisplayName("CustomCharacters")]
        [StringFormat(StringFormat.Tokens, ' ')]
        public string GenerateCustomCharacters
        {
            get => generateCustomCharacters;
            set
            {
                var old = string.Empty;

                var result = string.Empty;
                foreach (var i in value)
                {
                    if (i != ' ')
                    {

                        if (!old.Contains(i))
                        {
                            result += i;
                            old += i;
                        }
                    }
                    else
                    {
                        result += i;
                        old = string.Empty;
                    }
                }

                this.Change(ref generateCustomCharacters, result);
                this.Changed(() => GenerateCustomCharactersList);
            }
        }

        [Hidden]
        public StringCollection GenerateCustomCharactersList
        {
            get
            {
                var result = new StringCollection();
                generateCustomCharacters.Split(Array<char>.New(' '), StringSplitOptions.RemoveEmptyEntries).ForEach(i => result.Add(i));
                return result;
            }
        }

        bool generateDistinct = false;
        [Hidden]
        public bool GenerateDistinct
        {
            get => generateDistinct;
            set => this.Change(ref generateDistinct, value);
        }

        string generateFontFamily;
        [Category(Category.Generate)]
        [DisplayName("FontFamily")]
        public FontFamily GenerateFontFamily
        {
            get
            {
                if (generateFontFamily == null)
                    return default;

                FontFamily result = null;
                Try.Invoke(() => result = new FontFamily(generateFontFamily));
                return result;
            }
            set => this.Change(ref generateFontFamily, value.Source);
        }

        double generateFontSize = 16;
        [Category(Category.Generate)]
        [DisplayName("FontSize")]
        [Range(8.0, 72.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double GenerateFontSize
        {
            get => generateFontSize;
            set => this.Change(ref generateFontSize, value);
        }

        HorizontalAlignment generateHorizontalAlignment = HorizontalAlignment.Left;
        [Category(Category.Generate)]
        [DisplayName("TextAlignmentHorizontal")]
        public HorizontalAlignment GenerateHorizontalAlignment
        {
            get => generateHorizontalAlignment;
            set => this.Change(ref generateHorizontalAlignment, value);
        }

        VerticalAlignment generateVerticalAlignment = VerticalAlignment.Top;
        [Category(Category.Generate)]
        [DisplayName("TextAlignmentVertical")]
        public VerticalAlignment GenerateVerticalAlignment
        {
            get => generateVerticalAlignment;
            set => this.Change(ref generateVerticalAlignment, value);
        }

        StringCollection generateHistory = new StringCollection();
        [Hidden]
        public StringCollection GenerateHistory
        {
            get => generateHistory ?? (generateHistory = new StringCollection());
            set => this.Change(ref generateHistory, value);
        }

        int generateHistoryLimit = 20;
        [Category(Category.Generate)]
        [DisplayName("History limit")]
        [Range(0, 64, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int GenerateHistoryLimit
        {
            get => generateHistoryLimit;
            set => this.Change(ref generateHistoryLimit, value);
        }

        uint generateLength = 50;
        [Hidden]
        public uint GenerateLength
        {
            get => generateLength;
            set => this.Change(ref generateLength, value);
        }

        string generateText = string.Empty;
        [Hidden]
        public string GenerateText
        {
            get => generateText;
            set => this.Change(ref generateText, value);
        }

        #endregion

        #region Passwords

        bool encryptPasswords = false;
        [Category(Category.Passwords)]
        [DisplayName("Encrypt")]
        public bool EncryptPasswords
        {
            get => encryptPasswords;
            set => this.Change(ref encryptPasswords, value);
        }

        ObservableCollection<Vault.Category> categories = new ObservableCollection<Vault.Category>();
        [Hidden]
        public ObservableCollection<Vault.Category> Categories
        {
            get => categories;
            set => this.Change(ref categories, value);
        }

        Encryption passwordEncryption = new Encryption();
        [Category(Category.Passwords)]
        [DisplayName("Encryption")]
        public Encryption PasswordEncryption
        {
            get => passwordEncryption;
            set => this.Change(ref passwordEncryption, value);
        }

        [field: NonSerialized]
        XmlWriter<Password> passwords;
        [Hidden]
        public XmlWriter<Password> Passwords
        {
            get => passwords;
            set => this.Change(ref passwords, value);
        }

        PasswordSortNames passwordSortName = PasswordSortNames.Name;
        [Category(Category.Passwords)]
        [DisplayName("SortName")]
        public PasswordSortNames PasswordSortName
        {
            get => passwordSortName;
            set => this.Change(ref passwordSortName, value);
        }

        SortDirection passwordSortDirection = SortDirection.Ascending;
        [Category(Category.Passwords)]
        [DisplayName("SortDirection")]
        public SortDirection PasswordSortDirection
        {
            get => passwordSortDirection;
            set => this.Change(ref passwordSortDirection, value);
        }

        TaskSortNames taskSortName = TaskSortNames.LastActive;
        [Category(Category.Tasks)]
        [DisplayName("SortName")]
        public TaskSortNames TaskSortName
        {
            get => taskSortName;
            set => this.Change(ref taskSortName, value);
        }

        SortDirection taskSortDirection = SortDirection.Descending;
        [Category(Category.Tasks)]
        [DisplayName("SortDirection")]
        public SortDirection TaskSortDirection
        {
            get => taskSortDirection;
            set => this.Change(ref taskSortDirection, value);
        }

        #endregion

        #region Tasks

        [field: NonSerialized]
        XmlWriter<CopyTask> tasks;
        [Hidden]
        public XmlWriter<CopyTask> Tasks
        {
            get => tasks;
            set => this.Change(ref tasks, value);
        }

        #endregion

        #region Other

        bool showInTaskBar = false;
        [DisplayName("ShowInTaskbar")]
        public bool ShowInTaskBar
        {
            get => showInTaskBar;
            set => this.Change(ref showInTaskBar, value);
        }

        #endregion

        public override void OnApplicationExit()
        {
            base.OnApplicationExit();
            Passwords.Save();
            Tasks.Save();
        }

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            Passwords = new XmlWriter<Password>(nameof(Passwords), $@"{Get.Current<App>().Data.FolderPath}\Passwords.xml", new Limit(1000, Limit.Actions.ClearAndArchive))
            {
                Encrypt = encryptPasswords,
                Encryption = passwordEncryption
            };
            //Passwords.Added += OnPasswordAdded;
            //Passwords.Removed += OnPasswordRemoved;
            Passwords.Load();

            Tasks = new XmlWriter<CopyTask>(nameof(Tasks), $@"{Get.Current<App>().Data.FolderPath}\Tasks.xml", new Limit(50, Limit.Actions.ClearAndArchive))
            {
                Encrypt = false
            };
            Tasks.Load();
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(EncryptPasswords):
                    Passwords.Encrypt = encryptPasswords;
                    break;

                case nameof(PasswordEncryption):
                    Passwords.Encryption = passwordEncryption;
                    break;
            }
        }

        [field: NonSerialized]
        ICommand addCategoryCommand;
        [Hidden]
        public ICommand AddCategoryCommand
        {
            get
            {
                addCategoryCommand = addCategoryCommand ?? new RelayCommand(() => categories.Add(new Vault.Category()), () => true);
                return addCategoryCommand;
            }
        }

        [field: NonSerialized]
        ICommand deleteCategoryCommand;
        [Hidden]
        public ICommand DeleteCategoryCommand
        {
            get
            {
                deleteCategoryCommand = deleteCategoryCommand ?? new RelayCommand<Vault.Category>(i => categories.Remove(i), i => i != null);
                return deleteCategoryCommand;
            }
        }
        
        [field: NonSerialized]
        ICommand deletePasswordCommand;
        [Hidden]
        public ICommand DeletePasswordCommand
        {
            get
            {
                deletePasswordCommand = deletePasswordCommand ?? new RelayCommand<Password>(i =>
                {
                    var result = Dialog.Show("Delete", "Are you sure you want to delete this?", DialogImage.Warning, DialogButtons.YesNo);
                    if (result == 0)
                        Passwords.Remove(i);
                },
                i => i != null);
                return deletePasswordCommand;
            }
        }
    }
}