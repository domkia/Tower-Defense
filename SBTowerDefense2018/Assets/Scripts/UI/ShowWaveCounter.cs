using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a wave counter on the screen, which tells the player the current wave and how
/// many waves there are in total. The player can then track his progress.
/// </summary>
class ShowWaveCounter : MonoBehaviour
{

    public Text WaveCounter;

    private int currentWave;

    private void Start()
    {
        WaveCounter.text = "";
        currentWave = 0;
        MonsterSpawner.OnNewWaveStart += UpdateText;
    }

    /// <summary>
    /// Updates the wave counter's text.
    /// </summary>
    private void UpdateText()
    {
        currentWave++;
        WaveCounter.text = string.Format("Wave {0}/{1}", currentWave, MonsterSpawner.TotalNumberOfWaves);
    }

}