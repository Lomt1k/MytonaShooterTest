using MyTonaShooterTest.Units;
using UnityEngine;

public class GrenadeTrajectoryCalc : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform grenadeTransform;
    public Unit _weaponOwner;
    public int pointsNumber;
    public float heightModificator = 25f;

    private Vector3[] _positions;
    private float _maxDistance;

    public Vector3[] positions => _positions;

    public void StartAiming(float maxDrawDistance, float explosiveRange)
    {
        lineRenderer.enabled = true;
        _maxDistance = maxDrawDistance;
        RenderArc();
    }

    public void StopAiming()
    {
        lineRenderer.enabled = false;
    }

    private void Start()
    {
        lineRenderer.enabled = false;
        if (pointsNumber % 2 == 0) pointsNumber++; //pointsNumber должен быть нечетным для корректного просчета параболлы в CalculatePoints
        lineRenderer.positionCount = pointsNumber;
    }

    private void RenderArc()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 endPos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            endPos = hit.point;
        }
        if (Vector3.Distance(grenadeTransform.position, endPos) > _maxDistance)
        {
            endPos = grenadeTransform.position + transform.forward * _maxDistance;
        }
        lineRenderer.SetPositions(CalculatePoints(grenadeTransform.position, endPos));

        //update grenade pos
        _weaponOwner.weaponHolder.armedGrenade.transform.position = positions[0];
    }

    private void Update()
    {
        if (lineRenderer.enabled)
        {
            RenderArc();
        }
    }

    private Vector3[] CalculatePoints(Vector3 origin, Vector3 destination)
    {
        _positions = new Vector3[pointsNumber];

        //выстраиваем прямую линию от origin к destination
        Vector3 delta = destination - origin;
        for (int i = 0; i < pointsNumber; i++)
        {
            float x, y, z;
            x = delta.x * ((i + 1) / (float)pointsNumber);
            y = delta.y * ((i + 1) / (float)pointsNumber);
            z = delta.z * ((i + 1) / (float)pointsNumber);
            Vector3 vect = new Vector3(x, y, z);
            _positions[i] = vect + origin;
        }

        //далее с центра этой линии меняем высоту по параболе
        int centerPoint = pointsNumber / 2;
        for (int i = 0; i <= centerPoint; i++)
        {
            positions[centerPoint + i].y = -i * i / heightModificator;
            positions[centerPoint - i].y = -i * i / heightModificator;
        }

        //подъем всей линии (до этого момента она находится ниже, чем нужно)
        float deltaY = destination.y - positions[pointsNumber - 1].y;
        for (int i = 0; i < pointsNumber; i++)
        {
            positions[i].y += deltaY;
        }

        return _positions;
    }

}
