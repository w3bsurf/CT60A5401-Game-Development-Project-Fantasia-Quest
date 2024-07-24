using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine.UIElements;
using static PlasticPipe.PlasticProtocol.Messages.NegotiationCommand;
using UnityEditor.UIElements;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments. Removed unnecessary parts such as adding classes and stylings, added new command types
 */

// Class for the cutscene editor window in Unity editor mode
public class CutsceneEditor : EditorWindow
{
    private static Cutscene cutscene;

    // Shows the editor UI window
    public static void ShowWindow(Cutscene scene)
    {
        cutscene = scene;
        var window = GetWindow<CutsceneEditor>();
        window.titleContent = new GUIContent("Cutscene editor");
        window.minSize = new Vector2(800, 600);
    }

    private void OnEnable()
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Windows/CutsceneEditorUIXML.uxml");
        TemplateContainer treeAsset = original.CloneTree();
        rootVisualElement.Add(treeAsset);
        scene = cutscene;
        commands = FindAllCutsceneCommands();

        InitializeWindow();

        RenderWindow();
    }

    private Cutscene scene;
    private List<CutsceneCommand> commands;

    private GroupBox commandListBox;
    private ListView commandList;
    private IntegerField commandPosition;
    private Button addButton;
    private HelpBox helpBox;
    private ScrollView scrollView;

    // Initializes UI window
    private void InitializeWindow()
    {
        commandListBox = rootVisualElement.Query<GroupBox>("command-list-box").First();
        commandList = rootVisualElement.Query<ListView>("command-list").First();
        commandPosition = rootVisualElement.Query<IntegerField>("position").First();
        addButton = rootVisualElement.Query<Button>("add-button").First();
        helpBox = new HelpBox();
        helpBox.text = "Position is invalid";

        scrollView = rootVisualElement.Query<ScrollView>("scene-info-scroll").First();

        addButton.clicked += () =>
        {
            if (commandList.selectedItem != null)
            {
                scene.InsertCommand(commandPosition.value, CreateCommand((CutsceneCommand)commandList.selectedItem));
            }

            RenderWindow();
            EditorUtility.SetDirty(scene);
        };
    }

    private CutsceneCommand selectedCommand => (CutsceneCommand)commandList.selectedItem;

    // Renders the editor window
    private void RenderWindow()
    {
        DisplayCommands();
        DisplaySceneContents();
    }

    // Function for displaying different selectable commands that can be added to a scene
    private void DisplayCommands()
    {
        commandListBox.Clear();

        commandList.makeItem = () => new Label();
        commandList.bindItem = (element, i) => (element as Label).text = commands[i].ToString();
        commandList.itemsSource = commands;
        commandList.fixedItemHeight = 16;
        commandList.selectionType = SelectionType.Single;
        commandListBox.Add(commandList);

        commandPosition.value = scene.Commands.Count;
        commandListBox.Add(commandPosition);

        if (commandPosition.value < 0 || commandPosition.value > scene.Commands.Count)
        {
            commandListBox.Add(helpBox);
        }
        else
        {
            commandListBox.Add(addButton);
        }
    }

    // Function for displaying the different commands for cutscene
    private void DisplaySceneContents() 
    {
        scrollView.Clear();

        SerializedObject cutsceneSerializedObject = new SerializedObject(scene);

        for (int i = 0; i < scene.Commands.Count; i++)
        {
            VisualElement commandContainer = new VisualElement();

            VisualElement header = GenerateHeaderRow(i);
            commandContainer.Add(header);

            SerializedProperty serializedCommand = cutsceneSerializedObject.FindProperty("commands").GetArrayElementAtIndex(i);
            List<SerializedProperty> alreadyDrawn = new List<SerializedProperty>();

            foreach (SerializedProperty property in serializedCommand)
            {
                if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
                {
                    alreadyDrawn.Add(property);
                    ListView listView = new ListView();
                    MakeNicerListView(listView);
                    listView.BindProperty(property);
                    listView.headerTitle = property.displayName;  

                    commandContainer.Add(listView);
                }
                else if ( !alreadyDrawn.Contains(property))
                {
                    PropertyField field = new PropertyField(property);
                    field.Bind(cutsceneSerializedObject);
                    commandContainer.Add(field);
                }  
            }

            scrollView.Add(commandContainer);
        }
    }

    // Tidies up the listview
    private void MakeNicerListView(ListView listView)
    {
        listView.showFoldoutHeader = true;
        listView.showAddRemoveFooter = true;
        listView.showBorder = true;
        listView.showBoundCollectionSize = false;
        listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        listView.reorderable = true;
        listView.reorderMode = ListViewReorderMode.Animated;
        listView.selectionType = SelectionType.Single; 
    }

    // Generates the header row for an added cutscene command
    private VisualElement GenerateHeaderRow(int i) 
    {
        VisualElement container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;

        Label label = new Label();
        label.text = $"({i}) " + scene.Commands[i].ToString();
        container.Add(label);

        Button deleteButton = new Button();
        deleteButton.text = "Delete";

        deleteButton.clicked += () =>
        {
            scene.RemoveAt(i);
            DisplaySceneContents();
        };
        container.Add(deleteButton);

        if (i > 0)
        {
            Button upButton = new Button();
            upButton.text = "Up";
            upButton.clicked += () =>
            {
                scene.SwapCommands(i - 1, i);
                DisplaySceneContents();
            };
            container.Add(upButton);
        }

        if (i < scene.Commands.Count - 1)
        {
            Button downButton = new Button();
            downButton.text = "Down";
            downButton.clicked += () =>
            {
                scene.SwapCommands(i, i + 1);
                DisplaySceneContents();
            };
            container.Add(downButton);
        }
        return container;
    }

    // Finds all of the different types of cutscene commands with the exception of the main cutscenecommand class
    private List<CutsceneCommand> FindAllCutsceneCommands()
    {
        var type = typeof(CutsceneCommand);

        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(assembly => type.IsAssignableFrom(assembly) && !assembly.IsInterface).Select(type => Activator.CreateInstance(type) as CutsceneCommand).ToList();
    }

    // Creates a command for each different cutscene command type
    private CutsceneCommand CreateCommand(CutsceneCommand command)
    {
        if (command is MoveCharacterCommand)
        {
            return new MoveCharacterCommand();
        }

        if (command is MovePlayerCommand)
        {
            return new MovePlayerCommand();
        }

        if (command is WaitCommand)
        {
            return new WaitCommand();
        }

        if (command is DialogueCommand)
        {
            return new DialogueCommand();
        }

        if (command is AddMageCommand)
        {
            return new AddMageCommand();
        }

        if (command is StartBossBattleCommand)
        {
            return new StartBossBattleCommand();
        }

        if (command is StartCreditsCommand)
        {
            return new StartCreditsCommand();
        }

        if (command is HideCharacterCommand)
        {
            return new HideCharacterCommand();
        }

        if (command is ShowCharacterCommand)
        {
            return new ShowCharacterCommand();
        }

        if (command is MoveBothCommand)
        {
            return new MoveBothCommand();
        }

        if (command is TurnBothCommand)
        {
            return new TurnBothCommand();
        }

        else
        {
            Debug.LogError("Bad command");
            return null;
        }
    }
}
