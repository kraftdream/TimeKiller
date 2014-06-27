using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour
{
    public Texture2D _texture;
    private GUIContent _guiContent;
    private Rect _buttonRect;

    void Start()
    {
        _guiContent = new GUIContent {image = _texture};
        _buttonRect = new Rect(Screen.width - 150, Screen.height - 150, 100, 100);
    }

    void OnGUI()
    {
        if (GUI.Button(_buttonRect, _texture))
        {
            
        }
    }
}
