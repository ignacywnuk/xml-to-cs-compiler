public class Mushroom
{
    public string name { get; set; }
    public string edible { get; set; }

    public mushroom() { }
    public mushroom(string name, string edible)
    {
        name = name;
        edible = edible;
    }

    public override string ToString()
    {
        return $"Name: {name}, edible: {edible}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return ToString() == obj.ToString();
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override Type GetType()
    {
        return typeof(Mushroom);
    }

}

