using System;
using Simulant.Game.FFCS.Client.Game.InstanceContent;

namespace Simulant.Game.FFCS.Client.Game.Event
{
    // Client::Game::Event::EventFramework
    // ctor "E8 ?? ?? ?? ?? 48 89 05 ?? ?? ?? ?? 48 83 C4 28 E9"
    public struct EventFramework : IMemoryObject
    {
        public IntPtr Ptr { get; set; }

        [SigPattern("48 8B 05 * * * * 48 85 C0 74 ? 83 B8 ? ? ? ? ? 7C")]
        public static IntPtr InstancePtrPtr { get; set; }
        public static EventFramework Instance
            => InstancePtrPtr.ReadPtr().As<EventFramework>();

        public EventHandlerModule EventHandlerModule => Ptr.As<EventHandlerModule>(0x00);
        public DirectorModule DirectorModule => Ptr.As<DirectorModule>(0xC0);

        // from Hyperborea/Hyperborea/Utils.cs
        // GetMapEffectModule() => *(nint*)(((nint)EventFramework.Instance()) + 344);
        // Not sure about the actual type (just ContentDirector?), but it is not valid for public contents
        public InstanceContentDirector InstanceContentDirector => Ptr.As<InstanceContentDirector>(0x158);

        // [FieldOffset(0x160)] public LuaActorModule LuaActorModule;
        public MemoryField<int> LoadState => Ptr.Field<int>(0x3BF8); //0=Exd, 1=EventHandler, 2=Director, 3=LuaActor, 4=EventScene, 5=Idle?, 6=Ready?

        [SigPattern("E8 * * * * 33 D2 48 8B D8 48 85 C0 0F 84")]
        public static IntPtr GetContentDirectorFuncPtr { get; set; }
        public ContentDirector GetContentDirector()
            => GetContentDirectorFuncPtr.Call<IntPtr>(Ptr).As<ContentDirector>();

        [SigPattern("E8 * * * * 41 8B 5E 18 48 8B F8")]
        public static IntPtr GetInstanceContentDirectorFuncPtr { get; set; }
        public InstanceContentDirector GetInstanceContentDirector()
            => GetInstanceContentDirectorFuncPtr.Call<IntPtr>(Ptr).As<InstanceContentDirector>();

        [SigPattern("E8 * * * * 48 8B D0 48 85 C0 74 ? 80 B8")]
        public static IntPtr GetPublicContentDirectorFuncPtr { get; set; }
        public PublicContentDirector GetPublicContentDirector()
            => GetPublicContentDirectorFuncPtr.Call<IntPtr>(Ptr).As<PublicContentDirector>();

        [SigPattern("40 53 48 83 EC 20 48 83 3D ? ? ? ? ? 8B D9 74 1D")]
        public static IntPtr GetPublicContentDirectorByTypeFuncPtr { get; set; }
        public static PublicContentDirector GetPublicContentDirectorByType(PublicContentDirectorType publicContentDirectorType)
            => GetPublicContentDirectorByTypeFuncPtr.Call<IntPtr>(publicContentDirectorType).As<PublicContentDirector>();

        /*
        [SigPattern("E8 * * * * 44 0F B6 65 ? 4C 8B F8")]
        public static IntPtr GetEventHandlerByIdFuncPtr { get; set; }
        public EventHandler GetEventHandlerById(uint id)
            => GetEventHandlerByIdFuncPtr.Call<IntPtr>(Ptr, id).As<EventHandler>();
        
        public EventHandler GetEventHandlerById(ushort id)
            => GetEventHandlerById((uint)(id | 0x10000));
        */

        [SigPattern("48 89 5C 24 ? 48 89 7C 24 ? 41 56 48 83 EC ? 48 8B D9 48 89 6C 24")]
        public static IntPtr SetTerritoryTypeIdFuncPtr { get; set; }
        public void SetTerritoryTypeId(ushort territoryType)
            => SetTerritoryTypeIdFuncPtr.Call(Ptr, territoryType);

        [SigPattern("E8 * * * * 8B D8 3B 85")]
        public static IntPtr GetCurrentContentIdFuncPtr { get; set; }
        public static uint GetCurrentContentId()
            => GetCurrentContentIdFuncPtr.Call<uint>();

        [SigPattern("E8 * * * * 38 46 ? 75")]
        public static IntPtr GetCurrentContentTypeFuncPtr { get; set; }
        public static ContentType GetCurrentContentType()
            => GetCurrentContentTypeFuncPtr.Call<ContentType>();

        [SigPattern("E8 * * * * 8B D8 EB ? 0F B7 DF")]
        public static IntPtr GetContentFinderConditionFuncPtr { get; set; }
        public static ushort GetContentFinderCondition(ContentType contentType, uint contentId)
            => GetContentFinderConditionFuncPtr.Call<ushort>(contentType, contentId);

        [SigPattern("48 83 EC 28 48 8B 05 ? ? ? ? 48 85 C0 74 2C")]
        public static IntPtr CanLeaveCurrentContentFuncPtr { get; set; }
        public static bool CanLeaveCurrentContent()
            => CanLeaveCurrentContentFuncPtr.Call<bool>();

        [SigPattern("E8 * * * * 48 8B 43 ? 41 B2")]
        public static IntPtr LeaveCurrentContentFuncPtr { get; set; }
        public static void LeaveCurrentContent(bool forced = false)
            => LeaveCurrentContentFuncPtr.Call(forced);

    }

    public enum ContentType : byte
    {
        None, // used for raids
        Instance,
        Party, // SkyIsland - used in early phases of the Diadem
        Public,
        GoldSaucer
    }
}