using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System.Windows.Controls;

namespace Alarm
{
    public partial class MathWindow : BaseWindow
    {
        public double CorrectAnswer = 0;

        string equation = string.Empty;
        public string Equation => equation;

        double answer = 0;
        public double Answer
        {
            get => answer;
            set
            {
                answer = value;
                OnPropertyChanged();
            }
        }

        bool solved = false;
        public bool Solved
        {
            get => solved;
            set
            {
                solved = value;
                OnPropertyChanged();
            }
        }

        readonly MathParser mathParser = new MathParser();

        public MathWindow(MathParser.Difficulties difficulty)
        {
            InitializeComponent();
            DataContext = this;

            equation = mathParser.Create(difficulty);
            CorrectAnswer = mathParser.Solve(equation);

            this.Changed(() => Equation);
        }

        void OnAnswerChanged(object sender, TextChangedEventArgs e)
        {
            var answer = sender.As<Imagin.Common.Controls.TextBox>().Text;
            var realAnswer = 0d;

            if (answer == "-")
            {
                realAnswer = -1;
            }
            else if (answer == ".")
            {
                realAnswer = 0.1d;
            }
            else
            {
                double.TryParse(answer, out realAnswer);
            }
            Answer = realAnswer;
            Solved = Answer == CorrectAnswer;

            if (Solved)
            {
                Close();
            }
        }

        uint Count(char needle, string stack)
        {
            uint result = 0;
            for (var i = 0; i < stack.Length; i++)
            {
                if (stack[i] == needle)
                {
                    result++;
                }
            }
            return result;
        }

        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = (Imagin.Common.Controls.TextBox)sender;
            if (e.Text == "-")
            {
                if (Count('-', textBox.Text) == 1)
                {
                    e.Handled = true;
                }
            }
            else if (e.Text == ".")
            {
                if (Count('.', textBox.Text) == 1)
                {
                    e.Handled = true;
                }
            }
            else if (!char.IsNumber(char.Parse(e.Text)))
            {
                e.Handled = true;
            }

            if (textBox.Text.Contains("-"))
            {
                if (!textBox.Text.StartsWith("-"))
                {
                    e.Handled = true;
                }
            }
        }
    }
}