using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;


public class QuickTool : EditorWindow
{
    // menu tab at the top & its hotkey
    [MenuItem("QuickTool/Open _%#T")] 

    public static void ShowWindow() {
        // Opens the window, otherwise focuses it if it's already open.
        var window = GetWindow<QuickTool>();

        // Adds a title to the window.
        window.titleContent = new GUIContent("QuickTool");

        // Sets a minimum size to the window.
        window.minSize = new Vector2(280, 50);
    }

    private void CreateGUI() {
        // Reference to the root of the window.
        var root = rootVisualElement;

        // Associates a stylesheet to our root. Thanks to inheritance, all root’s
        // children will have access to it.
        root.styleSheets.Add(Resources.Load<StyleSheet>("QuickTool_Style"));

        // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("QuickTool_Main");
        quickToolVisualTree.CloneTree(root);

        // Queries all the buttons (via type) in our root and passes them
        // in the SetupButton method.
        var toolButtons = root.Query(className: "quicktool-button");
        toolButtons.ForEach(SetupButton);
    }

    private void SetupButton(VisualElement button) {
    // Reference to the VisualElement inside the button that serves
    // as the button's icon.
    // class name "quicktool-button-icon" assigned inside the UXML file.
    var buttonIcon = button.Q(className: "quicktool-button-icon");

    // Icon's path in our project.
    // var iconPath = "Icons/" + button.parent.name + "_icon";
    var iconPath = "P:/dd-pipeline/Icons/viola_icon";
    
    string[] filePaths = Directory.GetFiles(@"P:/dd-pipeline/Icons/", "*.png"); // get every file in chosen directory with the extension.png
    // write those files out so I can see them
    Debug.Log(filePaths);
    WWW www = new WWW("file://" + filePaths[0]);                  // "download" the first file from disk
    yield return www;                                                               // Wait unill its loaded
    Texture2D new_texture = new Texture2D(512,512);               // create a new Texture2D (you could use a gloabaly defined array of Texture2D )
    www.LoadImageIntoTexture(new_texture);                           // put the downloaded image file into the new Texture2D
    var iconAsset = new_texture;           // put the new image into the current material as defuse material for testing.
       

    // Loads the actual asset from the above path.
    //var iconAsset = Resources.Load<Texture2D>(iconPath);

    // Applies the above asset as a background image for the icon.
    buttonIcon.style.backgroundImage = iconAsset;

    // Instantiates our primitive object on a left click.
    button.RegisterCallback<PointerUpEvent, string>(CreateObject, button.parent.name);

    // Sets a basic tooltip to the button itself.
    button.tooltip = button.parent.name;
    }

    private void CreateObject(PointerUpEvent _, string primitiveTypeName) {   
    var pt = (PrimitiveType) Enum.Parse(typeof(PrimitiveType), primitiveTypeName, true);
    var go = ObjectFactory.CreatePrimitive(pt);
    go.transform.position = Vector3.zero;
    }
}
