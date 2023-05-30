public class Mushroom_picker
{
    public string name { get; set; }
    public int mushrooms_picked { get; set; }

    public mushroom_picker() { }
    public mushroom_picker(string name, int mushrooms_picked)
    {
        name = name;
        mushrooms_picked = mushrooms_picked;
    }

    public override string ToString()
    {
        return $"Name: {name}, mushrooms_picked: {mushrooms_picked}";
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
        return typeof(Mushroom_picker);
    }

}

