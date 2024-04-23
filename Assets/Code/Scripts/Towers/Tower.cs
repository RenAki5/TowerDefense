using System;
using UnityEngine;

[Serializable]
public class Tower {

    public string name;
    public int cost;
    public GameObject prefab;
    public bool isMelee;
    public int rally;

    public Tower (string _name, int _cost, GameObject _prefab, bool _isMelee, int _rally)
    {
        name = _name;
        cost = _cost;
        rally = _rally;
        prefab = _prefab;
        isMelee = _isMelee;
    }

}


