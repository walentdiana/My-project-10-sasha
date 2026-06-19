using UnityEngine;

namespace GameName.Input
{
    public class InputComponent : MonoBehaviour
    {
        internal Vector2 GetMove()
        {
            return new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        }

        internal bool GetJump()
        {
            if (UnityEngine.Input.GetButtonDown("Jump"))
            {
                return true;
            }
            return false;
        }

        internal bool GetFire()
        {
            /*if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                return true;
            }*/
            return false;
        }

        internal bool GetClick()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                return true;
            }
            return false;
        }

        internal bool InventoryMode()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                return true;
            }
            return false;
        }
    }
}
