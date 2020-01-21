using System.Collections;
using UnityEngine;
using MyTonaShooterTest.Units;

public class PowerUp : MonoBehaviour
{
    public Ability ability; //способность, которая выдается при подборе powerUp
    public float respawnTime = 30f; //время респавна powerUp после его подбора

    public Collider col;
    public MeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            StartCoroutine(OnPickup(unit, respawnTime));
        }
    }

    private IEnumerator OnPickup(Unit unit, float time)
    {
        unit.unitStats.AddAbility(ability);
        //исчезновение и респавн
        col.enabled = false;
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(time);
        col.enabled = true;
        meshRenderer.enabled = true;
    }
}
