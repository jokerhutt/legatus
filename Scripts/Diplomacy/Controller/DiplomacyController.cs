using System.Collections.Generic;
using Practice.Scripts.Diplomacy.Events;
using Practice.Scripts.Diplomacy.Model;
using Practice.Scripts.Diplomacy.Model.Enum;
using Practice.Scripts.Diplomacy.Views;
using Practice.Scripts.Economy;
using Practice.Scripts.Faction;

namespace Practice.Scripts.Diplomacy.Controller;

public class DiplomacyController
{
    private const int GiftCost = 50;
    private const int GiftOpinionBoost = 25;
    
    private DiplomacyService _diplomacyService;
    private EconomyService _economyService;
    private DiplomacyEvents _diplomacyEvents;
    private FactionService _factionService;
    
    public DiplomacyController(FactionService factionService, EconomyService economyService, DiplomacyEvents diplomacyEvents, List<Treaty> treaties)
    {
        _factionService = factionService;
        _economyService = economyService;
        _diplomacyService = new DiplomacyService(factionService, treaties);
        _diplomacyEvents = diplomacyEvents;
    }
    
    public DiplomacyViewDTO GetDiplomacyViewData(string ownFactionId, string otherFactionId)
    {
        return _diplomacyService.GetDiplomacyViewData(ownFactionId, otherFactionId);
    }

    // -- Actions --

    public void DeclareWar(string from, string to)
    {
        _diplomacyService.DeclareWar(from, to);
        _diplomacyEvents.EmitTreatyChanged(from, to);
    }

    public void MakePeace(string from, string to)
    {
        _diplomacyService.MakePeace(from, to);
        _diplomacyEvents.EmitTreatyChanged(from, to);
    }

    public void SignNonAggression(string from, string to)
    {
        _diplomacyService.CreateTreaty(from, to, TreatyType.NonAggression);
        _diplomacyEvents.EmitTreatyChanged(from, to);
    }

    public void CancelNonAggression(string from, string to)
    {
        _diplomacyService.CancelTreaty(from, to, TreatyType.NonAggression);
        _diplomacyEvents.EmitTreatyChanged(from, to);
    }

    public void FormAlliance(string from, string to)
    {
        _diplomacyService.FormAlliance(from, to);
        _diplomacyEvents.EmitTreatyChanged(from, to);
    }

    public void CancelAlliance(string from, string to)
    {
        _diplomacyService.CancelAlliance(from, to);
        _diplomacyEvents.EmitTreatyChanged(from, to);
    }

    public void SendGift(string from, string to)
    {
        _economyService.TransactCoins(from, to, GiftCost);
        _diplomacyService.UpdateOpinion(to, from, GiftOpinionBoost);
        _diplomacyEvents.EmitOpinionChanged(to, from, _diplomacyService.GetOpinionOf(to, from));
    }

    // -- Queries --

    public bool CanDeclareWar(string a, string b) => _diplomacyService.CanDeclareWar(a, b);

    public bool IsAtWar(string a, string b) => _diplomacyService.HasTreatyType(a, b, TreatyType.War);

    public bool HasNonAggression(string a, string b) => _diplomacyService.HasTreatyType(a, b, TreatyType.NonAggression);

    public bool HasAlliance(string a, string b) => _diplomacyService.HasTreatyType(a, b, TreatyType.Alliance);

    public bool CanAffordGift(string factionId)
    {
        var f = _factionService.GetFaction(factionId);
        return f != null && f.Coins >= GiftCost;
    }

    public int GetGiftCost() => GiftCost;

    public Godot.Texture2D GetFactionCrest(string factionId)
    {
        var f = _factionService.GetFaction(factionId);
        return f?.Crest;
    }
}