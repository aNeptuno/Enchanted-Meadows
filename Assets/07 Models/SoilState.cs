using System;

[Serializable]
public class SoilState
{
	public enum DirtStates {
        NATURAL,
		TILLED,
		WATERED,
		SEEDED,
		PLANTED,
    }

	DirtStates CurrentDirtState = DirtStates.NATURAL;
}
