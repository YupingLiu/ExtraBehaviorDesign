using ExtraBehaviorDesign.TestScripts;
using UnityEngine;
using UnityEngine.UI;

public class CurrentBTInformationControl : MonoBehaviour {

    public Text m_btName;
    public Text m_btDescription;
    public Text m_btExtension;

    void Start()
    {
        if (null == UIBTInformationNotifier.instance)
        {
            UIBTInformationNotifier notifier = new UIBTInformationNotifier();
        }
        UIBTInformationNotifier.instance.RemoveColorInfoChangeHandler(ShowBasicInformation);
        UIBTInformationNotifier.instance.RemoveExtensionInfoChangeHandler(ShowExtensionInformation);
        UIBTInformationNotifier.instance.AddColorInfoChangeHandler(ShowBasicInformation);
        UIBTInformationNotifier.instance.AddExtensionInfoChangeHandler(ShowExtensionInformation);
	}

    public void ShowBasicInformation(string color)
    {
        if (color != UIBTInformationNotifier.instance.colorString && "" != UIBTInformationNotifier.instance.colorString)
        {
            return;
        }
        // Only if sth. change
        if ("" != color)
	    {
            UIBTInformationNotifier.instance.colorString = color;
            m_btName.text = "The Current Behavior Name is MiddleBTRandom with " + color + "Action";
            m_btDescription.text = "The Current Behavior with A " + color + " differenet in the Enmey with the original behavior";
	    }
    }

    public void ShowExtensionInformation(string extension)
    {
        if (extension != UIBTInformationNotifier.instance.extensionString && "" != UIBTInformationNotifier.instance.extensionString)
        {
            return;
        }
        // Only if sth. change
        if ("" != extension)
        {
            m_btExtension.text = "The Current Behavior extension is : " + extension;
        }
    }

    public void OnDestroy()
    {
        UIBTInformationNotifier.instance.RemoveColorInfoChangeHandler(ShowBasicInformation);
        UIBTInformationNotifier.instance.RemoveColorInfoChangeHandler(ShowExtensionInformation);
    }
}
