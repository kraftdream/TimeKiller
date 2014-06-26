using System;
using UnityEngine;
using System.Collections;

public class GUIText: MyGUI
{
    private const int GUI_LAYER = 8;

    private GameObject _textObject;
    private TextMesh _textMesh;
    private ScreenPossition _currentScreenPosition;

    [SerializeField]
    private Font _textFont;
    [SerializeField]
    private int _fontSize;
    [SerializeField]
    private string _textObjectName;
    [SerializeField]
    private ScreenPossition _screenPosition;
    [SerializeField]
    private string _valueText;

    [SerializeField]
    private float _marginLeft;
    [SerializeField]
    private float _marginTop;
    [SerializeField]
    private float _marginRight;
    [SerializeField]
    private float _marginBottom;


    public ScreenPossition ScreenPosition
    {
        get { return _screenPosition; }
        set { _screenPosition = value; }
    }

    public string ValueText
    {
        get { return _valueText; }
        set { _valueText = value; }
    }

    void Awake ()
    {
        InitTextObject();
        SetTextScreenPosition(ScreenPosition, _textObject);
        SetMargins(new Margins(_marginLeft, _marginTop, _marginRight, _marginBottom), _textObject);
	}

    void Update()
    {
        UpdateScreenPosition();
    }

    void InitTextObject()
    {
        _textObject = new GameObject(_textObjectName);
        _textObject.transform.parent = transform;
        _textObject.layer = GUI_LAYER;
        _textMesh = _textObject.AddComponent<TextMesh>();

        _textMesh.font = _textFont;
        _textMesh.renderer.material = _textFont.material;
        _textMesh.characterSize = 0.1f;
        _textMesh.fontSize = _fontSize;
        _textMesh.anchor = TextAnchor.MiddleCenter;
        SetText(ValueText);
    }

    void SetText(String _text)
    {
        _textMesh.text = _text;
    }

    void UpdateScreenPosition()
    {
        if (!_currentScreenPosition.Equals(ScreenPosition))
        {
            _currentScreenPosition = ScreenPosition;
            SetTextScreenPosition(ScreenPosition, _textObject);
            SetMargins(new Margins(_marginLeft, _marginTop, _marginRight, _marginBottom), _textObject);
        }
    }
}
