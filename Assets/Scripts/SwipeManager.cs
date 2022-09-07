using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[System.Serializable]

public enum Swipe { None, Up, Down, Left, Right };
public class SwipeManager : MonoBehaviour
{
    private Touch playerTouch;
    private Vector2 startPos, currentPos, touchDif;
    private float swipeSensitivity = 50f;
    public Swipe swipeDirection;
    private bool movedOnce = false;

    private List<int> boundaryAngles = new List<int> {60, 150, 210, 300};
        private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    void Start()
    {
        startPos = Vector2.zero;
        currentPos = Vector2.zero;
    }

    // https://gamedev-resources.com/implementing-touch-with-input-systems-enhanced-touch-api/
    void FixedUpdate()
    {
        if (Touch.activeFingers.Count > 0)
        {
            playerTouch = Touch.activeFingers[0].currentTouch;
            if (playerTouch.phase == TouchPhase.Began)
            {
                startPos = playerTouch.screenPosition;
            }
            else if (playerTouch.phase == TouchPhase.Moved)
            {
                if (!movedOnce) CalculateSwipe();
                else swipeDirection = Swipe.None;
            }
            else if (playerTouch.phase == TouchPhase.Ended)
            {
                movedOnce = false;
            }
        }
        else
        {
            swipeDirection = Swipe.None;
        }
    }

    void CalculateSwipe()
    {
        currentPos = playerTouch.screenPosition;
        touchDif = (currentPos - startPos);
        if(Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
        {
            if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
            {
                swipeDirection = Swipe.Up;
            }
            else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
            {
                swipeDirection = Swipe.Down;
            }
            else if(touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
            {
                swipeDirection = Swipe.Right;
            }
            else if(touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
            {
                swipeDirection = Swipe.Left;
            }

            movedOnce = true;
        }
        //터치.
        else
        {
            swipeDirection = Swipe.None;
        }
        //Debug.Log($"Swipe: {swipeDirection}");
    }
    
    
}
