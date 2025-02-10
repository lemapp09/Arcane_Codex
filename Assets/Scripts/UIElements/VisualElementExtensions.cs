using UnityEditor.UIElements;
using UnityEngine.UIElements;

public static class VisualElementExtensions {

    #region CreateChild Visual Element
    /// <summary>
    /// Creates a new VisualElement, adds it to the specified <paramref name="parent"/>,
    /// and adds the specified <paramref name="classes"/> to the child.
    /// </summary>
    /// <param name="parent">The parent VisualElement to which the new child will be added.</param>
    /// <param name="classes">The classes to add to the child.</param>
    /// <returns>The new child VisualElement.</returns>
    public static VisualElement CreateChild(this VisualElement parent, params string[] classes) {
        var child = new VisualElement();
        child.AddClass(classes).AddTo(parent);
        return child;
    }
    #endregion
    
    #region CreateChild Visual Element of Type T
    /// <summary>
    /// Creates a new VisualElement of type <typeparamref name="T"/>, adds it to the specified <paramref name="parent"/>,
    /// and adds the specified <paramref name="classes"/> to the child.
    /// </summary>
    /// <typeparam name="T">The type of the VisualElement to create.</typeparam>
    /// <param name="parent">The parent VisualElement to which the new child will be added.</param>
    /// <param name="classes">The classes to add to the child.</param>
    /// <returns>The new child VisualElement.</returns>
    public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new() {
        var child = new T();
        child.AddClass(classes).AddTo(parent);
        return child;
    }
    #endregion

    #region AddTo
    /// <summary>
    /// Adds the VisualElement <paramref name="child"/> to the specified <paramref name="parent"/> and returns the child.
    /// </summary>
    /// <typeparam name="T">The type of the VisualElement.</typeparam>
    /// <param name="child">The VisualElement to add to the parent.</param>
    /// <param name="parent">The parent VisualElement to which the child will be added.</param>
    /// <returns>The child VisualElement that was added to the parent.</returns>
    public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement {
        parent.Add(child);
        return child;
    }
    #endregion

    #region AddClass
    /// <summary>
    /// Adds each class in <paramref name="classes"/> to <paramref name="visualElement"/>'s class list.
    /// </summary>
    /// <param name="visualElement">The visual element to add the classes to.</param>
    /// <param name="classes">The classes to add to the visual element.</param>
    /// <returns>The visual element with the added classes.</returns>
    public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement {
        foreach (string cls in classes) {
            if (!string.IsNullOrEmpty(cls)) {
                visualElement.AddToClassList(cls);
            }
        }
        return visualElement;
    }
    #endregion
    
    #region WithManipulator
    /// <summary>
    /// Adds a manipulator to the VisualElement and returns the element.
    /// </summary>
    /// <typeparam name="T">The type of the VisualElement.</typeparam>
    /// <param name="visualElement">The VisualElement to which the manipulator will be added.</param>
    /// <param name="manipulator">The manipulator to add to the VisualElement.</param>
    /// <returns>The VisualElement with the manipulator added.</returns>
    public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement {
        visualElement.AddManipulator(manipulator);
        return visualElement;
    }
    #endregion
    
    #region Clickable
    /// <summary>
    /// Sets the picking mode of the VisualElement to Position, making it clickable.
    /// </summary>
    /// <typeparam name="T">The type of the VisualElement.</typeparam>
    /// <param name="visualElement">The VisualElement to modify.</param>
    /// <returns>The VisualElement with its picking mode set to clickable.</returns>
    public static T Clickable<T>(this T visualElement) where T : VisualElement {
        visualElement.pickingMode = PickingMode.Position;
        return visualElement;
    }
    #endregion
    
    #region Unclickable
    /// <summary>
    /// Sets the picking mode of the VisualElement to Ignore, making it unclickable.
    /// </summary>
    /// <typeparam name="T">The type of the VisualElement.</typeparam>
    /// <param name="visualElement">The VisualElement to modify.</param>
    /// <returns>The VisualElement with its picking mode set to Ignore.</returns>
    public static T Unclickable<T>(this T visualElement) where T : VisualElement {
        visualElement.pickingMode = PickingMode.Ignore;
        return visualElement;
    }
    #endregion
    
    #region Name
    /// <summary>
    /// Sets the name of the VisualElement and returns it.
    /// </summary>
    /// <param name="visualElement">The VisualElement to set the name of.</param>
    /// <param name="name">The name to set.</param>
    /// <returns>The VisualElement with the name set.</returns>
    public static T Name<T>(this T visualElement, string name) where T : VisualElement {
        visualElement.name = name;
        return visualElement;
    }
    #endregion
}