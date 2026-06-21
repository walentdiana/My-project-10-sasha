using UnityEngine; // Unity

namespace GameName.Input
{
    // Компонент ввода — читает клавиатуру и мышь
    // MonoBehaviour — вешается на игрока
    // internal — методы видны только внутри сборки (не снаружи проекта)
    public class InputComponent : MonoBehaviour
    {
        // Возвращает вектор движения: X=горизонталь, Y=вертикаль
        // GetAxis возвращает число от -1 до 1 (плавно, с ускорением)
        internal static Vector2 GetMove()
        {
            return new Vector2(
                UnityEngine.Input.GetAxis("Horizontal"), // A/D или стрелки влево/вправо
                UnityEngine.Input.GetAxis("Vertical")    // W/S или стрелки вверх/вниз
            );
        }

        // Возвращает true если нажат прыжок (пробел)
        internal bool GetJump()
        {
            if (UnityEngine.Input.GetButtonDown("Jump")) // "Jump" = пробел по умолчанию
            {
                return true;
            }
            return false;
        }

        // Возвращает true если нужно выстрелить
        // Сейчас отключено (закомментировано) — всегда false
        internal bool GetFire()
        {
            /*if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                return true;
            }*/
            return false; // стрельба отключена
        }

        // Возвращает true если нажата клавиша 1 (Alpha1)
        // Используется в PlayerMovement.Click() для взаимодействия с тайлами
        internal bool GetClick()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) // цифра 1 на клавиатуре
            {
                return true;
            }
            return false;
        }

        // Возвращает true если нажата I (инвентарь)
        // Сейчас нигде не подключён — InventoryController.Toggle() надо вызвать вручную
        internal bool InventoryMode()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I)) // клавиша I
            {
                return true;
            }
            return false;
        }
    }
}