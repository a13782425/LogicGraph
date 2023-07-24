using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    //public class BoxShadow : VisualElement
    //{
    //    private const string UssClassName = "logic-graph-box-shadow";

    //    public BoxShadow()
    //    {
    //        AddToClassList(UssClassName);
    //    }
    //}
    //public static class BoxShadowExtension
    //{
    //    public static VisualElement AddBoxShadow(this VisualElement ve)
    //    {
    //        var boxShadow = new BoxShadow();
    //        ve.hierarchy.Insert(0, boxShadow);
    //        ve.RegisterCallback<GeometryChangedEvent>(evt =>
    //        {
    //            var style = boxShadow.style;
    //            var resolvedStyle = boxShadow.resolvedStyle;

    //            style.width = evt.newRect.width + resolvedStyle.borderLeftWidth + resolvedStyle.borderRightWidth - 2f;
    //            style.height = evt.newRect.height + resolvedStyle.borderTopWidth + resolvedStyle.borderBottomWidth - 2f;

    //            var windowStyle = ve.resolvedStyle;

    //            style.marginLeft = -(resolvedStyle.borderLeftWidth + windowStyle.borderLeftWidth) + 0.5f;
    //            style.marginTop = -(resolvedStyle.borderTopWidth + windowStyle.borderTopWidth) + 0.5f;
    //        });
    //        return ve;
    //    }

    //    public static VisualElement AddBoxShadow(this GenericDropdownMenu menu)
    //    {
    //        var outerContainer = menu.contentContainer.parent.parent.parent.parent;
    //        outerContainer.AddBoxShadow();
    //        return outerContainer;
    //    }
    //}
}
