using System;
using System.Collections.Generic;
using System.Linq;
using RAGE;

namespace BEASTFUCKINGMP
{
    public static class GUI
    {
		public static float GetDeltaTime()
        {
            return RAGE.Game.Misc.GetFrameTime();
        }
		
		public static float Lerp(float a, float b, float t)
        {
            if (t > 1.0f)
                t = 1.0f;
            if (t < 0.0f)
                t = 0.0f;
            return (1f - t) * a + t * b;
        }
		
        public static PhoneObject Phone = new PhoneObject(0, 0);
        public class PhoneObject
        {
            AppObject Settings_Main = null;
            AppObject Settings_Theme = null;
            AppObject Settings_Volume = null;
            AppObject Settings_Wallpaper = null;
            AppObject Settings_Wallpaper_Ext = null;

            AppObject AppScroll = null;
            public void Initialize()
            {
                //Create app containers and set links between them
                Settings_Main = new AppObject("Settings", AppContainer.Settings);
                Settings_Theme = new AppObject("Theme Settings", AppContainer.Settings);
                Settings_Theme.Backward = Settings_Main; //on back go to the main settings
                Settings_Volume = new AppObject("Volume Settings", AppContainer.Settings);
                Settings_Volume.Backward = Settings_Main;
                Settings_Wallpaper = new AppObject("Wallpaper Settings", AppContainer.Settings);
                Settings_Wallpaper_Ext = new AppObject("Wallpaper Demo", AppContainer.HomeMenu);
                AppScroll = new AppObject("Test", AppContainer.TodoView);

                Settings_Wallpaper_Ext.Backward = Settings_Wallpaper;
                Settings_Wallpaper.Backward = Settings_Main;

                //Set color for soft keys of each app container
                Settings_Main.SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Select, true, new RGBA(46, 204, 113));
                Settings_Main.SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Back, true, new RGBA(255, 255, 255));

