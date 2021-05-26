using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Text;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Vault
{
    [Serializable]
    public class Options : Data<MainViewModel>
    {
        #region Passwords

        bool encryptPasswords = false;
        [Category(nameof(Passwords))]
        [DisplayName("Encrypt")]
        public bool EncryptPasswords
        {
            get => encryptPasswords;
            set => this.Change(ref encryptPasswords, value);
        }

        ObservableCollection<Category> passwordCategories = new ObservableCollection<Category>();
        [Hidden]
        public ObservableCollection<Category> PasswordCategories
        {
            get => passwordCategories;
            set => this.Change(ref passwordCategories, value);
        }

        Encryption passwordEncryption = new Encryption();
        [Category(nameof(Passwords))]
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
        [DisplayName("Show in task bar")]
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
        ICommand addPasswordCategoryCommand;
        [Hidden]
        public ICommand AddPasswordCategoryCommand
        {
            get
            {
                addPasswordCategoryCommand = addPasswordCategoryCommand ?? new RelayCommand(() => passwordCategories.Add(new Category()), () => true);
                return addPasswordCategoryCommand;
            }
        }

        [field: NonSerialized]
        ICommand deletePasswordCategoryCommand;
        [Hidden]
        public ICommand DeletePasswordCategoryCommand
        {
            get
            {
                deletePasswordCategoryCommand = deletePasswordCategoryCommand ?? new RelayCommand<Category>(i => passwordCategories.Remove(i), i => i != null);
                return deletePasswordCategoryCommand;
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