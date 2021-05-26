namespace Notes
{
    public class Note : TextDocument
    {
        public Note(string name, string text) : base(name, text) { }

        public override string ToString() => nameof(Note);
    }
}