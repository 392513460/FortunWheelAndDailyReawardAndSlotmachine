using UnityEngine;
using System.Collections;

public enum DSct
{
    Unknown,
    Temp,
    Starting,
    
    Network,
    Social,

    Dialogs,

    GameEngine
}


static public class UDebug 
{
    static public bool enabled = false;

    static private bool inited = false;
    static private Hashtable disabledSections;
    static private Hashtable disabledCallers;

    static private void Init()
    {
        UDebug.inited = true;
        UDebug.disabledSections = new Hashtable();
        UDebug.disabledCallers = new Hashtable();

        if (UDebug.disabledSections != null && UDebug.disabledSections.Count > 0)
        {
            foreach (DictionaryEntry pair in UDebug.disabledSections)
            {
                if ((bool)pair.Value)
                {
                    UDebug.LogWarning("[UDebug] disabled for [" + pair.Key + "] section.");
                }
            }
        }
        if (UDebug.disabledCallers != null && UDebug.disabledCallers.Count > 0)
        {
            foreach (DictionaryEntry pair in UDebug.disabledCallers)
            {
                if ((bool)pair.Value)
                {
                    UDebug.LogWarning("[UDebug] disabled for [" + pair.Key + "] caller.");
                }
            }
        }
    } // Init

    static public void Log(object message)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        UDebug.Log(message, DSct.Unknown, null);
    }

    static public void Log(object message, DSct section)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        if (!UDebug.inited)
        {
            UDebug.Init();
        }
        if (UDebug.IsSectionDisabled(section))
        {
            return;
        }
        UDebug.Log(message, section, null);
    }

    static public void Log(object message, Object context)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        UDebug.Log(message, DSct.Unknown, context);
    } // Log

    static private void Log(object message, DSct section, Object context)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        if (!UDebug.inited)
        {
            UDebug.Init();
        }
        if (UDebug.IsSectionDisabled(section))
        {
            return;
        }
        
        string prefix = "";
        if (section != DSct.Unknown)
        {
            prefix += "[" + section.ToString() + "] ";
        }
        if (context != null)
        {
            prefix += "[" + context.name + "] ";
        }

        if (context != null)
        {
            Debug.Log(prefix + message.ToString(), context);
        }
        else
        {
            //Debug.Log(prefix + message.ToString());
        }
    } // Log

    // ERROR
    static public void LogError(object message)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        UDebug.LogError(message, DSct.Unknown, null);
    } // LogError

    static public void LogError(object message, DSct section)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        if (!UDebug.inited)
        {
            UDebug.Init();
        }
        if (UDebug.IsSectionDisabled(section))
        {
            return;
        }
        UDebug.LogError(message, section, null);
    } // LogError

    static public void LogError(object message, Object context)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        UDebug.LogError(message, DSct.Unknown, context);
    } // LogError
    
    static private void LogError(object message, DSct section, Object context)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        if (!UDebug.inited)
        {
            UDebug.Init();
        }
        if (UDebug.IsSectionDisabled(section))
        {
            return;
        }

        string prefix = "";
        if (section != DSct.Unknown)
        {
            prefix += "[" + section.ToString() + "] ";
        }
        if (context != null)
        {
            prefix += "[" + context.name + "] ";
        }

        if (context != null)
        {
            Debug.Log("[ERROR] " + prefix + message.ToString(), context);
        }
        else
        {
            Debug.Log("[ERROR] " + prefix + message.ToString());
        }

    } // LogError

    static public void LogException()
    {

    }

    // WARNING
    static public void LogWarning(object message)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        UDebug.LogWarning(message, DSct.Unknown, null);
    } // LogWarning

    static public void LogWarning(object message, DSct section)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        if (!UDebug.inited)
        {
            UDebug.Init();
        }
        if (UDebug.IsSectionDisabled(section))
        {
            return;
        }
        UDebug.LogWarning(message, section, null);
    } // LogWarning

    static public void LogWarning(object message, Object context)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        UDebug.LogWarning(message, DSct.Unknown, context);
    } // LogWarning
    
    static private void LogWarning(object message, DSct section, Object context)
    {
        if (!UDebug.enabled)
        {
            return;
        }
        if (!UDebug.inited)
        {
            UDebug.Init();
        }
        if (UDebug.IsSectionDisabled(section))
        {
            return;
        }

        string prefix = "";
        if (section != DSct.Unknown)
        {
            prefix += "[" + section.ToString() + "] ";
        }
        if (context != null)
        {
            prefix += "[" + context.name + "] ";
        }

        if (context != null)
        {
            Debug.Log("[WARNING] " + prefix + message.ToString(), context);
        }
        else
        {
            Debug.Log("[WARNING] " + prefix + message.ToString());
        }

    } // LogWarning



    static private bool IsSectionDisabled(DSct section)
    {
        if (!UDebug.enabled)
        {
            return true;
        }
        if (UDebug.disabledSections == null)
        {
            return false;
        }
        if (UDebug.disabledSections.ContainsKey(section) && (bool)UDebug.disabledSections[section])
        {
            return true;
        }
        return false;
    } // IsSectionDisabled

    static private bool IsCallerDisabled(string callerName)
    {
        if (!UDebug.enabled)
        {
            return true;
        }
        if (UDebug.disabledCallers == null)
        {
            return false;
        }
        if (UDebug.disabledCallers.ContainsKey(callerName) && (bool)UDebug.disabledCallers[callerName])
        {
            return true;
        }
        return false;
    } // IsCallerDisabled

} // UDebug