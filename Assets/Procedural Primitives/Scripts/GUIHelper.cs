namespace ProceduralPrimitives
{
    using UnityEngine;

    public static class GUILayoutHelper
    {
        private static float MaxLabelWidth = 130;
        private static float MinSliderWidth = 40;

        public static float Slider(string label, float value, float leftValue, float rightValue)
        {
            GUILayout.BeginHorizontal();
            // force the width of the label to prevent the slider from changing size
            GUILayout.Label(string.Format("{0}{1:F2}", label, value), GUILayout.MaxWidth(MaxLabelWidth));
            float newValue = GUILayout.HorizontalSlider(value, leftValue, rightValue, GUILayout.MinWidth(MinSliderWidth));
            GUILayout.EndHorizontal();

            return newValue;
        }

        public static int IntSlider(string label, int value, int leftValue, int rightValue)
        {
            GUILayout.BeginHorizontal();
            // force the width of the label to prevent the slider from changing size
            GUILayout.Label(string.Format("{0}{1}", label, value), GUILayout.MaxWidth(MaxLabelWidth));
            int newValue =
                (int) GUILayout.HorizontalSlider(value, leftValue, rightValue, GUILayout.MinWidth(MinSliderWidth));
            GUILayout.EndHorizontal();

            return newValue;
        }
    }
}