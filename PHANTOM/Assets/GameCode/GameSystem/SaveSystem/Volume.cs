[System.Serializable]
public class Volume
{
    public Volume(float effect,float bgm)
    {
        Effect = effect;
        BGM = bgm;
    }

    public float Effect;
    public float BGM;
    public override bool Equals(object obj)
    {
        return obj is Volume other &&
               Effect == other.Effect &&
               BGM == other.BGM;
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
