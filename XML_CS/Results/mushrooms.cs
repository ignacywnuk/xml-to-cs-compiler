public class Mushrooms
{
    public Mushroom mushroom { get; set; }

    public mushrooms() { }
    public mushrooms(Mushroom mushroom)
    {
        mushroom = new Mushroom(mushroom);
    }

    public override string ToString()
    {
        return $"Mushroom: {mushroom}";
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
        return typeof(Mushrooms);
    }

}

