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
    private FMODUnity.StudioEventEmitter _scoreSound;
    private List<string> scores = new List<string>();
    private string _path = "Assets/Resources/Scores/Highscores.txt";
    private int _highScore = 0;

    void Awake()
    {
        using (System.IO.StreamReader _streamreader = new System.IO.StreamReader(_path))
        {
            int counter = 0;
            while (_streamreader.Peek() >= 0)
            {
                scores.Add(_streamreader.ReadLine());
                int score = int.Parse(scores[counter]);
                if (score > _highScore)
                {
                    _highScore = score;
                }
                counter++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _lastScoreTime = Time.time;
        _scoreSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _scoreSound.Event = GLOB.CelebrationSound;
        _scoreSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        highscoreText.text = "Hoogste \nScore : " + _highScore;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastScoreTime > _resetTimer)
        {
            _lastScoreTime = Time.time;
            highscoreText.text = "Hoogste \nScore : " + _highScore;
            _score = 0;
            text.text = "Huidige \nScore : " + _score;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (Time.time > _lastScoreTime + 0.01f)
        {
            if (!_scoreSound.IsPlaying())
            {
                _scoreSound.Play();
            }
            particleEmitter.GetComponent<ParticleSystem>().Play();
            _lastScoreTime = Time.time;
            _score++;
            if (_score > _highScore)
            {
                _highScore = _score;
            }
            text.text = "Huidige \nScore : " + _score;
            FMODUnity.RuntimeManager.PlayOneShot(GLOB.CelebrationSound, GetComponent<Transform>().position);
        }
    }

    void OnApplicationQuit()
    {
        using (System.IO.StreamWriter _streamWriter = new System.IO.StreamWriter(_path, false))
        {
            _streamWriter.WriteLine(_highScore.ToString());
            _streamWriter.Flush();
            _streamWriter.Close();
        }
    }
}