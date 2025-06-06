using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "Entity Data")] 
[System.Serializable]
public class EntityData : ScriptableObject
{
    /// Atributes
    [SerializeField] private int speed;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int damage;

    /// Called when the inspector updates
    private void onValidate()
    {

    }
    public int dataDMG
    {
        get { return damage; }
        set { damage = value; }
    }

    public int dataSpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int dataMaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    public int dataCurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
}