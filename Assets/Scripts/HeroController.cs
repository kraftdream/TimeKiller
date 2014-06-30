using System.Threading;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class HeroController : MonoBehaviour
{

    private enum axis { Left, Rigth, Up, Down };

    public Joystick joyStickLeft;
    private Animator heroAnimation;
    private bool isFacingRight = true;

    private static string HERO_POSITION_X = "PositionX";
    private static string HERO_POSITION_Y = "PositionY";
    private static string HERO_TOP = "Top";
    private static string HERO_BOTTOM = "Bottom";
    private int[] moveAxis; //left, right, up, down

    private Vector3 heroParams;
    private Vector3 bordersParams;
    private Vector3 textureParams;

	[Range(1, 20)]
	public float moveForce;
    [Range(0, 1)]
    public float borderLeftRightWitdh;
    [Range(0, 1)]
    public float borderUpDownWitdh;

    [SerializeField]
    private GameObject GUI;

    private My3DText _scoreText;
    private My3DText _comboText;

    private float _comboTime;
    private float _stopComboTime;

    void Awake() 
    {
        moveAxis = new[] { 1, 1, 1, 1 };

        heroAnimation = GetComponent<Animator>();

        //combo will stop after 1 sec
        _stopComboTime = 1.0f;

		if (joyStickLeft == null)
			Debug.LogError ("Please set the joystick prefab!");

        foreach (My3DText guiScripts in GUI.GetComponents<My3DText>())
        {
            if (guiScripts.TextObjectName.Equals("Score"))
                _scoreText = guiScripts;
            if (guiScripts.TextObjectName.Equals("Combo"))
                _comboText = guiScripts;
        }
    }

    void Update()        
    {

        heroParams = transform.position;
        bordersParams = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float positionX = joyStickLeft.position.x;
        float positionY = joyStickLeft.position.y;
        textureParams = gameObject.GetComponent<SpriteRenderer>().bounds.size;

        if (joyStickLeft.position.x > 0.0f && heroParams.x + textureParams.x / 2 > bordersParams.x - borderLeftRightWitdh)
            moveAxis[(int)axis.Rigth] = 0;
        else
            moveAxis[(int)axis.Rigth] = 1;
            
        heroAnimation.SetFloat(HERO_POSITION_X, Mathf.Abs(positionX));
        heroAnimation.SetFloat(HERO_POSITION_Y, Mathf.Abs(positionY));

        ChangeAnimationPosition(positionX, positionY);

        if (positionX > 0 && !isFacingRight)
            Flip();
        else if (positionX < 0 && isFacingRight)
            Flip();

        if (joyStickLeft.position.x < 0.0f && heroParams.x - textureParams.x / 2 < borderLeftRightWitdh - bordersParams.x)
            moveAxis[(int)axis.Left] = 0;
        else
            moveAxis[(int)axis.Left] = 1;

        if (joyStickLeft.position.y > 0.0f && heroParams.y + textureParams.y / 2 > bordersParams.y - borderUpDownWitdh)
            moveAxis[(int)axis.Up] = 0;
        else
            moveAxis[(int)axis.Up] = 1;

        if (joyStickLeft.position.y < 0.0f && heroParams.y - textureParams.y / 2 < borderUpDownWitdh - bordersParams.y)
            moveAxis[(int)axis.Down] = 0;
        else
            moveAxis[(int)axis.Down] = 1;

		if (joyStickLeft != null)
            gameObject.transform.Translate(new Vector3(moveAxis[(int)axis.Left] * moveAxis[(int)axis.Rigth] * joyStickLeft.position.x * Time.deltaTime * moveForce, moveAxis[(int)axis.Up] * moveAxis[(int)axis.Down] * joyStickLeft.position.y * Time.deltaTime * moveForce, 0));

        //implement to stop combo
        if (_comboTime + _stopComboTime < Time.time)
        {
			_comboText.StopAnimation();
			_comboText.FontSize = _comboText.DefaultFontSize;
			_comboText.Value = 0;
        }
	}
    
     private void ChangeAnimationPosition(float positionX, float positionY)
    {
        heroAnimation.SetBool(HERO_TOP, false);
        heroAnimation.SetBool(HERO_BOTTOM, false);

        if (positionX <= 0 && positionY <= 0)
        {
            if (positionX > positionY)
                heroAnimation.SetBool(HERO_BOTTOM, true);
            else
                heroAnimation.SetBool(HERO_BOTTOM, false);
        }
        else if (positionX <= 0 && positionY >= 0)
        {
            if (Mathf.Abs(positionX) < Mathf.Abs(positionY))
                heroAnimation.SetBool(HERO_TOP, true);
            else
                heroAnimation.SetBool(HERO_TOP, false);
        }
        else if (positionX >= 0 && positionY >= 0)
        {
            if (positionX < positionY)
                heroAnimation.SetBool(HERO_TOP, true);
            else
                heroAnimation.SetBool(HERO_TOP, false);
        }
        else if (positionX >= 0 && positionY <= 0)
        {
            if (Mathf.Abs(positionX) < Mathf.Abs(positionY))
                heroAnimation.SetBool(HERO_BOTTOM, true);
            else
                heroAnimation.SetBool(HERO_BOTTOM, false);
        }
    }


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
	}

    //Hero collision
    /*void OnTriggerEnter2D(Collider2D other)
    {
        //if hero complete attack
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemyScript = other.GetComponent<Enemy>();
            _scoreText.Value = enemyScript.ScoreValue + _scoreText.Value + _comboText.Value;
            _comboText.Value = _comboText.Value + 1;
            _comboTime = Time.time;
			_scoreText.PlayAmination();
			_comboText.PlayAmination();

			if (_comboText.FontSize < _comboText.MaxFontSize)
				_comboText.FontSize += 10;
        }
        //if hero dead
        //ToDO Hero dead
    }*/
}
