using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;
using UnityEngine;

public class Grenade : Weapon
{
    public LayerMask wallMask;
    public float _flyTime = 0.8f;

    private Vector3[] _positions;
    private bool _isFly;

    private float _currentFlyTime;
    private float _timeForOnePos;

    protected override void Shot()
    {
        base.Shot();
        _weaponOwner.weaponHolder.OnGrenadeDraw();

        _positions = _weaponOwner.weaponHolder.grenadeController.positions;
        _isFly = true;
        transform.position = _positions[0];
        transform.parent = null;

        _timeForOnePos = _flyTime / _positions.Length;
    }

    protected void FlyUpdate()
    {
        _currentFlyTime += Time.fixedDeltaTime;
        int currentPos = (int)(_currentFlyTime / _timeForOnePos);
        if (currentPos + 1 >= _positions.Length)
        {
            Explosion();
            return;
        }
        CollisionCheck(currentPos);

        float flyTimeFromLastPos = _currentFlyTime - (currentPos * _timeForOnePos);
        Vector3 newPos = Vector3.zero;
        newPos.x = Mathf.Lerp(transform.position.x, _positions[currentPos + 1].x, flyTimeFromLastPos / _timeForOnePos);
        newPos.y = Mathf.Lerp(transform.position.y, _positions[currentPos + 1].y, flyTimeFromLastPos / _timeForOnePos);
        newPos.z = Mathf.Lerp(transform.position.z, _positions[currentPos + 1].z, flyTimeFromLastPos / _timeForOnePos);
        transform.position = newPos;        
    }

    protected void Explosion()
    {
        //vfx
        var explosion = GameManager.instance.explosionPool.GetPoolObject();
        explosion.SetExplosionInfo(weaponData.explolsionTime);
        explosion.transform.position = transform.position;
        explosion.transform.localScale = Vector3.one * weaponData.explosiveRange;

        //damage
        foreach (var unit in UnitsHolder.units)
        {
            if (Vector3.Distance(transform.position, unit.transform.position) <= weaponData.explosiveRange)
            {
                //проверяем что между unit и взрывом нет препятствий
                Vector3 direction = unit.transform.position - transform.position;
                direction.Normalize();
                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, unit.transform.position), wallMask))
                {
                    continue;
                }
                //наносим урон
                unit.TakeDamage(_weaponOwner, this, weaponData.damage);
            }
        }
        Destroy(gameObject);
    }

    private void CollisionCheck(int currentPos)
    {
        RaycastHit hit;
        float radius = transform.localScale.x / 2f;
        Vector3 direction = -transform.up;
        if (currentPos + 1 < _positions.Length)
        {
            direction = (_positions[currentPos + 1] - _positions[currentPos]);
            direction.Normalize();
        }

        if (Physics.SphereCast(transform.position, radius, direction, out hit, radius))
        {
            if (hit.transform.gameObject != _weaponOwner.gameObject)
            {
                print(hit.transform.gameObject.name);
                Explosion();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isFly)
        {
            FlyUpdate();
        }
    }
}
