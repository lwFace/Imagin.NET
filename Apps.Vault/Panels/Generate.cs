using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System.Linq;
using System.Windows.Input;

namespace Vault
{
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

        Characters characters = Characters.Numbers;
        public Characters Characters
        {
            get => characters;
            set => this.Change(ref characters, value);
        }

        uint length = 50;
        public uint Length
        {
            get => length;
            set => this.Change(ref length, value);
        }

        string actualCharacters = string.Empty;
        public string ActualCharacters
        {
            get => actualCharacters;
            set => this.Change(ref actualCharacters, value);
        }

        string text;
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        public override string Title => "Generate";

        public GeneratePanel() : base(Resources.Uri(nameof(Vault), "/Images/Scroll.png")) { }

        ICommand addCharactersCommand;
        public ICommand AddCharactersCommand
        {
            get
            {
                addCharactersCommand = addCharactersCommand ?? new RelayCommand(() =>
                {
                    var result = string.Empty;
                    switch (characters)
                    {
                        case Characters.Lower:
                            result += Lower;
                            break;

                        case Characters.Numbers:
                            0.For(10, i => result = $"{result}{i}");
                            break;

                        case Characters.Special:
                            result += Special;
                            break;

                        case Characters.Upper:
                            result += Upper;
                            break;
                    }

                    ActualCharacters = string.Concat($"{actualCharacters}{result}".Distinct());
                }, 
                () => true);
                return addCharactersCommand;
            }
        }
        ICommand generateCommand;
        public ICommand GenerateCommand
        {
            get
            {
                generateCommand = generateCommand ?? new RelayCommand(() => Text = Random.String(string.Concat(actualCharacters.Distinct()), (int)length, (int)length), () => !actualCharacters.NullOrEmpty());
                return generateCommand;
            }
        }
    }
}