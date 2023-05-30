public class Root
{
    public Mushroom_picker mushroom_picker { get; set; }
    public Mushrooms mushrooms { get; set; }

    public root() { }
    public root(Mushroom_picker mushroom_picker, Mushrooms mushrooms)
    {
        mushroom_picker = new Mushroom_picker(mushroom_picker);
        mushrooms = new Mushrooms(mushrooms);
    }

    public override string ToString()
    {
        return $"Mushroom_picker: {mushroom_picker}, Mushrooms: {mushrooms}";
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
        return typeof(Root);
    }

}

