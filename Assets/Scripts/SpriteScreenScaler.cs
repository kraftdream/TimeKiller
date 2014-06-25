using UnityEngine;
using System.Collections;

public class SpriteScreenScaler : MonoBehaviour {

    private float mScreenWidth = 0;
    private float mScreenHeight = 0;
    private float mScreenAspectRatio = 0;

    private float mSpriteWidth = 0;
    private float mSpriteHeight = 0;
    private float mSpriteAspectRatio = 0;

    private bool isSprite = false;
    private SpriteRenderer mSpriteRenderer = null;

    private float coeffScale = 0;

    void Awake() {
        if ((mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>()) != null) {
            isSprite = true;
        }

        if (isSprite) {

            mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            mScreenWidth = Screen.width;
            mScreenHeight = Screen.height;

            mScreenAspectRatio = mScreenWidth / mScreenHeight;

            mSpriteWidth = mSpriteRenderer.sprite.bounds.size.x;
            mSpriteHeight = mSpriteRenderer.sprite.bounds.size.y;

            mSpriteAspectRatio = mSpriteWidth / mSpriteHeight;

            mSpriteHeight = 0;
            mSpriteAspectRatio = 0;

            //mSpriteRenderer.sprite = Sprite.Create(mSpriteRenderer.sprite.texture, )
        }
        else {
            Debug.LogError("This component need the sprite gameobject!");
        }
    }
}
