
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
    public string type;
    public string gender;
    public Location location;
    public Origin origin;
}

[System.Serializable]
public class Location
{
    public string name;
    public string url;
}

[System.Serializable]
public class Origin
{
    public string name;
    public string url;
}


