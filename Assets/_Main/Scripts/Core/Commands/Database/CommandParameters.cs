using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CommandParameters 
    {
        private const char PARAMETER_IDENTIFIER = '-';

        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        public CommandParameters(string[] parameterArray)
        {
            for (int i = 0; i < parameterArray.Length; i++)
            {
                if (parameterArray[i].StartsWith(PARAMETER_IDENTIFIER))
                {
                    string pName = parameterArray[i];
                    string pValue = ""; 

                    if (i + 1 < parameterArray.Length && !parameterArray[i + 1].StartsWith(PARAMETER_IDENTIFIER))
                    {
                        pValue = parameterArray[i + 1];
                        i++;

                    }

                    parameters.Add(pName, pValue);
                }
            }
        }

        public bool TryGetValue<T>(string parameterName, out T value, T defaultValue = default(T)) => TryGetValue(new string[] { parameterName }, out value, defaultValue);

        public bool TryGetValue<T>(string[] parameterNames, out T value, T defaultValue = default(T))
        {
            foreach (string parameterName in parameterNames)
            {
                if (parameters.TryGetValue(parameterName, out string parameterValue))
                {
                    if (TryCastParameter(parameterValue, out value))
                    {
                        return true;
                    }
                }
            }

            value = defaultValue;
            return false;
        }

        private bool TryCastParameter<T>(string parameterValue, out T value)
        {
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(parameterValue, out bool boolValue))
                {
                    value = (T)(object)boolValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(parameterValue, out int intValue))
                {
                    value = (T)(object)intValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                value = (T)(object)parameterValue;
                return true;
            }

            value = default(T); 
            return false;
        }

    }
}