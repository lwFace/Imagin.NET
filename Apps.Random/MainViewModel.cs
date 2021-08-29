using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Windows.Input;

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

                    Get.Current<Options>().Characters = string.Concat($"{Get.Current<Options>().Characters}{result}".Distinct());
                },
                i => true);
                return addCharactersCommand;
            }
        }

        ICommand generateCommand;
        public ICommand GenerateCommand
        {
            get
            {
                generateCommand = generateCommand ?? new RelayCommand(() => Get.Current<Options>().Text = Imagin.Common.Random.String(string.Concat(Get.Current<Options>().Characters.Distinct()), (int)Get.Current<Options>().Length, (int)Get.Current<Options>().Length), () => !Get.Current<Options>().Characters.NullOrEmpty());
                return generateCommand;
            }
        }
    }
}