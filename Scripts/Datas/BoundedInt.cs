using UnityEngine;

[CreateAssetMenu(fileName = "New Bounded Int Data", menuName = "Create Datas/Create Bounded Int Data", order = 51)]
public sealed class BoundedInt : ScriptableObject
{
    [SerializeField]
    private int startValue;

    [SerializeField]
    private int value;

    public void InitValue() => this.value = startValue;

    public int Value
    {
        get => this.value;
        set => this.value = value;
    }

}
