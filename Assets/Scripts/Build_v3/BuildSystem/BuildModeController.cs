using UnityEngine;

namespace BuildSystem
{
    public class BuildModeController : MonoBehaviour
    {
        [SerializeField] private KeyCode          _toggleKey = KeyCode.B;
        [SerializeField] private PaletteUIManager _paletteUI;

        private bool _bIsActive;
        
        private void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
                Toggle();
        }

        private void Toggle()
        {
            _bIsActive = !_bIsActive;
            _paletteUI.SetVisible(_bIsActive);
        }

        public void Activate()
        {
            _bIsActive = true;
            _paletteUI.SetVisible(true);
        }

        public void Deactivate()
        {
            _bIsActive = false;
            _paletteUI.SetVisible(false);
        }
    }
}