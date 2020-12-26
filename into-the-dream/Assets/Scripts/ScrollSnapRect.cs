using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
[RequireComponent(typeof(ScrollRect))]
public class ScrollSnapRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Tooltip("Set starting page index - starting from 0")]
    public int startingPage = 0;
    [Tooltip("Threshold time for fast swipe in seconds")]
    public float fastSwipeThresholdTime = 0.3f;
    [Tooltip("Threshold time for fast swipe in (unscaled) pixels")]
    public int fastSwipeThresholdDistance = 100;
    [Tooltip("How fast will page lerp to target position")]
    public float decelerationRate = 10f;
    [Tooltip("Button to go to the previous page (optional)")]
    public GameObject prevButton;
    [Tooltip("Button to go to the next page (optional)")]
    public GameObject nextButton;
    [Tooltip("Sprite for unselected page (optional)")]
    public Sprite unselectedPage;
    [Tooltip("Sprite for selected page (optional)")]
    public Sprite selectedPage;
    [Tooltip("Container with page images (optional)")]
    public Transform pageSelectionIcons;

    public static bool CheckGameStatus;

    // fast swipes should be fast and short. If too long, then it is not fast swipe
    private int _fastSwipeThresholdMaxLimit;

    private ScrollRect _scrollRectComponent;
    private RectTransform _scrollRectRect;
    private RectTransform _container;

    private bool _horizontal;
    
    // number of pages in container
    private int _pageCount;
    private int _currentPage;

    // whether lerping is in progress and target lerp position
    private bool _lerp;
    private Vector2 _lerpTo;

    // target position of every page
    private List<Vector2> _pagePositions = new List<Vector2>();

    // in draggging, when dragging started and where it started
    private bool _dragging;
    private float _timeStamp;
    private Vector2 _startPosition;

    // for showing small page icons
    private bool _showPageSelection;
    private int _previousPageSelectionIndex;
    // container with Image components - one Image for each page
    private List<Image> _pageSelectionImages;

    public GameObject BackgroundStar;
    //------------------------------------------------------------------------
    void Start()
    {
        _scrollRectComponent = GetComponent<ScrollRect>();
        _scrollRectRect      = GetComponent<RectTransform>();
        _container           = _scrollRectComponent.content;
        _pageCount           = _container.childCount;

        SongData songData = ItdData.Song.getSongData(ItdData.User.CurrentSongCode);

        // is it horizontal or vertical scrollrect
        if (_scrollRectComponent.horizontal && !_scrollRectComponent.vertical)
        {
            _horizontal = true;
        }
        else if (!_scrollRectComponent.horizontal && _scrollRectComponent.vertical)
        {
            _horizontal = false;
        }
        else
        {
            Debug.LogWarning("Confusing setting of horizontal/vertical direction. Default set to horizontal.");
            _horizontal = true;
        }

        _lerp = false;

        // init
        SetPagePositions();
        if (CheckGameStatus) startingPage = songData.Chapter;
        SetPage(startingPage);
        InitPageSelection();
        SetPageSelection(startingPage);

        // prev and next buttons
        if (nextButton)
        {
            nextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });
        }

        if (prevButton)
        {
            prevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });
        }
	}

    //------------------------------------------------------------------------
    void Update()
    {
        // if moving to target position
        if (_lerp)
        {
            // prevent overshooting with values greater than 1
            float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            _container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);
            // time to stop lerping?
            if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f)
            {
                // snap to target and stop lerping
                _container.anchoredPosition = _lerpTo;
                _lerp = false;
                // clear also any scrollrect move that may interfere with our lerping
                _scrollRectComponent.velocity = Vector2.zero;
            }

            // switches selection icon exactly to correct page
            if (_showPageSelection)
            {
                SetPageSelection(GetNearestPage());
            }
        }
        if(SongInfo.toggle == true)
        {
            gameObject.GetComponent<ScrollRect>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<ScrollRect>().enabled = true;
        }
        focusOn();
    }


    //------------------------------------------------------------------------
    private void SetPagePositions()
    {
        int width           = 0;
        int height          = 0;
        int offsetX         = 0;
        int offsetY         = 0;
        int containerWidth  = 0;
        int containerHeight = 0;

        if (_horizontal)
        {
            width                       = (int)_scrollRectRect.rect.width; // screen width in pixels of scrollrect window
            offsetX                     = width / 2;                       // center position of all pages
            containerWidth              = width * _pageCount;              // total width
            _fastSwipeThresholdMaxLimit = width;                           // limit fast swipe length - beyond this length it is fast swipe no more
        }
        else
        {
            height                      = (int)_scrollRectRect.rect.height;
            offsetY                     = height / 2;
            containerHeight             = height * _pageCount;
            _fastSwipeThresholdMaxLimit = height;
        }

        // set width of container
        Vector2 newSize             = new Vector2(containerWidth * 2, containerHeight);
        _container.sizeDelta        = newSize;
        Vector2 newPosition         = new Vector2(containerWidth /2 , containerHeight / 2);
        _container.anchoredPosition = newPosition;

        _pagePositions.Clear(); // delete any previous settings

        // iterate through all container childern and set their positions
        for (int i = 0; i < _pageCount; i++)
        {
            RectTransform child = _container.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition;
            if (_horizontal)
            {
                childPosition = new Vector2(i * width, 0f);
                ////////////////////////////////////// *-* ////////////////////////////////////
                // i가 0번째 일때 (0) - (모든 패널의 크기 / 2) + (패널 하나의 크기 / 2) 이므로 첫번째 패널이 음수좌표에 표시가 되던것.
                // 2000 * i - 2000 * 5 / 2  + 1000
                // 2000 * i - 5000 + 1000
                // 0, 2000
                // 이것을 고치려면
                // childPosition = new Vector2(i * width + offsetX, 0f);로 두어야 할 것 같다.
            }
            else
            {
                childPosition = new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
                // 세로의 경우 쓸일 없겠지만
                // childPosition = new Vector2(0f, -(i * hight + offsetY));
            }
            child.anchoredPosition = childPosition;
            _pagePositions.Add(-childPosition);
        }
    }

    //------------------------------------------------------------------------
    private void SetPage(int aPageIndex)
    {
        aPageIndex                  = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
        _container.anchoredPosition = _pagePositions[aPageIndex];
        _currentPage                = aPageIndex;
    }

    //------------------------------------------------------------------------
    private void LerpToPage(int aPageIndex)
    {
        aPageIndex   = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
        _lerpTo      = _pagePositions[aPageIndex];
        _lerp        = true;
        _currentPage = aPageIndex;
    }

    //------------------------------------------------------------------------
    private void InitPageSelection()
    {
        // page selection - only if defined sprites for selection icons
        _showPageSelection = unselectedPage != null && selectedPage != null;
        if (_showPageSelection)
        {
            // also container with selection images must be defined and must have exatly the same amount of items as pages container
            if (pageSelectionIcons == null || pageSelectionIcons.childCount != _pageCount)
            {
                Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                _showPageSelection = false;
            }
            else
            {
                _previousPageSelectionIndex = -1;
                _pageSelectionImages        = new List<Image>();

                // cache all Image components into list
                for (int i = 0; i < pageSelectionIcons.childCount; i++)
                {
                    Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                    if (image == null)
                    {
                        Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
                    }
                    _pageSelectionImages.Add(image);
                }
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPageSelection(int aPageIndex)
    {
        // nothing to change
        if (_previousPageSelectionIndex == aPageIndex)
        {
            return;
        }
        
        // unselect old
        if (_previousPageSelectionIndex >= 0)
        {
            _pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
            _pageSelectionImages[_previousPageSelectionIndex].SetNativeSize();
        }

        // select new
        _pageSelectionImages[aPageIndex].sprite = selectedPage;
        _pageSelectionImages[aPageIndex].SetNativeSize();

        _previousPageSelectionIndex = aPageIndex;
    }

    //------------------------------------------------------------------------
    private void NextScreen()
    {
        LerpToPage(_currentPage + 1);
    }

    //------------------------------------------------------------------------
    private void PreviousScreen()
    {
        LerpToPage(_currentPage - 1);
    }

    //------------------------------------------------------------------------
    private int GetNearestPage()
    {
        // based on distance from current position, find nearest page
        Vector2 currentPosition = _container.anchoredPosition;

        float distance  = float.MaxValue;
        int nearestPage = _currentPage;

        for (int i = 0; i < _pagePositions.Count; i++) {
            // 현재 컨테이너의 중앙
            float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);
            if (testDist < distance) {
                distance    = testDist;
                nearestPage = i;
            }
        }

        return nearestPage;
    }

    //------------------------------------------------------------------------
    public void OnBeginDrag(PointerEventData aEventData)
    {
        _lerp     = false; // if currently lerping, then stop it as user is draging
        _dragging = false; // not dragging yet
    }

    //------------------------------------------------------------------------
    public void OnEndDrag(PointerEventData aEventData)
    {
        // how much was container's content dragged
        float difference;
        if (_horizontal)
        {
            difference = _startPosition.x - _container.anchoredPosition.x;
        }
        else
        {
            difference = - (_startPosition.y - _container.anchoredPosition.y);
        }

        // test for fast swipe - swipe that moves only +/-1 item
        if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime     &&
            Mathf.Abs(difference)          > fastSwipeThresholdDistance &&
            Mathf.Abs(difference)          < _fastSwipeThresholdMaxLimit)
        {
            if (difference > 0)
            {
                NextScreen();
            }
            else
            {
                PreviousScreen();
            }
        }
        else
        {
            // if not fast time, look to which page we got to
            LerpToPage(GetNearestPage());
        }

        _dragging = false;
    }

    //------------------------------------------------------------------------
    public void OnDrag(PointerEventData aEventData)
    {
        if (!_dragging)
        {
            _dragging      = true; // dragging started
            _timeStamp     = Time.unscaledTime; // save time - unscaled so pausing with Time.scale should not affect it
            _startPosition = _container.anchoredPosition; // save current position of cointainer
        }
        else
        {
            if (_showPageSelection)
            {
                SetPageSelection(GetNearestPage());
            }
        }
    }

    public void focusOn() {
        if (_currentPage == 0) {
            BackgroundStar.SetActive(false);
        }
        else {
            BackgroundStar.SetActive(true);
        }
    }
}
