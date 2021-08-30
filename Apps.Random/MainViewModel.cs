using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;

namespace Random
{
    [Serializable]
    public enum Characters
    {
        Lower,
        Numbers,
        Special,
        Upper
    }

    public class MainViewModel : MainViewModel<MainWindow>
    {
        const string Lower = "abcdefghijklmnopqrstuvwxyz";

        const string Special = "!@#$%^&*()-=_+[]{};':\",./<>?`~\\|";

        const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //............................................................................
        
        bool generating = false;
        public bool Generating
        {
            get => generating;
            set => this.Change(ref generating, value);
        }

        bool viewOptions = false;
        public bool ViewOptions
        {
            get => viewOptions;
            set => this.Change(ref viewOptions, value);
        }

        //............................................................................

        public MainViewModel() : base() { }

        //............................................................................

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        //............................................................................

        ICommand addCharactersCommand;
        public ICommand AddCharactersCommand
        {
            get
            {
                addCharactersCommand = addCharactersCommand ?? new RelayCommand<Characters>(i =>
                {
                    var result = string.Empty;
                    switch (i)
                    {
                        case Characters.Lower:
                            result += Lower;
                            break;

                        case Characters.Numbers:
                            0.For(10, j => result = $"{result}{j}");
                            break;

                        case Characters.Special:
                            result += Special;
                            break;

                        case Characters.Upper:
                            result += Upper;
                            break;
                    }

                    Get.Current<Options>().Characters = $"{Get.Current<Options>().Characters}{result}";
                },
                i => true);
                return addCharactersCommand;
            }
        }

        ICommand addCustomCharactersCommand;
        public ICommand AddCustomCharactersCommand => addCustomCharactersCommand = addCustomCharactersCommand ?? new RelayCommand<string>(i => Get.Current<Options>().Characters = $"{Get.Current<Options>().Characters}{i}", i => true);
        
        async Task Generate()
        {
            Generating = true;

            await App.Current.Dispatcher.BeginInvoke(() =>
            {
                Get.Current<Options>().History.Add(Get.Current<Options>().Text);
                Get.Current<Options>().Text = string.Empty;
            });

            var result = new StringBuilder();
            var length = (int)Get.Current<Options>().Length;

            var characters = Get.Current<Options>().Characters;
            await Task.Run(() => result.Append(Imagin.Common.Random.String(characters, length, length)));

            await App.Current.Dispatcher.BeginInvoke(() => Get.Current<Options>().Text = result.ToString());
            Generating = false;
        }

        ICommand copyCommand;
        public ICommand CopyCommand => copyCommand = copyCommand ?? new RelayCommand(() => System.Windows.Clipboard.SetText(Get.Current<Options>().Text), () => !Get.Current<Options>().Text.NullOrEmpty());

        ICommand generateCommand;
        public ICommand GenerateCommand
        {
            get
            {
                generateCommand = generateCommand ?? new RelayCommand(() => _ = Generate(), () => !Generating && !Get.Current<Options>().Characters.NullOrEmpty());
                return generateCommand;
            }
        }
    }
}