                Settings_Theme.SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Yes, true, new RGBA(46, 204, 113));
                Settings_Theme.SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Back, true, new RGBA(255, 255, 255));

                Settings_Volume.SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Select, true, new RGBA(46, 204, 113));
                Settings_Volume.SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Back, true, new RGBA(255, 255, 255));

                Settings_Wallpaper.SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Yes, true, new RGBA(46, 204, 113));
                Settings_Wallpaper.SoftKey_Middle = new SoftkeyObject(SoftkeyIcon.Police, true, new RGBA(241, 196, 15));
                Settings_Wallpaper.SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Back, true, new RGBA(255, 255, 255));

                Settings_Wallpaper_Ext.SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Back, true, new RGBA(255, 255, 255));

                //Start adding buttons to each container and maybe add links to other apps on click
                Settings_Main.AddItem(new AppSettingItem("id_theme", "Theme Settings", ListIcons.Settings1, null, Settings_Theme)); //go to theme settings
                Settings_Main.AddItem(new AppSettingItem("id_volume", "Volume Settings", ListIcons.Volume, null, Settings_Volume));
                Settings_Main.AddItem(new AppSettingItem("id_wallpaper", "Wallpaper Settings", ListIcons.Settings1, null, Settings_Wallpaper));

                Settings_Theme.AddItem(new AppSettingItem("id_blue", "Blue", ListIcons.Settings1, () => //when the user clicks the item
                {
                    SetTheme(Theme.LightBlue);
                    SetPhoneColor(0);
                    RefreshDisplay();
                }));

                Settings_Theme.AddItem(new AppSettingItem("id_red", "Red", ListIcons.Settings1, () =>
                {
                    SetTheme(Theme.Red);
                    SetPhoneColor(2);
                    RefreshDisplay();
                }));

                Settings_Volume.AddItem(new AppSettingItem("id_air", "Airplane Mode [OFF]", ListIcons.SleepMode, null, null, () =>
                {
                    SetSleepMode(!SleepMode);
                    if(SleepMode)
                        Settings_Volume.GetItemByID<AppSettingItem>("id_air").Name = "Airplane Mode [ON]";
                    else
                        Settings_Volume.GetItemByID<AppSettingItem>("id_air").Name = "Airplane Mode [OFF]";
                    RefreshDisplay();
                }));
                Settings_Volume.AddItem(new AppSettingItem("id_mute", "Mute Phone [ON]", ListIcons.VibrateOn));

                Settings_Wallpaper.AddItem(new AppSettingItem("id_wall_blued", "Blue Diamonds", ListIcons.Settings1, () =>
                {
                    HomescreenImage = BackgroundImage.Diamonds;
                    RefreshDisplay();
                }));

                Settings_Wallpaper.AddItem(new AppSettingItem("id_wall_purpg", "Purple Glow", ListIcons.Settings1, () =>
                {
                    HomescreenImage = BackgroundImage.PurpleGlow;
                    RefreshDisplay();
                }));

                //Specific for wallpaper demo
                AppSettingItem id = Settings_Wallpaper.GetItemByID<AppSettingItem>("id_wall_purpg");
                id.OnSoftkey_Middle = new Action(() =>
                {
                    SetBackgroundImage(BackgroundImage.PurpleGlow);
                    ClearHomescreen();
                    OpenApp(Settings_Wallpaper_Ext);
                    HideCurrentSelection();
                    SetHeaderText("Purple Glow");
                });

                id = Settings_Wallpaper.GetItemByID<AppSettingItem>("id_wall_blued");
                id.OnSoftkey_Middle = new Action(() =>
                {
                    SetBackgroundImage(BackgroundImage.Diamonds);
                    ClearHomescreen();
                    OpenApp(Settings_Wallpaper_Ext);
                    HideCurrentSelection();
                    SetHeaderText("Diamonds");
                });

                SetHomeObjectsData(
                    new HomeObject("Contacts", HomeIcon.Call, HomescreenNumber.One, HomescreenLocation.TopLeft, 5, AppScroll),
                    new HomeObject("Messages", HomeIcon.TextMessage, HomescreenNumber.One, HomescreenLocation.TopMiddle, 99),
                    new HomeObject("Camera", HomeIcon.Camera, HomescreenNumber.One, HomescreenLocation.TopRight, 0),
                    new HomeObject("Email", HomeIcon.Email, HomescreenNumber.One, HomescreenLocation.MiddleLeft, 54),
                    new HomeObject("Settings", HomeIcon.Settings2, HomescreenNumber.One, HomescreenLocation.Middle, 1, Settings_Main),
                    new HomeObject("GPS", HomeIcon.Trackify, HomescreenNumber.One, HomescreenLocation.MiddleRight, 0),
                    new HomeObject("Browser", HomeIcon.Eyefind, HomescreenNumber.One, HomescreenLocation.BottomLeft, 0),

                    new HomeObject("TestApp1", HomeIcon.Broadcast, HomescreenNumber.Two, HomescreenLocation.Middle, 0),
                    new HomeObject("TestApp2", HomeIcon.Broadcast, HomescreenNumber.Two, HomescreenLocation.MiddleRight, 0),
                    new HomeObject("TestApp3", HomeIcon.Broadcast, HomescreenNumber.Two, HomescreenLocation.MiddleLeft, 0),
                    new HomeObject("TestApp4", HomeIcon.Broadcast, HomescreenNumber.Two, HomescreenLocation.BottomLeft, 0),

                    new HomeObject("TestApp5", HomeIcon.Multiplayer, HomescreenNumber.Three, HomescreenLocation.TopLeft, 55),
                    new HomeObject("TestApp6", HomeIcon.Multiplayer, HomescreenNumber.Three, HomescreenLocation.TopRight, 55),
                    new HomeObject("TestApp7", HomeIcon.Multiplayer, HomescreenNumber.Three, HomescreenLocation.BottomMiddle, 55)
                );
            }

            public void ClearHomescreen()
            {
                for (int i = 0; i < 9; i++)
                    SetDataSlotForHome(1, i, 23, 0, "");
            }

            public void HideCurrentSelection()
            {
                DisplayView(1, -1);
            }

            public enum HomescreenNumber
            {
                One,
                Two,
                Three
            }

            public enum HomescreenLocation
            {
                TopLeft,
                TopMiddle,
                TopRight,
                MiddleLeft,
                Middle,
                MiddleRight,
                BottomLeft,
                BottomMiddle,
                BottomRight
            }
            public enum HomeIcon
            {
                Camera = 1,
                TextMessage,
                Calendar,
                Email,
                Call,
                Eyefind,
                Map,
                Apps,
                Media = 9,
                NewContact = 11,
                BAWSAQ = 13,
                Multiplayer,
                Music,
                GPS,
                Spare = 17,
                Settings2 = 24,
                MissedCall = 27,
                UnreadEmail,
                ReadEmail,
                ReplyEmail,
                ReplayMission,
                ShitSkip,
                UnreadSMS,
                ReadSMS,
                PlayerList,
                CopBackup,
                GangTaxi,
                RepeatPlay = 38,
                Sniper = 40,
                ZitIT,
                Trackify,
                Save,
                AddTag,
                RemoveTag,
                Location,
                Party = 47,
                Broadcast = 49,
                Gamepad = 50,
                InvitesPending = 52,
                OnCall,
            }

            public enum ListIcons
            {
                Attachment = 10,
                SideTasks = 12,
                RingTone = 18,
                TextTone = 19,
                VibrateOn = 20,
                VibrateOff = 21,
                Volume = 22,
                Settings1 = 23,
                Profile = 25,
                SleepMode = 26,
                Checklist = 39,
                Ticked = 48,
                Silent = 51
        }

            public enum Theme
            {
                LightBlue = 1,
                Green,
                Red,
                Orange,
                Grey,
                Purple,
                Pink,
                Black
            }

            public enum Direction
            {
                Up = 1,
                Right,
                Down,
                Left,
            }
            public enum BackgroundImage
            {
                Default = 0,
                None = 1, //2, 3
                PurpleGlow = 4,
                GreenSquares = 5,
                OrangeHerringBone = 6,
                OrangeHalfTone = 7,
                GreenTriangles = 8,
                GreenShards = 9,
                BlueAngles = 10,
                BlueShards = 11,
                BlueCircles = 12,
                Diamonds = 13,
                GreenGlow = 14,
                Orange8Bit = 15,
                OrangeTriangles = 16,
                PurpleTartan = 17
            }

            public enum SoftKey
            {
                Left = 1,
                Middle,
                Right
            }

            public enum SoftkeyIcon
            {
                Blank = 1,
                Select = 2,
                Pages = 3,
                Back = 4,
                Call = 5,
                Hangup = 6,
                Hangup_Human = 7,
                Hide_Phone = 8,
                Keypad = 9,
                Open = 10,
                Reply = 11,
                Delete = 12,
                Yes = 13,
                No = 14,
                Sort = 15,
                Website = 16,
                Police = 17,
                Ambulance = 18,
                Fire = 19,
                Pages2 = 20
            }

            private int PhoneType = 0;
            private int PhoneBody = 0;
            private Vector3 PhonePosition_Final = new Vector3(99.62f, -45.305f, -113f);
            private Vector3 PhoneRotation_Final = new Vector3(-90f, 0f, 0f);
            private Vector3 PhonePosition_Start = new Vector3(99.62f, -180f, -113f);
            private Vector3 PhoneRotation_Start = new Vector3(-90f, 180f, 0f);
            private Vector3 PhonePosition_Current = new Vector3(99.62f, -180f, -113f);
            private Vector3 PhoneRotation_Current = new Vector3(-90f, 180f, 0f);
            private float PhoneScale = 500f;
            private int PhoneRenderID = -1;
            private int PhoneScaleform = -1;
            public bool IsOn { get; private set; } = false;
            private int HomeIndex = 0;
            private int HomescreenSelection = 0;
            private int CurrentAppSelection = 0;
            private AppObject CurrentApp = null;
            private float BatteryLevel = 1f;
            private BackgroundImage HomescreenImage = BackgroundImage.BlueAngles;
            public bool IsOnHomeScreen()
            {
                return CurrentApp == null;
            }

            public int GetCurrentIndex()
            {
                if (IsOnHomeScreen())
                    return HomescreenSelection;
                else
                    return 0;
            }

            public enum AppContainer
            {
                HomeMenu = 1,
                Contacts = 2,
                CallScreen = 4,
                MessageList = 6,
                MessageView = 7,
                EmailList = 8,
                EmailView = 9,
                Settings = 22,
                ToDoList = 17,
                TodoView = 15,
                MissionRepeat = 18,
                MissionStats = 19,
                JobList = 20,
                EmailResponse = 21
            }

            public interface AppItem
            {
                string Type { get; set; }
                AppObject Forward { get; set; }
                Action Invoker { get; set; }
                string Id { get; set; }
                Action OnSoftkey_Left { get; set; }
                Action OnSoftkey_Right { get; set; }
                Action OnSoftkey_Middle { get; set; }
            }

            public class AppSettingItem : AppItem
            {
                public string Type { get; set; } = "Setting";
                public string Name;
                public string Id { get; set; }
                public ListIcons Icon;
                public AppObject Forward { get; set; }
                public Action Invoker { get; set; }
                public Action OnSoftkey_Left { get; set; } = null;
                public Action OnSoftkey_Right { get; set; } = null;
                public Action OnSoftkey_Middle { get; set; } = null;
                public AppSettingItem(string id, string name, ListIcons icon, Action invoke = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
                {
                    Id = id;
                    Name = name;
                    Icon = icon;
                    Forward = forward;
                    Invoker = invoke;
                    OnSoftkey_Left = onleft;
                    OnSoftkey_Middle = onmiddle;
                    OnSoftkey_Right = onright;
                }
            }

            public class AppMessageItem : AppItem
            {
                public string Type { get; set; } = "Message";
                public string Id { get; set; }
                public string Hour;
                public string Minute;
                public bool Seen;
                public string FromAddress;
                public string SubjectTitle;
                public AppObject Forward { get; set; }
                public Action Invoker { get; set; }
                public Action OnSoftkey_Left { get; set; } = null;
                public Action OnSoftkey_Right { get; set; } = null;
                public Action OnSoftkey_Middle { get; set; } = null;
                public AppMessageItem(string id, string hour, string minute, bool seen, string from, string subject, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
                {
                    Id = id;
                    Hour = hour;
                    Minute = minute;
                    Seen = seen;
                    FromAddress = from;
                    SubjectTitle = subject;
                    Forward = forward;
                    Invoker = act;
                    OnSoftkey_Left = onleft;
                    OnSoftkey_Middle = onmiddle;
                    OnSoftkey_Right = onright;
                }
            }

            public class AppCallscreenItem : AppItem
            {
                public string Type { get; set; } = "Message";
                public string Id { get; set; }
                public AppObject Forward { get; set; }
                public Action Invoker { get; set; }
                public string FromAddress;
                public string JobTitle;
                public string Icon;
                public Action OnSoftkey_Left { get; set; } = null;
                public Action OnSoftkey_Right { get; set; } = null;
                public Action OnSoftkey_Middle { get; set; } = null;
                public AppCallscreenItem(string id, string from, string title, string icon, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
                {
                    Id = id;
                    FromAddress = from;
                    JobTitle = title;
                    Icon = icon;
                    Forward = forward;
                    Invoker = act;
                    OnSoftkey_Left = onleft;
                    OnSoftkey_Middle = onmiddle;
                    OnSoftkey_Right = onright;
                }
            }

            public class AppMessageViewItem : AppItem
            {
                public string Type { get; set; } = "Message";
                public string FromAddress;
                public string Id { get; set; }
                public string Message;
                public string Icon = "CHAR_HUMANDEFAULT";
                public AppObject Forward { get; set; }
                public Action Invoker { get; set; }
                public Action OnSoftkey_Left { get; set; } = null;
                public Action OnSoftkey_Right { get; set; } = null;
                public Action OnSoftkey_Middle { get; set; } = null;
                public AppMessageViewItem(string from, string msg, string icon, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
                {
                    FromAddress = from;
                    Message = msg;
                    Icon = icon;
                    Invoker = act;
                    Forward = forward;
                    OnSoftkey_Left = onleft;
                    OnSoftkey_Middle = onmiddle;
                    OnSoftkey_Right = onright;
                }
            }

            public class AppContactItem : AppItem
            {
                public string Type { get; set; } = "Message";
                public AppObject Forward { get; set; }
                public string Id { get; set; }
                public Action Invoker { get; set; }
                public bool MissedCall;
                public string Name;
                public string Icon;
                public Action OnSoftkey_Left { get; set; } = null;
                public Action OnSoftkey_Right { get; set; } = null;
                public Action OnSoftkey_Middle { get; set; } = null;
                public AppContactItem(string id, bool missedcall, string name, string icon, Action act = null, AppObject forward = null, Action onleft = null, Action onright = null, Action onmiddle = null)
                {
                    Id = id;
                    MissedCall = missedcall;
                    Name = name;
                    Icon = icon;
                    Invoker = act;
                    Forward = forward;
                    OnSoftkey_Left = onleft;
                    OnSoftkey_Middle = onmiddle;
                    OnSoftkey_Right = onright;
                }
            }

            public class AppObject
            {
                public string Name;
                public AppContainer Container;

                public List<AppItem> Items = new List<AppItem>();
                public AppObject Backward;
                public Action Invoker = null;
                public SoftkeyObject SoftKey_Left = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0,0,0));
                public SoftkeyObject SoftKey_Right = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0, 0, 0));
                public SoftkeyObject SoftKey_Middle = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0, 0, 0));
                public Action OnSoftKey_Right = null;
                public Action OnSoftKey_Left = null;
                public Action OnSoftKey_Middle = null;

                public AppObject(string name, AppContainer cont)
                {
                    Name = name;
                    Container = cont;
                }

                public T GetItemByID<T>(string id)
                {
                    return (T)Items.FirstOrDefault(x => x.Id == id);
                }

                public void AddItem(object item)
                {
                    if(Container == AppContainer.Settings)
                    {
                        if (item is AppSettingItem)
                        {
                            Items.Add((AppItem)item);
                        }
                        else
                            return;
                    }
                    else if(Container == AppContainer.MessageList)
                    {
                        if (item is AppMessageItem)
                        {
                            Items.Add((AppItem)item);
                        }
                        else
                            return;
                    }
                    else if(Container == AppContainer.MessageView)
                    {
                        if (item is AppMessageViewItem)
                        {
                            Items.Add((AppItem)item);
                        }
                        else
                            return;
                    }
                    else if(Container == AppContainer.Contacts)
                    {
                        if (item is AppContactItem)
                        {
                            Items.Add((AppItem)item);
                        }
                        else
                            return;
                    }
                    else if(Container == AppContainer.CallScreen)
                    {
                        if (item is AppCallscreenItem)
                        {
                            Items.Add((AppItem)item);
                        }
                        else
                            return;
                    }
                }
            }

            public void OpenApp(AppObject app)
            {
                if (app == null)
                    return;
                CurrentApp = app;
                CurrentAppSelection = 0;
                SetDataSlotEmpty((int)app.Container);
                for(int i = 0; i < app.Items.Count; i++)
                {
                    if(app.Container == AppContainer.Settings)
                    {
                        SetDataSlotForSetting((int)AppContainer.Settings, i, (int)(app.Items[i] as AppSettingItem).Icon, (app.Items[i] as AppSettingItem).Name);
                    }
                    else if(app.Container == AppContainer.MessageList)
                    {
                        SetDataSlotForMessageList((int)AppContainer.MessageList, i, (app.Items[i] as AppMessageItem).Hour, (app.Items[i] as AppMessageItem).Minute, (app.Items[i] as AppMessageItem).Seen, (app.Items[i] as AppMessageItem).FromAddress, (app.Items[i] as AppMessageItem).SubjectTitle);
                    }
                    else if(app.Container == AppContainer.MessageView)
                    {
                        RAGE.Chat.Output("In2");
                        SetDataSlotForMessageView((int)AppContainer.MessageView, i, (app.Items[i] as AppMessageViewItem).FromAddress, (app.Items[i] as AppMessageViewItem).Message, (app.Items[i] as AppMessageViewItem).Icon);
                    }
                    else if(app.Container == AppContainer.Contacts)
                    {
                        SetDataSlotForContactList((int)AppContainer.Contacts, i, (app.Items[i] as AppContactItem).MissedCall, (app.Items[i] as AppContactItem).Name, (app.Items[i] as AppContactItem).Icon);
                    }
                    else if(app.Container == AppContainer.CallScreen)
                    {
                        SetDataSlotForCallscreen((int)AppContainer.CallScreen, i, (app.Items[i] as AppCallscreenItem).FromAddress, (app.Items[i] as AppCallscreenItem).JobTitle, (app.Items[i] as AppCallscreenItem).Icon);
                    }
                }
                if (app.Invoker != null)
                    app.Invoker.Invoke();
                SetSoftKey_Data(SoftKey.Left, app.SoftKey_Left);
                SetSoftKey_Data(SoftKey.Right, app.SoftKey_Right);
                SetSoftKey_Data(SoftKey.Middle, app.SoftKey_Middle);
                SetHeaderText(app.Name);
                DisplayView((int)app.Container, CurrentAppSelection);
            }

            public void CloseApp(AppObject app)
            {
                if (app == null)
                    return;

                if (app.Backward == null)
                    GoHome();
                else
                    OpenApp(app.Backward);
            }

            public class HomeObject
            {
                public int NotificationNumber = 5;
                public AppObject Forward;
                public HomeIcon Icon = 0;
                public string Name = "NULL";
                public HomescreenLocation Location;
                public HomescreenNumber HomeNumber;

                public HomeObject(string name, HomeIcon icon, HomescreenNumber home, HomescreenLocation location, int notifications, AppObject link = null)
                {
                    Name = name;
                    NotificationNumber = notifications;
                    Icon = icon;
                    Location = location;
                    Forward = link;
                    HomeNumber = home;
                }
            }

            public HomeObject GetHomeObjectByIndex(int index)
            {
                if (index < 0)
                    index = 0;
                if (index > 8)
                    index = 8;
                return HomeObjects_Stored[HomeIndex][index];
            }

            List<List<HomeObject>> HomeObjects_Stored = new List<List<HomeObject>>()
            {
                new List<HomeObject>()
                {
                    null, null, null, null, null, null, null, null, null
                },
                new List<HomeObject>()
                {
                    null, null, null, null, null, null, null, null, null
                },
                new List<HomeObject>()
                {
                    null, null, null, null, null, null, null, null, null
                }
            };

            public void SetHomeObjectsData(params HomeObject[] objs)
            {
                for(int i = 0; i < objs.Length; i++)
                {
                    //SetDataSlotForHome(1, (int)objs[i].Location, (int)objs[i].Icon, objs[i].NotificationNumber, objs[i].Name);
                    HomeObjects_Stored[(int)objs[i].HomeNumber][(int)objs[i].Location] = objs[i];
                }

                for(int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (HomeObjects_Stored[j][i] == null)
                        {
                            HomeObjects_Stored[j][i] = new HomeObject(" ", HomeIcon.Spare, (HomescreenNumber)j, (HomescreenLocation)i, 0);
                            //SetDataSlotForHome(1, (int)HomeObjects[i].Location, (int)HomeObjects[i].Icon, HomeObjects[i].NotificationNumber, HomeObjects[i].Name);
                        }
                    }
                }
            }

            public void SetHomeObjectsTemp(params HomeObject[] objs)
            {
                for(int i = 0; i < objs.Length; i++)
                {
                    SetDataSlotForHome(1, (int)objs[i].Location, (int)objs[i].Icon, objs[i].NotificationNumber, objs[i].Name);
                }
            }

            public void SwitchHomeScreen(int index)
            {
                SetHomeObjectsTemp(HomeObjects_Stored[index].ToArray());
                DisplayView((int)AppContainer.Settings, 0);
            }

            public class SoftkeyObject
            {
                public SoftkeyIcon Icon = SoftkeyIcon.Blank;
                public bool Visible = false;
                public RAGE.RGBA RGBA = new RAGE.RGBA(255, 255, 255);

                public SoftkeyObject(SoftkeyIcon icon, bool vis, RGBA col)
                {
                    Icon = icon;
                    Visible = vis;
                    RGBA = col;
                }
            }

            private SoftkeyObject SoftkeyLeft = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0,0,0));
            private SoftkeyObject SoftkeyRight = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0, 0, 0));
            private SoftkeyObject SoftkeyMiddle = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0, 0, 0));

            public SoftkeyObject Home_SoftkeyLeft = new SoftkeyObject(SoftkeyIcon.Select, true, new RGBA(46, 204, 113));
            public SoftkeyObject Home_SoftkeyRight = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0, 0, 0));
            public SoftkeyObject Home_SoftkeyMiddle = new SoftkeyObject(SoftkeyIcon.Blank, false, new RGBA(0, 0, 0));

            public PhoneObject(int type, int body)
            {
                if (!(type == 0 || type == 1 || type == 2 || type == 4))
                    type = 0;

                PhoneType = type;
                PhoneBody = body;
                RAGE.Game.Mobile.DestroyMobilePhone();
                RAGE.Game.Mobile.ScriptIsMovingMobilePhoneOffscreen(true);
            }

            private bool IsInvalid()
            {
                if (PhoneScaleform == -1)
                    return true;
                return false;
            }

            public int GetCurrentSelection()
            {
                if (CurrentApp == null)
                    return HomescreenSelection;

                //RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "GET_CURRENT_SELECTION");
                return CurrentAppSelection;//RAGE.Game.Graphics.PopScaleformMovieFunction();
            }

            public bool SleepMode = false;
            public void SetSleepMode(bool active)
            {
                if (IsInvalid())
                    return;

                SleepMode = active;
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_SLEEP_MODE");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterBool(active);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void SetBackgroundImage(BackgroundImage img)
            {
                if (IsInvalid())
                    return;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_BACKGROUND_IMAGE");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)img);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void RefreshDisplay()
            {
                if(IsOnHomeScreen())
                {
                    DisplayView(1, HomescreenSelection);
                }
                else
                {
                    int select = CurrentAppSelection;
                    if(CurrentApp.Container == AppContainer.Settings)
                    {
                        AppSettingItem id = CurrentApp.Items[select] as AppSettingItem;
                        SetDataSlotForSetting((int)AppContainer.Settings, select, (int)id.Icon, id.Name);
                    }

                    DisplayView((int)CurrentApp.Container, CurrentAppSelection);
                }
            }

            public void SetHeaderText(string text)
            {
                if (IsInvalid())
                    return;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_HEADER");
                RAGE.Game.Graphics.BeginTextCommandScaleformString("STRING");
                RAGE.Game.Ui.AddTextComponentAppTitle(text, -1);
                RAGE.Game.Graphics.EndTextCommandScaleformString();
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void SetPhoneColor(int theme)
            {
                RAGE.Game.Player.SetPlayerResetFlagPreferRearSeats(theme);
            }

            public void SetTitlebarTime(int hour, int minute, string day)
            {
                if (IsInvalid())
                    return;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_TITLEBAR_TIME");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(hour);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(minute);
                RAGE.Game.Graphics.BeginTextCommandScaleformString("STRING");
                RAGE.Game.Ui.AddTextComponentAppTitle(day, -1);
                RAGE.Game.Graphics.EndTextCommandScaleformString();
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void Click()
            {
                if (IsInvalid())
                    return;

                if (IsOnHomeScreen())
                {
                    OpenApp(HomeObjects_Stored[HomeIndex][GetCurrentSelection()].Forward);
                }
                else
                {
                    if(CurrentApp.Items.Count > 0)
                    {
                        if (CurrentApp.Items[GetCurrentSelection()].Invoker != null)
                            CurrentApp.Items[GetCurrentSelection()].Invoker.Invoke();
                        if (CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Left != null)
                            CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Left.Invoke();
                    }

                    if (CurrentApp.OnSoftKey_Left != null)
                        CurrentApp.OnSoftKey_Left.Invoke();

                    if (CurrentApp.Items.Count > 0)
                        if (CurrentApp.Items[GetCurrentSelection()].Forward != null)
                             OpenApp(CurrentApp.Items[GetCurrentSelection()].Forward);
                }

                RAGE.Game.Mobile.MoveFinger(1);
            }

            public void Back()
            {
                if (IsInvalid())
                    return;

                if(!IsOnHomeScreen())
                {
                    RAGE.Chat.Output("Cur: " + GetCurrentSelection() + " Cnt: " + CurrentApp.Items.Count);
                    if (GetCurrentSelection() < CurrentApp.Items.Count)
                    {
                        if (CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Right != null)
                            CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Right.Invoke();
                    }

                    if (CurrentApp.OnSoftKey_Right != null)
                        CurrentApp.OnSoftKey_Right.Invoke();
                    CloseApp(CurrentApp);
                }
                else
                {
                    TurnOff();
                }

                RAGE.Game.Mobile.MoveFinger(2);
            }

            public void Middle()
            {
                if (IsInvalid())
                    return;

                if (!IsOnHomeScreen())
                {
                    if (CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Middle != null)
                        CurrentApp.Items[GetCurrentSelection()].OnSoftkey_Middle.Invoke();

                    if (CurrentApp.OnSoftKey_Middle != null)
                        CurrentApp.OnSoftKey_Middle.Invoke();
                }

                RAGE.Game.Mobile.MoveFinger(5);
            }

            public void SetInputEvent(Direction dir)
            {
                if (IsInvalid())
                    return;

                if(dir == Direction.Left)
                {
                    RAGE.Game.Mobile.MoveFinger(3);
                }
                else if(dir == Direction.Right)
                {
                    RAGE.Game.Mobile.MoveFinger(4);
                }

                if (CurrentApp == null)
                {
                    if (dir == Direction.Left)
                        HomescreenSelection--;
                    else if (dir == Direction.Right)
                        HomescreenSelection++;
                    else if (dir == Direction.Up)
                        HomescreenSelection -= 3;
                    else if (dir == Direction.Down)
                        HomescreenSelection += 3;

                    if (HomescreenSelection > 8)
                    {
                        HomeIndex++;
                        if (HomeIndex > 2)
                            HomeIndex = 0;
                        HomescreenSelection -= 9;
                        SwitchHomeScreen(HomeIndex);
                    }
                    else if (HomescreenSelection < 0)
                    {
                        HomeIndex--;
                        if (HomeIndex < 0)
                            HomeIndex = 2;
                        HomescreenSelection += 9;
                        SwitchHomeScreen(HomeIndex);
                    }

                    DisplayView(1, HomescreenSelection);
                }
                else
                {
                    if (CurrentApp.Items.Count == 1)
                    {
                        RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_INPUT_EVENT");
                        RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)dir);
                        RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
                        return;
                    }
                    else if(CurrentApp.Items.Count == 0)
                    {
                        return;
                    }
                    if (dir == Direction.Down)
                        CurrentAppSelection++;
                    else if (dir == Direction.Up)
                        CurrentAppSelection--;

                    if (CurrentAppSelection > CurrentApp.Items.Count - 1)
                        CurrentAppSelection = 0;
                    if (CurrentAppSelection < 0)
                        CurrentAppSelection = CurrentApp.Items.Count - 1;

                    DisplayView((int)CurrentApp.Container, CurrentAppSelection);
                    //RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_INPUT_EVENT");
                    //RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)dir);
                    //RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
                }
            }

            public void SetSignalStrength(int strength)
            {
                if (IsInvalid())
                    return;

                if (strength < 0)
                    strength = 0;
                if (strength > 5)
                    strength = 5;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_SIGNAL_STRENGTH");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(strength);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void SetTheme(Theme th)
            {
                if (IsInvalid())
                    return;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_THEME");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)th);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void SetSoftKey_Data(SoftKey key, SoftkeyObject obj)
            {
                SetSoftKey_Visible(key, obj.Visible);
                SetSoftKey_Color(key, obj.RGBA);
                SetSoftKey_Icon(key, obj.Icon);
            }

            public void SetSoftKey_Visible(SoftKey key, bool visible)
            {
                int icon = 0;
                if(key == SoftKey.Left)
                {
                    SoftkeyLeft.Visible = visible;
                    icon = (int)SoftkeyLeft.Icon;
                }
                else if(key == SoftKey.Middle)
                {
                    SoftkeyMiddle.Visible = visible;
                    icon = (int)SoftkeyMiddle.Icon;
                }
                else
                {
                    SoftkeyRight.Visible = visible;
                    icon = (int)SoftkeyRight.Icon;
                }

                if (IsInvalid())
                    return;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_SOFT_KEYS");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)key);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterBool(visible);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(icon);
                //RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString("null");
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void SetSoftKey_Icon(SoftKey key, SoftkeyIcon icon)
            {
                bool visible = false;
                if (key == SoftKey.Left)
                {
                    SoftkeyLeft.Icon = icon;
                    visible = SoftkeyLeft.Visible;
                }
                else if (key == SoftKey.Middle)
                {
                    SoftkeyMiddle.Icon = icon;
                    visible = SoftkeyMiddle.Visible;
                }
                else
                {
                    SoftkeyRight.Icon = icon;
                    visible = SoftkeyRight.Visible;
                }

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_SOFT_KEYS");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)key);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterBool(visible);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)icon);
                //RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString("null");
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void SetSoftKey_Color(SoftKey key, RAGE.RGBA rgba)
            {
                if(key == SoftKey.Left)
                {
                    SoftkeyLeft.RGBA = rgba;
                }
                else if(key == SoftKey.Middle)
                {
                    SoftkeyMiddle.RGBA = rgba;
                }
                else
                {
                    SoftkeyRight.RGBA = rgba;
                }

                if (IsInvalid())
                    return;

                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_SOFT_KEYS_COLOUR");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)key);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)rgba.Red);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)rgba.Green);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt((int)rgba.Blue);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            private void DisplayView(int view, int select)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "DISPLAY_VIEW");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(select);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            private void SetDataSlotForHome(int view, int slot, int icon, int notf, string name, int alpha = 100)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(slot);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(icon);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(notf);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(name);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(alpha);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            private void SetDataSlotForSetting(int view, int slot, int icon, string name)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(slot);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(icon);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(name);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            //s1 = 33 - unread sms, 34 - readsms or anything else invisible
            //s2 = string, icon? name?
            //s3 = <FONT COLOr=" support actual text?
            private void SetDataSlotForMessageList(int view, int slot, string hour, string minute, bool seen, string fromAddress, string subjectTitle)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(slot);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(hour);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(minute);
                if(seen)
                    RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(34);
                else
                    RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(33);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(fromAddress);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(subjectTitle);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            //7
            private void SetDataSlotForMessageView(int view, int slot, string from, string message, string icon)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(slot);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(from);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(message);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(icon);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            private void SetDataSlotForContactList(int view, int slot, bool missedcall, string name, string icon)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(slot);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterBool(missedcall);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(name);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString("");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(icon);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            private void SetDataSlotForCallscreen(int view, int slot, string from, string title, string icon)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(slot);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString("");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(from);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(icon);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(title);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            private void SetDataSlotEmpty(int view)
            {
                RAGE.Game.Graphics.PushScaleformMovieFunction(PhoneScaleform, "SET_DATA_SLOT_EMPTY");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(view);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }

            public void GoHome()
            {
                SetSoftKey_Data(SoftKey.Left, Home_SoftkeyLeft);
                SetSoftKey_Data(SoftKey.Right, Home_SoftkeyRight);
                SetSoftKey_Data(SoftKey.Middle, Home_SoftkeyMiddle);
                SetBackgroundImage(HomescreenImage);

                foreach(HomeObject obj in HomeObjects_Stored[HomeIndex])
                    SetDataSlotForHome(1, (int)obj.Location, (int)obj.Icon, obj.NotificationNumber, obj.Name);

                DisplayView(1, HomescreenSelection);
                CurrentAppSelection = 0;
                CurrentApp = null;
            }

            public void TurnOn()
            {
                if (IsOn)
                    return;
                IsOn = true;
                if (PhoneType == 0)
                    PhoneScaleform = RAGE.Game.Graphics.RequestScaleformMovie("cellphone_ifruit");
                else if (PhoneType == 1)
                    PhoneScaleform = RAGE.Game.Graphics.RequestScaleformMovie("cellphone_facade");
                else if(PhoneType == 2)
                    PhoneScaleform = RAGE.Game.Graphics.RequestScaleformMovie("cellphone_badger");
                else
                    PhoneScaleform = RAGE.Game.Graphics.RequestScaleformMovie("cellphone_prologue");

                RAGE.Game.Utils.Settimera(0);
                while (!RAGE.Game.Graphics.HasScaleformMovieLoaded(PhoneScaleform))
                {
                    RAGE.Game.Invoker.Wait(0);
                    if (RAGE.Game.Utils.Timera() > 2000)
                        return;
                }

                RAGE.Game.Mobile.CreateMobilePhone(PhoneBody);
                RAGE.Game.Mobile.SetMobilePhonePosition(PhonePosition_Start.X, PhonePosition_Start.Y, PhonePosition_Start.Z);
                RAGE.Game.Mobile.SetMobilePhoneRotation(PhoneRotation_Start.X, PhoneRotation_Start.Y, PhoneRotation_Start.Z, 0);
                RAGE.Game.Mobile.SetMobilePhoneScale(PhoneScale);
                RAGE.Game.Mobile.GetMobilePhoneRenderId(ref PhoneRenderID);
                RAGE.Game.Mobile.ScriptIsMovingMobilePhoneOffscreen(false);

                SetDataSlotEmpty(1);

                SetSleepMode(false);
                SetTheme(Theme.Purple);
                HomescreenImage = BackgroundImage.BlueCircles;
                SetSignalStrength(5);
                SetSoftKey_Icon(SoftKey.Left, SoftkeyIcon.Select);
                SetSoftKey_Color(SoftKey.Left, new RGBA(46, 204, 113));
                SetSoftKey_Visible(SoftKey.Left, true);
                SetSoftKey_Icon(SoftKey.Middle, SoftkeyIcon.Keypad);
                SetSoftKey_Color(SoftKey.Middle, new RGBA(149, 165, 166));
                SetSoftKey_Visible(SoftKey.Middle, true);
                SetSoftKey_Icon(SoftKey.Right, SoftkeyIcon.Website);
                SetSoftKey_Color(SoftKey.Right, new RGBA(52, 152, 219));
                SetSoftKey_Visible(SoftKey.Right, true);
                SetTitlebarTime(69, 69, "RAGE");
                SetBatteryLevel(0.65f);

                Initialize();

                GoHome();

                DoBlackLerp = true;
                CurBlackLerp = 0f;
                CurLerp = 0f;
                DoLerpDown = false;
                DoLerpUp = true;
                DoBlackLerpInverse = false;
            }

            public void ChangePhysicalColor(int col)
            {
                if (!IsOn)
                    return;
                uint objHash = 0;

                if (PhoneType == 0)
                    objHash = RAGE.Game.Misc.GetHashKey("prop_phone_ing");
                else if (PhoneType == 1)
                    objHash = RAGE.Game.Misc.GetHashKey("prop_phone_ing_02");
                else if (PhoneType == 2)
                    objHash = RAGE.Game.Misc.GetHashKey("prop_phone_ing_03");
                else
                    return;

                Vector3 pos = RAGE.Elements.Player.LocalPlayer.Position;
                int objMy = RAGE.Game.Object.GetClosestObjectOfType(pos.X, pos.Y, pos.Z, 3f, objHash, false, false, false);
                RAGE.Game.Object.SetObjectTextureVariant(objMy, col);
            }

            public void TurnOff()
            {
                if (!IsOn)
                    return;
                IsOn = false;
                RAGE.Game.Mobile.ScriptIsMovingMobilePhoneOffscreen(true);
                //DisplayView(0, 0);
                CurLerp = 0f;
                DoLerpUp = false;
                DoLerpDown = true;
                DoBlackLerp = true;
                DoBlackLerpInverse = true;
                CurBlackLerp = 0f;
            }

            private float CalculatedBattery = 0.075f;
            public void SetBatteryLevel(float percent)
            {
                if (percent > 1f)
                    percent = 1f;
                if (percent < 0f)
                    percent = 0f;

                float max = 0.075f;
                CalculatedBattery = max * percent;
                BatteryLevel = percent;
            }

            private float CurLerp = 0f;
            private float TimeLerp = 2f;
            private float BlackLerp = 1f;
            private float BlackLerpInverse = 0.125f;
            private bool DoLerpUp = false;
            private float BlackValue = 255f;
            private bool DoLerpDown = false;
            private bool DoBlackLerp = false;
            private bool DoBlackLerpInverse = false;
            private float CurBlackLerp = 0f;

            public void Draw()
            {
                float f = GUI.GetDeltaTime();
                if (DoBlackLerp)
                {
                    if(CurBlackLerp >= BlackLerp)
                    {
                        DoBlackLerp = false;
                    }
                    else
                    {
                        CurBlackLerp += f;
                        RAGE.Chat.Output("F: " + CurBlackLerp);
                        if(DoBlackLerpInverse)
                            BlackValue = GUI.Lerp(0f, 255f, (CurBlackLerp / BlackLerpInverse));
                        else
                            BlackValue = GUI.Lerp(255f, 0f, (CurBlackLerp / BlackLerp));
                    }
                }
                if (DoLerpUp)
                {
                    if (CurLerp >= TimeLerp)
                    {
                        DoLerpUp = false;
                        IsOn = true;
                    }
                    else
                    {
                        CurLerp += f;
                        PhonePosition_Current.Y = GUI.Lerp(PhonePosition_Current.Y, PhonePosition_Final.Y, (CurLerp / TimeLerp));
                        PhoneRotation_Current.Y = GUI.Lerp(PhoneRotation_Current.Y, PhoneRotation_Final.Y, (CurLerp / TimeLerp));
                        RAGE.Game.Mobile.SetMobilePhonePosition(PhonePosition_Current.X, PhonePosition_Current.Y, PhonePosition_Current.Z);
                        RAGE.Game.Mobile.SetMobilePhoneRotation(PhoneRotation_Current.X, PhoneRotation_Current.Y, PhoneRotation_Current.Z, 0);
                    }
                }
                else if (DoLerpDown)
                {
                    if (CurLerp >= TimeLerp)
                    {
                        DoLerpDown = false;
                        PhoneRenderID = -1;
                        RAGE.Game.Mobile.DestroyMobilePhone();
                        RAGE.Game.Graphics.SetScaleformMovieAsNoLongerNeeded(ref PhoneScaleform);
                        PhoneScaleform = -1;
                    }
                    else
                    {
                        CurLerp += f;
                        PhonePosition_Current.Y = GUI.Lerp(PhonePosition_Current.Y, PhonePosition_Start.Y, (CurLerp / TimeLerp));
                        PhoneRotation_Current.Y = GUI.Lerp(PhoneRotation_Current.Y, PhoneRotation_Start.Y, (CurLerp / TimeLerp));
                        RAGE.Game.Mobile.SetMobilePhonePosition(PhonePosition_Current.X, PhonePosition_Current.Y, PhonePosition_Current.Z);
                        RAGE.Game.Mobile.SetMobilePhoneRotation(PhoneRotation_Current.X, PhoneRotation_Current.Y, PhoneRotation_Current.Z, 0);
                    }
                }

                if (PhoneScaleform != -1 && PhoneRenderID != -1)
                {
                    RAGE.Game.Ui.SetTextRenderId(PhoneRenderID);
                    RAGE.Game.Graphics.Set2dLayer(4);
                    RAGE.Game.Graphics.DrawScaleformMovie(PhoneScaleform, 0.1f, 0.179f, 0.2f, 0.356f, 255, 0, 255, 255, 0);
                    RAGE.Game.Graphics.DrawRect(0.925f, 0.04375f, 0.075f, 0.02f, 0, 0, 0, 255, 0);
                    if (BatteryLevel <= 0.25f)
                        RAGE.Game.Graphics.DrawRect(0.925f - ((0.075f - CalculatedBattery) / 2f), 0.04375f, CalculatedBattery, 0.02f, 231, 76, 60, 255, 0);
                    else if (BatteryLevel <= 0.5f)
                        RAGE.Game.Graphics.DrawRect(0.925f - ((0.075f - CalculatedBattery) / 2f), 0.04375f, CalculatedBattery, 0.02f, 243, 156, 18, 255, 0);
                    else
                        RAGE.Game.Graphics.DrawRect(0.925f - ((0.075f - CalculatedBattery) / 2f), 0.04375f, CalculatedBattery, 0.02f, 46, 204, 113, 255, 0);

                    if(CurrentApp == null)
                    {
                        if (HomeIndex == 0)
                        {
                            RAGE.Game.Graphics.DrawRect(0.5f - 0.085f - 0.015f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 255, 0);
                            RAGE.Game.Graphics.DrawRect(0.5f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 60, 0);
                            RAGE.Game.Graphics.DrawRect(0.5f + 0.085f + 0.015f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 60, 0);
                        }
                        else if (HomeIndex == 1)
                        {
                            RAGE.Game.Graphics.DrawRect(0.5f - 0.085f - 0.015f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 60, 0);
                            RAGE.Game.Graphics.DrawRect(0.5f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 255, 0);
                            RAGE.Game.Graphics.DrawRect(0.5f + 0.085f + 0.015f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 60, 0);
                        }
                        else
                        {
                            RAGE.Game.Graphics.DrawRect(0.5f - 0.085f - 0.015f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 60, 0);
                            RAGE.Game.Graphics.DrawRect(0.5f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 60, 0);
                            RAGE.Game.Graphics.DrawRect(0.5f + 0.085f + 0.015f, 0.855f, 0.065f, 0.015f, 255, 255, 255, 255, 0);
                        }
                    }
                    RAGE.Game.Graphics.DrawRect(0.5f, 0.5f, 1.0f, 1.0f, 0, 0, 0, (int)BlackValue, 0);
                    RAGE.Game.Ui.SetTextRenderId(1);
                }
            }
        }

        public void Tick(List<Events.TickNametagData> nametags)
        {
            if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollDown))
            {
                if (Phone.IsOnHomeScreen())
                    Phone.SetInputEvent(PhoneObject.Direction.Right);
                else
                    Phone.SetInputEvent(PhoneObject.Direction.Down);
            }
            else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollUp))
            {
                if(Phone.IsOnHomeScreen())
                    Phone.SetInputEvent(PhoneObject.Direction.Left);
                else
                    Phone.SetInputEvent(PhoneObject.Direction.Up);
            }
            else if(RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.PhoneUp))
            {
                Phone.TurnOn();
            }
            else if(RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.PhoneDown))
            {
                Phone.TurnOff();
            }
            else if(RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Attack))
            {
                if(Phone.IsOn)
                {
                    Phone.Click();
                    //RAGE.Chat.Output("Click->" + Phone.GetCurrentSelection());
                }
            }
            else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Aim))
            {
                if (Phone.IsOn)
                {
                    Phone.Back();
                    //RAGE.Chat.Output("Back");
                }
            }
            else if(RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.PhoneExtraOption))
            {
                RAGE.Chat.Output("ya");
                if(Phone.IsOn)
                {
                    Phone.Middle();
                }
            }

            Phone.Draw();
        }
    }
}
