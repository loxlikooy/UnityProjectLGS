using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    private KeyCode[] _konamiCode;
    private int _konamiIndex;

    private void Start()
    {
        _konamiCode = new KeyCode[]
        {
            KeyCode.UpArrow,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.B,
            KeyCode.A
        };
        _konamiIndex = 0;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(_konamiCode[_konamiIndex]))
            {
                _konamiIndex++;
                if (_konamiIndex == _konamiCode.Length)
                {
                    TriggerEasterEgg();
                    _konamiIndex = 0;
                }
            }
            else
            {
                _konamiIndex = 0;
            }
        }
    }

    private void TriggerEasterEgg()
    {
        transform.position = new Vector2(500, 500);
    }
}