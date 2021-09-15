namespace Imagin.Common.Models
{
    public interface IDockViewModel
    {
        DocumentCollection Documents { get; }

        PanelCollection Panels { get; }
    }
}