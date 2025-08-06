using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] private RectTransform _wheelTransform;
    [SerializeField] private GameObject _sectorPrefab;
    [SerializeField] private AnimationCurve _spinCurve;

    private readonly List<SectorBounds> _sectorBounds = new();

    [SerializeField] private int _numberOfSectors = 8;
    [SerializeField][Range(0, 10)] private int _minCost = 50;
    [SerializeField][Range(10, 50)] private int _maxCost = 50;

    [SerializeField] private float _spinDuration = 3f;

    public Action<int> WinAmount;
    public Action StartGame;

    public void GenerateSectors(int sectorsCount, int cost)
    {
        ClearSectors();
        _wheelTransform.transform.rotation = Quaternion.Euler(0, 0, 0);
        var sectorAngle = 360f / _numberOfSectors;

        for (var i = 0; i < _numberOfSectors; i++)
        {
            var newSector = Instantiate(_sectorPrefab, _wheelTransform.transform);
            var sectorImage = newSector.GetComponent<Image>();

            sectorImage.type = Image.Type.Filled;
            sectorImage.fillMethod = Image.FillMethod.Radial360;
            sectorImage.fillOrigin = 2;


            sectorImage.fillAmount = 1f / _numberOfSectors;
            newSector.transform.localPosition = Vector3.zero;
            newSector.transform.localRotation = Quaternion.Euler(0, 0, -sectorAngle * i);

            var rectTransform = newSector.GetComponent<RectTransform>();
            rectTransform.pivot = new(0.5f, 0.5f);

            var prize = newSector.GetComponentInChildren<Gift>();
            prize.SetCost(Random.Range(_minCost, _maxCost));

            var newPrizeAngle = -180 * sectorImage.fillAmount;
            prize.SetRotation(newPrizeAngle);


            var sectorStartAngle = sectorAngle * i;
            var sectorEndAngle = sectorAngle * (i + 1);

            sectorEndAngle = (sectorEndAngle + 360) % 360;
            sectorStartAngle = (sectorStartAngle + 360) % 360;

            if (i + 1 == _numberOfSectors)
                sectorEndAngle = 360f;

            _sectorBounds.Add(new(prize, sectorStartAngle, sectorEndAngle));
        }
    }

    private void ClearSectors()
    {
        foreach (Transform child in _wheelTransform.transform)
            Destroy(child.gameObject);

        _sectorBounds.Clear();
    }

    public void SpinWheel()
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        float elapsedTime = 0f;
        float startAngle = Mathf.Repeat(_wheelTransform.eulerAngles.z, 360f);

        float fullRotations = 360f * Random.Range(3, 6);
        float randomOffset = Random.Range(0f, 360f);
        float endAngle = startAngle + fullRotations + randomOffset;

        while (elapsedTime < _spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _spinDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, _spinCurve.Evaluate(t));
            _wheelTransform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }

        float finalAngle = Mathf.Repeat(_wheelTransform.eulerAngles.z, 360f);
        _wheelTransform.eulerAngles = new Vector3(0, 0, finalAngle);

        yield return new WaitForSeconds(1f);

        var gift = DetermineWinningSector();
        WinAmount?.Invoke(gift.GetCost());
    }

    public Gift DetermineWinningSector()
    {
        var wheelAngleZ = Mathf.Repeat(_wheelTransform.transform.eulerAngles.z, 360f);

        foreach (var sectorBound in _sectorBounds)
            if (wheelAngleZ >= sectorBound.startAngle && wheelAngleZ < sectorBound.endAngle)
                return sectorBound.prize;

        return null;
    }

    [Serializable]
    public class SectorBounds
    {
        public Gift prize;
        public float startAngle;
        public float endAngle;

        public SectorBounds(Gift prize, float startAngle, float endAngle)
        {
            this.prize = prize;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
        }
    }
}