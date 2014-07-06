using UnityEngine;
using System.Collections;
using System;

public class ActionButton : MonoBehaviour
{
    public delegate void OnButtonClickListener(object sender, EventArgs e);
    public event OnButtonClickListener OnBtnClick;

    public Texture2D _texture;
    private Rect _actionArea;
    private bool phaseBegan;

    void Start()
    {
        _actionArea = new Rect(Screen.width / 2, 0, Screen.width, Screen.height);
    }

    void OnGUI()
    {
        foreach (Touch touchItem in Input.touches)
        {
            Vector2 touchVector = touchItem.position;
            touchVector.y = Screen.height - touchVector.y;

            if (_actionArea.Contains(touchVector))
            {
                if (touchItem.phase == TouchPhase.Began)
                    phaseBegan = true;
                if (touchItem.phase == TouchPhase.Ended && phaseBegan)
                {
                    phaseBegan = false;
                    OnBtnClick(this, new EventArgs());
                }
            } 
        }

        #if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
            OnBtnClick(this, new EventArgs());
        #endif
    }
}
