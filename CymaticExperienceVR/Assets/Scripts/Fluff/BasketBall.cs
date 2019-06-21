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
    private string scores;
  //  private System.IO.StreamReader _streamReader;
  //  private System.IO.StreamWriter _streamWriter;
    private string _path = "Assets/Resources/Scores/Highscores.txt";
    private int _highScore;

    void Awake()
    {
        //_streamReader = new System.IO.StreamReader(_path, true);
        //scores = _streamReader.ReadToEnd();
        //_streamReader.Dispose();
        //_streamReader.Close();
        //if (scores != "")
        //{
        //    _highScore = int.Parse(scores);
        //}
        //else
        //{
        //    _highScore = 0;
        //}
        //_streamWriter = new System.IO.StreamWriter(_path, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _lastScoreTime = Time.time;
        _scoreSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _scoreSound.Event = GLOB.CelebrationSound;
        _scoreSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        highscoreText.text = "Hoogste \nScore : " + _highScore;
       // _streamWriter.WriteLine(_highScore.ToString());
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
        if (!_scoreSound.IsPlaying())
        {
            _scoreSound.Play();
        }
        particleEmitter.GetComponent<ParticleSystem>().Play();
        _lastScoreTime = Time.time;
        _score++;
        if(_score > _highScore)
        {
            //_streamWriter.WriteLine(_score.ToString());
        }
        text.text = "Huidige \nScore : " + _score;
        FMODUnity.RuntimeManager.PlayOneShot(GLOB.CelebrationSound, GetComponent<Transform>().position);
    }

}
