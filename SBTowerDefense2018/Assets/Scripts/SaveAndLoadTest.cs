using UnityEngine;

public class SaveAndLoadTest : MonoBehaviour {

    public void Save()
    {
        PlayerStats.Instance.Save();
    }

    public void Load()
    {
        PlayerStats.Instance.Load();
    }

}
