using System.Dynamic;

public class Level
{

    public int enemies { get; set; }
    public int levelName { get; set; }

    private int currentPlaneInt = 0;

    public Level(int levelNamer, int enemiesCount)
    {

        levelName = levelNamer;
        enemies = enemiesCount;

    }



    public int getPlaneIndex()
    {

        return currentPlaneInt;
    }

    public void UpdatePlaneSpawned()
    {

        currentPlaneInt++;
    }

    public bool PlaneSpawnReached()
    {
        if (currentPlaneInt >= enemies)
        {
            return true;
        }

        return false;
    }


}