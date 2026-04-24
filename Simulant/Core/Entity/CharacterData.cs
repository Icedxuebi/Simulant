
public class CharacterData
{
    public byte Level = 1;
    public byte ClassJob = 0;

    public uint HP;
    public uint MaxHP = 44;
    public uint MP;
    public uint MaxMP = 0;

    public CharacterData()
    {
        HP = MaxHP;
        MP = MaxMP;
    }
}