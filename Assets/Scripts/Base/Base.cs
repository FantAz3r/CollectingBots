using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(DataBase))]
//[RequireComponent(typeof(FlagSeter))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;
    
    private Queue<Bot> _freeBots = new Queue<Bot>();
    private DataBase _dataBase;
    private Scaner _scaner;
    private Storage _storage;
    //private FlagSeter _flagSeter;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
        _storage = GetComponent<Storage>();
        _dataBase = GetComponent<DataBase>();
        //_flagSeter = GetComponent<FlagSeter>();
    }

    private void Start()
    {
        foreach (Bot bot in _allBots)
        {
            bot.SetBase(transform.position);     
            _freeBots.Enqueue(bot);
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
        if(_storage.Overflow() == false)
        {
            if (_freeBots.Count > 0)
            {
                ResourceNode target = _dataBase.GetClosestNode(targets);
                Bot bot = _freeBots.Dequeue();
                bot.WorkEnded += OnWorkComplite;
                bot.Returned += OnReturn;
                bot.StartWork(target);
            }
        }
    }

    private void OnReturn(Bot bot, ResourcePiece resource, int amount )
    {
        _storage.Collect(resource, amount);
    }

    private void OnWorkComplite(Bot bot)
    {
        _freeBots.Enqueue(bot);
        bot.Returned -= OnReturn;
        bot.WorkEnded -= OnWorkComplite;
    }
}
