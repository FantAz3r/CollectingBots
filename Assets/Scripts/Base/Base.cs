using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(Builder))]
[RequireComponent(typeof(Garage))]
[RequireComponent(typeof(FlagSeter))]

public class Base : MonoBehaviour
{
    private Database _database;
    private FlagSeter _flagSeter;
    private Garage _garage;
    private Scaner _scaner;
    private Storage _storage;
    private Builder _builder;
    private WaitForSeconds _delay;
    private float _buildDelay = 0.2f;

    private void Awake()
    {
        _delay = new WaitForSeconds(_buildDelay);
        _database = Camera.main.GetComponent<Database>();
        _garage = GetComponent<Garage>();
        _scaner = GetComponent<Scaner>();
        _storage = GetComponent<Storage>();
        _builder = GetComponent<Builder>();
        _flagSeter = GetComponent<FlagSeter>();
    }

    private void OnEnable()
    {
        _storage.ResourceAdded += TryAddBotIfNoBuild;
        _scaner.ScanComplited += SendBot;
        _flagSeter.Seted += OnFlagSeted;
    }

    private void OnDisable()
    {
        _storage.ResourceAdded -= TryAddBotIfNoBuild;
        _scaner.ScanComplited -= SendBot;
        _flagSeter.Seted -= OnFlagSeted;
    }

    private void SendBot(List<ResourceNode> targets)
    {
        if (_storage.IsOverflow() == false)
        {
            ResourceNode target = _database.GetFreeNode(targets, this);

            if (target == null)
                return;

            Bot bot = _garage.Get();

            if (bot == null)
                return;

            if (_database.RequestAccess(this, target))
            {
                _database.BuzyNode(target, this);
                bot.WorkEnded += OnWorkComplite;
                bot.Returned += OnReturn;
                bot.StartWork(target, transform);
            }
        }
    }

    private void TryAddBotIfNoBuild()
    {
        if (_flagSeter.IsSet == false)
        {
            TryAddBot();
        }
    }

    private void TryAddBot()
    {
        if (_storage.IsEnoughResource(_builder.BuildingObject.ReturnCost()))
        {
            _storage.SpendResource(_builder.BuildingObject.ReturnCost());
            GameObject buildingObject = _builder.Build(transform.position);

            if (buildingObject != null && buildingObject.TryGetComponent(out Bot bot))
            {
                _garage.Return(bot);
            }
        }
    }

    private void OnFlagSeted(Flag flag)
    {
        StartCoroutine(BuildNewBase(flag, _builder.BuildingObject.ReturnCost()));
    }

    private IEnumerator BuildNewBase(Flag flag, Dictionary<ResourceType, int> baseCost)
    {
        while (enabled)
        {
            if (_garage.IsBuildBot)
            {
                if (_storage.IsEnoughResource(baseCost) && _garage.IsFreeBot)
                {

                    Bot bot = _garage.Get();
                    _storage.SpendResource(baseCost);
                    _garage.Relocate(bot);
                    bot.BuildStarted += _flagSeter.Remove;
                    bot.GoToFlag(flag.transform.position);
                    yield break;
                }

            }
            else
            {
                TryAddBot();
            }

            yield return _delay;
        }
    }

    private void OnReturn(Bot bot, ResourcePiece resource, int amount)
    {
        _storage.Collect(resource, amount);
        bot.IsEnoughGather(_storage.IsOverflow());
    }

    private void OnWorkComplite(Bot bot)
    {
        _garage.Return(bot);
        bot.Returned -= OnReturn;
        bot.WorkEnded -= OnWorkComplite;
    }
}