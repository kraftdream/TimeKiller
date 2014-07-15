 using System;
using System.Globalization;
 using UnityEngine;
using System.Collections;

public class My3DText: MyGUI
{
    public delegate void OnTextClickListener(object sender);
    public event OnTextClickListener OnTextClick;

    //GUI Layer to camera render
    private const int MAX_FONT_SIZE_INDEX = 50;

    private GameObject _textObject;
    private TextMesh _textMesh;
    private Animation _textAnimation;
    private ScreenPossition _currentScreenPosition;
    private bool phaseBegan;

    [SerializeField]
    private LayerMask _guiLayer;

    [SerializeField]
    private Font _textFont;

    [SerializeField]
    private int _fontSize;

    [SerializeField]
    private int _defaultFontSize;

    [SerializeField]
    private string _textObjectName;

    [SerializeField]
    private bool _isClickable;

    [SerializeField]
    private ScreenPossition _screenPosition;

    [SerializeField]
    private int _value;

    [SerializeField]
    private string _valueText;

    [SerializeField]
    private AnimationClip _guiAnimation;

    [SerializeField]
    private Color _textColor;

    [SerializeField]
    private Quaternion _rotateText;

    [SerializeField]
    private float _marginLeft;

    [SerializeField]
    private float _marginTop;

    [SerializeField]
    private float _marginRight;

    [SerializeField]
    private float _marginBottom;

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public string ValueText
    {
        get { return _valueText; }
        set { _valueText = value; }
    }

    public int MaxFontSize { get; set; }

    public int FontSize
	{
		get { return _fontSize; }
		set { _fontSize = value; }
	}

    public int DefaultFontSize
    {
		get { return _defaultFontSize; }
		set { _defaultFontSize = value; }
    }

    public string TextObjectName
    {
        get { return _textObjectName; }
        set { _textObjectName = value; }
    }

    public GameObject TextObject
    {
        get { return _textObject; }
    }

    public Animation TextAnimation
    {
        get { return _textAnimation; }
        set { _textAnimation = value; }
    }

    public ScreenPossition ScreenPosition
    {
        get { return _screenPosition; }
        set { _screenPosition = value; }
    }

    public Quaternion RotateText
    {
        get { return _rotateText; }
        set { _rotateText = value; }
    }

    public Color TextColor
    {
        get { return _textColor; }
        set { _textColor = value; }
    }

    public TextMesh TextMesh
    {
        get { return _textMesh; }
    }

    public AnimationClip GuiAnimation
    {
        get { return _guiAnimation; }
    }

    public bool IsVisible
    {
        get { return TextObject.active; }
        set { TextObject.active = value; }
    }

    void Awake ()
    {
        InitTextObject();
	}

    void Update()
    {
        UpdateText();
        if (_isClickable && IsVisible)
            ClickCatcher();
    }

    void InitTextObject()
    {
        _textObject = new GameObject(_textObjectName);
        _textObject.transform.parent = transform;

        //Get layers int = 4 bytes = 32 bits
        _textObject.layer = _guiLayer.value / 32;

        _textObject.transform.localRotation = RotateText;

        _textAnimation = _textObject.AddComponent<Animation>();
        _textAnimation.playAutomatically = true;
        _textAnimation.AddClip(_guiAnimation, _guiAnimation.name);

        _textMesh = _textObject.AddComponent<TextMesh>();

        _textMesh.font = _textFont;
        _textMesh.renderer.material = _textFont.material;
        _textMesh.characterSize = 0.1f;
        _textMesh.fontSize = _fontSize;
        _textMesh.anchor = TextAnchor.MiddleCenter;
        _textMesh.alignment = TextAlignment.Center;
        _textMesh.color = TextColor;
        MaxFontSize = FontSize + MAX_FONT_SIZE_INDEX;
        DefaultFontSize = FontSize;

        if (!string.IsNullOrEmpty(ValueText))
            _textMesh.text = ValueText;
		if (Value != -1)
			_textMesh.text += Value;
    }

    void UpdateText()
    {
        if (!_textAnimation.isPlaying)
        {
            SetTextScreenPosition(ScreenPosition, _textObject);
            SetMargins(new Margins(_marginLeft, _marginTop, _marginRight, _marginBottom), _textObject);
        }

        _textMesh.fontSize = FontSize;
		if (!string.IsNullOrEmpty(ValueText) && Value != -1)
			_textMesh.text = ValueText + Value;
		if (string.IsNullOrEmpty(ValueText) && Value != -1)
			_textMesh.text = Value.ToString();
		if (!string.IsNullOrEmpty(ValueText) && Value == -1)
			_textMesh.text = ValueText;
    }

    public void PlayAmination()
    {
        _textAnimation.Play(_guiAnimation.name);
    }

    public void StopAnimation()
    {
        if (_textAnimation != null)
        {
            _textAnimation.Stop();
        }
    }

    private void ClickCatcher()
    {
        foreach (Touch touchItem in Input.touches)
        {
            Vector2 touchVector = touchItem.position;
            touchVector.y = Screen.height - touchVector.y;

            if (TextObject.renderer.bounds.Contains(touchVector))
            {
                if (touchItem.phase == TouchPhase.Began)
                    phaseBegan = true;
                if (touchItem.phase == TouchPhase.Ended && phaseBegan)
                {
                    phaseBegan = false;
                    OnTextClick(this);
                }
            }
        }

        #if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
            OnTextClick(this);
        #endif
    }
}
