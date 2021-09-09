using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vault
{
    [Serializable]
    public enum Characters
    {
        Lower,
        Numbers,
        Special,
        Upper
    }

    public class GeneratePanel : Panel
    {
        const string Lower = "abcdefghijklmnopqrstuvwxyz";

        const string Special = "!@#$%^&*()-=_+[]{};':\",./<>?`~\\|";

        const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public override string Title => "Generate";

        bool generating = false;
        public bool Generating
        {
            get => generating;
            set => this.Change(ref generating, value);
        }

        public GeneratePanel() : base(Resources.Uri(nameof(Vault), "Images/Refresh.png")) { }

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

                    Get.Current<Options>().GenerateCharacters = $"{Get.Current<Options>().GenerateCharacters}{result}";
                },
                i => true);
                return addCharactersCommand;
            }
        }

        ICommand addCustomCharactersCommand;
        public ICommand AddCustomCharactersCommand => addCustomCharactersCommand = addCustomCharactersCommand ?? new RelayCommand<string>(i => Get.Current<Options>().GenerateCharacters = $"{Get.Current<Options>().GenerateCharacters}{i}", i => true);

        async Task Generate()
        {
            Generating = true;

            await App.Current.Dispatcher.BeginInvoke(() =>
            {
                Get.Current<Options>().GenerateHistory.Add(Get.Current<Options>().GenerateText);
                Get.Current<Options>().GenerateText = string.Empty;
            });

            var result = new StringBuilder();
            var length = (int)Get.Current<Options>().GenerateLength;

            var characters = Get.Current<Options>().GenerateCharacters;
            await Task.Run(() => result.Append(Imagin.Common.Random.String(characters, length, length)));

            await App.Current.Dispatcher.BeginInvoke(() =>
            {
                var finalResult = result.ToString();
                finalResult = Get.Current<Options>().GenerateDistinct ? string.Concat(finalResult.Distinct()) : finalResult;
                Get.Current<Options>().GenerateText = finalResult;
            });
            Generating = false;
        }

        ICommand copyCommand;
        public ICommand CopyCommand => copyCommand = copyCommand ?? new RelayCommand(() => System.Windows.Clipboard.SetText(Get.Current<Options>().GenerateText), () => !Get.Current<Options>().GenerateText.NullOrEmpty());

        ICommand generateCommand;
        public ICommand GenerateCommand
        {
            get
            {
                generateCommand = generateCommand ?? new RelayCommand(() => _ = Generate(), () => true);
                return generateCommand;
            }
        }
    }
}