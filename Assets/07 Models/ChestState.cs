using System;
using System.Collections.Generic;

[Serializable]
public class ChestState
{
	public List<CropModel> cropsInChest = new List<CropModel>
	{
		new CropModel("Potato", 4),
		new CropModel("Carrot", 4)
    };
}
