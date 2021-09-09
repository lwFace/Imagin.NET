namespace Notes
{
    public class Markdown : TextDocument
    {
        public Markdown(string name, string text) : base(name, text) { }

        public override string ToString() => nameof(Markdown);
    }
}