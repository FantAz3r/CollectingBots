using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(DataBase))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;
    
    private Queue<Bot> _freeBots = new Queue<Bot>();
    private DataBase _dataBase;
    private Scaner _scaner;
    private Storage _storage;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
        _storage = GetComponent<Storage>();
        _dataBase = GetComponent<DataBase>();
    }

    private void Start()
    {
        foreach (Bot bot in _allBots)
        {
            bot.SetBase(transform.position);     
            _freeBots.Enqueue(bot);
            bot.WorkEnded += BotWorkComplite;
        }
    }

    private void OnEnable()
    {
        _scaner.ScanComplited += SendBot;
    }

    private void OnDisable()
    {
        _scaner.ScanComplited -= SendBot;
    }

    private void SendBot(List<ResourceNode> targets)
    {
        if (_freeBots.Count > 0)
        {
            ResourceNode target = _dataBase.SetValidNode(targets);
            Bot bot = _freeBots.Dequeue();
            bot.Returned += BotReturned;
            bot.StartCorutine(target);
            _dataBase.SetBuzyNode(target);
        }
    }

    private void BotReturned(Bot bot, ResourcePiece resource, int amount )
    {
        _storage.Collect(resource, amount);
    }

    private void BotWorkComplite(Bot bot)
    {
        _freeBots.Enqueue(bot);
        bot.Returned -= BotReturned;
        bot.WorkEnded -= BotWorkComplite;
    }
}
