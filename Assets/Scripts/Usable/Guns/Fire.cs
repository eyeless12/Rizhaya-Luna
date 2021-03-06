using UnityEngine;


public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject pf_bullet;
    [SerializeField] private Transform initialBulletPoint;
    [SerializeField] private GameObject bulletCasing;
    private Weapon _weaponCharacteristics;
    private Transform _gun;
    private AudioSource _audio;
    
    private float _shootTime;
    private bool _canShoot = true;
    private int _magazine;

    private void Awake()
    {
        _weaponCharacteristics = GetComponent<Weapon>();
        _gun = GetComponent<Transform>();
        _magazine = _weaponCharacteristics.maxCapacity;
        _audio = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (!_canShoot || _magazine <= 0)
        {
            return;
        }
        
        foreach (var direction in Utils.GenerateDirections(
            _weaponCharacteristics.Spread,
            _weaponCharacteristics.SpreadWidth,
            _weaponCharacteristics.accuracy,
            _weaponCharacteristics.OwnerLookDirection))
        {
            var bullet = Instantiate(
                    pf_bullet, 
                    initialBulletPoint.position, 
                    Quaternion.Euler(1, 1, direction.x > 0 ? 0 : 180))
                .GetComponent<Bullet>();
            if (bulletCasing)
                Instantiate(bulletCasing, transform.position, Quaternion.Euler(1, 1, direction.x > 0 ? 90 : 0));
            bullet.direction = direction;
            bullet.lifetime = _weaponCharacteristics.bulletLifetime;
        }
        
        PerformRecoil();
        _audio.Play();
        
        GameManager.CameraShake.ActivateShake(.1f, .1f);
        _canShoot = false;
        _shootTime = _weaponCharacteristics.BulletThresholdTime;
        _magazine -= 1;
    }

    public void FixedUpdate()
    {
        if (!_canShoot)
            _shootTime -= Time.deltaTime;

        if (_shootTime < 0)
            _canShoot = true;
    }

    private void PerformRecoil()
    {
        var owner = _weaponCharacteristics.Owner;
        var ownerPhysics = owner.GetComponent<Rigidbody2D>();
        var recoilVector = new Vector2(
             _weaponCharacteristics.verticalRecoil * _weaponCharacteristics.OwnerLookDirection.x * -1, _weaponCharacteristics.horizontalRecoil);
        ownerPhysics.AddForce(recoilVector, ForceMode2D.Impulse);
    }
}
