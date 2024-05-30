using System;

[Serializable]
public class MatrixSoilState
{
	public SoilState[,] SoilStateMatrix = new SoilState[6,6];

	public MatrixSoilState()
    {
        SoilStateMatrix = new SoilState[6, 6];

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                SoilStateMatrix[i, j] = new SoilState();
            }
        }
    }

	public void AddMemberToMatrix(int i, int j, SoilState soilState)
    {
        SoilStateMatrix[i, j] = new SoilState(soilState.CurrentDirtState, soilState.CurrentCrop);
    }
}
