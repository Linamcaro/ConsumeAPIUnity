using UnityEngine;


[System.Serializable]
public class CharacterResponse
{
    public Character[] results;
    public Info info;
}


[System.Serializable]
public class Info
{
    public int count;
    public int pages;
}


[System.Serializable]
public class Character
{
    public string name;
    public string status;
    public string species;
    public string gender;
    public string image;
}

