using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MobileControls : MonoBehaviour
{
    public TMP_Text phaseInfo;
    public GameObject movablePic, backPic, bioBackground, bioText;
    public Sprite[] profilePics;
    public Sprite Match;

    public float swipeSpeed, scrollSpeed;
    public float swipeThreshold = 500f;
    public float endPosL, endPosR, returnL, returnR;
    public float lerpTime, bioBGLerp, bioTLerp;
    public float timelasped;
    public float movementSpeed;
    public Vector3 bioPosition;
    public Vector3 bioTextPosition;
    public float textTop;
    public PlayableDirector matchclip;

    private int index = 0;
    private float t = 0f;
    private int dir;
    private Touch userInput;
    private Vector2 touchStart, touchEnd;
    private float swipeXEnd, swipeYEnd;
    private bool swiped = false;
    private bool bioOpened, bioSwipe;
    private Vector3 endPoint, originPosition;
    private float Tpercentage, BGpercentage;
    private bool posOffcourse = false;
    private float miny, maxY;

    // Start is called before the first frame update
    void Start()
    {
        endPoint = movablePic.transform.position;
        originPosition = movablePic.transform.position;
        movablePic.GetComponent<SpriteRenderer>().sprite = profilePics[index];
        backPic.GetComponent<SpriteRenderer>().sprite = profilePics[index+1];
        index++;
        bioOpened = false;
        bioSwipe = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            userInput = Input.GetTouch(0);

            if (userInput.phase == TouchPhase.Began)
            {
                phaseInfo.text = userInput.phase.ToString();
                touchStart = userInput.position;
                float x = touchStart.x + movablePic.transform.position.x;
                phaseInfo.text = userInput.phase.ToString();
            }

            if(userInput.phase == TouchPhase.Moved)
            {
                touchEnd = userInput.position;

                float x = touchStart.x - touchEnd.x;
                float y = touchStart.y - touchEnd.y;
                swipeXEnd = x * swipeSpeed;
                swipeYEnd = y * scrollSpeed;
                movablePic.transform.position = new Vector3(swipeXEnd, 0);
                phaseInfo.text = userInput.phase.ToString() + '\n' + x + '\n' + y;
                

                if (bioSwipe)
                {
                    //if(bioText.transform.position.y >= 650 || bioText.transform.position.y <= 2250)
                    bioText.transform.position = new Vector3(bioText.transform.position.x, /*userInput.position.y*/ bioText.transform.position.y + swipeYEnd /*newTextPosition*/);
                    //bioText.transform.Translate(new Vector3(0, swipeYEnd));
                }

            }

            if(userInput.phase == TouchPhase.Stationary)
            {
                if(bioSwipe)
                touchStart = userInput.position;
            }

            if (userInput.phase == TouchPhase.Ended)
            {
                //Debug.Log(swipeEnd);
                if ((swipeXEnd / swipeSpeed) >= swipeThreshold)
                {
                    timelasped = Time.time;
                    //Debug.Log("L");
                    swiped = true;
                    dir = -1;
                    endPoint.x = endPosL;
                }
                else if ((swipeXEnd / swipeSpeed) <= -swipeThreshold)
                {
                    timelasped = Time.time;
                    //Debug.Log("R");
                    swiped = true;
                    dir = 1;
                    endPoint.x = endPosR;
                }
                else
                {
                    movablePic.transform.position = Vector3.zero;
                }

                //movableObject.transform.position = Vector3.zero;
                phaseInfo.text = userInput.phase.ToString();
            }
        }

        if (swiped)
        {
            float _time = Time.time - timelasped;
            float percentage = _time / lerpTime;
            movablePic.transform.position = Vector3.Lerp(movablePic.transform.position, endPoint, percentage);

            if(dir == 1)
            {
                //play timeline
                if(movablePic.GetComponent<SpriteRenderer>().sprite == Match)
                    matchclip.Play();
            }

            if ((movablePic.transform.position.x <= endPosL) || (movablePic.transform.position.x >= endPosR))
            {
                swiped = false;
                movablePic.GetComponent<SpriteRenderer>().sprite = backPic.GetComponent<SpriteRenderer>().sprite;
                if (index + 1 > profilePics.Length-1)
                {
                    index = 1;
                }
                else { index++; }
                Debug.Log(index);
                backPic.GetComponent<SpriteRenderer>().sprite = profilePics[index];
                movablePic.transform.position = originPosition;
            }

            
        }

        if (bioOpened)
        {
            float _BGtime = Time.time - timelasped;
            float _Ttime = Time.time - timelasped;
            BGpercentage = _BGtime / bioBGLerp;
            Tpercentage = _Ttime / bioTLerp;
            bioBackground.transform.position = Vector3.Lerp(bioBackground.transform.position, bioPosition, BGpercentage);
            bioText.transform.position = Vector3.Lerp(bioText.transform.position, bioTextPosition, Tpercentage);
            if (Tpercentage >= 0.1)
            {
                bioOpened = false;
                bioSwipe = true;
            }
        }

    }

    public void SkipToMatch()
    {
        matchclip.Play();
    }
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenBio()
    {
        timelasped = Time.time;
        bioOpened = true;
    }

}
