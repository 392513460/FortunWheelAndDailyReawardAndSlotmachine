using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class LocalizationManager : MonoBehaviour {
    public string key;
    public string[] keyParams;

    [HideInInspector]
    public Text text;

	void OnEnable () {
        if (this.text != null)
        {
            return; // already inited
        }
        this.text = this.GetComponent<Text>();
        if (this.text == null)
        {
            UDebug.LogError("[LocalizationManager] [Start] Component 'Text' is not found");
            return;
        }
        if (string.IsNullOrEmpty(this.key) && this.IsTextKey(this.text.text))
        {
            this.key = this.text.text;
        }
        if (string.IsNullOrEmpty(this.key))
        {
            // no keys for work with
#if DEBUG
            UDebug.LogWarning("[LocalizationManager] [Start] key not found for object '" + this.gameObject.name + "'");
#endif
            return;
        }
        this.Localize();

        LocalizationController.Instance.onLanguageChangeEvent += this.OnLanguageUpdate;
    } // OnEnable

    void OnDisable()
    {
        if (LocalizationController.hasInstance)
            LocalizationController.Instance.onLanguageChangeEvent -= this.OnLanguageUpdate;
    }

    void OnLanguageUpdate(object sender, System.EventArgs args)
    {
        this.Localize(); 
    }

    public void Localize()
    {
        this.Localize(this.keyParams);
    } // Localize

    public void Localize(string[] paramsForText)
    {
        if (this.text == null)
        {
            this.OnEnable();
        }
        if (this.text != null && !string.IsNullOrEmpty(this.key))
        {
            this.text.text = LocalizationController.Instance.GetText(this.key, paramsForText);
        } else
        {
            this.text.text = "DEBUG - no key";
        }
    } // Localize

    private bool IsTextKey(string t)
    {
        if (t.Split('.').Length - 1 >= 2)
        {
            // it may be a key
            return true;
        }
        return false;
    } // IsTextKey

} // LocalizationManager