using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    private int _score = 0;
    public TextMesh text;
    public TextMesh highscoreText;
    public GameObject particleEmitter;

    private GameObject _lastObjectHit;
    private float _resetTimer = 30.0f;
    private float _lastScoreTime;

    // Start is called before the first frame update
    void Start()
    {
        _lastScoreTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastScoreTime > _resetTimer)
        {
            _lastScoreTime = Time.time;
            highscoreText.text = "Hoogste \nScore : " + _score;
            _score = 0;
            text.text = "Huidige \nScore : " + _score;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        particleEmitter.GetComponent<ParticleSystem>().Play();
        _lastScoreTime = Time.time;
        _score++;
        text.text = "Huidige \nScore : " + _score;
    }
}
