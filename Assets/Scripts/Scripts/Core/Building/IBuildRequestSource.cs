using System;
using BuildSystem;

namespace Core.Building
{
    public interface IBuildRequestSource
    {
        event Action<BuildPalette, int> OnBuild;
    }
}