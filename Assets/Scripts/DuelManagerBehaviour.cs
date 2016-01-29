using UnityEngine;
using System.Collections;

public enum DuelStates
{
    start,
    pistolSelection,
    ready,
    walk,
    turn,
    shoot,
    p1win,
    p2win,
    tie,
    none
}

public class DuelManagerBehaviour : SingletonBehaviour<DuelManagerBehaviour>
{
    protected DuelStates currentState;
    protected float walkTimer, turnTimer;
    protected DuelStates nextState;

    [SerializeField]
    protected float walkTime = 10f;
    [SerializeField]
    protected float turnDelay = 3f;
    [SerializeField]
    protected float turnVariance = 0.5f;

    public DuelStates State
    {
        get { return currentState; }
    }

    public float WalkFraction
    {
        get { return walkTimer; }
    }

	// Use this for initialization
	public void Start()
    {
        currentState = DuelStates.none;
        ChangeStateTo(DuelStates.start);
        nextState = DuelStates.none;
        DontDestroyOnLoad(this.gameObject);
        walkTimer = 0f;
	}
	
	// Update is called once per frame
	void Update()
    {
        //Switch states "next frame"
        if (nextState != DuelStates.none)
        {
            DuelStates temp = nextState;
            nextState = DuelStates.none;
            ChangeStateTo(temp);
        }

        //Temporary input stuff...
        if (Input.GetKeyUp(KeyCode.Space))
        {
            KeyPressed();
        }

        //State specific updates
        switch (currentState)
        {
            case DuelStates.walk: WalkUpdate(); break;
            case DuelStates.turn: TurnUpdate(); break;
        }
	}

    protected void WalkUpdate()
    {
        walkTimer += Time.deltaTime / walkTime;
        if (walkTimer >= 1f)
        {
            ChangeStateNextFrame(DuelStates.turn);
            return;
        }

        Debug.Log("Walk time so far: " + walkTimer.ToString());
    }

    protected void TurnUpdate()
    {
        turnTimer -= Time.deltaTime;
        if (turnTimer < 0f)
        {
            ChangeStateNextFrame(DuelStates.shoot);
            return;
        }

        Debug.Log("Turning... " + turnDelay.ToString());
    }

    protected void KeyPressed()
    {
        switch (currentState)
        {
            case DuelStates.start: ChangeStateTo(DuelStates.ready); break;
            case DuelStates.ready: ChangeStateTo(DuelStates.walk); break;
            case DuelStates.tie:
            case DuelStates.p1win:
            case DuelStates.p2win: ChangeStateTo(DuelStates.start); break;
        }
    }

    protected void ChangeStateNextFrame(DuelStates newState)
    {
        nextState = newState;
    }

    protected void ChangeStateTo(DuelStates newState)
    {
        //Start setup
        if (newState == DuelStates.start)
        {
            Debug.Log("START");
        }

        //Ready setup
        if (newState == DuelStates.ready)
        {
            Debug.Log("Are you ready to duel?");
        }

        //Walk setup
        if (newState == DuelStates.walk)
        {
            walkTimer = 0f;
        }

        //Turn setup
        if (newState == DuelStates.turn)
        {
            Debug.Log("Turn started...");
            turnTimer = turnDelay + Random.Range(-turnVariance, turnVariance);
        }

        //Shoot setup
        if (newState == DuelStates.shoot)
        {
            Debug.Log("BANG!");

            //Pick a random outcome of the fight...
            if (Random.Range(0f, 1f) < 0.3f)
            {
                ChangeStateNextFrame(DuelStates.tie);
            }
            else if (Random.Range(0f, 1f) < 0.5f)
            {
                ChangeStateNextFrame(DuelStates.p1win);
            }
            else
            {
                ChangeStateNextFrame(DuelStates.p2win);
            }
        }

        //Tie setup
        if (newState == DuelStates.tie)
        {
            Debug.Log("Tie! Honour not satisfied!");
        }

        //Player 1 win setup
        if (newState == DuelStates.p1win)
        {
            Debug.Log("Player 1 is satisfied!");
        }

        //Player 2 win setup
        if (newState == DuelStates.p2win)
        {
            Debug.Log("Player 2 is satisfied!");
        }

        //Switch!
        currentState = newState;
    }
}
