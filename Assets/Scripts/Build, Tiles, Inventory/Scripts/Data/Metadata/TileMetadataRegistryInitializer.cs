using UnityEngine;
using Zenject;

namespace Data.Metadata
{
    public class TileMetadataRegistryInitializer : IInitializable
    {
        private TileMetadataRegistry _registry;

        [Inject]
        public void Construction(TileMetadataRegistry registry)
        {
            _registry = registry;
        }

        public void Initialize()
        {
            _registry.Initialize();
        }
    }
}