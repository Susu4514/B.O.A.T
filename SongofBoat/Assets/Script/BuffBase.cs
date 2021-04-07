[System.Serializable]
public class BuffBase
{
    private int buffID;
    private int BuffType;
    private string BuffDescription;
    private int BuffStartIndex;
    private string BuffIcon;
    private int BuffDestroyType;
    private int BuffIndex1;
    private int BuffIndex2;
    private int BuffIndex3;
    private int BuffIndex4;
    private int BuffIndex5;
}

public enum BuffType{
    DirectBuff = 1,
    GivingBuff = 2,
    CalculateBuff = 3,
    StartBuff = 4,
    HittedBuff = 5,
}

public enum BuffDestroyType{
    HiteedBuff = 1,
    StartBuff = 2,
}