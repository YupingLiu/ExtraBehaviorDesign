using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtraBehaviorDesign.TestScripts
{
    public class UIBTInformationNotifier
    {
        public static UIBTInformationNotifier instance;
        // Random Color Info
        public string colorString = "";
        // Extension Action Info
        public string extensionString = "";

        public UIBTInformationNotifier()
        {
            if (null == instance)
            {
                instance = this;
            }
        }

        public delegate void OnColorInfoChangeHandler(string info);
        private OnColorInfoChangeHandler m_ColorInfoChangeHandler;

        public delegate void OnExtensionInfoChangeHandler(string info);
        private OnExtensionInfoChangeHandler m_ExtensionInfoChangeHandler;

        public void AddColorInfoChangeHandler(OnColorInfoChangeHandler handler)
        {
            m_ColorInfoChangeHandler += handler;
        }

        public void RemoveColorInfoChangeHandler(OnColorInfoChangeHandler handler)
        {
            m_ColorInfoChangeHandler -= handler;
        }

        public void AddExtensionInfoChangeHandler(OnExtensionInfoChangeHandler handler)
        {
            m_ExtensionInfoChangeHandler += handler;
        }

        public void RemoveExtensionInfoChangeHandler(OnExtensionInfoChangeHandler handler)
        {
            m_ExtensionInfoChangeHandler -= handler;
        }

        public void NotifyColorInfoChangeEvent(string info)
        {
            if (null != m_ColorInfoChangeHandler)
            {
                m_ColorInfoChangeHandler(info);
            }
        }

        public void NotifyExtensionInfoChangeEvent(string info)
        {
            if (null != m_ExtensionInfoChangeHandler)
            {
                m_ExtensionInfoChangeHandler(info);
            }
        }
    }
}
