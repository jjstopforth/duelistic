using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    protected List<WalkEvent> walkEventsP1, walkEventsP2;

    [SerializeField]
    protected PlayerBehaviour player1, player2;
    [SerializeField]
    protected float walkTime = 10f;
    [SerializeField]
    protected float turnDelay = 3f;
    [SerializeField]
    protected float turnVariance = 0.5f;

    //Input variables
    [Header("Input variables")]
    [SerializeField]
    protected float holdThreshold = 0.15f;
    [SerializeField]
    protected float mashThreshold = 0.2f;
    protected float keyDownP1, keyDownP2, keyUpP1, keyUpP2;

    public DuelStates State
    {
        get { return currentState; }
    }

    public float WalkFraction
    {
        get { return walkTimer; }
    }

    public override void Awake()
    {
        base.Awake();

        walkEventsP1 = new List<WalkEvent>();
        walkEventsP2 = new List<WalkEvent>();
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
            case DuelStates.ready: ReadyUpdate(); break;
            case DuelStates.walk: WalkUpdate(); break;
            case DuelStates.turn: TurnUpdate(); break;
        }
	}

    protected void ReadyUpdate()
    {
        //both players have to hit their keys at the same time...
        if (keyUpP1 >= 0f) keyUpP1 += Time.deltaTime;
        if (keyUpP2 >= 0f) keyUpP2 += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.A))
        {
            keyUpP1 = 0f;
        }
        
        if (Input.GetKeyUp(KeyCode.L))
        {
            keyUpP2 = 0f;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            keyUpP1 = -1f;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            keyUpP2 = -1f;
        }

        if (((keyUpP1 == 0f) && (keyUpP2 < mashThreshold)) || ((keyUpP2 == 0f) && (keyUpP1 < mashThreshold)))
        {
            ChangeStateNextFrame(DuelStates.walk);
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

        //Check player inputs:
        if (keyDownP1 >= 0f) keyDownP1 += Time.deltaTime;
        if (keyDownP2 >= 0f) keyDownP2 += Time.deltaTime;
        if (keyUpP1 >= 0f) keyUpP1 += Time.deltaTime;
        if (keyUpP2 >= 0f) keyUpP2 += Time.deltaTime;
        if (keyDownP1 >= holdThreshold) PlayerHolding(player1);
        if (keyDownP2 >= holdThreshold) PlayerHolding(player2);

        //Player 1
        if (Input.GetKeyDown(KeyCode.A))
        {
            keyDownP1 = 0f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if ((keyDownP1 >= 0f) && (keyDownP1 < holdThreshold))
            {
                if ((keyUpP1 > 0f) && (keyUpP1 < mashThreshold)) PlayerMash(player1);
                else PlayerTap(player1);
            }
            keyUpP1 = 0f;
            keyDownP1 = -1f;
        }
        //Player 2
        if (Input.GetKeyDown(KeyCode.L))
        {
            keyDownP2 = 0f;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            if ((keyDownP2 >= 0f) && (keyDownP2 < holdThreshold))
            {
                if ((keyUpP2 > 0f) && (keyUpP2 < mashThreshold)) PlayerMash(player2);
                else PlayerTap(player2);
            }
            keyUpP2 = 0f;
            keyDownP2 = -1f;
        }

        //Debug.Log("Walk time so far: " + walkTimer.ToString());
    }

    public void PlayerHolding(PlayerBehaviour _player)
    {
        List<WalkEvent> events = walkEventsP1;
        if (_player == player2) events = walkEventsP2;

        foreach (WalkEvent w in events)
        {
            if ((w.Start <= WalkFraction) && (w.Start + w.Duration >= WalkFraction))
            {
                w.Hold(_player);
            }
        }

        Debug.Log("Holding!");
    }

    public void PlayerTap(PlayerBehaviour _player)
    {
        List<WalkEvent> events = walkEventsP1;
        if (_player == player2) events = walkEventsP2;

        foreach (WalkEvent w in events)
        {
            if ((w.Start <= WalkFraction) && (w.Start + w.Duration >= WalkFraction))
            {
                w.Tap(_player);
            }
        }

        Debug.Log("Tapping!");
    }

    public void PlayerMash(PlayerBehaviour _player)
    {
        List<WalkEvent> events = walkEventsP1;
        if (_player == player2) events = walkEventsP2;

        foreach (WalkEvent w in events)
        {
            if ((w.Start <= WalkFraction) && (w.Start + w.Duration >= WalkFraction))
            {
                w.Mash(_player);
            }
        }

        Debug.Log("Mashing!");
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
            //case DuelStates.ready: ChangeStateTo(DuelStates.walk); break;
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

            walkTimer = 0f;

            walkEventsP1.Clear();
            walkEventsP2.Clear();
            player1.ResetStats();
            player2.ResetStats();

            if (Camera.main != null)
            {
                ZoomController zoomController = Camera.main.GetComponent<ZoomController>();
                zoomController.ResetCamera();
            }
        }

        //Ready setup
        if (newState == DuelStates.ready)
        {
            Debug.Log("Are you ready to duel?");

            //Populate the walk events...
            for (int i = 0; i < 10; i++)
            {
                //Just with footsteps for now
                walkEventsP1.Add(new WalkEvent(0.1f * i + Random.Range(-0.01f, 0.01f), 0.05f, WalkEventTypes.footStep, 10f));
                walkEventsP2.Add(new WalkEvent(0.1f * i + Random.Range(-0.01f, 0.01f), 0.05f, WalkEventTypes.footStep, 10f));
            }
            WalkEvent w = new WalkEvent(0f, 1f, WalkEventTypes.hatTwirl);
            w.SetHoldFactors(0, 2, 2f);
            walkEventsP1.Add(w);

            w = new WalkEvent(0f, 1f, WalkEventTypes.hatTwirl);
            w.SetHoldFactors(0, 3, 3f);
            walkEventsP2.Add(w);

            w = new WalkEvent(0f, 1f, WalkEventTypes.owlSalute);
            w.SetMashFactors(Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f));
            walkEventsP1.Add(w);

            w = new WalkEvent(0f, 1f, WalkEventTypes.owlSalute);
            w.SetMashFactors(Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f));
            walkEventsP2.Add(w);

            walkEventsP1.Sort();
            walkEventsP2.Sort();
        }

        //Walk setup
        if (newState == DuelStates.walk)
        {
            walkTimer = 0f;

            //Reset keyboard inputs:
            keyDownP1 = -1f;
            keyDownP2 = -1f;
            keyUpP1 = -1f;
            keyUpP2 = -1f;

            Debug.Log("Duelling! (banjos)");
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
