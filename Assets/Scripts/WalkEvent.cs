using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WalkEventTypes
{
    footStep,
    hatTwirl, //Temporary events yo...
    owlSalute,
    mustacheFridge,
    invertedJosephine
}

public class WalkEvent : IComparable
{
    protected float start, duration;
    protected float gumptionTapDelta, mannersTapDelta, breedingTapDelta, bottleTapDelta;
    protected int mashFrom, mashTo;
    protected float mashRate;
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

    public WalkEvent(float _start, float _duration, WalkEventTypes _event, float _gumptionDelta = 0f, float _mannersDelta = 0f, float _breedingDelta = 0f, float _bottleDelta = 0f)
    {
        start = _start;
        duration = _duration;
        walkEvent = _event;
        gumptionTapDelta = _gumptionDelta;
        mannersTapDelta = _mannersDelta;
        breedingTapDelta = _breedingDelta;
        bottleTapDelta = _bottleDelta;

        mashFrom = 0;
        mashTo = 0;
        mashRate = 0f;

        tapped = false;
    }

    public void SetTapFactors(float _gumptionDelta, float _mannersDelta, float _breedingDelta, float _bottleDelta)
    {
        gumptionTapDelta = _gumptionDelta;
        mannersTapDelta = _mannersDelta;
        breedingTapDelta = _breedingDelta;
        bottleTapDelta = _bottleDelta;
    }

    public void SetMashFactors(int _mashFrom, int _mashTo, float _mashRate)
    {
        mashFrom = _mashFrom;
        mashTo = _mashTo;
        mashRate = _mashRate;
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

    public void Mash(PlayerBehaviour _player)
    {
        float mashAmount = 0f;

        switch (mashFrom) //where does the amount come from?
        {
            case 0:
                mashAmount = Mathf.Min(_player.gumption, mashRate);
                _player.gumption -= mashAmount;
                break;
            case 1:
                mashAmount = Mathf.Min(_player.manners, mashRate);
                _player.manners -= mashAmount;
                break;
            case 2:
                mashAmount = Mathf.Min(_player.breeding, mashRate);
                _player.breeding -= mashAmount;
                break;
            case 3:
                mashAmount = Mathf.Min(_player.bottle, mashRate);
                _player.bottle -= mashAmount;
                break;
        }

        switch (mashTo) //where does the amount go to?
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
