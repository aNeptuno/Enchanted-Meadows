[System.Serializable]
public class CropModel
{
    public string Name = "";

	public int AmountInStorage = 0;

	public CropModel(string name, int amountInStorage)
    {
        Name = name;
        AmountInStorage = amountInStorage;
    }
}
