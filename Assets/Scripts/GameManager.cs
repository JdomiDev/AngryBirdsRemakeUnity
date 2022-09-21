using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{


    // vars

    public CameraFollow cameraFollow;
    public SlingShot slingshot;

    [HideInInspector]
    public static GameState CurrentGameState = GameState.Start;

    public static List<GameObject> Bricks;
    public static List<GameObject> Birds;
    public static List<GameObject> BirdsCopy;
    public static List<GameObject> Pigs;
    public static List<GameObject> AliveBirds;

    public static int currentBirdIndex;

    [HideInInspector]
    public List<Vector2> vectors;
    [HideInInspector]
    public List<Vector2> vectorsCopy;

    public static int birdsNumber;



    // voids 
    private void Start()
    {
        // initialize slingshot and game state

        CurrentGameState = GameState.Start;
        slingshot.enabled = true;

        slingshot.BirdThrown -= Slingshot_BirdThrown; slingshot.BirdThrown += Slingshot_BirdThrown;
        birdList();

        foreach (var obj in Birds)
        {
            vectors.Add(obj.transform.position);
        }

        vectorsCopy = vectors;
    }


    private void Awake()
    {

        //reset static vars

        AliveBirds = null;
        vectors = null;
        vectorsCopy = null;
        Bricks = null;
        Birds = null;
        BirdsCopy = null;
        Pigs = null;
        currentBirdIndex = 0;

        // define lists

        AliveBirds = new List<GameObject>();
        Bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        Pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));

        Birds = new List<GameObject>();
        BirdsCopy = new List<GameObject>();
        vectors = new List<Vector2>();
        vectorsCopy = new List<Vector2>();


        // change physics back to normal

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
    }

    private void Update()
    {

        if (AllPigsDestroyed())
        {
            CurrentGameState = GameState.Won;
        }

        switch (CurrentGameState)
        {
            case GameState.Won:
                StartCoroutine(cameMoveDelayCour());
                break;
            case GameState.Lost:
                StartCoroutine(cameMoveDelayCour());
                break;
            case GameState.Start:
                AnimateBirdToSlingshot();
                break;
            case GameState.BirdMovingToSlingshot:
                break;
            case GameState.Playing:
                if (slingshot.slingshotState == SlingshotState.BirdFlying && (BricksBirdsPigsStoppedMoving() || Time.time - slingshot.TimeSinceThrown > 5f))
                {
                    StartCoroutine(delayCour());
                }
                break;
            default:
                break;
        }
    }

    private void birdList()
    {
        List<GameObject> objectsWithTag = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));

        List<int> birdList = new List<int>();
        List<int> birdListCopy = new List<int>();
        List<int> originalIndex = new List<int>();

        foreach (var obj in objectsWithTag)
        {
            birdList.Add(obj.GetComponent<Bird>().birdOrder);
            birdListCopy.Add(obj.GetComponent<Bird>().birdOrder);
        }

        birdList.Sort();

        int index;

        foreach (var obj in birdList)
        {
            index = 0;
            foreach (var obj2 in birdListCopy)
            {
                if (obj == obj2)
                {
                    originalIndex.Add(index);
                    break;
                }
                index++;
            }
        }
        foreach (var obj in originalIndex)
        {
            Birds.Add(objectsWithTag[obj]);
            BirdsCopy.Add(objectsWithTag[obj]);
        }
        birdsNumber = Birds.Count;

    }
    
    private void AnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 2.5f;
        if (duration == 0.0f) duration = 0.1f;
        //animate the camera to start
        Camera.main.transform.DOMove(cameraFollow.StartingPosition, duration);

        cameraFollow.IsFollowing = false;

        if (AllPigsDestroyed())
        {
            CurrentGameState = GameState.Won;
        }
        else if (currentBirdIndex == Birds.Count - 1 && CurrentGameState != GameState.Won && birdsNumber == 0)
        {
            CurrentGameState = GameState.Lost;
        }
        else
        {
            slingshot.slingshotState = SlingshotState.Idle;
            currentBirdIndex++;
            AnimateBirdToSlingshot();
        }
    }

    private void AnimateBirdToSlingshot()
    {
        StartCoroutine(moveBirdToNextPost());
        CurrentGameState = GameState.BirdMovingToSlingshot;
        AudioPlayer.audio.PlayOneShot(Birds[currentBirdIndex].GetComponent<Bird>().selectSound[0]);
        Birds[currentBirdIndex].transform.DOJump(slingshot.BirdWaitPosition.transform.position, 2f, 1, Vector2.Distance(Birds[currentBirdIndex].transform.position, slingshot.BirdWaitPosition.transform.position) / 2f).OnComplete(() => completedAnimate());
    }
    private void completedAnimate()
    {
        CurrentGameState = GameState.Playing;
        slingshot.enabled = true;
        slingshot.BirdToThrow = Birds[currentBirdIndex];
        Birds[currentBirdIndex].GetComponent<AudioSource>().PlayOneShot(Birds[currentBirdIndex].GetComponent<Bird>().selectSound[0]);
    }
    private void Slingshot_BirdThrown(object sender, System.EventArgs e)
    {
        cameraFollow.BirdToFollow = Birds[currentBirdIndex].transform;
        cameraFollow.IsFollowing = true;
    }


    // var voids


    private bool AllPigsDestroyed()
    {
        return Pigs.All(x => x == null);
    }

    public static bool BricksBirdsPigsStoppedMoving()
    {
        foreach (var item in Bricks.Union(Birds).Union(Pigs))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.MinVelocity)
            {
                return false;
            }
        }

        return true;
    }

    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }


    // enums 


    private IEnumerator cameMoveDelayCour()
    {
        yield return new WaitForSeconds(1.5f);
        AnimateCameraToStartPosition();
    }


    private IEnumerator delayCour()
    {
        slingshot.enabled = true;
        if (currentBirdIndex == Birds.Count - 1 && CurrentGameState != GameState.Won && birdsNumber == 0)
        {
            CurrentGameState = GameState.Lost;
        }
        else
        {
            CurrentGameState = GameState.BirdMovingToSlingshot;
        }
        yield return new WaitForSeconds(1f);
        AnimateCameraToStartPosition();
    }



    public IEnumerator moveBirdToNextPost()
    {
        BirdsCopy.RemoveAt(0);
        int birdCount = BirdsCopy.Count();
        int birdIndex = 0;
        bool isRunning = false;

        yield return new WaitForSeconds(0.4f);

        void isRunningComplete()
        {
            isRunning = false;
        }

        while (birdCount > birdIndex)
        {
            yield return new WaitForSeconds(0.1f);

            if (!isRunning)
            {
                isRunning = true;
                BirdsCopy[birdIndex].transform.DOJump(vectorsCopy[birdIndex], 2f, 1, Vector2.Distance(BirdsCopy[birdIndex].transform.position, vectorsCopy[birdIndex]) / 2f).OnComplete(() => isRunningComplete());
                birdIndex++;
            }
        }
    }
}
