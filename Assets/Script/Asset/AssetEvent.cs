public enum AssetEvent
{
    HunkRes = ManagerID.AssetManager + 1,
    ReleaseSingleObj,//释放单个object
    ReleaseBundleObjes,//释放一个bundle包里所有的object
    ReleaseSceneObjes,//释放单个场景中所有的object
    ReleaseSingleBundle,//释放单个assetbundle
    ReleaseSceneBundle,//释放一个场景中的所有assetbundle
    ReleaseAll//释放一个场景中所有的bundle和objects
}