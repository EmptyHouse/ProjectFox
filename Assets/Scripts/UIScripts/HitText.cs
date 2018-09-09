using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitText : MonoBehaviour {
    public Text hitTextUIReference;
    [Range(0f, 1f)]
    public float driftVariance = .2f;
    public float timeBeforeFadeOut = .5f;
    public float driftSpeed = 10f;
    private Vector3 directionOfHitText;
    public float damageThatWasDealt { get; set; }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void SetUpHitText(float damageThatWasDealt)
    {
        this.damageThatWasDealt = damageThatWasDealt;
        directionOfHitText = new Vector3(Random.Range(-driftVariance, driftVariance), 1, 0).normalized;
        hitTextUIReference.text = "-" + this.damageThatWasDealt.ToString();
        StartCoroutine(UpdateMovementOfHitText());
    }

    private IEnumerator UpdateMovementOfHitText()
    {
        float timer = 0;
        while (timer < timeBeforeFadeOut)
        {
            timer += Time.deltaTime;
            this.transform.position = transform.position + (directionOfHitText * driftSpeed * Time.deltaTime);
            yield return null;
        }
        timer = 0;
        Color col = hitTextUIReference.color;
        while (timer < .2f)
        {
            timer += Time.deltaTime;
            this.transform.position = this.transform.position + (directionOfHitText * driftSpeed * Time.deltaTime);
            hitTextUIReference.color = new Color(col.r, col.g, col.b, 1 - (timer / .2f));
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
