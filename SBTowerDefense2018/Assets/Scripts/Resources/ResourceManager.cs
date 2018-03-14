using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TEMPORARY Test class, use PlayerStats class instead
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public Text text;
    public Resource[] stock;

    private void Start()
    {
        for (int i = 0; i < stock.Length; i++)
        {
            stock[i].Reset();
        }
    }

    private void Update()
    {
        StringBuilder s = new StringBuilder();
        for (int i = 0; i < stock.Length; i++)
        {
            s.AppendFormat("{0}: {1} \t", stock[i].name, stock[i].Amount);
        }
        text.text = s.ToString();
    }
}
