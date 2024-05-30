[System.Serializable]
public class CropInSoil
{
    public string Name = "";

	public bool StartGrowing = false;

	public bool StartedGrowing = false;

	public int SpriteIndex = 0;

	public bool ReadyToCollect = false;

	public CropInSoil(string name, bool startGrowing, bool startedGrowing, int spriteIndex, bool readyToCollect)
    {
        Name = name;
        StartGrowing = startGrowing;
		StartedGrowing = startedGrowing;
		SpriteIndex = spriteIndex;
		ReadyToCollect = readyToCollect;
    }

	public CropInSoil()
    {
    }
}
