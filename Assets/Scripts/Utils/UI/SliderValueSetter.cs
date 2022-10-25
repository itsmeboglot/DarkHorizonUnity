using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueSetter : MonoBehaviour
{
   public TMP_Text TextObject;

   public void SetSliderValue(Single value)
   {
      TextObject.text = value.ToString(CultureInfo.InvariantCulture);
   }
}
