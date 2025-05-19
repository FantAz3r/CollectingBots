using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;
    
    private Queue<Bot> _freeBots = new Queue<Bot>();
    private int _minBotCountToBuild = 2;

    public bool IsFreeBot => _freeBots.Count > 0;
    public bool IsBuildBot => _allBots.Count >= _minBotCountToBuild;

    private void Awake()
    {
        foreach (Bot bot in _allBots)
        {
            bot.SetBase(transform.position, this);
            _freeBots.Enqueue(bot);
            bot.transform.SetParent(transform);
        }
    }

    public Bot Get()
    {
        if (_freeBots.Count > 0)
        {
            return _freeBots.Dequeue();
        }

        return null;
    }

    public void Relocate(Bot bot)
    {
        _allBots.Remove(bot);
    }

    public void Return(Bot bot)
    {
        if (_allBots.Contains(bot) ==false)
        {
            bot.SetBase(transform.position, this);
            _allBots.Add(bot);
            bot.transform.SetParent(transform);
        }

        _freeBots.Enqueue(bot);
    }
}
