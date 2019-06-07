using UnityEngine;
using VRTK;
using VRTK.Controllables;

public class FrequencyChanger : MonoBehaviour
{
    public VRTK_BaseControllable controllable;
    public VRTK_ControllerEvents controllerEvents;
    private Chladni chladni;
    private Tutorial _tutorial;
    // Start is called before the first frame update
    void Start()
    {
        if (chladni == null)
        {
            chladni = GameObject.Find("TableHolder").GetComponent<Chladni>();
        }
    }

    protected virtual void OnEnable()
    {
        controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
        controllable.ValueChanged += ValueChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        chladni.changedValue = true;
        _tutorial.CompletedTutorial();
    }
}
