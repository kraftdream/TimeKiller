using System;
using UnityEngine;
using System.Collections;

public abstract  class Achievement
{
    protected HeroControll _hero;
    protected String _name;
    protected int _progress;
    protected int _currentProgress;
    protected bool _unlocked;

    public Achievement(String name, int progress, HeroControll hero)
    {
        _name = name;
        _progress = progress;
        _hero = hero;
    }

    public bool AddProgress(int currentProgress)
    {
        _currentProgress += currentProgress;
        if (_currentProgress >= _progress)
        {
            _currentProgress = 0;
            return true;
        }
        return false;
    }

    public String GetName()
    {
        return _name;
    }

    public abstract void AddBonus();
}

public class KillVampires : Achievement
{
    public KillVampires(String name, int progress, HeroControll hero):base(name, progress, hero)
    {}

    public override void AddBonus()
    {   
        _hero.BonusCombo = 2;
        _hero.Invoke("RemoveBonuses", 1);
    }
}


