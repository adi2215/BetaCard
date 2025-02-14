using UnityEngine;

public class GameStageManager : MonoBehaviour
{
    public BallAutoMove autoMove;
    public DragMove dragMove;
    public EditableLine editableLine;
   //public GameObject hintLetter;
    private int currentStage = 1;

    public static GameStageManager Instance { get; private set; }

    public TrailBall trailManager;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartPlay()
    {

        SetStage(1);
    }

    public void NextStage()
    {
        currentStage++;
        trailManager.ClearTrails();
        SetStage(currentStage);
    }

    private void SetStage(int stage)
    {
        switch (stage)
        {
            case 1:
                autoMove.enabled = true;
                dragMove.enabled = false;
                autoMove.StartMoving();
                break;
            case 2: 
                autoMove.enabled = false;
                dragMove.enabled = true;
                dragMove.ResetPosition();
                editableLine.SetLineVisibility(true);
                break;
            case 3: 
                dragMove.ResetPosition();
                editableLine.SetLineVisibility(false);
                break;
            case 4:
                dragMove.enabled = false;
                Debug.Log("You Win");
                break;
        }
    }
}
