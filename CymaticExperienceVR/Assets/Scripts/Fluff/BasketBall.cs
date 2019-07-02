using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    private int _score = 0;
    public TextMesh text;
    public TextMesh highscoreText;
    public TextMesh timeText;
    public GameObject particleEmitter;

    private GameObject _lastObjectHit;
    private float _resetTimer = 30.0f;
    private float _lastScoreTime = 0;
    private FMODUnity.StudioEventEmitter _scoreSound;
    private List<string> scores = new List<string>();
    private string _path = "Assets/Resources/Scores/Highscores.txt";
    private int _highScore = 0;
    private float _timertext = 0;

    void Awake()
    {
        //Resources.Load(_path);
        if (!Application.isEditor)
        {
            _path = Application.dataPath + "/Assets/Highscores.txt";
        }
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
        highscoreText.text = _highScore.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastScoreTime > _resetTimer)
        {
            _lastScoreTime = Time.time;
            highscoreText.text = _highScore.ToString();
            _score = 0;
            text.text = _score.ToString();
        }
        if(_timertext > 0)
        {
            _timertext -= Time.deltaTime;
            timeText.text = Mathf.Round(_timertext).ToString();
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
            text.text = _score.ToString();
            if (!_scoreSound.IsPlaying())
            {
                _scoreSound.Play();
            }
        }
        timeText.text = 30.ToString();
        _timertext = 30f;
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