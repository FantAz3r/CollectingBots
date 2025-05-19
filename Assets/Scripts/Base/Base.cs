using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(Database))]
[RequireComponent(typeof(Garage))]
public class Base : MonoBehaviour
{
    private Garage _garage;
    private Database _dataBase;
    private Scanner _scaner;
    private Storage _storage;

    private void Awake()
    {
        _garage = GetComponent<Garage>();
        _scaner = GetComponent<Scanner>();
        _storage = GetComponent<Storage>();
        _dataBase = GetComponent<Database>();
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
        if (_storage.IsOverflow() == false)
        {
            Bot bot = _garage.Get();

            if (bot != null)
            {
                ResourceNode target = _dataBase.GetClosestNode(targets);
                bot.WorkEnded += OnWorkComplite;
                bot.Returned += OnReturn;
                bot.StartWork(target);
            }
        }
    }

    private void OnReturn(Bot bot, ResourcePiece resource, int amount)
    {
        _storage.Collect(resource, amount);
    }

    private void OnWorkComplite(Bot bot)
    {
        _garage.Return(bot);
        bot.Returned -= OnReturn;
        bot.WorkEnded -= OnWorkComplite;
    }
}