using UnityEngine;
using System.Collections;

public class MyGUI: MonoBehaviour
{
    public class Margins
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        public Margins(float _left, float _top, float _right, float _bottom)
        {
            Left = _left;
            Top = _top;
            Right = _right;
            Bottom = _bottom;
        }
    }

    public enum ScreenPossition
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottonLeft,
        BottonMiddle,
        BottonRight
    }

    public ScreenPossition DefaultScreenPossition {
        get { return ScreenPossition.MiddleCenter; }
    }

    public static void SetTextScreenPosition(ScreenPossition _screenPossition, GameObject _gameObject)
    {
        Vector3 screenSize = GetScreenSizeInWorldPoint();
        Vector3 textSize = _gameObject.GetComponent<MeshRenderer>().bounds.size;
        
        switch(_screenPossition) {
            case ScreenPossition.TopLeft:
                _gameObject.transform.localPosition = new Vector3(textSize.x / 2 - screenSize.x, screenSize.y - textSize.y / 2, 0f);
                break;

            case ScreenPossition.TopCenter:
                _gameObject.transform.localPosition = new Vector3(0, screenSize.y - textSize.y / 2, 0f);
                break;

            case ScreenPossition.TopRight:
                _gameObject.transform.localPosition = new Vector3(screenSize.x - textSize.x / 2, screenSize.y - textSize.y / 2, 0);
                break;

            case ScreenPossition.MiddleLeft:
                _gameObject.transform.localPosition = new Vector3(textSize.x / 2 - screenSize.x, 0, 0f);
                break;

            case ScreenPossition.MiddleCenter:
                _gameObject.transform.localPosition = new Vector3(0, 0, 0f);
                break;

            case ScreenPossition.MiddleRight:
                _gameObject.transform.localPosition = new Vector3(screenSize.x - textSize.x / 2, 0, 0);
                break;

            case ScreenPossition.BottonLeft:
                _gameObject.transform.localPosition = new Vector3(textSize.x / 2 - screenSize.x, textSize.y / 2 - screenSize.y, 0f);
                break;

            case ScreenPossition.BottonMiddle:
                _gameObject.transform.localPosition = new Vector3(0, textSize.y / 2 - screenSize.y, 0f);
                break;

            case ScreenPossition.BottonRight:
                _gameObject.transform.localPosition = new Vector3(screenSize.x - textSize.x / 2, textSize.y / 2 - screenSize.y, 0);
                break;
        }
    }

    public static void SetMargins(Margins _guiMargins, GameObject _gameObject)
    {
        _gameObject.transform.localPosition = new Vector3(_gameObject.transform.localPosition.x + (_guiMargins.Left - _guiMargins.Right), _gameObject.transform.localPosition.y + (_guiMargins.Bottom - _guiMargins.Top), 0);
    }

    private static Vector3 GetScreenSizeInWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
}
