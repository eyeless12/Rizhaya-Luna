using UnityEngine;

public class Weapon : Item
{
    public int Spread;
    public int SpreadWidth;
    public float BulletThresholdTime; 
    public float horizontalRecoil;
    public float verticalRecoil;
    public float bulletLifetime;
    public int maxCapacity;
    public bool canBeHold;
    
    [Range(0f , 1f)] [SerializeField] public float accuracy;

    private Fire _weaponAction;

    public override void Start()
    {
        base.Start();
        _weaponAction = GetComponent<Fire>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        _weaponAction.Shoot();
        //Debug.Log("Shoot!");
    }
}
