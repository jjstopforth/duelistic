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
    protected float walkTimer, prevWalkTimer, turnTimer;
    protected DuelStates nextState;
    protected GameObject alPrompt;

    protected List<WalkEvent> walkEventsP1, walkEventsP2;

    [SerializeField]
    protected PlayerBehaviour player1, player2;
    [SerializeField]
    protected float walkTime = 10f;
    [SerializeField]
    protected float turnDelay = 3f;
    [SerializeField]
    protected float turnVariance = 0.5f;

	//Pistol Selection vars
    public HandController pistolSelector;


    //Input variables
    [Header("Input variables")]
    [SerializeField]
    protected float holdThreshold = 0.15f;
    [SerializeField]
    protected float mashThreshold = 0.2f;
    protected float keyDownP1, keyDownP2, keyUpP1, keyUpP2;

    //Animations?
    [Header("Animations...")]
    public GameObject footstepAnim = null;
    public GameObject windAnim = null;
    public GameObject rabbitAnim = null;
    public GameObject hatAnim = null;
    public GameObject handAnim = null;

    //Prompts...
    [Header("Prompts...")]
    public GameObject pressALprompt = null;


    public SoundManager SoundManager = null;


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
        DontDestroyOnLoad(this.gameObject);
        walkTimer = 0f;

        currentState = DuelStates.none;
        ChangeStateTo(DuelStates.start);
        nextState = DuelStates.none;
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
            case DuelStates.pistolSelection: PistolSelectionUpdate(); break;
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

        if (keyUpP1 + keyUpP2 < 0f) //Prevent auto-progression...
        {
            if (((keyUpP1 == 0f) && (keyUpP2 < mashThreshold)) || ((keyUpP2 == 0f) && (keyUpP1 < mashThreshold)))
            {
                ChangeStateNextFrame(DuelStates.walk);
            }
        }
    }

    protected void PistolSelectionUpdate ()
	{
		//Do pistol input stuff here, woo!
		if (Input.GetKeyDown (KeyCode.A))
        {
			pistolSelector.SetHandTarget ("player1");
		}
        else if (Input.GetKeyUp (KeyCode.A))
        {
			pistolSelector.ResetPlayerHand("player1");
		}

		if (Input.GetKeyDown (KeyCode.L))
        {
			pistolSelector.SetHandTarget("player2");
		}
        else if (Input.GetKeyUp (KeyCode.L))
        {
			pistolSelector.ResetPlayerHand("player2");
		}

		//At some point:
		if ((pistolSelector.Player1DecisionConfidence >= 1f) && (pistolSelector.Player2DecisionConfidence >= 1f)) 
		{
			ChangeStateNextFrame(DuelStates.ready);
            pistolSelector.gameObject.SetActive(false);
		}
    }

    protected void WalkUpdate()
    {
        prevWalkTimer = walkTimer;
        walkTimer += Time.deltaTime / walkTime;

        //Check for active events...
        foreach (WalkEvent w in walkEventsP1)
        {
            if (w.Start <= walkTimer)
            {
                if (w.Start > prevWalkTimer)
                {
                    //First frame this has been live in, trigger it!
                    w.StartEvent(player1);
                }

                if (w.Start + w.Duration > walkTimer)
                {
                    w.UpdateEvent(player1);
                }
                else if (w.Start + w.Duration > prevWalkTimer)
                {
                    w.EndEvent(player1);
                }
            }
        }
        foreach (WalkEvent w in walkEventsP2)
        {
            if (w.Start <= walkTimer)
            {
                if (w.Start > prevWalkTimer)
                {
                    //First frame this has been live in, trigger it!
                    w.StartEvent(player2);
                }

                if (w.Start + w.Duration > walkTimer)
                {
                    w.UpdateEvent(player2);
                }
                else if (w.Start + w.Duration > prevWalkTimer)
                {
                    w.EndEvent(player2);
                }
            }
        }

        //Move on to next state...
        if (walkTimer >= 1f)
        {
            ChangeStateNextFrame(DuelStates.turn);
            //return;
        }

        //Check player inputs:
        if (keyDownP1 >= 0f) keyDownP1 += Time.deltaTime;
        if (keyDownP2 >= 0f) keyDownP2 += Time.deltaTime;
        if (keyUpP1 >= 0f) keyUpP1 += Time.deltaTime;
        if (keyUpP2 >= 0f) keyUpP2 += Time.deltaTime;
        if (keyDownP1 >= holdThreshold) PlayerHolding(player1);
        if (keyDownP2 >= holdThreshold) PlayerHolding(player2);

        //Player 1 input
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

        //Player 2 input
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

        //Debug.Log("Turning... " + turnDelay.ToString());
    }

    protected void KeyPressed()
    {
        switch (currentState)
        {
            case DuelStates.start: ChangeStateTo(DuelStates.pistolSelection); break;
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

            SoundManager.PlayMusic();
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

        //Pistol Select setup
        if (newState == DuelStates.pistolSelection)
        {
            Debug.Log("Pistol select start");

            //ChangeStateTo(DuelStates.ready);

            pistolSelector.gameObject.SetActive(true);
            pistolSelector.Reset();
        }

        //Ready setup
        if (newState == DuelStates.ready)
        {
            Debug.Log("Are you ready to duel?");

            if (pressALprompt != null)
            {
                pressALprompt.SetActive(true);
            }

            //Populate the walk events...
            for (int i = 0; i < 10; i++)
            {
                //Just with footsteps for now
                walkEventsP1.Add(new WalkFootstepEvent(0.1f * i + Random.Range(-0.01f, 0.01f), 0.05f));
                walkEventsP2.Add(new WalkFootstepEvent(0.1f * i + Random.Range(-0.01f, 0.01f), 0.05f));
            }

            WalkEvent w;

            //hat
            w = new PlayerHatEvent(Random.Range(0f, 0.5f), Random.Range(0.35f, 0.5f));
            walkEventsP1.Add(w);
            w = new PlayerHatEvent(Random.Range(0f, 0.5f), Random.Range(0.35f, 0.5f));
            walkEventsP2.Add(w);

            //hand
            float handStart = Random.Range(0.4f, 0.75f);
            w = new PlayerHandEvent(handStart, Random.Range(0.25f, Mathf.Min(1f - handStart, 0.5f)));
            walkEventsP1.Add(w);
            handStart = Random.Range(0.4f, 0.75f);
            w = new PlayerHandEvent(handStart, Random.Range(0.25f, Mathf.Min(1f - handStart, 0.5f)));
            walkEventsP2.Add(w);

            //wind
            float windStart = Random.Range(0.0f, 0.7f);
            float windDuration = Random.Range(0.3f, Mathf.Min(1f - windStart, 0.5f));
            w = new WindEvent(windStart, windDuration);
            walkEventsP1.Add(w);
            w = new WindEvent(windStart, windDuration);
            walkEventsP2.Add(w);

            //rabbit
            float rabbitStart = Random.Range(0.0f, 0.7f);
            float rabbitDuration = Random.Range(0.3f, Mathf.Min(1f - windStart, 0.5f));
            w = new RabbitEvent(rabbitStart, rabbitDuration);
            walkEventsP1.Add(w);
            w = new RabbitEvent(rabbitStart, rabbitDuration);
            walkEventsP2.Add(w);

            walkEventsP1.Sort();
            walkEventsP2.Sort();

            //Prevent key auto-progression: God this is hacky...
            keyUpP1 = 100f;
            keyUpP2 = 100f;
        }

        //Walk setup
        if (newState == DuelStates.walk)
        {
            walkTimer = 0f;
            SoundManager.PlayWalk();


            //Reset keyboard inputs:
            keyDownP1 = -1f;
            keyDownP2 = -1f;
            keyUpP1 = -1f;
            keyUpP2 = -1f;

            Debug.Log("Duelling! (banjos)");

            if (pressALprompt != null)
            {
                pressALprompt.SetActive(false);
            }
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
            player1.SetAnimationBool("isWinner", true);
            player2.SetAnimationBool("isWinner", false);
            Debug.Log("Player 1 is satisfied!");
        }

        //Player 2 win setup
        if (newState == DuelStates.p2win)
        {
            player2.SetAnimationBool("isWinner", true);
            player1.SetAnimationBool("isWinner", false);
            Debug.Log("Player 2 is satisfied!");
        }

        //Switch!
        currentState = newState;
    }
}
