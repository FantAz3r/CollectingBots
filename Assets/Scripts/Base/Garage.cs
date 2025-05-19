using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;

    private Queue<Bot> _freeBots = new Queue<Bot>();

    private void Start()
    {
        foreach (Bot bot in _allBots)
        {
            bot.SetBase(transform.position);
            _freeBots.Enqueue(bot);
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

    public void Return(Bot bot)
    {
        if (_allBots.Contains(bot) ==false)
        {
            _allBots.Add(bot);
        }

        _freeBots.Enqueue(bot);
    }
}
