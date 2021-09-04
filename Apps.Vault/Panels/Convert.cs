using System.Runtime.CompilerServices;
using Imagin.Common;
using Imagin.Common.Models;
using Imagin.Common.Text;
using Imagin.Common.Threading;

namespace Vault
{
    public class ConvertPanel : Panel
    {
        bool destinationTextChangeHandled;

        bool sourceTextChangeHandled;

        BackgroundQueue queue = new BackgroundQueue();

        SymmetricAlgorithm algorithm;
        public SymmetricAlgorithm Algorithm
        {
            get => algorithm;
            set => this.Change(ref algorithm, value);
        }

        string destinationText;
        public string DestinationText
        {
            get => destinationText;
            set
            {
                this.Change(ref destinationText, value);
                OnDestinationTextChanged(destinationText);
            }
        }

        Encoding encoding;
        public Encoding Encoding
        {
            get => encoding;
            set => this.Change(ref encoding , value);
        }

        bool isDestinationTextInvalid = false;
        public bool IsDestinationTextInvalid
        {
            get => isDestinationTextInvalid;
            set => this.Change(ref isDestinationTextInvalid, value);
        }

        bool isSourceTextInvalid = false;
        public bool IsSourceTextInvalid
        {
            get => isSourceTextInvalid;
            set => this.Change(ref isSourceTextInvalid, value);
        }

        int maxLength = 10000;
        public int MaxLength
        {
            get => maxLength;
            set => this.Change(ref maxLength, value);
        }

        string password;
        public string Password
        {
            get => password;
            set => this.Change(ref password, value);
        }

        string sourceText;
        public string SourceText
        {
            get => sourceText;
            set
            {
                if (value?.Length > maxLength)
                    value = value.Substring(0, maxLength);

                this.Change(ref sourceText, value);
                OnSourceTextChanged(sourceText);
            }
        }

        public override string Title => "Convert";

        public ConvertPanel() : base(Resources.Uri(nameof(Vault), "/Images/Binary.png")) { }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Algorithm):
                case nameof(Encoding):
                case nameof(Password):
                    OnSourceTextChanged(sourceText);
                    break;
            }
        }

        void OnDestinationTextChanged(string destinationText)
        {
            if (!destinationTextChangeHandled)
            {
                queue.Add(() =>
                {
                    sourceTextChangeHandled = true;

                    var resultText = destinationText;
                    var result = Converter.TryDecryptText(ref resultText, destinationText, Password, Algorithm, Encoding);

                    if (!result)
                    {
                        IsDestinationTextInvalid = true;
                    }
                    else
                    {
                        IsDestinationTextInvalid = false;
                        IsSourceTextInvalid = false;
                    }

                    SourceText = resultText;
                    sourceTextChangeHandled = false;
                });
            }
        }

        void OnSourceTextChanged(string sourceText)
        {
            if (!sourceTextChangeHandled)
            {
                queue.Add(() =>
                {
                    destinationTextChangeHandled = true;

                    var resultText = sourceText;
                    var result = Converter.TryEncryptText(ref resultText, sourceText, Password, Algorithm, Encoding);

                    if (!result)
                    {
                        IsSourceTextInvalid = true;
                    }
                    else
                    {
                        IsDestinationTextInvalid = false;
                        IsSourceTextInvalid = false;
                    }

                    DestinationText = resultText;
                    destinationTextChangeHandled = false;
                });
            }
        }
    }
}