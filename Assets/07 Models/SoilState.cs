using System;

[Serializable]
public class SoilState
{
	public DirtStates CurrentDirtState = DirtStates.NATURAL;

	public CropInSoil CurrentCrop = new CropInSoil();

	public SoilState()
	{
		CurrentDirtState = DirtStates.NATURAL;
		CurrentCrop = new CropInSoil();
	}

	public SoilState(DirtStates state, CropInSoil currentCrop)
	{
		CurrentDirtState = state;
		CurrentCrop = currentCrop;
	}
}
