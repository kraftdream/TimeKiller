using UnityEngine;
using System.Collections;
using System;

public class ActionButton : MonoBehaviour
{
    public delegate void OnButtonClickListener(object sender, EventArgs e);
    public event OnButtonClickListener OnBtnClick;

    public Texture2D _texture;
    private Rect _buttonRect;
    private HeroAttack _heroAttack;
    private bool phaseBegan;

    void Start()
    {
        _buttonRect = new Rect(Screen.width - 150, Screen.height - 150, 100, 100);
        _heroAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroAttack>();
    }

    void OnGUI()
    {
        GUI.Button(_buttonRect, _texture);

        foreach (Touch touchItem in Input.touches)
        {
            Vector2 touchVector = touchItem.position;
            touchVector.y = Screen.height - touchVector.y;

            if (_buttonRect.Contains(touchVector))
            {
                if (touchItem.phase == TouchPhase.Began)
                    phaseBegan = true;
                if (touchItem.phase == TouchPhase.Ended && phaseBegan)
                {
                    phaseBegan = false;
                    //_heroAttack.IsAttacked = true;
                    OnBtnClick(this, new EventArgs());
                }
            } 
        }
        //if (Input.GetMouseButtonUp(0))
            //OnBtnClick(this, new EventArgs());
    }
}
