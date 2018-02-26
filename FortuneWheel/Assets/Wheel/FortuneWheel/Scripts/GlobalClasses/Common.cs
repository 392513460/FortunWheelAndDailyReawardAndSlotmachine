using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

//----
public static class Common
{
	//------------------------------------
	public static float RealClamp(float value, float min, float max)
	{
		return Mathf.Clamp(value, min, Mathf.Max(max, min));
	}
	//------------------------------------	
	
	//------------------------------------
	public static int RealClamp(int value, int min, int max)
	{
		return Mathf.Clamp(value, min, Mathf.Max(max, min));
	}
	//------------------------------------	
	
	//------------------------------------
	public static float ClampLow(float value, float min)
	{
		return (value >= min) ? value : min;
	}
	//------------------------------------		
	
	//------------------------------------
	public static int ClampLow(int value, int min)
	{
		return (value >= min) ? value : min;
	}
	//------------------------------------	
	
	//------------------------------------
	public static float ClampHigh(float value, float max)
	{
		return (value < max) ? value : max;
	}
	//------------------------------------		
	
	//------------------------------------
	public static int ClampHigh(int value, int max)
	{
		return (value < max) ? value : max;
	}
	//------------------------------------
	
	
	private static string RemoveCarriageReturn(string text)
	{
#if !UNITY_FLASH
		text = text.Trim();

		text = text.Replace("\\n","\n");
		text = text.Replace("\\r","\r");
		text = text.Replace("\r\n"	, string.Empty);
		text = text.Replace("\r"	, string.Empty);
		text = text.Replace("\n"	, string.Empty);
#endif
		return text;
	}

	private static string ReverString(string input)
	{
		string result = "";
		
		for (int i = input.Length - 1; i >= 0 ; i--)
		{
            result += input.Substring(i, 1);
		}
		
		return result;
	}
	
	//format int to formatted string
	public static string PointsToFormatString(int points, bool needK = false)
	{
		string	sPoints		= ReverString(points + "");
		int		fromCount	= sPoints.Length - 1;
		
		for (int i = fromCount; i >= 0 ; i--)
		{
			if (i % 3 == 0 && i != 0)
			{
                sPoints = sPoints.Substring(0, i) + "," + sPoints.Substring(i, sPoints.Length - i);
			}
		}
		
		string result = ReverString(sPoints);
        if (needK && result.Length > 8)
        {
            result = result.Substring(0, result.Length - 4) + "K";
        }
	    return result;
	}

    /// <summary>
    /// Cuts string to fit within the specified maxLength if length of string more than maxLength.
    /// </summary>
    /// <param name="text">Text to be cuted</param>
    /// <param name="maxLengt">maxLengt in characters</param>
    /// <param name="postFix">The symbols should be added to the end of the string if it cutted</param>
    /// <returns>The modified text</returns>
    public static string StringMaxLength(string text, int maxLength, string postFix = "...")
    {
        if (text.Length <= maxLength) {
            return text;
        }
        string newString = text.Substring(0, maxLength - postFix.Length);
        if (postFix.Length > 0)
        {
            newString += postFix;
        }
        return newString;
    } // StringMaxLength
    
    public static string StringNumberFormat(string text, int blockLength, string delim = ",")
    {
        if (text.Length <= blockLength)
        {
            return text;
        }
        text = Common.ReverString(text);
        string newString = "";
        string nextBlock = "";
        int current = 0;
        do
        {
            nextBlock = text.Substring(0, blockLength);
            UDebug.Log("current = " + current + " || nextBlock = " + nextBlock);
            if (nextBlock.Length > 0)
            {
                if (newString.Length > 0)
                {
                    newString += delim;
                }
                newString += nextBlock;
            }
            else
            {
                break;
            }
            current += blockLength;
        } while (nextBlock.Length > 0 && current < text.Length);
        return Common.ReverString(newString);
    } // StringNumberFormat
	
    //----
	static	int	iDays		= 24;
	static	int iMonths		= 31 * iDays;

    public static int GetHours(System.DateTime date)
    {
        return date.Month * iMonths + date.Day * iDays + date.Hour;
    }
    //----
	
	/// <summary>
	/// Locates position to break the given line so as to avoid
	/// breaking words.
	/// </summary>
	/// <param name="text">String that contains line of text</param>
	/// <param name="pos">Index where line of text starts</param>
	/// <param name="max">Maximum line length</param>
	/// <returns>The modified line length</returns>
	private static int BreakLine(string text, int pos, int max)
	{
	    // Find last whitespace in line
	    int i = max;
	    while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
	        i--;
	
	    // If no whitespace found, break at maximum length
	    if (i < 0)
	        return max;
	
	    // Find start of whitespace
	    while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
	        i--;
	
	    // Return length of text before whitespace
	    return i + 1;
	}

	public static string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
	 
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
	 
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
	 
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
	 
		return hashString.PadLeft(32, '0');
	} // Md5Sum

    public static Transform GetControllerHolder()
    {
        GameObject holder = GameObject.Find("ControllersHolder") as GameObject;
        if (holder == null)
        {
            holder = new GameObject();
            holder.name = "ControllersHolder";
            //holder.AddComponent<ControllersHolder>();
        }
        MonoBehaviour.DontDestroyOnLoad(holder);
        return holder.transform;
    } // GetControllerHolder

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool GetBoolFromPlayerPrefs(string key, bool defaultValue = false)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, Convert.ToInt32(defaultValue));
            return defaultValue;
        }
        if (PlayerPrefs.GetInt(key) == 0)
        {
            return false;
        }
        return true;
    } // GetBoolFromPlayerPrefs

    public static string GetStringFromPlayerPrefs(string key, string defaultValue = null)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultValue;
        }
        return PlayerPrefs.GetString(key);
    } // GetStringFromPlayerPrefs

    public static List<int> SumIntLists(List<int> l1, List<int> l2)
    {
        //DebugMy.Log("[SumIntLists] l1.Count = " + l1.Count + " | l2.Count = " + l2.Count);
        int minSize = Math.Min(l1.Count, l2.Count);
        //DebugMy.Log("[SumIntLists] minSize = " + minSize);
        List <int> result = new List<int>(new int[minSize]);
        for (int i = 0; i < minSize; i++)
        {
            result[i] = l1[i] + l2[i];
        }
        return result;
    } // SumIntLists

} // Common
