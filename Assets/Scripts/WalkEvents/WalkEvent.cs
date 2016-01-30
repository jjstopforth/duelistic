using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WalkEventTypes
{
    footStep,
    hatTwirl, //Temporary events yo...
    wind,
    rabbit,
    owlSalute,
    mustacheFridge,
    invertedJosephine
}

public class WalkEvent : IComparable
{
    protected float start, duration;
    protected float gumptionTapDelta, mannersTapDelta, breedingTapDelta, bottleTapDelta;
    protected int holdFrom, holdTo;
    protected float holdRate;
    protected float gumptionMashDelta, mannersMashDelta, breedingMashDelta, bottleMashDelta;
    protected WalkEventTypes walkEvent;
    protected bool tapped;

    public WalkEventTypes MyEvent
    {
        get { return walkEvent; }
    }

    public float Start
    {
        get { return start; }
    }

    public float Duration
    {
        get { return duration; }
    }

    public virtual void StartEvent(PlayerBehaviour _currentPlayer)
    {
        //Event starts!
    }

    public virtual void UpdateEvent(PlayerBehaviour _currentPlayer)
    {
        //Call this every frame the event is active...
    }

    public virtual void EndEvent(PlayerBehaviour _currentPlayer)
    {
        //End the event (clean shit up yo)
    }

    public WalkEvent(float _start, float _duration, WalkEventTypes _event, float _gumptionDelta = 0f, float _mannersDelta = 0f, float _breedingDelta = 0f, float _bottleDelta = 0f)
    {
        start = _start;
        duration = _duration;
        walkEvent = _event;
        gumptionTapDelta = _gumptionDelta;
        mannersTapDelta = _mannersDelta;
        breedingTapDelta = _breedingDelta;
        bottleTapDelta = _bottleDelta;

        holdFrom = 0;
        holdTo = 0;
        holdRate = 0f;

        gumptionMashDelta = 0f;
        mannersMashDelta = 0f;
        breedingMashDelta = 0f;
        bottleMashDelta = 0f;

        tapped = false;
    }

    public void SetTapFactors(float _gumptionDelta, float _mannersDelta, float _breedingDelta, float _bottleDelta)
    {
        gumptionTapDelta = _gumptionDelta;
        mannersTapDelta = _mannersDelta;
        breedingTapDelta = _breedingDelta;
        bottleTapDelta = _bottleDelta;
    }

    public void SetHoldFactors(int _holdFrom, int _holdTo, float _mashRate)
    {
        holdFrom = _holdFrom;
        holdTo = _holdTo;
        holdRate = _mashRate;
    }

    public void SetMashFactors(float _gumptionDelta, float _mannersDelta, float _breedingDelta, float _bottleDelta)
    {
        gumptionMashDelta = _gumptionDelta;
        mannersMashDelta = _mannersDelta;
        breedingMashDelta = _breedingDelta;
        bottleMashDelta = _bottleDelta;
    }

    public void Tap(PlayerBehaviour _player)
    {
        if (tapped) return;

        _player.gumption += gumptionTapDelta;
        _player.manners += mannersTapDelta;
        _player.breeding += breedingTapDelta;
        _player.bottle += bottleTapDelta;

        tapped = true;
    }

    public void Hold(PlayerBehaviour _player)
    {
        float mashAmount = 0f;

        switch (holdFrom) //where does the amount come from?
        {
            case 0:
                mashAmount = Mathf.Min(_player.gumption, holdRate);
                _player.gumption -= mashAmount;
                break;
            case 1:
                mashAmount = Mathf.Min(_player.manners, holdRate);
                _player.manners -= mashAmount;
                break;
            case 2:
                mashAmount = Mathf.Min(_player.breeding, holdRate);
                _player.breeding -= mashAmount;
                break;
            case 3:
                mashAmount = Mathf.Min(_player.bottle, holdRate);
                _player.bottle -= mashAmount;
                break;
        }

        switch (holdTo) //where does the amount go to?
        {
            case 0:
                _player.gumption += mashAmount;
                break;
            case 1:
                _player.manners += mashAmount;
                break;
            case 2:
                _player.breeding += mashAmount;
                break;
            case 3:
                _player.bottle += mashAmount;
                break;
        }
    }

    public void Mash(PlayerBehaviour _player)
    {
        _player.gumption += gumptionMashDelta;
        _player.manners += mannersMashDelta;
        _player.breeding += breedingMashDelta;
        _player.bottle += bottleMashDelta;
    }

    public int CompareTo(object _o)
    {
        WalkEvent otherWalkEvent = _o as WalkEvent;

        if (otherWalkEvent != null)
        {
            return start.CompareTo(otherWalkEvent.start);
        }
        return 0;
    }
}
