using System.Windows.Media.Effects;

namespace Imagin.Common.Effects
{
    public abstract class BaseEffect : ShaderEffect
    {
        public abstract string RelativePath { get; }
    }
}