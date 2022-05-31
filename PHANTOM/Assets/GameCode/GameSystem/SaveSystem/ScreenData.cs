[System.Serializable]
public class ScreenData
{
    public int width;
    public int height;
    public bool fullscene;

    public ScreenData() { }
    public ScreenData(int width, int height, bool fullsize)
    {
        this.width = width;
        this.height = height;
        this.fullscene = fullsize;
    }
    public override bool Equals(object obj)
    {
        return obj is ScreenData other &&
       width == other.width &&
       height == other.height &&
       fullscene == other.fullscene;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
