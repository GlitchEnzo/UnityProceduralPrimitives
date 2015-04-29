namespace ProceduralPrimitives
{
    using UnityEngine;

    public class Popup
    {
        private static int popupListHash = "PopupList".GetHashCode();

        public delegate void ListCallBack();

        public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent,
            GUIContent[] list, GUIStyle listStyle)
        {
            return List(position, ref showList, ref listEntry, buttonContent, list, "button", "box", listStyle, null);
        }

        public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent,
            GUIContent[] list, GUIStyle listStyle, ListCallBack callBack)
        {
            return List(position, ref showList, ref listEntry, buttonContent, list, "button", "box", listStyle, callBack);
        }

        public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent,
            GUIContent[] list, GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle, ListCallBack callBack)
        {
            int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
            bool done = false;
            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.mouseDown:
                    if (position.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        showList = true;
                    }
                    break;
                case EventType.mouseUp:
                    if (showList)
                    {
                        done = true;

                        if (callBack != null)
                        {
                            callBack();
                        }
                    }
                    break;
            }

            GUI.Label(position, buttonContent, buttonStyle);
            if (showList)
            {
                string[] text = new string[list.Length];
                for (int i = 0; i < list.Length; i++)
                {
                    text[i] = list[i].text;
                }

                Rect listRect = new Rect(position.x, position.y, position.width, list.Length*20);
                GUI.Box(listRect, "", boxStyle);
                listEntry = GUI.SelectionGrid(listRect, listEntry, text, 1, listStyle);
            }
            if (done)
            {
                showList = false;
            }
            return done;
        }
    }
}