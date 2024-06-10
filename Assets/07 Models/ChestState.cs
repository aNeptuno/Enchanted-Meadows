using System;
using System.Collections.Generic;

[Serializable]
public class ChestState
{
	public List<CropModel> cropsInChest = new List<CropModel>();

	public void EmptyList()
	{
		cropsInChest = new List<CropModel>();
	}

	public ChestState()
	{
	}

	public void InitializeChest()
	{
		cropsInChest.Add(new CropModel("Potato", 4));
		cropsInChest.Add(new CropModel("Carrot", 4));
	}
}
